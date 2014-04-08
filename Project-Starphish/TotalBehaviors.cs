//This is the class that will keep track of all of the information about a behavior
using System;

namespace GUI
{
    internal class TotalBehaviors
    {
        public string Behavior { get; set; }

        public string Severity { get; set; }

        public DateTime Date { get; set; }

        public string Shift { get; set; }

        public int shiftTotal { get; set; }

        public string Staff { get; set; }

        /// <summary>
        /// Creates a new behavior.
        /// </summary>  
        /// <param name="behavior">The behavior that occured.</param>
        /// <param name="date">The date the behavior occured on.</param>
        public TotalBehaviors(string Behavior, string Severity, DateTime date, string Shift, int shiftTotal, string Staff)
        {
            this.Behavior = Behavior;
            this.Severity = Severity;
            this.Date = date;
            this.Shift = Shift;
            this.shiftTotal = shiftTotal;
            this.Staff = Staff;
        }
    }
}
