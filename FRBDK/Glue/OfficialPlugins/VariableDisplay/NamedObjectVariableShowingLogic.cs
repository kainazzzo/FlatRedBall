﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using FlatRedBall.Glue.Elements;
using FlatRedBall.Glue.FormHelpers;
using FlatRedBall.Glue.FormHelpers.PropertyGrids;
using FlatRedBall.Glue.Plugins.ExportedImplementations;
using FlatRedBall.Glue.SaveClasses;
using FlatRedBall.Glue.SetVariable;
using FlatRedBall.Instructions.Reflection;
using FlatRedBall.Utilities;
using Glue;
using WpfDataUi;
using WpfDataUi.DataTypes;
using FlatRedBall.Glue.Plugins;

namespace OfficialPlugins.VariableDisplay
{
    static class NamedObjectVariableShowingLogic
    {
        public static void UpdateShownVariables(DataUiGrid grid, NamedObjectSave instance, IElement container)
        {
            grid.Categories.Clear();

            List<MemberCategory> categories = new List<MemberCategory>();
            var defaultCategory = new MemberCategory("Variables");
            defaultCategory.FontSize = 14;
            categories.Add(defaultCategory);

            AssetTypeInfo ati = instance.GetAssetTypeInfo();

            // not sure if this is needed:
            if (instance.TypedMembers.Count == 0)
            {
                instance.UpdateCustomProperties();
            }

            for (int i = 0; i < instance.TypedMembers.Count; i++)
            {

                TypedMemberBase typedMember = instance.TypedMembers[i];
                InstanceMember instanceMember = CreateInstanceMember(instance, container, typedMember, ati);

                var categoryToAddTo = GetOrCreateCategoryToAddTo(categories, ati, typedMember);
                
                if (instanceMember != null)
                {
                    categoryToAddTo.Members.Add(instanceMember);
                }
            }

            if (ati != null)
            {
                SortCategoriesAndMembers(ref categories, ati);
            }


            if (defaultCategory.Members.Count == 0)
            {
                categories.Remove(defaultCategory);
            }
            else if(categories.Count != 1)
            {
                defaultCategory.Name = "Other Variables";
            }

            if (categories.Count != 0)
            {
                // "Name" shoul be the very first property:
                var nameCategory = CreateNameInstanceMember(instance);
                //categories.Add(nameCategory);
                categories.Insert(0, nameCategory);
                //categories.First().Members.Insert(0, nameInstanceMember);
            }

            // skip the first category in putting the alternating colors:
            for (int i = 0; i < categories.Count; i++)
            {
                var category = categories[i];
                if (i != 0)
                {
                    const byte brightness = 227;
                    category.SetAlternatingColors(new SolidColorBrush(Color.FromRgb(brightness, brightness, brightness)), Brushes.Transparent);
                }
                grid.Categories.Add(category);
            }
            grid.Refresh();
        }

        private static MemberCategory CreateNameInstanceMember(NamedObjectSave instance)
        {
            var instanceMember = new DataGridItem();
            instanceMember.DisplayName = "Name";
            instanceMember.UnmodifiedVariableName = "Name";
            // This won't actually be the old value after one change, but it's as good as we can get
            string oldValue = instance.InstanceName;

            instanceMember.CustomSetEvent += (throwaway, value) =>
            {
                instanceMember.IsDefault = false;
                RefreshLogic.IgnoreNextRefresh();

                instance.InstanceName = value as string;

                EditorObjects.IoC.Container.Get<SetVariableLogic>().ReactToPropertyChanged(
                    "InstanceName", oldValue, "InstanceName", null);


                //GlueCommands.Self.GluxCommands.SetVariableOn(
                //    instance,
                //    "Name",
                //    typeof(string),
                //    value);


                GlueCommands.Self.GluxCommands.SaveGlux();

                GlueCommands.Self.RefreshCommands.RefreshPropertyGrid();

                GlueCommands.Self.GenerateCodeCommands.GenerateCurrentElementCode();
            };
            instanceMember.CustomGetEvent += (throwaway) =>
                {
                    return instance.InstanceName;
                };

            instanceMember.CustomGetTypeEvent += (throwaway) => typeof(string);

            MemberCategory category = new MemberCategory();
            category.Name = "";
            category.HideHeader = true;
            category.Members.Add(instanceMember);

            return category;
        }

