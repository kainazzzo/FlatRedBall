﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FlatRedBall.Content.AnimationChain;
using FlatRedBall.AnimationEditorForms.Preview;
using System.Drawing;
using ToolsUtilities;
using FlatRedBall.Math;

namespace FlatRedBall.AnimationEditorForms
{
    public partial class TreeViewManager
    {
        #region Fields

        TreeView mTreeView;
        TreeNode mGrabbedNode;

        static TreeViewManager mSelf;

        #endregion


        #region Properties

        public static TreeViewManager Self
        {
            get
            {
                if (mSelf == null)
                {
                    mSelf = new TreeViewManager();
                }
                return mSelf;
            }
        }

        #endregion

        #region Events


        public event EventHandler AnimationChainsChange;

        #endregion

        #region Initialize

        public void Initialize(TreeView treeView)
        {
            mTreeView = treeView;
            InitializeRightClick();
        }

        #endregion

        public void RefreshTreeView()
        {
            mTreeView.Invoke((MethodInvoker)delegate()
            {
                AnimationFrameSave selectedAnimationFrame = SelectedState.Self.SelectedFrame;
                AnimationChainSave selectedAnimationChain = SelectedState.Self.SelectedChain;

                mTreeView.Nodes.Clear();


                if (ProjectManager.Self.AnimationChainListSave != null)
                {
                    TreeNode[] nodesToAdd = new TreeNode[ProjectManager.Self.AnimationChainListSave.AnimationChains.Count];

                    int index = 0;
                    foreach (var animationChain in ProjectManager.Self.AnimationChainListSave.AnimationChains)
                    {
                        TreeNode treeNode = new TreeNode();

                        nodesToAdd[index] = treeNode;

                        WithoutEnvokeRefreshTreeNode(treeNode, animationChain);
                        index++;
                    }

                    mTreeView.Nodes.AddRange(nodesToAdd);


                    if (selectedAnimationFrame != null)
                    {
                        SelectedState.Self.SelectedFrame = selectedAnimationFrame;
                    }
                    if (mTreeView.SelectedNode == null && selectedAnimationChain != null)
                    {
                        SelectedState.Self.SelectedChain = selectedAnimationChain;
                    }
                }
            });

        }

        public void RefreshTreeNode(AnimationChainSave animationChain)
        {
            mTreeView.Invoke((MethodInvoker)delegate()
            {
                TreeNode treeNode = GetTreeNodeByTag(animationChain, mTreeView.Nodes);

                if(treeNode == null)
                {
                    treeNode = new TreeNode();
                    mTreeView.Nodes.Add(treeNode);
                }

                WithoutEnvokeRefreshTreeNode(treeNode, animationChain);
            });
        }

        public void RefreshTreeNode(AnimationFrameSave animationFrame)
        {
            mTreeView.Invoke((MethodInvoker)delegate()
            {
                var node = GetTreeNodeFor(animationFrame);

                node.Text = animationFrame.TextureName;
                if (string.IsNullOrEmpty(animationFrame.TextureName))
                {
                    node.Text = "<UNTEXTURED>";
                }
                else
                {
                    var texture = WireframeManager.Self.GetTextureForFrame(animationFrame);

                    int left = MathFunctions.RoundToInt(animationFrame.LeftCoordinate * texture.Width);
                    int top = MathFunctions.RoundToInt(animationFrame.TopCoordinate * texture.Height);


                    node.Text += string.Format(
                        " {0},{1}", left, top);
                }

            });
        }

        

        private void WithoutEnvokeRefreshTreeNode(TreeNode treeNode, AnimationChainSave animationChain)
        {
            treeNode.Nodes.Clear();
            treeNode.Tag = animationChain;
            treeNode.Text = animationChain.Name;

            foreach (var frame in animationChain.Frames)
            {
                TreeNode frameNode = new TreeNode();
                frameNode.Text = frame.TextureName;
                if (string.IsNullOrEmpty(frame.TextureName))
                {
                    frameNode.Text = "<UNTEXTURED>";
                }
                else
                {
                    var texture = WireframeManager.Self.GetTextureForFrame(frame);
                    if (texture != null)
                    {
                        frameNode.Text += string.Format(
                            " {0},{1}", frame.LeftCoordinate * texture.Width, frame.TopCoordinate * texture.Height);
                    }
                }

                frameNode.Tag = frame;

                


                treeNode.Nodes.Add(frameNode);
            }
        }


