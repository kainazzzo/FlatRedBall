﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FlatRedBall.Resources.WindowsPhone {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FlatRedBall.Resources.WindowsPhone.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to info face=&quot;Arial&quot; size=16 bold=0 italic=0 charset=&quot;&quot; unicode=1 stretchH=100 smooth=1 aa=1 padding=0,0,0,0 spacing=1,1
        ///common lineHeight=16 base=13 scaleW=256 scaleH=256 pages=1 packed=0
        ///page id=0 file=&quot;defaultFont_00.tga&quot;
        ///chars count=319
        ///char id=32   x=0     y=0     width=1     height=0     xoffset=0     yoffset=16    xadvance=4     page=0  chnl=0 
        ///char id=33   x=227   y=74    width=1     height=10    xoffset=1     yoffset=3     xadvance=3     page=0  chnl=0 
        ///char id=34   x=54    y=97    width=3     h [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string defaultFont {
            get {
                return ResourceManager.GetString("defaultFont", resourceCulture);
            }
        }
        
        internal static byte[] defaultText {
            get {
                object obj = ResourceManager.GetObject("defaultText", resourceCulture);
                return ((byte[])(obj));
            }
        }
    }
}
