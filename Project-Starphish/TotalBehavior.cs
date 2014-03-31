using System;

namespace GUI
{
    internal class TotalBehavior
    {
        public string Behavior { get; set; }

        public DateTime Date { get; set; }

        public int Occurences { get; set; }

        /// <summary>
        /// Creates a new behavior.
        /// </summary>  
        /// <param name="behavior">The behavior that occured.</param>
        /// <param name="date">The date the behavior occured on.</param>
        public TotalBehavior(string Behavior, DateTime date)
        {
            this.Behavior = Behavior;
            this.Date = date;
            Occurences = 0;
        }
    }
}
