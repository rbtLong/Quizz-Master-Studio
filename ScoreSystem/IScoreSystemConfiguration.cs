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
    public interface IScoreSystemConfiguration
    {
        /// <summary>
        /// The base amount subtracted from the total score when
        /// a wrong answered is triggered.
        /// </summary>
        double WrongPenalty { get; }

        /// <summary>
        /// The base amount added to the total score when correct
        /// answer has been triggered.
        /// </summary>
        double CorrectReward { get; }

        /// <summary>
        /// The marginal percent to decrement for every
        /// consecutive mistake.
        /// </summary>
        double ConsecutiveBonusPenalty { get; }

        /// <summary>
        /// The marginal percent to increment for every
        /// consecutive right answer.
        /// </summary>
        double ConsecutiveBonusReward { get;}

        double RewardMultiplier { get; }

        double PenaltyMultiplier { get; }

        TimeSpan MaxWaitTimeForComboChain { get; }
    }
}
