using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEvents
{
    /// <summary>
    /// This interface defines anything that would like to be Notified by an object implementing ISubscribable interface with new 
    /// IPublishable data object
    /// </summary>
    public interface INotifiable
    {
        IPublishable CheckNotification(IPublishable data);
    }
}
