using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEvents
{
    /// <summary>
    /// This interface is used for object that contain data that will be passed to many listeners
    /// </summary>
    public interface IPublishable
    {
        IPublishable ParseData();
    }
}
