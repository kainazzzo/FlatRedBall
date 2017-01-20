﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FlatRedBall.IO;
using System.Reflection;

using FlatRedBall.Glue.Parsing;
using FlatRedBall.Instructions.Reflection;
using ModelEditor.SaveClasses;
using System.Xml.Serialization;

#if !GLUE
using FlatRedBall.Instructions;
using FlatRedBall.Instructions.Reflection;
#endif

namespace FlatRedBall.Glue.Elements
{
    #region PlatformSpecificType class
    public struct PlatformSpecificType
    {
        public const string AllPlatform = "All";

        public string QualifiedType;
        public string Platform;
    }
    #endregion


    public class MemberTypeComparer : IEqualityComparer<MemberWithType>
    {
        public bool AreEqual(MemberWithType first, MemberWithType second)
        {
            return first.Member == second.Member;
        }

        public bool Equals(MemberWithType x, MemberWithType y)
        {
            return x.Member == y.Member;
        }

        public int GetHashCode(MemberWithType obj)
        {
            return obj.Member.GetHashCode();
        }
    }

    public class MemberWithType
    {
        public string Member;
        public string Type;

        public override string ToString()
        {
            return string.Format("{0} ({1})", Member, Type);
        }
    }


    public class AssetTypeInfo
	{
		#region Fields

        [XmlIgnore]
        public List<MemberWithType> CachedExtraVariables = new List<MemberWithType>();

        private string mSaveTypeName;

		public string Extension;

        public string RuntimeTypeName
        {
            get
            {
                if (string.IsNullOrEmpty(QualifiedRuntimeTypeName.QualifiedType))
                {
                    return null;
                }
                else if (QualifiedRuntimeTypeName.QualifiedType.Contains('.'))
                {
                    int lastPeriod = QualifiedRuntimeTypeName.QualifiedType.LastIndexOf('.');

                    int startingIndex = lastPeriod + 1;
                    int length = QualifiedRuntimeTypeName.QualifiedType.Length - startingIndex;

                    return QualifiedRuntimeTypeName.QualifiedType.Substring(startingIndex, length);
                }
                else
                {
                    return QualifiedRuntimeTypeName.QualifiedType;
                }
            }
        }

        // This used to be a string, but different
        // platforms may have different types or namespaces
        // for different types.  For example, FSB is built on
        // the namespace structure of XNA 3 which has Color in 
        // a different namespace than XNA 4.
        public PlatformSpecificType QualifiedRuntimeTypeName;

        public string AdjustRelativeZ;
        public string QualifiedSaveTypeName
        {
            get
            {
                return mSaveTypeName;
            }
            set
            {
                mSaveTypeName = value;
                if (!string.IsNullOrEmpty(mSaveTypeName))
                {
                    string name = Assembly.CreateQualifiedName("FlatRedBall", mSaveTypeName);
                    mSaveType = Type.GetType(name);

                    if (mSaveType == null)
                    {
                        name = Assembly.CreateQualifiedName("FlatRedBallMdx", mSaveTypeName);
                        mSaveType = Type.GetType(name);

                        if (mSaveType == null)
                        {
                            // We used to throw an exception here, but now it's okay - we allow custom types
                            //throw new Exception();
                        }
                    }
                }
            }
        }
		public string FriendlyName;
		public List<string> AddToManagersMethod = new List<string>();
		public List<string> LayeredAddToManagersMethod = new List<string>();
        public string MakeManuallyUpdatedMethod;
        public string ActivityMethod;
        public string AfterCustomActivityMethod;
		public string DestroyMethod;
        public string RecycledDestroyMethod;

        string mExtraVariablesPattern;
        public string ExtraVariablesPattern
        {
            get
            {
                return mExtraVariablesPattern;
            }
            set
            {
                mExtraVariablesPattern = value;
                UpdateExtraVariableCache();
            }
        }
        public List<VariableDefinition> VariableDefinitions = new List<VariableDefinition>();

