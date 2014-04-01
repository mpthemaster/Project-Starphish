//Holds information about a behavior.
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
        /// <param name="behavior">The behavior that occured.</param>
        /// <param name="date">The date the behavior occured on.</param>
        public BehaviorsOnSpecifiedDate(DateTime date)
        {
            this.Date = date;
            Occurences = 0;
        }
    }
}
