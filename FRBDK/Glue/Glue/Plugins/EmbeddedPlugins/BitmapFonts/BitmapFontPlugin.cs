﻿using EditorObjects.SaveClasses;
using FlatRedBall.Glue.Managers;
using FlatRedBall.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace FlatRedBall.Glue.Plugins.EmbeddedPlugins.BitmapFonts
{
    [Export(typeof(PluginBase))]
    public class BitmapFontPlugin : EmbeddedPlugin
    {

        public override void StartUp()
        {
            this.ReactToLoadedGlux = HandleLoadedGlux;
            this.TryHandleCopyFile = HandleTryHandleCopyFile;
        }

        private bool HandleTryHandleCopyFile(string sourceFile, string sourceDirectory, string targetFile)
        {
            string extension = FileManager.GetExtension(sourceFile);
            bool succeeded = false;

            if (extension == "bmfc")
            {
                System.IO.File.Copy(sourceFile, targetFile, true);
                succeeded = true;
            }
            return succeeded;
        }

        private void HandleLoadedGlux()
        {
            AddIfNecessary(MakeBmfcBuildToolAssociation());
        }


        BuildToolAssociation MakeBmfcBuildToolAssociation()
        {
            BuildToolAssociation toReturn = new BuildToolAssociation();

            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

            if (!folder.EndsWith("\\") && !folder.EndsWith("/"))
            {
                folder += "\\";
            }

            string bmFontAbsolutePath = folder + @"AngelCode\BMFont\bmfont.exe";

            toReturn.BuildTool = bmFontAbsolutePath;
            toReturn.SourceFileType = "bmfc";
            toReturn.DestinationFileType = "fnt";

            toReturn.SourceFileArgumentPrefix = "-c";
            toReturn.DestinationFileArgumentPrefix = "-o";

            return toReturn;

        }

        void AddIfNecessary(BuildToolAssociation association)
        {
            if (System.IO.File.Exists(association.BuildTool))
            {
                var buildToolList = BuildToolAssociationManager.Self.ProjectSpecificBuildTools.BuildToolList;

                bool found = buildToolList.Any(
                    possible => possible.ToString().ToLowerInvariant() == association.ToString().ToLowerInvariant());

                if (!found)
                {
                    BuildToolAssociationManager.Self.ProjectSpecificBuildTools.BuildToolList.Add(association);

                    BuildToolAssociationManager.Self.SaveProjectSpecificBuildTools();

                }
            }
        }
    }
}
