﻿using System;
using System.Windows.Forms;
using FRBDKUpdater.Actions;
using Ionic.Zip;

namespace FRBDKUpdater
{
    internal static class Program
    {
        static FrmMain mMainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static int Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);


                // If there are 0 args then we look in the user data for this application
                // for the FRBDK file to update
                // If there is one, then we use the argument for the location
                // of the FRBDK settings file or the Runtime settings file - depending on extension
                // If there are two or more, then an action is going to be run.
                if (args.Length == 0)
                {
                    mMainForm = new FrmMain(
                        OfficialPlugins.FrbdkUpdater.FrbdkUpdaterSettings.DefaultSaveLocation);    
                    Application.Run(mMainForm);
                }
                else if (args.Length == 1)
                {
                    mMainForm = new FrmMain(args[0]);

                    Application.Run(mMainForm);
                }
                else
                {
                    switch (args[1])
                    {
                        case "CleanAndZip":
                            try
                            {
                                Messaging.ShowAlerts = Convert.ToBoolean(args[5]);
                                CleanAndZipAction.CleanAndZip(args[0], args[2], args[3], args[4]);
                            }
                            catch (ZipException zipException)
                            {
                                Messaging.AlertError(
                                    "The file " + args[3] +
                                    " seems to be corrupt.  Please try running the updater/installer again.",
                                    zipException);
                                throw new Exception("The file " + args[3] +
                                                    " seems to be corrupt.  Please try running the updater/installer again.  Additional information:\n\n" +
                                                    zipException.Message);
                            }
                            catch (Exception ex)
                            {
                                Messaging.AlertError(@"Unknown error.", ex);
                                throw new Exception(@"Unknown error: \n" + ex);
                            }

                            break;
                        default:

                            string message = "Unknown Action: " + args[1];

                            throw new Exception(message);
                    }
                }
                Logger.Flush(Settings.UserAppPath, false);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
                Messaging.AlertError("Unknown Error", e);
            }

            if (mMainForm == null)
            {
                return 1;
            }
            else if (mMainForm.Succeeded == false)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }
    }
}