        private static void SortCategoriesAndMembers(ref List<MemberCategory> categories, AssetTypeInfo ati)
        {
            categories = SortCategories(categories, ati);

            SortMembers(categories, ati);
        }

        private static void SortMembers(List<MemberCategory> categories, AssetTypeInfo ati)
        {
            foreach(var category in categories)
            {
                string categoryName = category.Name;

                var variableDefinitions = ati.VariableDefinitions
                    .Where(item => item.Category == categoryName)
                    .Select(item=>item.Name)
                    .ToList();

                var sorted = category.Members
                    .OrderBy(item =>
                    {
                        var castedItem = item as DataGridItem;
                        var index = variableDefinitions.IndexOf(castedItem.UnmodifiedVariableName);

                        if(index == -1)
                        {
                            return int.MaxValue;
                        }
                        else
                        {
                            return index;
                        }
                    })
                    .ToList();

                category.Members.Clear();

                foreach(var item in sorted)
                {
                    category.Members.Add(item);
                }
            }
        }

        private static List<MemberCategory> SortCategories(List<MemberCategory> categories, AssetTypeInfo ati)
        {
            var orderedCategoryNames = ati.VariableDefinitions.Select(item => item.Category).Distinct().ToList();

            categories = categories.OrderBy(item =>
            {
                int index = orderedCategoryNames.IndexOf(item.Name);

                if (index == -1)
                {
                    return int.MaxValue;
                }
                else
                {
                    return index;
                }
            }).ToList();
            return categories;
        }

        private static MemberCategory GetOrCreateCategoryToAddTo(List<MemberCategory> categories, AssetTypeInfo ati, TypedMemberBase typedMember)
        {
            // By defaut make the last category get used (this is "Variables")
            var categoryToAddTo = categories.Last();
            // If there is an AssetTypeInfo...
            if (ati != null)
            {
                // ... see if there is avariable definition for this variable...
                var foundVariableDefinition = ati.VariableDefinitions.FirstOrDefault(item => item.Name == typedMember.MemberName);
                if (foundVariableDefinition != null)
                {
                    //... if so, see the category that it's a part of...
                    string categoryName = foundVariableDefinition.Category;

                    if (!string.IsNullOrEmpty(categoryName))
                    {
                        //... if a category is defined, see if we have a MemberCategory that we've created for it...
                        categoryToAddTo = categories.FirstOrDefault(item => item.Name == categoryName);

                        if (categoryToAddTo == null)
                        {
                            //... if not, make one, and insert it before the last:
                            categoryToAddTo = new MemberCategory(categoryName);
                            categoryToAddTo.FontSize = 14;

                            categories.Insert(categories.Count - 1, categoryToAddTo);
                        }
                    }
                }
            }
            return categoryToAddTo;
        }

