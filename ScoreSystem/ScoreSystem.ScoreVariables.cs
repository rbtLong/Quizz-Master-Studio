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

namespace ScoreSystem
{
    partial class ScoreSystem
    {
        List<Score> scores;

        int rightAnswers,
            wrongAnswers,
            consecutiveStreak;

        double totalScore,

               correctReward,
               wrongPenalty,
               
               consecutiveBonusReward,
               consecutiveBonusPenalty,
               
               rewardMultiplier,
               penaltyMultiplier;

        TimeSpan maxWaitTimeForComboChain = new TimeSpan(0, 0, 0, 3, 428),
                 maxWaitTimeBonus         = new TimeSpan(0, 0, 0, 6, 428);

        #region Public Properties
        public TimeSpan MaxWaitTimeForComboChain
        {
            get { return maxWaitTimeForComboChain; }
            set { maxWaitTimeForComboChain = value; }
        }

        /// <summary>
        /// The base amount subtracted from the total score when
        /// a wrong answered is triggered.
        /// </summary>
        public double WrongPenalty
        {
            get { return wrongPenalty; }
            set { wrongPenalty = value; }
        }

        /// <summary>
        /// The base amount added to the total score when correct
        /// answer has been triggered.
        /// </summary>
        public double CorrectReward
        {
            get { return correctReward; }
            set { correctReward = value; }
        }

        /// <summary>
        /// The collection of type Scores that contain their
        /// instantial informations (response time, points being added or
        /// subtracted, ect...).
        /// </summary>
        public List<Score> Scores
        {
            get { return scores; }
            set { scores = value; }
        }

        /// <summary>
        /// The total number of wrong answers in this session.
        /// </summary>
        public int WrongAnswers
        {
            get { return wrongAnswers; }
        }

        /// <summary>
        /// The total correct answers in this session.
        /// </summary>
        public int RightAnswers
        {
            get { return rightAnswers; }
        }

        /// <summary>
        /// The marginal percent to decrement for every
        /// consecutive mistake.
        /// </summary>
        public double ConsecutiveBonusPenalty
        {
            get { return consecutiveBonusPenalty; }
        }

        /// <summary>
        /// The marginal percent to increment for every
        /// consecutive right answer.
        /// </summary>
        public double ConsecutiveBonusReward
        {
            get { return consecutiveBonusReward; }
        }       
        
        /// <summary>
        /// The total score calculated based on right/wrong answers
        /// and the consecutive bonuses.
        /// </summary>
        public double TotalScore
        {
            get { return totalScore; }
        }
        #endregion

        void initializeScoreVariables()
        {
            scores = new List<Score>();

            correctReward = 13.1;
            wrongPenalty = 7.32;

            rightAnswers = wrongAnswers =
                consecutiveStreak = 0;
            totalScore = 0;

            consecutiveBonusReward = .30;
            consecutiveBonusPenalty = 1.68;

            rewardMultiplier = .13;
            penaltyMultiplier = .18;
        }

        void initializeScoreVariables(IScoreSystemConfiguration ScoreConfiguration)
        {
            scores = new List<Score>();
            rightAnswers = wrongAnswers =
                consecutiveStreak = 0;
            totalScore = 0;

            this.wrongPenalty = ScoreConfiguration.WrongPenalty;
            this.correctReward = ScoreConfiguration.CorrectReward;
            this.consecutiveBonusPenalty = ScoreConfiguration.ConsecutiveBonusPenalty;
            this.consecutiveBonusReward = ScoreConfiguration.ConsecutiveBonusReward;
            this.rewardMultiplier = ScoreConfiguration.RewardMultiplier;
            this.penaltyMultiplier = ScoreConfiguration.PenaltyMultiplier;
        }

        void updateScore()
        {
            double oldScore = totalScore;
            totalScore = 0;
            foreach (Score itm in scores)
                totalScore += itm.CurrentScore;

            if (OnScoreChanged != null)
                OnScoreChanged(this, scores[scores.Count - 1]);

            if (oldScore > totalScore)
            {
                if (OnScoreDecreased != null)
                    OnScoreDecreased(this, scores[scores.Count - 1]);
            }
            else
                if (OnScoreIncreased != null)
                    OnScoreIncreased(this, scores[scores.Count - 1]);

        }

        void updateRightWrongAnswers()
        {
            foreach (Score itm in scores)
                if (itm.IsCorrect)
                    ++rightAnswers;
                else
                    ++wrongAnswers;
        }


    }
}
