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

namespace GameEngine
{
    partial class Core
    {
        void initializeScoreSystemEventHandlers()
        {
            scoreSystem.OnCorrectAnswer += new Scores.ScoreSystem.OnGradedQuestionEventHandler(scoreSystem_OnCorrectAnswer);
            scoreSystem.OnWrongAnswer += new Scores.ScoreSystem.OnGradedQuestionEventHandler(scoreSystem_OnWrongAnswer);

            scoreSystem.OnScoreChanged += (object o, ScoreSystem.Score score) =>
            {
                if (OnScoreChanged != null)
                    OnScoreChanged(o, score, scoreSystem.TotalScore);
            };

            scoreSystem.OnScoreIncreased += (object o, ScoreSystem.Score score) =>
            {
                if (OnScoreIncreased != null)
                    OnScoreIncreased(o, score, scoreSystem.TotalScore);
            };

            scoreSystem.OnScoreDecreased += (object o, ScoreSystem.Score score) =>
            {
                if (OnScoreDecreased != null)
                    OnScoreDecreased(o, score, scoreSystem.TotalScore);
            };
        }

    }
}
