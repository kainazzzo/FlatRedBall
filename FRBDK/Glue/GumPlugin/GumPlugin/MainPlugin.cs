﻿using FlatRedBall.Glue;
using FlatRedBall.Glue.Elements;
using FlatRedBall.Glue.Plugins;
using FlatRedBall.Glue.Plugins.ExportedImplementations;
using FlatRedBall.Glue.VSHelpers;
using FlatRedBall.IO;
using Gum.DataTypes;
using GumPlugin.CodeGeneration;
using GumPlugin.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using FlatRedBall.Glue.SaveClasses;
using GumPlugin.Controls;
using GumPlugin.ViewModels;
using System.ComponentModel;
using EditorObjects.Parsing;
using Gum.Managers;
using System.Drawing;

namespace GumPlugin
{
    [Export(typeof(PluginBase))]
    public class MainPlugin : PluginBase
    {
        #region Fields

        GumControl control;
        GumViewModel viewModel;
        GumxPropertiesManager propertiesManager;

        ToolStripMenuItem addGumProjectMenuItem;

        #endregion

        #region Properties

        public override string FriendlyName
        {
            get { return "Gum Plugin"; }
        }

        public override Version Version
        {
            get { return new Version(0, 7, 0, 0); }
        }

        #endregion

        #region Methods


        public override void StartUp()
        {
            propertiesManager = new GumxPropertiesManager();

            AssetTypeInfoManager.Self.AddCommonAtis();

            this.ReactToLoadedGlux += HandleGluxLoad;

            this.ReactToLoadedGluxEarly += HandleGluxLoadEarly;

            this.ReactToUnloadedGlux += HandleUnloadedGlux;

            this.CanFileReferenceContent += FileReferenceTracker.CanTrackDependenciesOn;

            this.GetFilesReferencedBy += FileReferenceTracker.Self.HandleGetFilesReferencedBy;

            this.GetFilesNeededOnDiskBy += FileReferenceTracker.Self.HandleGetFilesNeededOnDiskBy;

            this.TryAddContainedObjects += ContainedObjectsManager.Self.HandleTryAddContainedObjects;

            addGumProjectMenuItem = this.AddMenuItemTo("Add New Gum Project", HandleAddNewGumProject, "Content");
            //var bmp = new Bitmap(WindowsFormsApplication1.Properties.Resources.myimage);
            addGumProjectMenuItem.Image = new Bitmap(GumPlugin.Resource1.GumIcon);

            this.ReactToTreeViewRightClickHandler += RightClickManager.Self.HandleTreeViewRightClick;

            this.ReactToFileChangeHandler += HandleFileChange;

            this.ReactToNewFileHandler += HandleNewFile;

            this.AddEventsForObject += EventsManager.Self.HandleAddEventsForObject;

            this.ReactToItemSelectHandler += HandleItemSelected;

            this.ReactToNewScreenCreated += HandleNewScreen;

            CodeGeneratorManager.Self.CreateCodeGenerators();

            Gum.Managers.StandardElementsManager.Self.Initialize();
            
        }

        private void HandleNewScreen(FlatRedBall.Glue.SaveClasses.ScreenSave newScreen)
        {
            bool createGumScreen = propertiesManager.GetAutoCreateGumScreens();

            if(createGumScreen && AppState.Self.GumProjectSave != null)
            {
                string gumScreenName = FileManager.RemovePath( newScreen.Name ) + "Gum";

                bool exists = AppState.Self.GumProjectSave.Screens.Any(item => item.Name == gumScreenName);
                if (!exists)
                {
                    Gum.DataTypes.ScreenSave gumScreen = new Gum.DataTypes.ScreenSave();
                    gumScreen.Initialize(StandardElementsManager.Self.GetDefaultStateFor("Screen"));
                    gumScreen.Name = gumScreenName;

                    string gumProjectFileName = GumProjectManager.Self.GetGumProjectFileName();
                    var directory = FileManager.GetDirectory(gumProjectFileName) + ElementReference.ScreenSubfolder + "/";

                    AppState.Self.GumProjectSave.Screens.Add(gumScreen);
                    var elementReference = new ElementReference
                    {
                        ElementType = ElementType.Screen,
                        Name = gumScreenName
                    };
                    AppState.Self.GumProjectSave.ScreenReferences.Add(elementReference);
                    AppState.Self.GumProjectSave.ScreenReferences.Sort((first, second) => first.Name.CompareTo(second.Name));

                    AppState.Self.GumProjectSave.Save(gumProjectFileName, saveElements: false);

                    string screenFileName =
                        directory + gumScreen.Name + "." + GumProjectSave.ScreenExtension;


                    gumScreen.Save(screenFileName);

                }
                // Select the screen to add the file to this
                GlueState.Self.CurrentScreenSave = newScreen;

                RightClickManager.Self.AddScreenByName(gumScreenName);
            }
        }

