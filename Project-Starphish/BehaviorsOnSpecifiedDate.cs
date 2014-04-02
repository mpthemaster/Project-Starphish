//This class keeps track of how many behaviors occured on a specific day, for use in the totalBehaviors
//Graph
using System;

namespace GUI
{
    internal class BehaviorsOnSpecifiedDate
    {
        public DateTime Date { get; set; }

        public int Occurences { get; set; }

        /// <summary>
        /// Creates a new behavior.
        /// </summary>  
        /// <param name="date">The date the behavior occured on.</param>
        public BehaviorsOnSpecifiedDate(DateTime date)
        {
            this.Date = date;
            Occurences = 0;
        }
    }
}
