﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FlatRedBall.Glue.Controls;
using FlatRedBall.Glue.Elements;
using FlatRedBall.Glue.SaveClasses;
using FlatRedBall.Glue.FormHelpers.PropertyGrids;
using FlatRedBall.Glue.GuiDisplay;

namespace FlatRedBall.Glue.Plugins
{
    public delegate void InitializeTabDelegate(TabControl tabControl);
    public delegate void ReactToFileChangeDelegate(string fileName);
    public delegate void RefreshCurrentElementDelegate();
    public delegate void InitializeMenuDelegate(MenuStrip menuStrip);
    public delegate void ReactToNamedObjectChangedValueDelegate(string changedMember, object oldValue);
    public delegate void AddNewFileOptionsDelegate(NewFileWindow newFileWindow);
    public delegate bool CreateNewFileDelegate(AssetTypeInfo assetTypeInfo, object extraData, string directory, string name, out string resultingName);
    /// <summary>
    /// Delegate raised when the user creates a brand new file.
    /// </summary>
    /// <param name="newFile">The newly-created ReferencedFileSave.</param>
    public delegate void ReactToNewFileDelegate(ReferencedFileSave newFile);
    public delegate void ReactToNewObjectDelegate(NamedObjectSave newNamedObject);
    public delegate bool OpenSolutionDelegate(string solution);
    
    // Not sure why this used to return bool, but the plugin manager doesn't use it for anything:
    //public delegate bool OpenProjectDelegate(string project);
    // Update - turns out that we do need the bool.  This plugin type is raised when the user clicks
    // the menu item to open the project in Visual Studio (or whatever IDE).  The bool value returns whether
    // any plugins handled the opening.  If none do, then Glue will open it by default (probably in Visual Studio).
    //public delegate void OpenProjectDelegate(string project);
    public delegate bool OpenProjectDelegate(string project);

    public delegate void OnOutputDelegate(string output);
    public delegate void OnErrorOutputDelegate(string output);
    public delegate void ReactToChangedPropertyDelegate(string changedMember, object oldValue);
    public delegate void ReactToRightClickDelegate(PropertyGrid rightClickedPropertyGrid, ContextMenu menuToModify);
    public delegate void ReactToStateNameChangeDelegate(IElement element, string oldName, string newName);
    public delegate void ReactToStateRemovedDelegate(IElement element, string stateName);
    public delegate void ReactToItemSelectDelegate(TreeNode selectedTreeNode);
    public delegate void ReactToTreeViewRightClickDelegate(TreeNode rightClickedTreeNode, ContextMenuStrip menuToModify);

    public delegate void AdjustDisplayedScreenDelegate(ScreenSave screenSave, ScreenSavePropertyGridDisplayer displayer);
    public delegate void AdjustDisplayedEntityDelegate(EntitySave entitySave, EntitySavePropertyGridDisplayer displayer);

    public delegate void AdjustDisplayedReferencedFileDelegate(ReferencedFileSave referencedFileSave, ReferencedFileSavePropertyGridDisplayer displayer);
    public delegate void AdjustDisplayedCustomVariableDelegate(CustomVariable customVariable, CustomVariablePropertyGridDisplayer displayer);
    public delegate void AdjustDisplayedNamedObjectDelegate(NamedObjectSave namedObjectSave, NamedObjectPropertyGridDisplayer displayer);

    public delegate bool TryHandleCopyFileDelegate(string sourceFile, string sourceDirectory, string targetFile);

    public delegate bool TryAddContainedObjectsDelegate(string absoluteFile, List<string> availableObjects);

}
