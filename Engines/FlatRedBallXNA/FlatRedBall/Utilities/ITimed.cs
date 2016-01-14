using System;
using System.Collections.Generic;
using System.Text;

namespace FlatRedBall.Utilities
{
    #region XML Docs
    /// <summary>
    /// Interface for an object which has a specified time.
    /// </summary>
    #endregion
    public interface ITimed
    {
        #region XML Docs
        /// <summary>
        /// The time associated with the object.
        /// </summary>
        #endregion
        double Time { get; set;}
    }
}
