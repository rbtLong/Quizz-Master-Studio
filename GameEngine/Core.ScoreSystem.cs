#region License
/*   QuizzMasterStudio Copyright © 2012 Robert Long
 *  
 *   This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *   The full license is also included in the root folder.
 */
#endregion

#region Contact

/* Robert Long
 * Email: rbtLong@live.com
 */

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scores = ScoreSystem;
using ScoreSystem;
using GameEngine.GameQuizzDataTypes;

namespace GameEngine
{
    partial class Core
    {
        Scores.ScoreSystem scoreSystem;
        
        /// <summary>
        /// The number of poblems that the engine has generated
        /// and graded (already used).
        /// </summary>
        public int GradedProblems
        {
            get
            {
                int used = 0;
                foreach (GameProblem itm in qSet.Problems)
                    used += (itm.Score.Count > 0 ? 1 : 0);
                return used;
            }
        }

        /// <summary>
        /// The number of problems that the engine has not yet
        /// graded (unused).
        /// </summary>
        public int UngradedProblems
        {
            get 
            {
                return (qSet.Problems.Count - GradedProblems);
            }
        }

        public int UngradedEnabledQuestions
        {
            get 
            {
                int count = 0;
                foreach (GameProblem itm in qSet.Problems)
                    if (itm.IsEnabled && itm.Score.Count < 1)
                        count++;
                return count;
            }
        }

        public int GradedEnabledQuestions
        {
            get 
            {
                return GetEnabledQuestions - UngradedEnabledQuestions;
            }
        }

        void initializeScoreSystem()
        {
            scoreSystem = new Scores.ScoreSystem();
            initializeScoreSystemEventHandlers();
        }

        void initializeScoreSystem(Scores.IScoreSystemConfiguration ScoreConfig)
        {
            scoreSystem = new Scores.ScoreSystem(ScoreConfig);
            initializeScoreSystemEventHandlers();
        }

        //Eventhandler fired from ScoreSystem after RightAnswer triggered
        void scoreSystem_OnWrongAnswer(object o, Score Amount)
        {
            qSet.Problems[currentQuestionIdx].Score.Add(Amount);
        }

        //Eventhandler fired from ScoreSystem after WrongAnswer triggered
        void scoreSystem_OnCorrectAnswer(object o, Score Amount)
        {
            qSet.Problems[currentQuestionIdx].Score.Add(Amount);
        }

        public void RightAnswer()
        {
            lastQuestionAnswered = DateTime.Now;
            scoreSystem.CorrectAnswer(
                lastQuestionAnswered.Subtract(lastQuestionGenerated),
                lastQuestionAnswered);

            if (OnRightAnswer != null)
                OnRightAnswer(this, qSet.Problems[currentQuestionIdx]);
            if (OnAnswered != null)
                OnAnswered(this, qSet.Problems[currentQuestionIdx]);

            if (checkLastQuestion()
                && OnGameCompleted != null)
                gameCompleted();
        }

        public void WrongAnswer()
        {
            lastQuestionAnswered = DateTime.Now;
            scoreSystem.WrongAnswer(
                lastQuestionGenerated.Subtract(lastQuestionAnswered),
                lastQuestionAnswered);

            if (OnWrongAnswer != null)
                OnWrongAnswer(this, qSet.Problems[currentQuestionIdx]);
            if (OnAnswered != null)
                OnAnswered(this, qSet.Problems[currentQuestionIdx]);

            if (proceedOnWrongAnswer)
                checkLastQuestion();
        }

        public void Grade(string Answer)
        {
            foreach (string itm in CurrentQuestion.Answer)
            {
                if (Answer == itm)
                {
                    RightAnswer();
                    generateUnusedQuestion();
                    return;
                }
            }

            WrongAnswer();
        }

        private bool checkLastQuestion()
        {
            return (UngradedProblems == 0
                || GetEnabledQuestions == 0
                || UngradedEnabledQuestions == 0);

        }

    }
}
