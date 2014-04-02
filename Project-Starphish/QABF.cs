using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    public class QABF
    {
        /// <summary>
        /// This QABF's questions and their answers.
        /// </summary>
        public QuestionQABF[] questions = new QuestionQABF[25];

        public bool Completed { get; set; }

        /// <summary>
        /// Sets what behavior this QABF is for and all the default QABF questions.
        /// </summary>
        public QABF()
        {
            Completed = false;

            questions[0] = new QuestionQABF("1. Engages in the behavior to get attention.");
            questions[1] = new QuestionQABF("2. Engages in behavior to escape work or learning situations.");
            questions[2] = new QuestionQABF("3. Engages in the behavior as a form of \"self-stimulation\".");
            questions[3] = new QuestionQABF("4. Engages in the behavior because he/she is in pain.");
            questions[4] = new QuestionQABF("5. Engages in the behavior to get access to items such as preferred toys, food, or beverages.");
            questions[5] = new QuestionQABF("6. Engages in the behavior because he/she likes to be reprimanded.");
            questions[6] = new QuestionQABF("7. Engages in the behavior when asked to do something (get dressed, brush teeth, work, etc.).");
            questions[7] = new QuestionQABF("8. Engages in the behavior even if he/she thinks no one is in the room.");
            questions[8] = new QuestionQABF("9. Engages in the behavior more frequently when he/she is ill.");
            questions[9] = new QuestionQABF("10. Engages in the behavior when you take something away from him/her.");
            questions[10] = new QuestionQABF("11. Engages in the behavior to draw attention to himself/herself.");
            questions[11] = new QuestionQABF("12. Engages in the behavior when he/she does not want to do something.");
            questions[12] = new QuestionQABF("13. Engages in the behavior because there is nothing else to do.");
            questions[13] = new QuestionQABF("14. Engages in the behavior when there is something bothering him/her physically.");
            questions[14] = new QuestionQABF("15. Engages in the behavior when you have something that he/she wants.");
            questions[15] = new QuestionQABF("16. Engages in the behavior to try to get a reaction from you.");
            questions[16] = new QuestionQABF("17. Engages in the behavior to try to get people to leave him/her alone.");
            questions[17] = new QuestionQABF("18. Engages in the behavior in a highly repetitive manner, ignoring his/her surroundings.");
            questions[18] = new QuestionQABF("19. Engages in the behavior because he/she is physically uncomfortable.");
            questions[19] = new QuestionQABF("20. Engages in the behavior when a peer has something that he/she wants.");
            questions[20] = new QuestionQABF("21. Does he/she seem to be saying, \"come see me\" or \"look at me\" when engaging in the behavior?");
            questions[21] = new QuestionQABF("22. Does he/she seem to be saying, \"leave me alone\" or \"stop asking me to do this\" when engaging in the behavior?");
            questions[22] = new QuestionQABF("23. Does he/she seem to enjoy the behavior, even if no one is around?");
            questions[23] = new QuestionQABF("24. Does the behavior seem to indicate to you that he/she is not feeling well?");
            questions[24] = new QuestionQABF("25. Does he/she seem to be saying, \"give me that (toy, food, item)\" when engaging in the behavior?");
        }
    }
}