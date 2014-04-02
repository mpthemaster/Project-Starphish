using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    public class QuestionQABF
    {
        private string question;

        /// <summary>
        /// The question's text.
        /// </summary>
        public string Question { get { return question; } }

        /// <summary>
        /// The user's answer to this question.
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Gives this QABF question text.
        /// </summary>
        /// <param name="question">The question.</param>
        public QuestionQABF(string question)
        {
            this.question = question;
            Answer = "Doesn't Apply";
        }
    }
}