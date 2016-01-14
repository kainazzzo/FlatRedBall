﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlatRedBall.Glue.SaveClasses;

using FlatRedBall.Glue.CodeGeneration.CodeBuilder;
using FlatRedBall.IO;

namespace FlatRedBall.Glue.CodeGeneration
{
    public class LoadingScreenCodeGenerator : ElementComponentCodeGenerator
    {
        bool IsLoadingScreen(IElement element)
        {
            ScreenSave throwaway;
            return IsLoadingScreen(element, out throwaway);
        }

        bool IsLoadingScreen(IElement element, out ScreenSave screenSave)
        {
            screenSave = null;

            if (element is ScreenSave)
            {
                ScreenSave asScreenSave = element as ScreenSave;

                return asScreenSave.IsLoadingScreen;
            }
            return false;
        }

        public override ICodeBlock GenerateFields(ICodeBlock codeBlock, IElement element)
        {
            if (IsLoadingScreen(element))
            {
                codeBlock.Line("double mSavedTargetElapedTime;");
            }
            return codeBlock;
        }
        public override CodeBuilder.ICodeBlock GenerateInitialize(CodeBuilder.ICodeBlock codeBlock, SaveClasses.IElement element)
        {
            if(IsLoadingScreen(element))
            {
                codeBlock.Line("mSavedTargetElapedTime = FlatRedBallServices.Game.TargetElapsedTime.TotalSeconds;");
                codeBlock.Line("FlatRedBall.FlatRedBallServices.Game.TargetElapsedTime = TimeSpan.FromSeconds(.1);");

            }
            return codeBlock;
        }

        public override ICodeBlock GenerateActivity(ICodeBlock codeBlock, IElement element)
        {
            if(IsLoadingScreen(element))
            {
                codeBlock.Line("AsyncActivity();");
            }
            return codeBlock;
        }

        public override ICodeBlock GenerateAdditionalMethods(ICodeBlock codeBlock, IElement element)
        {
            if(IsLoadingScreen(element))
            {
                codeBlock
                    .Line("static string mNextScreenToLoad;")
                    .Property("public static string", "NextScreenToLoad")
                        .Get()
                            .Line("return mNextScreenToLoad;")
                        .End()
                        .Set()
                            .Line("mNextScreenToLoad = value;")
                        .End()
                    .End();


                string screenName =
                    FileManager.RemovePath(element.Name);

                

                codeBlock
                    .Function("public static void", "TransitionToScreen", "System.Type screenType")
                        .Line("TransitionToScreen(screenType.FullName);")
                    .End();


                codeBlock
                    .Function("public static void", "TransitionToScreen", "string screenName")
                        .Line("Screen currentScreen = ScreenManager.CurrentScreen;")
                        .Line("currentScreen.IsActivityFinished = true;")
                        .Line("currentScreen.NextScreen = typeof(" + screenName + ").FullName;")
                        .Line("mNextScreenToLoad = screenName;")
                    .End();


                codeBlock
                    .Function("void", "AsyncActivity", "")
                        .Switch("AsyncLoadingState")
                            .Case("FlatRedBall.Screens.AsyncLoadingState.NotStarted")
                                .If("!string.IsNullOrEmpty(mNextScreenToLoad)")
                                    .Line("#if REQUIRES_PRIMARY_THREAD_LOADING")
                                    .If("HasDrawBeenCalled")
                                        .Line("MoveToScreen(mNextScreenToLoad);")
                                    .End()
                                    .Line("#else")
                                    .Line("StartAsyncLoad(mNextScreenToLoad);")
                                    .Line("#endif")
                                .End()
                            .End()
                            .Case("FlatRedBall.Screens.AsyncLoadingState.LoadingScreen")
                            .End()
                            .Case("FlatRedBall.Screens.AsyncLoadingState.Done")
                    // The loading screen can be used to rehydrate.  
                                .Line("ScreenManager.ShouldActivateScreen = false;")
                                .Line("IsActivityFinished = true;")
                            .End()
                        .End()
                    .End();
            }

            return codeBlock;
        }

        public override ICodeBlock GenerateDestroy(ICodeBlock codeBlock, IElement element)
        {
            if (IsLoadingScreen(element))
            {
                codeBlock.Line("FlatRedBall.FlatRedBallServices.Game.TargetElapsedTime = TimeSpan.FromSeconds(mSavedTargetElapedTime);");
            }
            return codeBlock;
        }
    }
}