        private void HandleItemSelected(TreeNode selectedTreeNode)
        {
            bool shouldShowTab = GetIfShouldShowTab(selectedTreeNode);

            if(shouldShowTab)
            {
                if (control == null)
                {
                    control = new GumControl();
                    viewModel = new GumViewModel();
                    viewModel.PropertyChanged += HandleViewModelPropertyChanged;
                    control.DataContext = viewModel;

                    this.AddToTab(PluginManager.CenterTab, control, "Gum Properties");
                }
                else
                {
                    AddTab();
                }
                viewModel.SetFrom(AppState.Self.GumProjectSave, selectedTreeNode.Tag as ReferencedFileSave);
            }
            else
            {
                RemoveTab();
            }
        }

        private void HandleViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Maybe we'll need this at some point:
            //var referencedFileSave = GlueState.Self.CurrentReferencedFileSave;
            //var absoluteFileName = GlueCommands.Self.GetAbsoluteFileName(referencedFileSave);

            propertiesManager.HandlePropertyChanged(e.PropertyName);
        }

        private bool GetIfShouldShowTab(TreeNode selectedTreeNode)
        {
            if(selectedTreeNode != null && selectedTreeNode.Tag is ReferencedFileSave)
            {
                var rfs = selectedTreeNode.Tag as ReferencedFileSave;

                return FileManager.GetExtension( rfs.Name ) == "gumx";
            }

            return false;
        }

        private void HandleUnloadedGlux()
        {
            AssetTypeInfoManager.Self.UnloadProjectSpecificAtis();

            UpdateMenuItemVisibility();
        }

        private void HandleNewFile(ReferencedFileSave newFile)
        {
            string extension = FileManager.GetExtension(newFile.Name);
            // If it's a component then assign the specific type:
            if(extension == GumProjectSave.ComponentExtension)
            {
                string componentName = FileManager.RemovePath(FileManager.RemoveExtension(newFile.Name));

                var componentType = Gum.Managers.ObjectFinder.Self.GetComponent(componentName);
                var componentTypeWithRuntime = componentName + "Runtime";

                var ati = AssetTypeInfoManager.Self.GetAtisForDerivedGues().FirstOrDefault(item => item.RuntimeTypeName == componentTypeWithRuntime);

                if (ati != null)
                {
                    newFile.RuntimeType = ati.QualifiedRuntimeTypeName.QualifiedType;
                }
            }

            UpdateMenuItemVisibility();
        }

        private void HandleFileChange(string fileName)
        {
            string extension = FileManager.GetExtension(fileName);


            if (extension == GumProjectSave.ComponentExtension ||
                extension == GumProjectSave.ScreenExtension ||
                extension == GumProjectSave.StandardExtension ||
                extension == GumProjectSave.ProjectExtension)
            {
                // November 1, 2015
                // Why do we reload the
                // entire project nad not
                // just the object that changed?
                GumProjectManager.Self.ReloadProject();

                // Something could have changed - more components could have been added
                AssetTypeInfoManager.Self.AddProjectSpecificAtis();

                if (extension == GumProjectSave.ProjectExtension)
                {
                    CodeGeneratorManager.Self.GenerateDerivedGueRuntimes();
                }
                else
                {
                    CodeGeneratorManager.Self.GenerateDueToFileChange(fileName);
                }
                EventsManager.Self.RefreshEvents();

                FileReferenceTracker.Self.RemoveUnreferencedMissingFilesFromVsProject();
            }
            else if (extension == "ganx")
            {
                // Animations have changed, so we need to regenerate animation code.
                // For now we'll generate everything, but we may want to make this faster
                // and more precise by only generating the selected element:
                CodeGeneratorManager.Self.GenerateDerivedGueRuntimes();
            }
        }

        private void HandleAddNewGumProject(object sender, EventArgs e)
        {
            var added = GumProjectManager.Self.TryAddNewGumProject();

            if (added)
            {
                EmbeddedResourceManager.Self.UpdateCodeInProjectPresence();
            }
        }

        private void HandleGluxLoadEarly()
        {
            GumProjectManager.Self.ReloadProject();

            propertiesManager.UpdateUseAtlases();

            // These are needed for code gen
            AssetTypeInfoManager.Self.AddProjectSpecificAtis();
            EventsManager.Self.RefreshEvents();
        }

        private void HandleGluxLoad()
        {
            // todo: Removing a file should cause this to get called, but I don't think Gum lets us subscribe to that yet.
            EmbeddedResourceManager.Self.UpdateCodeInProjectPresence();

            CodeGeneratorManager.Self.GenerateDerivedGueRuntimes();

            FileReferenceTracker.Self.RemoveUnreferencedMissingFilesFromVsProject();

            UpdateMenuItemVisibility();
        }

        private void UpdateMenuItemVisibility()
        {
            ReferencedFileSave gumRfs = null;
            if (GlueState.Self.CurrentGlueProject != null)
            {
                gumRfs = GumProjectManager.Self.GetRfsForGumProject();
            }
            addGumProjectMenuItem.Visible = GlueState.Self.CurrentGlueProject != null && gumRfs == null;

        }

        public override bool ShutDown(FlatRedBall.Glue.Plugins.Interfaces.PluginShutDownReason shutDownReason)
        {
            Glue.MainGlueWindow.Self.Invoke((MethodInvoker)RemoveAllMenuItems);

            CodeGeneratorManager.Self.RemoveCodeGenerators();


            return true;
        }

        #endregion
    }
}