        private static InstanceMember CreateInstanceMember(NamedObjectSave instance, IElement container, TypedMemberBase typedMember, AssetTypeInfo ati)
        {
            bool shouldBeSkipped = GetIfShouldBeSkipped(typedMember, instance, ati);

            DataGridItem instanceMember = null;

            if (!shouldBeSkipped)
            {
                var memberType = typedMember.MemberType;

                VariableDefinition variableDefinition = null;

                if (ati != null)
                {
                    variableDefinition = ati.VariableDefinitions.FirstOrDefault(item => item.Name == typedMember.MemberName);
                }

                instanceMember = new DataGridItem();

                instanceMember.FirstGridLength = new System.Windows.GridLength(140);

                instanceMember.UnmodifiedVariableName = typedMember.MemberName;
                string displayName = StringFunctions.InsertSpacesInCamelCaseString(typedMember.MemberName);
                instanceMember.DisplayName = displayName;

                instanceMember.TypeConverter = PluginManager.GetTypeConverter(
                     container, instance, typedMember);

                instanceMember.CustomGetTypeEvent += (throwaway) => memberType;


                instanceMember.IsDefault = instance.GetInstructionFromMember(typedMember.MemberName) == null;

                instanceMember.CustomGetEvent += (throwaway) =>
                {

                    var instruction = instance.GetInstructionFromMember(typedMember.MemberName);

                    if (instruction == null)
                    {
                        if (variableDefinition != null)
                        {
                            var toReturn = variableDefinition.DefaultValue;
                            if (memberType == typeof(bool))
                            {
                                bool boolToReturn = false;

                                bool.TryParse(variableDefinition.DefaultValue, out boolToReturn);

                                return boolToReturn;
                            }
                            else
                            {
                                return toReturn;
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return instruction.Value;
                    }
                };

                instanceMember.CustomSetEvent += (owner, value) =>
                {
                    instanceMember.IsDefault = false;
                    RefreshLogic.IgnoreNextRefresh();
                    GlueCommands.Self.GluxCommands.SetVariableOn(
                        instance,
                        typedMember.MemberName,
                        memberType,
                        value);


                    GlueCommands.Self.GluxCommands.SaveGlux();

                    GlueCommands.Self.RefreshCommands.RefreshPropertyGrid();

                    GlueCommands.Self.GenerateCodeCommands.GenerateCurrentElementCode();
                };

                instanceMember.IsDefaultSet += (owner, args) =>
                    {
                        MakeDefault(instance, typedMember.MemberName);
                    };

                instanceMember.SetValueError += (newValue) =>
                    {
                        if (newValue is string && string.IsNullOrEmpty(newValue as string))
                        {
                            MakeDefault(instance, typedMember.MemberName);
                        }
                    };


            }
            return instanceMember;
        }

        private static bool GetIfShouldBeSkipped(TypedMemberBase typedMember, NamedObjectSave instance, AssetTypeInfo ati)
        {
            var name = typedMember.MemberName;

            if (ati != null)
            {

                if (ati.IsPositionedObject)
                {
                    if (name.EndsWith("Velocity") || name.EndsWith("Acceleration") || name.StartsWith("Relative") ||
                        name == "ParentBone" || name == "KeepTrackOfReal" || name == "Drag"

                        )
                    {
                        return true;
                    }

                }

                if (ati.QualifiedRuntimeTypeName.QualifiedType == "FlatRedBall.Math.Geometry.AxisAlignedRectangle")
                {
                    return name == "ScaleX" || name == "ScaleY" || name == "Top" || name == "Bottom" ||
                        name == "Left" || name == "Right";
                }

                if (ati.QualifiedRuntimeTypeName.QualifiedType == "FlatRedBall.Graphics.Text")
                {
                    return
                        name == "AlphaRate" || name == "RedRate" || name == "GreenRate" || name == "BlueRate" ||
                        name == "ScaleVelocity" || name == "SpacingVelocity" ||
                        name == "ScaleXVelocity" || name == "ScaleYVelocity" ||
                        // These used to be the standard way to size text, but now we just
                        // use "TextureScale"
                        name == "Scale" || name == "Spacing" || name == "NewLineDistance"
                        
                        ;
                }

                if (ati.QualifiedRuntimeTypeName.QualifiedType == "FlatRedBall.Camera")
                {
                    return
                        name == "AspectRatio" || name == "DestinationRectangle" || name == "CameraModelCullMode";

                }
             
                if (ati.QualifiedRuntimeTypeName.QualifiedType == "FlatRedBall.Math.Geometry.Polygon")
                {
                    return
                        name == "RotationX" || name == "RotationY" || name == "Points";
                }

                
                if (ati.QualifiedRuntimeTypeName.QualifiedType == "FlatRedBall.Graphics.Layer")
                {
                    return 
                        name == "LayerCameraSettings";
                }

                if (ati.QualifiedRuntimeTypeName.QualifiedType == "FlatRedBall.Sprite")
                {
                    return
                        name == "AlphaRate" || name == "RedRate" || name == "GreenRate" || name == "BlueRate" ||
                        name == "RelativeTop" || name == "RelativeBottom" || name == "RelativeLeft" || name == "RelativeRight" ||
                        name == "TimeCreated" || name == "TimeIntoAnimation" ||
                        name == "ScaleX" || name == "ScaleY" || name == "CurrentChainIndex" ||
                        name == "Top" || name == "Bottom" || name == "Left" || name == "Right" ||
                        name == "PixelSize" ||
                        name == "LeftTextureCoordinate" || name == "RightTextureCoordinate" ||
                        name == "BottomTextureCoordinate" || name == "TopTextureCoordinate" ||
                        name == "ScaleXVelocity" || name == "ScaleYVelocity";

                }
            }
            return false;
        }

        private static void MakeDefault(NamedObjectSave instance, string memberName)
        {
            PropertyGridRightClickHelper.SetVariableToDefault(instance, memberName);

            ElementViewWindow.GenerateSelectedElementCode();

            GlueCommands.Self.GluxCommands.SaveGlux();

            MainGlueWindow.Self.PropertyGrid.Refresh();
        }


    }
}