        public string PostInitializeCode;
        public bool ShouldAttach;
        public bool IsPositionedObject;
        public string AttachToNullOnlyMethod;
        public bool HasCursorIsOn;
        public bool HasVisibleProperty;
        public bool ImplementsICollidable;
        public string ContentImporter;
        public string ContentProcessor;

        public bool SupportsMakeOneWay;

        public string RemoveFromLayerMethod;

        public string FindByNameSyntax;

        public bool CanBeObject;
        public bool DefaultPublic;

		Type mSaveType;

        public bool MustBeAddedToContentPipeline;
        public bool ShouldBeDisposed;

        public bool CanBeCloned;
        public string CustomCloneMethod;

        public bool CanIgnorePausing;

        /// <summary>
        /// Line of code used to load a given piece of content. If this is blank, then FlatRedBallServices.Load will be used.
        /// </summary>
        /// <remarks>
        /// Supported parameters:
        /// 
        /// * {THIS}
        /// * {TYPE}
        /// * {FILE_NAME}
        /// * {CONTENT_MANAGER_NAME}
        /// </remarks>
        public string CustomLoadMethod;

        public string CustomBuildToolName;

        public bool ImplementsIWindow;

        

        // Making this a field for now because CSV deserializer can't deserialize list properties
        public List<FlatRedBall.Glue.CodeGeneration.ConversionCodeGenInfo> Conversion;

        /// <summary>
        /// If this is true, 
        /// then Glue will not 
        /// show this object in 
        /// the New File Window regardless 
        /// of other settings (such as whether 
        /// a runtime type and sample file exist)
        /// </summary>
        public bool HideFromNewFileWindow;

        /// <summary>
        /// If true, Glue will not add this file to the content project when it is regenerated.
        /// By default this is false, which means that files added to Glue will be added to the content project.
        /// </summary>
        public bool ExcludeFromContentProject;
        #endregion

        #region Properties

        [XmlIgnore]
        public Type SaveType
        {
            get { return mSaveType; }
        }

        public bool IsInstantiatedInAddToManagers
        {
            get
            {
                return 
                    AddToManagersMethod.Count != 0 &&
                    !string.IsNullOrEmpty(AddToManagersMethod[0]) &&
                    AddToManagersMethod[0].StartsWith("this =");
            }
        }

        

        #endregion

        #region Methods

        public AssetTypeInfo()
        {
            Conversion = new List<CodeGeneration.ConversionCodeGenInfo>();
        }

		public AssetTypeInfo(string extension, 
			string qualifiedRuntimeTypeName,
			Type saveType, string friendlyName, string addToManagersMethod,
			string destroyMethod)
		{
            Conversion = new List<CodeGeneration.ConversionCodeGenInfo>();
            Extension = extension;

			QualifiedRuntimeTypeName = new PlatformSpecificType();
            QualifiedRuntimeTypeName.QualifiedType = qualifiedRuntimeTypeName;

			mSaveType = saveType;
            if (mSaveType != null)
            {
                QualifiedSaveTypeName = mSaveType.FullName;
            }
			FriendlyName = friendlyName;
			AddToManagersMethod = new List<string>(){addToManagersMethod};
			DestroyMethod = destroyMethod;

		}


        private void UpdateExtraVariableCache()
        {
            CachedExtraVariables.Clear();

            if (!string.IsNullOrEmpty(ExtraVariablesPattern))
            {
                string[] values = ExtraVariablesPattern.Split(';');

                for (int i = 0; i < values.Length; i++)
                {
                    string value = values[i].Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        int spaceIndex = value.IndexOf(' ');

                        

                        string typeString = value.Substring(0, spaceIndex);
                        string memberName = value.Substring(spaceIndex + 1, value.Length - (spaceIndex + 1));

                        var toAdd = new MemberWithType();
                        toAdd.Member = memberName;
                        toAdd.Type = typeString;

                        CachedExtraVariables.Add(toAdd);
                    }
                }
            }

        }

		public override string ToString()
		{
			return FriendlyName;
		}

		#endregion

	}
}
