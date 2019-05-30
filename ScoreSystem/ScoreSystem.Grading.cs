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
        double calculateCorrectAnswerScore()
        {
            return Math.Abs(correctReward 
                + (correctReward * consecutiveStreak > 0 ?
                consecutiveBonusReward + (rewardMultiplier *
                consecutiveStreak) : 0));
        }

        double calculateWrongAnswerScore()
        {
            return -Math.Abs(wrongPenalty 
                + (wrongPenalty * -consecutiveStreak > 0 ?
                consecutiveBonusPenalty + (penaltyMultiplier *
                -consecutiveStreak) : 0));
        }

        public void CorrectAnswer(TimeSpan ResponseLength, DateTime AnswerTime)
        {
            consecutiveStreak = consecutiveStreak < 0 ?
                0 : consecutiveStreak + 1;

            double timeBonus = ResponseLength.TotalMilliseconds > maxWaitTimeBonus.TotalMilliseconds
                ? 0
                : ((maxWaitTimeBonus.TotalMilliseconds - ResponseLength.TotalMilliseconds)
                  / maxWaitTimeBonus.TotalMilliseconds);

            Score answer = new Score(true, calculateCorrectAnswerScore() +
                (calculateCorrectAnswerScore() * timeBonus),
                consecutiveStreak, AnswerTime, ResponseLength);
            scores.Add(answer);

            updateRightWrongAnswers();
            updateScore();

            if (this.OnCorrectAnswer != null)
                OnCorrectAnswer(this, answer);

            System.Diagnostics.Debug.WriteLine(consecutiveStreak.ToString());
        }

        public void WrongAnswer(TimeSpan ReponseLength, DateTime AnswerTime)
        {
            consecutiveStreak = consecutiveStreak > 0 ?
                0 : consecutiveStreak - 1;

            Score answer = new Score(false, calculateWrongAnswerScore(),
                consecutiveStreak, AnswerTime, ReponseLength); 
            scores.Add(answer);

            updateRightWrongAnswers();
            updateScore();

            if (this.OnWrongAnswer != null)
                OnWrongAnswer(this, answer);
        }
    }
}
