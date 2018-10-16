using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEvents
{
    public class GameEventManager : ISubscribable
    {
        private Stack<IPublishable> stackData = new Stack<IPublishable>();
        public event PublishableObjectAdded ListenForPublish;

        public GameEventManager()
        {

        }

        public Stack<IPublishable> DataStack
        {
            get { return stackData; }
        }

        public void AddPublishableObject(IPublishable data)
        {
            stackData.Push(data);
            NotifyListeners(data);
        }

        private void NotifyListeners(IPublishable data)
        {
            if(ListenForPublish != null)
            {
                ListenForPublish(data);
            }
        }


        
    }
}
