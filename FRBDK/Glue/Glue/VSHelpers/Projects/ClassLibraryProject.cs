﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.BuildEngine;

namespace FlatRedBall.Glue.VSHelpers.Projects
{
    public class ClassLibraryProject : VisualStudioProject
    {
        public override string NeededVisualStudioVersion
        {
            get { return "10"; }
        }

        public override List<string> LibraryDlls
        {
            get { return new List<string>(); }
        }

        public override string FolderName
        {
            get { return "Libraries"; }
        }

        public override string ProjectId
        {
            get { return "Class Library"; }
        }

        public override string PrecompilerDirective
        {
            get { return ""; }
        }

        public ClassLibraryProject(Project project) : base(project)
        {

        }

        public BuildItem AddCodeBuildItem(string fileName, bool addAsLink, string fileRelativeToThis)
        {
            return base.AddCodeBuildItem(fileName, addAsLink, fileRelativeToThis);
        }
    }
}
