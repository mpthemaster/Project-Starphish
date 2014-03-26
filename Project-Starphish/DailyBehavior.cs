//Holds information about a behavior.

namespace GUI
{
    internal class DailyBehavior
    {
        public string Name { get; set; }

        public string Severity { get; set; }

        public string Frequency { get; set; }

        public string Date { get; set; }

        public string Shift { get; set; }

        public string Staff { get; set; }

        /// <summary>
        /// Creates a new behavior.
        /// </summary>
        /// <param name="name">The name of the behavior.</param>
        /// <param name="severity">The severity of the behavior.</param>
        /// <param name="frequency">The frequency of the behavior.</param>
        /// <param name="date">The date the behavior occured on.</param>
        /// <param name="shift">The shift the behavior occured on.</param>
        /// <param name="staff">The staff member on duty when the behavior occured.</param>
        public DailyBehavior(string name, string severity, string frequency, string date, string shift, string staff)
        {
            this.Name = name;
            this.Severity = severity;
            this.Frequency = frequency;
            this.Date = date;
            this.Shift = shift;
            this.Staff = staff;
        }
    }
}