﻿using System;
using System.Windows.Forms;

namespace FlatRedBall.Glue.AutomatedGlue
{
    internal static class GlueGui
    {
        #region Fields

        static MenuStrip mMenuStrip;

        #endregion

        #region Properties

        public static MenuStrip MenuStrip
        {
            get
            {
                return mMenuStrip;
            }
        }

        public static bool ShowGui 
        { 
#if TEST
            get { return false; }
            set
            { // do nothing
            }
        
#else
            get;

            set; 
#endif
        }

        #endregion

        static GlueGui()
        {
            ShowGui = true;
        }

        public static void Initialize(MenuStrip menuStrip)
        {
            mMenuStrip = menuStrip;
        }


        public static void ShowMessageBox(string text, string caption)
        {
            if (ShowGui)
            {
                mMenuStrip.Invoke((MethodInvoker)delegate
                {
                    MessageBox.Show(text, caption);
                });
            }
        }

        public static void ShowMessageBox(string text)
        {
            if (ShowGui)
            {
                mMenuStrip.Invoke((MethodInvoker)delegate
                {
                    MessageBox.Show(text);
                });
            }
        }

        public static void ShowException(string text, string caption, Exception ex)
        {
            if (ShowGui)
            {
                mMenuStrip.Invoke((MethodInvoker)delegate
                {
                    // We want to show the exception here so we can diagnose it better.
                    MessageBox.Show(text + "\n\n\nDetails:\n\n" + ex, caption);
                });
            }
            else
            {
                throw new Exception(text, ex);
            }
        }

        public static void ShowWindow(Form form, IWin32Window owner)
        {
            if (ShowGui)
            {
                mMenuStrip.Invoke((MethodInvoker)delegate
                {
                    form.Show(owner);
                });
            }
        }

        public static bool TryShowDialog(Form form, out DialogResult result)
        {
            result = DialogResult.OK;
            if (ShowGui)
            {
                // Can't be invoked async.
                //mMenuStrip.Invoke((MethodInvoker)delegate
                //{
                    result = form.ShowDialog();
                //});
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
