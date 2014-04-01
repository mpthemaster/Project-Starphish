using System;

namespace GUI
{
    internal class BehaviorsOccured
    {
        public string Behavior { get; set; }

        public int Occurences { get; set; }

        /// <summary>
        /// Keeps track of how many times each behavior has occured.
        /// </summary>  
        /// <param name="behavior">The behavior that occured.</param>
        public BehaviorsOccured(string Behavior)
        {
            this.Behavior = Behavior;
            Occurences = 1;
        }
    }
}
