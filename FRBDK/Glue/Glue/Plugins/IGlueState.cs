﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlatRedBall.Glue.SaveClasses;

namespace FlatRedBall.Glue.Plugins
{
    public interface IGlueState
    {
        IElement CurrentElement
        {
            get;
        }
    }
}
