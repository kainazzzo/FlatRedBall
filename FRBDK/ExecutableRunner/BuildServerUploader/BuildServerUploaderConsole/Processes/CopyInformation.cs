﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using FlatRedBall.IO;

namespace BuildServerUploaderConsole.Processes
{
    public class CopyInformation
    {
        public string SourceFile
        {
            get;
            set;
        }

        public string DestinationFile
        {
            get;
            set;

        }

        public string Namespace
        {
            get;
            set;
        }

        public static CopyInformation CreateEngineCopy(string fileLocation, string targetDirectory)
        {
            var toReturn = new CopyInformation();
            string engineDirectory = DirectoryHelper.EngineDirectory;

            fileLocation = engineDirectory + fileLocation;
            toReturn.SourceFile = fileLocation;

            string targetLocation = DirectoryHelper.ReleaseDirectory + @"SingleDlls\" + targetDirectory + @"\";

            if (!System.IO.Directory.Exists(targetLocation))
            {
                System.IO.Directory.CreateDirectory(targetLocation);
            }

            string fileName = FileManager.RemovePath(fileLocation);

            toReturn.DestinationFile = targetLocation + fileName;
            return toReturn;
        }

        public static CopyInformation CreateTemplateCopy(string fileLocation, string targetDirectory)
        {
            var toReturn = new CopyInformation();
            string engineDirectory = DirectoryHelper.EngineDirectory;

            fileLocation = engineDirectory + fileLocation;
            toReturn.SourceFile = fileLocation;

            string targetLocation = DirectoryHelper.TemplateDirectory + targetDirectory + @"\";

            if (!System.IO.Directory.Exists(targetLocation))
            {
                System.IO.Directory.CreateDirectory(targetLocation);
            }

            string fileName = FileManager.RemovePath(fileLocation);

            toReturn.DestinationFile = targetLocation + fileName;
            return toReturn;
        }

        public static CopyInformation CreateFrbdkTemplateCopy(string fileLocation, string targetDirectory)
        {
            var toReturn = new CopyInformation();
            string frbdkDirectory = DirectoryHelper.FrbdkDirectory;

            fileLocation = frbdkDirectory + fileLocation;
            toReturn.SourceFile = fileLocation;

            string targetLocation = DirectoryHelper.TemplateDirectory + targetDirectory + @"\";

            if (!System.IO.Directory.Exists(targetLocation))
            {
                System.IO.Directory.CreateDirectory(targetLocation);
            }

            string fileName = FileManager.RemovePath(fileLocation);

            toReturn.DestinationFile = targetLocation + fileName;
            return toReturn;
        }

        public static CopyInformation CreateAddOnCopy(string fileLocation, string targetDirectory, string Namespace)
        {
            var toReturn = new CopyInformation();
            string addOnsDirectory = DirectoryHelper.AddOnsDirectory;

            fileLocation = addOnsDirectory + fileLocation;
            toReturn.SourceFile = fileLocation;

            string targetLocation = DirectoryHelper.TemplateDirectory + targetDirectory + @"\";

            if (!System.IO.Directory.Exists(targetLocation))
            {
                System.IO.Directory.CreateDirectory(targetLocation);
            }

            string fileName = FileManager.RemovePath(fileLocation);

            toReturn.DestinationFile = targetLocation + fileName;
            toReturn.Namespace = Namespace;
            return toReturn;
        }


        public void PerformCopy(IResults results, string message)
        {

            System.IO.File.Copy(SourceFile, DestinationFile, true);

            if(!string.IsNullOrEmpty(Namespace))
            {
                ReplaceNamespace(DestinationFile, Namespace);
            }

            results.WriteMessage(message);

        }

        private void ReplaceNamespace(string codeFile, string newNamespace)
        {
            string fileContents = FileManager.FromFileText(codeFile);

            int indexOfNamespaceStart = fileContents.IndexOf("namespace ") + "namespace ".Length;

            int indexOfEndOfNamespace = fileContents.IndexOf("\r", indexOfNamespaceStart);

            fileContents = fileContents.Remove(indexOfNamespaceStart, indexOfEndOfNamespace - indexOfNamespaceStart);

            fileContents = fileContents.Insert(indexOfNamespaceStart, newNamespace);

            FileManager.SaveText(fileContents, codeFile);
        }

        public static List<CopyInformation> CopyDirectory(string folderLocation, string targetDirectory)
        {
            string frbdkDirectory = DirectoryHelper.FrbdkDirectory;

            folderLocation = frbdkDirectory + folderLocation;

            string targetLocation = DirectoryHelper.TemplateDirectory + targetDirectory + @"\";

            //Create all of the directories
            foreach (var path in Directory.GetDirectories(folderLocation, "*", SearchOption.AllDirectories)
                                          .Select(dirPath => dirPath.Replace(folderLocation, targetLocation))
                                          .Where(path => !Directory.Exists(path)))
            {
                Directory.CreateDirectory(path);
            }

            //Copy all the files

            return (from newPath in Directory.GetFiles(folderLocation, "*.*", SearchOption.AllDirectories)
                    let path = newPath.Replace(folderLocation, targetLocation)
                    select new CopyInformation
                    {
                        SourceFile = newPath,
                        DestinationFile = path
                    }).ToList();
        }

        public override string ToString()
        {
            return SourceFile + " to " + DestinationFile;
        }
    }
}
