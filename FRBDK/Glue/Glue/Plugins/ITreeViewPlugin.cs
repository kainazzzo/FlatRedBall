﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FlatRedBall.Glue.Plugins
{
    public interface ITreeViewPlugin
    {
        void ReactToRightClick(TreeNode rightClickedTreeNode, ContextMenuStrip menuToModify);
    }
}