        public void AfterTreeItemSelect()
        {
            AnimationChainSave lastChain = SelectedState.Self.Snapshot.AnimationChainSave;
            AnimationFrameSave lastFrame = SelectedState.Self.Snapshot.AnimationFrameSave;

            // Refresh the wireframe before the property grid so the property 
            // grid can get the texture information.
            WireframeManager.Self.RefreshAll();
            PropertyGridManager.Self.Refresh();
            SelectedState.Self.TakeSnapshot();

            if (lastChain != SelectedState.Self.SelectedChain)
            {
                // handle new chain selected here
                WireframeManager.Self.HandleAnimationChainChanged();
                PreviewManager.Self.ReactToAnimationChainSelected();
            }
            if (lastFrame != SelectedState.Self.SelectedFrame)
            {
                PreviewManager.Self.ReactToAnimationFrameSelected();
            }


        }

        public TreeNode GetTreeNodeFor(AnimationFrameSave afs)
        {
            if (afs == null)
            {
                return null;
            }
            else
            {
                return GetTreeNodeByTag(afs, mTreeView.Nodes);

            }

        }

        public TreeNode GetTreeNodeFor(AnimationChainSave acs)
        {
            if (acs == null)
            {
                return null;
            }
            else
            {
                return GetTreeNodeByTag(acs, mTreeView.Nodes);

            }

        }

        private TreeNode GetTreeNodeByTag(object tag, TreeNodeCollection treeNodeCollection)
        {
            foreach (TreeNode treeNode in treeNodeCollection)
            {
                if (treeNode.Tag == tag)
                {
                    return treeNode;
                }

                TreeNode foundNode = GetTreeNodeByTag(tag, treeNode.Nodes);
                if (foundNode != null)
                {
                    return foundNode;
                }
            }

            return null;
        }

        void CallAnimationChainsChange()
        {
            if (AnimationChainsChange != null)
            {
                AnimationChainsChange(this, null);
            }
        }

