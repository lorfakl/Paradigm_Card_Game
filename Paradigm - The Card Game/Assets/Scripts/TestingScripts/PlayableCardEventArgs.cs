using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopLevelUIEvents
{
    public class PlayableCardEventArgs
    {
        #region Properties
        public List<GameObject> PlayableCards
        {
            get;
            private set;
        }

        #endregion

        public PlayableCardEventArgs()
        {
            PlayableCards = new List<GameObject>();
        }
    }

    public class LocationCountEventArgs
    {
        #region Properties
        public Dictionary<ValidLocations, int> LocationCounts
        {
            get;
            private set;
        }

        #endregion

        public LocationCountEventArgs()
        {
            LocationCounts = new Dictionary<ValidLocations, int>();
        }
    }

    public class LandscapeUpdateEventArgs
    {
        #region Properties
        public string LandscapeInstanceID
        {
            get;
            private set;
        }

        #endregion

        public LandscapeUpdateEventArgs()
        {
       
        }
    }

    public class PhilosopherUpdateEventArgs
    {
        #region Properties
        public string PhilosopherInstanceID
        {
            get;
            private set;
        }

        #endregion

        public PhilosopherUpdateEventArgs()
        {

        }
    }
}
