using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEvents
{
    public delegate void PublishableObjectAdded(IPublishable data);

    /// <summary>
    /// An interface that keeps track of all listeners and sends out data to them when an update occurs
    /// </summary>
    public interface ISubscribable
    {
        event PublishableObjectAdded ListenForPublish;
        void AddPublishableObject(IPublishable data);
    }
}
