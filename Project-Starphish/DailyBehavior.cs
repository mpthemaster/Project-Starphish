//Holds information about a behavior.
using System;

namespace GUI
{
    internal class DailyBehavior
    {

        public int SSN { get; set; }

        public string Behavior { get; set; }

        public string Severity { get; set; }

        public DateTime Date { get; set; }

        public string Shift { get; set; }

        public string Staff { get; set; }


        /// <summary>
        /// Creates a new behavior.
        /// </summary>
        /// <param name="SSN">The social security number of the client.</param>
        /// <param name="severity">The severity of the behavior.</param>
        /// <param name="behavior">The behavior that occured.</param>
        /// <param name="date">The date the behavior occured on.</param>
        /// <param name="shift">The shift the behavior occured on.</param>
        /// <param name="staff">The staff member on duty when the behavior occured.</param>
        public DailyBehavior(int SSN, string Behavior, string severity, DateTime date, string shift, string staff)
        {

            this.SSN = SSN;
            this.Severity = severity;
            this.Behavior = Behavior;
            this.Date = date;
            this.Shift = shift;
            this.Staff = staff;
        }
    }
}
