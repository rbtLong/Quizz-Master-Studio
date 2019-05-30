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
using GameEngine.GameQuizzDataTypes;

namespace GameEngine
{
	partial class Core
	{
        int seed,
            currentQuestionIdx;
        bool repeatQuestions;

        #region Public Properties
        public int CurrentQuestionIdx
        {
            get { return currentQuestionIdx; }
        }

        public GameProblem CurrentQuestion
        {
            get { return qSet.Problems[currentQuestionIdx]; }
        }

        public bool RepeatQuestions
        {
            get { return repeatQuestions; }
            set { repeatQuestions = value; }
        }
        #endregion

        void initializeQuestionGenerator()
        {
            currentQuestionIdx = -1;
            seed = (DateTime.Now.Day * DateTime.Now.Millisecond) % 100;
            repeatQuestions = false;
        }

        /// <summary>
        /// Returns the position of an unused problem in
        /// the question collection.  If all of the problems
        /// have been used, it will return -1.
        /// </summary>
        /// <param name="min">Zero-based minimum position within the 
        /// collection of qSet.Problems.</param>
        /// <param name="max">Zero-based maximum position within the
        /// collection of qSet.Problems.</param>
        /// <returns>The position of the unused problem within the
        /// set; returns -1 if all problems are used.</returns>
        int randomUnusedProblem(int min, int max)
        {
            int problem = -1;

            if (min > max)
                return problem;
            if (min == max)
                return min;
            if (checkLastQuestion())
                return -1;

            do
            {
                problem = new Random(++seed).Next(min, max + 1);
            }
            while (!qSet.Problems[problem].IsEnabled || qSet.Problems[problem].Score.Count > 0);

            return problem;
        }

        public void generateUnusedQuestion()
        {
            int randomPick = randomUnusedProblem(0, qSet.Problems.Count - 1);

            if (PlayState == GameState.Playing)
            {
                if (randomPick == -1)
                {
                    if (checkLastQuestion()
                        && OnGameCompleted != null)
                    {
                        gameCompleted();
                    }
                    return;
                }

                currentQuestionIdx = randomPick;
                lastQuestionGenerated = DateTime.Now;
                if (OnNewQuestion != null)
                    OnNewQuestion(this, qSet.Problems[randomPick], randomPick,
                        lastQuestionGenerated);
            }
        }
	}
}
