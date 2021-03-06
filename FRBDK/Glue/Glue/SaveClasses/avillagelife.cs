﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://schemas.microsoft.com/developer/msbuild/2003")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="http://schemas.microsoft.com/developer/msbuild/2003", IsNullable=false)]
public partial class ProjectSave {
    
    private ProjectPropertyGroup[] propertyGroupField;
    
    private ProjectItemGroup[] itemGroupField;
    
    private ProjectImport[] importField;
    
    private ProjectProjectExtensionsVisualStudioFlavorProperties[][][] projectExtensionsField;
    
    private string toolsVersionField;
    
    private string defaultTargetsField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("PropertyGroup")]
    public ProjectPropertyGroup[] PropertyGroup {
        get {
            return this.propertyGroupField;
        }
        set {
            this.propertyGroupField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ItemGroup")]
    public ProjectItemGroup[] ItemGroup {
        get {
            return this.itemGroupField;
        }
        set {
            this.itemGroupField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Import")]
    public ProjectImport[] Import {
        get {
            return this.importField;
        }
        set {
            this.importField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("VisualStudio", typeof(ProjectProjectExtensionsVisualStudioFlavorProperties[]), IsNullable=false)]
    [System.Xml.Serialization.XmlArrayItemAttribute("FlavorProperties", typeof(ProjectProjectExtensionsVisualStudioFlavorProperties), IsNullable=false, NestingLevel=1)]
    public ProjectProjectExtensionsVisualStudioFlavorProperties[][][] ProjectExtensions {
        get {
            return this.projectExtensionsField;
        }
        set {
            this.projectExtensionsField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ToolsVersion {
        get {
            return this.toolsVersionField;
        }
        set {
            this.toolsVersionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string DefaultTargets {
        get {
            return this.defaultTargetsField;
        }
        set {
            this.defaultTargetsField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://schemas.microsoft.com/developer/msbuild/2003")]
public partial class ProjectPropertyGroup {
    
    private string debugSymbolsField;
    
    private string debugTypeField;
    
    private string optimizeField;
    
    private string outputPathField;
    
    private string defineConstantsField;
    
    private string noStdLibField;
    
    private string noConfigField;
    
    private string errorReportField;
    
    private string warningLevelField;
    
    private string productVersionField;
    
    private string schemaVersionField;
    
    private string projectGuidField;
    
    private string projectTypeGuidsField;
    
    private string outputTypeField;
    
    private string appDesignerFolderField;
    
    private string rootNamespaceField;
    
    private string assemblyNameField;
    
    private string targetFrameworkVersionField;
    
    private string silverlightApplicationField;
    
    private string supportedCulturesField;
    
    private string xapOutputsField;
    
    private string generateSilverlightManifestField;
    
    private string xapFilenameField;
    
    private string silverlightManifestTemplateField;
    
    private string silverlightAppEntryField;
    
    private string testPageFileNameField;
    
    private string createTestPageField;
    
    private string validateXamlField;
    
    private string throwErrorsInValidationField;
    
    private string fileUpgradeFlagsField;
    
    private string upgradeBackupLocationField;
    
    private string oldToolsVersionField;
    
    private string publishUrlField;
    
    private string installField;
    
    private string installFromField;
    
    private string updateEnabledField;
    
    private string updateModeField;
    
    private string updateIntervalField;
    
    private string updateIntervalUnitsField;
    
    private string updatePeriodicallyField;
    
    private string updateRequiredField;
    
    private string mapFileExtensionsField;
    
    private string applicationRevisionField;
    
    private string applicationVersionField;
    
    private string isWebBootstrapperField;
    
    private string useApplicationTrustField;
    
    private string bootstrapperEnabledField;
    
    private ProjectPropertyGroupConfiguration[] configurationField;
    
    private ProjectPropertyGroupPlatform[] platformField;
    
    private string conditionField;
    
    /// <remarks/>
    public string DebugSymbols {
        get {
            return this.debugSymbolsField;
        }
        set {
            this.debugSymbolsField = value;
        }
    }
    
    /// <remarks/>
    public string DebugType {
        get {
            return this.debugTypeField;
        }
        set {
            this.debugTypeField = value;
        }
    }
    
    /// <remarks/>
    public string Optimize {
        get {
            return this.optimizeField;
        }
        set {
            this.optimizeField = value;
        }
    }
    
    /// <remarks/>
    public string OutputPath {
        get {
            return this.outputPathField;
        }
        set {
            this.outputPathField = value;
        }
    }
    
    /// <remarks/>
    public string DefineConstants {
        get {
            return this.defineConstantsField;
        }
        set {
            this.defineConstantsField = value;
        }
    }
    
    /// <remarks/>
    public string NoStdLib {
        get {
            return this.noStdLibField;
        }
        set {
            this.noStdLibField = value;
        }
    }
    
    /// <remarks/>
    public string NoConfig {
        get {
            return this.noConfigField;
        }
        set {
            this.noConfigField = value;
        }
    }
    
    /// <remarks/>
    public string ErrorReport {
        get {
            return this.errorReportField;
        }
        set {
            this.errorReportField = value;
        }
    }
    
    /// <remarks/>
    public string WarningLevel {
        get {
            return this.warningLevelField;
        }
        set {
            this.warningLevelField = value;
        }
    }
    
    /// <remarks/>
    public string ProductVersion {
        get {
            return this.productVersionField;
        }
        set {
            this.productVersionField = value;
        }
    }
    
    /// <remarks/>
    public string SchemaVersion {
        get {
            return this.schemaVersionField;
        }
        set {
            this.schemaVersionField = value;
        }
    }
    
    /// <remarks/>
    public string ProjectGuid {
        get {
            return this.projectGuidField;
        }
        set {
            this.projectGuidField = value;
        }
    }
    
    /// <remarks/>
    public string ProjectTypeGuids {
        get {
            return this.projectTypeGuidsField;
        }
        set {
            this.projectTypeGuidsField = value;
        }
    }
    
    /// <remarks/>
    public string OutputType {
        get {
            return this.outputTypeField;
        }
        set {
            this.outputTypeField = value;
        }
    }
    
    /// <remarks/>
    public string AppDesignerFolder {
        get {
            return this.appDesignerFolderField;
        }
        set {
            this.appDesignerFolderField = value;
        }
    }
    
    /// <remarks/>
    public string RootNamespace {
        get {
            return this.rootNamespaceField;
        }
        set {
            this.rootNamespaceField = value;
        }
    }
    
    /// <remarks/>
    public string AssemblyName {
        get {
            return this.assemblyNameField;
        }
        set {
            this.assemblyNameField = value;
        }
    }
    
    /// <remarks/>
    public string TargetFrameworkVersion {
        get {
            return this.targetFrameworkVersionField;
        }
        set {
            this.targetFrameworkVersionField = value;
        }
    }
    
    /// <remarks/>
    public string SilverlightApplication {
        get {
            return this.silverlightApplicationField;
        }
        set {
            this.silverlightApplicationField = value;
        }
    }
    
    /// <remarks/>
    public string SupportedCultures {
        get {
            return this.supportedCulturesField;
        }
        set {
            this.supportedCulturesField = value;
        }
    }
    
    /// <remarks/>
    public string XapOutputs {
        get {
            return this.xapOutputsField;
        }
        set {
            this.xapOutputsField = value;
        }
    }
    
    /// <remarks/>
    public string GenerateSilverlightManifest {
        get {
            return this.generateSilverlightManifestField;
        }
        set {
            this.generateSilverlightManifestField = value;
        }
    }
    
    /// <remarks/>
    public string XapFilename {
        get {
            return this.xapFilenameField;
        }
        set {
            this.xapFilenameField = value;
        }
    }
    
    /// <remarks/>
    public string SilverlightManifestTemplate {
        get {
            return this.silverlightManifestTemplateField;
        }
        set {
            this.silverlightManifestTemplateField = value;
        }
    }
    
    /// <remarks/>
    public string SilverlightAppEntry {
        get {
            return this.silverlightAppEntryField;
        }
        set {
            this.silverlightAppEntryField = value;
        }
    }
    
    /// <remarks/>
    public string TestPageFileName {
        get {
            return this.testPageFileNameField;
        }
        set {
            this.testPageFileNameField = value;
        }
    }
    
    /// <remarks/>
    public string CreateTestPage {
        get {
            return this.createTestPageField;
        }
        set {
            this.createTestPageField = value;
        }
    }
    
    /// <remarks/>
    public string ValidateXaml {
        get {
            return this.validateXamlField;
        }
        set {
            this.validateXamlField = value;
        }
    }
    
    /// <remarks/>
    public string ThrowErrorsInValidation {
        get {
            return this.throwErrorsInValidationField;
        }
        set {
            this.throwErrorsInValidationField = value;
        }
    }
    
    /// <remarks/>
    public string FileUpgradeFlags {
        get {
            return this.fileUpgradeFlagsField;
        }
        set {
            this.fileUpgradeFlagsField = value;
        }
    }
    
    /// <remarks/>
    public string UpgradeBackupLocation {
        get {
            return this.upgradeBackupLocationField;
        }
        set {
            this.upgradeBackupLocationField = value;
        }
    }
    
    /// <remarks/>
    public string OldToolsVersion {
        get {
            return this.oldToolsVersionField;
        }
        set {
            this.oldToolsVersionField = value;
        }
    }
    
    /// <remarks/>
    public string PublishUrl {
        get {
            return this.publishUrlField;
        }
        set {
            this.publishUrlField = value;
        }
    }
    
    /// <remarks/>
    public string Install {
        get {
            return this.installField;
        }
        set {
            this.installField = value;
        }
    }
    
    /// <remarks/>
    public string InstallFrom {
        get {
            return this.installFromField;
        }
        set {
            this.installFromField = value;
        }
    }
    
    /// <remarks/>
    public string UpdateEnabled {
        get {
            return this.updateEnabledField;
        }
        set {
            this.updateEnabledField = value;
        }
    }
    
    /// <remarks/>
    public string UpdateMode {
        get {
            return this.updateModeField;
        }
        set {
            this.updateModeField = value;
        }
    }
    
    /// <remarks/>
    public string UpdateInterval {
        get {
            return this.updateIntervalField;
        }
        set {
            this.updateIntervalField = value;
        }
    }
    
    /// <remarks/>
    public string UpdateIntervalUnits {
        get {
            return this.updateIntervalUnitsField;
        }
        set {
            this.updateIntervalUnitsField = value;
        }
    }
    
    /// <remarks/>
    public string UpdatePeriodically {
        get {
            return this.updatePeriodicallyField;
        }
        set {
            this.updatePeriodicallyField = value;
        }
    }
    
    /// <remarks/>
    public string UpdateRequired {
        get {
            return this.updateRequiredField;
        }
        set {
            this.updateRequiredField = value;
        }
    }
    
    /// <remarks/>
    public string MapFileExtensions {
        get {
            return this.mapFileExtensionsField;
        }
        set {
            this.mapFileExtensionsField = value;
        }
    }
    
    /// <remarks/>
    public string ApplicationRevision {
        get {
            return this.applicationRevisionField;
        }
        set {
            this.applicationRevisionField = value;
        }
    }
    
    /// <remarks/>
    public string ApplicationVersion {
        get {
            return this.applicationVersionField;
        }
        set {
            this.applicationVersionField = value;
        }
    }
    
    /// <remarks/>
    public string IsWebBootstrapper {
        get {
            return this.isWebBootstrapperField;
        }
        set {
            this.isWebBootstrapperField = value;
        }
    }
    
    /// <remarks/>
    public string UseApplicationTrust {
        get {
            return this.useApplicationTrustField;
        }
        set {
            this.useApplicationTrustField = value;
        }
    }
    
    /// <remarks/>
    public string BootstrapperEnabled {
        get {
            return this.bootstrapperEnabledField;
        }
        set {
            this.bootstrapperEnabledField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Configuration", IsNullable=true)]
    public ProjectPropertyGroupConfiguration[] Configuration {
        get {
            return this.configurationField;
        }
        set {
            this.configurationField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Platform", IsNullable=true)]
    public ProjectPropertyGroupPlatform[] Platform {
        get {
            return this.platformField;
        }
        set {
            this.platformField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Condition {
        get {
            return this.conditionField;
        }
        set {
            this.conditionField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://schemas.microsoft.com/developer/msbuild/2003")]
public partial class ProjectPropertyGroupConfiguration {
    
    private string conditionField;
    
    private string valueField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Condition {
        get {
            return this.conditionField;
        }
        set {
            this.conditionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value {
        get {
            return this.valueField;
        }
        set {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://schemas.microsoft.com/developer/msbuild/2003")]
public partial class ProjectPropertyGroupPlatform {
    
    private string conditionField;
    
    private string valueField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Condition {
        get {
            return this.conditionField;
        }
        set {
            this.conditionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value {
        get {
            return this.valueField;
        }
        set {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://schemas.microsoft.com/developer/msbuild/2003")]
public partial class ProjectItemGroup {
    
    private ProjectItemGroupContent[] contentField;
    
    private ProjectItemGroupNone[] noneField;
    
    private ProjectItemGroupApplicationDefinition[] applicationDefinitionField;
    
    private ProjectItemGroupPage[] pageField;
    
    private ProjectItemGroupCompile[] compileField;
    
    private ProjectItemGroupReference[] referenceField;
    
    private ProjectItemGroupBootstrapperPackage[] bootstrapperPackageField;
    
    private ProjectItemGroupProjectReference[] projectReferenceField;
    
    private ProjectItemGroupFolder[] folderField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Content")]
    public ProjectItemGroupContent[] Content {
        get {
            return this.contentField;
        }
        set {
            this.contentField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("None")]
    public ProjectItemGroupNone[] None {
        get {
            return this.noneField;
        }
        set {
            this.noneField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ApplicationDefinition")]
    public ProjectItemGroupApplicationDefinition[] ApplicationDefinition {
        get {
            return this.applicationDefinitionField;
        }
        set {
            this.applicationDefinitionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Page")]
    public ProjectItemGroupPage[] Page {
        get {
            return this.pageField;
        }
        set {
            this.pageField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Compile")]
    public ProjectItemGroupCompile[] Compile {
        get {
            return this.compileField;
        }
        set {
            this.compileField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Reference")]
    public ProjectItemGroupReference[] Reference {
        get {
            return this.referenceField;
        }
        set {
            this.referenceField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("BootstrapperPackage")]
    public ProjectItemGroupBootstrapperPackage[] BootstrapperPackage {
        get {
            return this.bootstrapperPackageField;
        }
        set {
            this.bootstrapperPackageField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ProjectReference")]
    public ProjectItemGroupProjectReference[] ProjectReference {
        get {
            return this.projectReferenceField;
        }
        set {
            this.projectReferenceField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Folder")]
    public ProjectItemGroupFolder[] Folder {
        get {
            return this.folderField;
        }
        set {
            this.folderField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://schemas.microsoft.com/developer/msbuild/2003")]
public partial class ProjectItemGroupContent {
    
    private string includeField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Include {
        get {
            return this.includeField;
        }
        set {
            this.includeField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://schemas.microsoft.com/developer/msbuild/2003")]
public partial class ProjectItemGroupNone {
    
    private string includeField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Include {
        get {
            return this.includeField;
        }
        set {
            this.includeField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://schemas.microsoft.com/developer/msbuild/2003")]
public partial class ProjectItemGroupApplicationDefinition {
    
    private string generatorField;
    
    private string subTypeField;
    
    private string includeField;
    
    /// <remarks/>
    public string Generator {
        get {
            return this.generatorField;
        }
        set {
            this.generatorField = value;
        }
    }
    
    /// <remarks/>
    public string SubType {
        get {
            return this.subTypeField;
        }
        set {
            this.subTypeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Include {
        get {
            return this.includeField;
        }
        set {
            this.includeField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://schemas.microsoft.com/developer/msbuild/2003")]
public partial class ProjectItemGroupPage {
    
    private string generatorField;
    
    private string subTypeField;
    
    private string includeField;
    
    /// <remarks/>
    public string Generator {
        get {
            return this.generatorField;
        }
        set {
            this.generatorField = value;
        }
    }
    
    /// <remarks/>
    public string SubType {
        get {
            return this.subTypeField;
        }
        set {
            this.subTypeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Include {
        get {
            return this.includeField;
        }
        set {
            this.includeField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://schemas.microsoft.com/developer/msbuild/2003")]
public partial class ProjectItemGroupCompile {
    
    private string linkField;
    
    private string dependentUponField;
    
    private string includeField;
    
    /// <remarks/>
    public string Link {
        get {
            return this.linkField;
        }
        set {
            this.linkField = value;
        }
    }
    
    /// <remarks/>
    public string DependentUpon {
        get {
            return this.dependentUponField;
        }
        set {
            this.dependentUponField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Include {
        get {
            return this.includeField;
        }
        set {
            this.includeField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://schemas.microsoft.com/developer/msbuild/2003")]
public partial class ProjectItemGroupReference {
    
    private string includeField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Include {
        get {
            return this.includeField;
        }
        set {
            this.includeField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://schemas.microsoft.com/developer/msbuild/2003")]
public partial class ProjectItemGroupBootstrapperPackage {
    
    private string visibleField;
    
    private string productNameField;
    
    private string installField;
    
    private string includeField;
    
    /// <remarks/>
    public string Visible {
        get {
            return this.visibleField;
        }
        set {
            this.visibleField = value;
        }
    }
    
    /// <remarks/>
    public string ProductName {
        get {
            return this.productNameField;
        }
        set {
            this.productNameField = value;
        }
    }
    
    /// <remarks/>
    public string Install {
        get {
            return this.installField;
        }
        set {
            this.installField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Include {
        get {
            return this.includeField;
        }
        set {
            this.includeField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://schemas.microsoft.com/developer/msbuild/2003")]
public partial class ProjectItemGroupProjectReference {
    
    private string projectField;
    
    private string nameField;
    
    private string includeField;
    
    /// <remarks/>
    public string Project {
        get {
            return this.projectField;
        }
        set {
            this.projectField = value;
        }
    }
    
    /// <remarks/>
    public string Name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Include {
        get {
            return this.includeField;
        }
        set {
            this.includeField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://schemas.microsoft.com/developer/msbuild/2003")]
public partial class ProjectItemGroupFolder {
    
    private string includeField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Include {
        get {
            return this.includeField;
        }
        set {
            this.includeField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://schemas.microsoft.com/developer/msbuild/2003")]
public partial class ProjectImport {
    
    private string projectField;
    
    private string conditionField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Project {
        get {
            return this.projectField;
        }
        set {
            this.projectField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Condition {
        get {
            return this.conditionField;
        }
        set {
            this.conditionField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://schemas.microsoft.com/developer/msbuild/2003")]
public partial class ProjectProjectExtensionsVisualStudioFlavorProperties {
    
    private string silverlightProjectPropertiesField;
    
    private string gUIDField;
    
    /// <remarks/>
    public string SilverlightProjectProperties {
        get {
            return this.silverlightProjectPropertiesField;
        }
        set {
            this.silverlightProjectPropertiesField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string GUID {
        get {
            return this.gUIDField;
        }
        set {
            this.gUIDField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://schemas.microsoft.com/developer/msbuild/2003")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="http://schemas.microsoft.com/developer/msbuild/2003", IsNullable=false)]
public partial class NewDataSet {
    
    private ProjectSave[] itemsField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Project")]
    public ProjectSave[] Items {
        get {
            return this.itemsField;
        }
        set {
            this.itemsField = value;
        }
    }
}
