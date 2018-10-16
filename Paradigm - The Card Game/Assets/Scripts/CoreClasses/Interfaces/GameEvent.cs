using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEvents
{
    public class GameEvent: IPublishable
    {
        public GameEvent()
        {

        }

        public IPublishable ParseData()
        {
            throw new NotImplementedException();
        }
    }
}
