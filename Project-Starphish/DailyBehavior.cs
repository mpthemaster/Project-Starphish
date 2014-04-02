//This is the class that will keep track of what behavior happened and when it happened, it will get
//these values directly from the database
using System;

namespace GUI
{
    internal class DailyBehavior
    {
        public string Behavior { get; set; }

        public DateTime Date { get; set; }

        /// <summary>
        /// Creates a new behavior.
        /// </summary>  
        /// <param name="behavior">The behavior that occured.</param>
        /// <param name="date">The date the behavior occured on.</param>
        public DailyBehavior(string Behavior, DateTime date)
        {
            this.Behavior = Behavior;
            this.Date = date;
        }
    }
}
