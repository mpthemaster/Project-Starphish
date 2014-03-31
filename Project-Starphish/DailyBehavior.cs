//Holds information about a behavior.
using System;

namespace GUI
{
    internal class DailyBehavior
    {
        public string Behavior { get; set; }

        public string Severity { get; set; }

        public DateTime Date { get; set; }

        public string Shift { get; set; }

        /// <summary>
        /// Creates a new behavior.
        /// </summary>  
        /// <param name="severity">The severity of the behavior.</param>
        /// <param name="behavior">The behavior that occured.</param>
        /// <param name="date">The date the behavior occured on.</param>
        /// <param name="shift">The shift the behavior occured on.</param>
        public DailyBehavior(string Behavior, string severity, DateTime date, string shift)
        {
            this.Severity = severity;
            this.Behavior = Behavior;
            this.Date = date;
            this.Shift = shift;
        }
    }
}
