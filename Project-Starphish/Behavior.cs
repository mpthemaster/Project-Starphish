//Holds information about a behavior.
using System.Collections.Generic;

namespace GUI
{
    internal class Behavior
    {
        public string Name { get; set; }

        public string Severity { get; set; }

        public string Frequency { get; set; }

        public Dictionary<string, string> Antecedents;

        /// <summary>
        /// Creates a new behavior.
        /// </summary>
        /// <param name="name">The name of the behavior.</param>
        /// <param name="severity">The severity of the behavior.</param>
        /// <param name="frequency">The frequency of the behavior.</param>
        public Behavior(string name, string severity, string frequency)
        {
            this.Name = name;
            this.Severity = severity;
            this.Frequency = frequency;
            Antecedents = new Dictionary<string, string>();
        }
    }
}