        internal void HandleDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("FileDrop"))
            {
                DragDropFile(sender, e);
            }
            else
            {
                TreeNode targetNode = null;
                TreeView treeView = (TreeView)sender;
                TreeNode movedNode = mGrabbedNode;


                Point pt = new Point(e.X, e.Y);
                pt = mTreeView.PointToClient(pt);
                targetNode = mTreeView.GetNodeAt(pt);

                if (targetNode != null)
                {
                    HandleDrop(movedNode, targetNode);
                }
            }
        }

        private void HandleDrop(TreeNode movedNode, TreeNode targetNode)
        {
            if (targetNode != movedNode)
            {
                AnimationChainSave animationDroppedOn = targetNode.Tag as AnimationChainSave;
                AnimationFrameSave frameDroppedOn = targetNode.Tag as AnimationFrameSave;

                if (frameDroppedOn != null)
                {
                    animationDroppedOn = targetNode.Parent.Tag as AnimationChainSave;
                }

                AnimationFrameSave movedFrame = movedNode.Tag as AnimationFrameSave;

                if (movedFrame != null && animationDroppedOn.Frames.Contains(movedFrame))
                {
                    animationDroppedOn.Frames.Remove(movedFrame);

                    int indexToInsert = 0;
                    if (frameDroppedOn != null)
                    {
                        indexToInsert = animationDroppedOn.Frames.IndexOf(frameDroppedOn) + 1;

                    }

                    animationDroppedOn.Frames.Insert(indexToInsert, movedFrame);

                    RefreshTreeNode(animationDroppedOn);
                }


            }
        }

        private void DragDropFile(object sender, DragEventArgs e)
        {
            
            string fileName = ((string[])e.Data.GetData("FileDrop")).FirstOrDefault();
            
            string extension = FileManager.GetExtension(fileName);
            
            if(extension == "achx")
            {
                HandleDroppedAchxFile(fileName);
            }
                // add more extensions here if needed:
            else if(extension == "png" || extension == "bmp" || extension == "jpg" || extension == "tga")
            {
                TreeView tree = (TreeView)sender;

                Point pt = new Point(e.X, e.Y);
                pt = tree.PointToClient(pt);
                TreeNode targetNode = tree.GetNodeAt(pt);

                if (targetNode != null && !string.IsNullOrEmpty(fileName))
                {
                    string creationReport;


                    bool isValidDrop = targetNode.Tag is AnimationFrameSave ||
                        targetNode.Tag is AnimationChainSave;

                    if (isValidDrop)
                    {
                        string folder;
                        string fileNameCopy = fileName;

                        bool shouldProceed = 
                            AnimationFrameDisplayer.TryAskToMoveFileRelativeToAnimationChainFile(ref fileNameCopy, out folder);

                        if (shouldProceed)
                        {
                            if (targetNode.Tag is AnimationFrameSave)
                            {
                                string message = "Set the AnimationFrame's texture?  Texture name:\n" + fileNameCopy;

                                DialogResult result = MessageBox.Show(message, "Set Texture?", MessageBoxButtons.OKCancel);

                                if (result == DialogResult.OK)
                                {
                                    ((AnimationFrameSave)targetNode.Tag).TextureName = FileManager.MakeRelative(fileNameCopy, folder);
                                    PreviewManager.Self.RefreshAll();
                                    WireframeManager.Self.RefreshAll();
                                    TreeViewManager.Self.RefreshTreeView();
                                    CallAnimationChainsChange();
                                }
                            }
                            else if (targetNode.Tag is AnimationChainSave)
                            {
                                string message = "Set all contained AnimationFrames' texture?  Texture name:\n" + fileNameCopy;

                                DialogResult result = MessageBox.Show(message, "Set all Textures?", MessageBoxButtons.OKCancel);

                                if (result == DialogResult.OK)
                                {
                                    AnimationChainSave animationChainSave = targetNode.Tag as AnimationChainSave;
                                    string relativeFile = FileManager.MakeRelative(fileNameCopy, folder);
                                    foreach (AnimationFrameSave animationFrame in animationChainSave.Frames)
                                    {
                                        animationFrame.TextureName = relativeFile;
                                    }
                                    PreviewManager.Self.RefreshAll();
                                    WireframeManager.Self.RefreshAll();
                                    TreeViewManager.Self.RefreshTreeView();
                                    CallAnimationChainsChange();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void HandleDroppedAchxFile(string fileName)
        {
            MainControl.Self.LoadAnimationChain(fileName);
        }





        internal void HandleKeyPress(KeyPressEventArgs e)
        {
            #region Copy ( (char)3 )

            if (e.KeyChar == (char)3)
            {
                e.Handled = true;

                CopyManager.Self.HandleCopy();
            }

            #endregion

            #region Paste ( (char)22 )

            else if (e.KeyChar == (char)22)
            {
                e.Handled = true;

                // Paste CTRL+V stuff

                CopyManager.Self.HandlePaste();

            }

            #endregion

        }

        internal void HandleKeyDown(KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Delete)
            {
                if (SelectedState.Self.SelectedFrame != null)
                {
                    DeleteAnimationFrameClick(null, null);
                }
                else
                {
                    DeleteAnimationChainClick(null, null);
                }
            }
        }

        internal void GoToPreviousFrame()
        {
            var chain = SelectedState.Self.SelectedChain;
            if (SelectedState.Self.SelectedFrame != null)
            {
                int index = chain.Frames.IndexOf(SelectedState.Self.SelectedFrame);
                index--;
                if (index < 0)
                {
                    index = chain.Frames.Count - 1;
                }

                SelectedState.Self.SelectedFrame = chain.Frames[index];
            }
            else if (chain != null && chain.Frames.Count != 0)
            {
                SelectedState.Self.SelectedFrame = chain.Frames.Last();
            }
        }

        internal void GoToNextFrame()
        {
            var chain = SelectedState.Self.SelectedChain;
            if (SelectedState.Self.SelectedFrame != null)
            {
                int index = chain.Frames.IndexOf(SelectedState.Self.SelectedFrame);
                index++;
                if (index >= chain.Frames.Count)
                {
                    index = 0;
                }

                SelectedState.Self.SelectedFrame = chain.Frames[index];
            }
            else if (chain != null && chain.Frames.Count != 0)
            {
                SelectedState.Self.SelectedFrame = chain.Frames[0];
            }

        }

        internal static void DragOver(object sender, DragEventArgs e)
        {
            TreeView tree = (TreeView)sender;
            Point pt = new Point(e.X, e.Y);
            pt = tree.PointToClient(pt);

            e.Effect = DragDropEffects.Move;
        }

        internal void ItemDrag(object sender, ItemDragEventArgs e)
        {

            // Get the tree.
            TreeView tree = (TreeView)sender;

            // Get the node underneath the mouse.
            TreeNode node = e.Item as TreeNode;
            //tree.SelectedNode = node;

            // Start the drag-and-drop operation with a cloned copy of the node.
            if (node != null)
            {
                mGrabbedNode = node;
                //ElementViewWindow.TreeNodeDraggedOff = node;

                TreeNode targetNode = null;
                //ElementViewWindow.ButtonUsed = e.Button;

                //ElementTreeView_DragDrop(node, DragDropEffects.Move | DragDropEffects.Copy);
                tree.DoDragDrop(node, DragDropEffects.Move | DragDropEffects.Copy);
            }
        }
    }
}
