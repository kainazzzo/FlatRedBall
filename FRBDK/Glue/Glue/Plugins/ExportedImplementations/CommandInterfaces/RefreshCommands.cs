﻿using System;
using System.Windows.Forms;
using FlatRedBall.Glue.FormHelpers;
using FlatRedBall.Glue.Plugins.ExportedInterfaces.CommandInterfaces;
using FlatRedBall.Glue.SaveClasses;
using Glue;

namespace FlatRedBall.Glue.Plugins.ExportedImplementations.CommandInterfaces
{
    class RefreshCommands : IRefreshCommands
    {
        public void RefreshUiForSelectedElement()
        {
            if (EditorLogic.CurrentElementTreeNode != null)
                MainGlueWindow.Self.BeginInvoke(
                    new EventHandler(delegate { EditorLogic.CurrentElementTreeNode.UpdateReferencedTreeNodes(false); }));
            
        }

        public void RefreshUi(IElement element)
        {
            if (ProjectManager.ProjectBase != null)
            {
                var elementTreeNode = GlueState.Self.Find.ElementTreeNode(element);
                if (elementTreeNode != null)
                {
                    MainGlueWindow.Self.BeginInvoke(new EventHandler(delegate { elementTreeNode.UpdateReferencedTreeNodes(false); }));
                }
                else
                {


                }
            }
        }

        public void RefreshGlobalContent()
        {
            ElementViewWindow.UpdateGlobalContentTreeNodes(false);
        }

        public void RefreshPropertyGrid()
        {
            MainGlueWindow.Self.BeginInvoke(new EventHandler(delegate { MainGlueWindow.Self.PropertyGrid.Refresh(); }));
            PropertyGridHelper.UpdateDisplayedPropertyGridProperties();

        }


        public void RefreshSelection()
        {
            if (!ProjectManager.WantsToClose)
            {
                MainGlueWindow.Self.BeginInvoke(new EventHandler(RefreshSelectionInternal));
            }

        }

        private void RefreshSelectionInternal(object sender, EventArgs e)
        {
            // During a reload the CurrentElement may no longer be valid:
            var element = EditorLogic.CurrentElement;
            if (element != null)
            {
                if (EditorLogic.CurrentCustomVariable != null)
                {
                    EditorLogic.CurrentCustomVariable = element.GetCustomVariable(EditorLogic.CurrentCustomVariable.Name);
                }
                else if (EditorLogic.CurrentReferencedFile != null)
                {
                    EditorLogic.CurrentReferencedFile = element.GetReferencedFileSave(EditorLogic.CurrentReferencedFile.Name);
                }
            }
        }



        public void RefreshDirectoryTreeNodes()
        {
            ElementViewWindow.AddDirectoryNodes();
        }
    }
}
