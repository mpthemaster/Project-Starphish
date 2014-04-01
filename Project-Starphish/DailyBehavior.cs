﻿//Holds information about a behavior.
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
