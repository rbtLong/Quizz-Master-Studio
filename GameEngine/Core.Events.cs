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
using ScrSys = ScoreSystem.ScoreSystem;
using GameEngine.GameQuizzDataTypes;

namespace GameEngine
{
    partial class Core
    {

        public delegate void NewQuestionEventHandler(object o, GameProblem Question,
            int ProblemIdx, DateTime TimeGenerated);
        public event NewQuestionEventHandler OnNewQuestion;

        public delegate void OnAnswerEventHandler(object o, GameProblem Question);
        public event OnAnswerEventHandler OnRightAnswer;
        public event OnAnswerEventHandler OnWrongAnswer;
        public event OnAnswerEventHandler OnAnswered;

        public delegate void GameClockTickEventHandler(object o, TimeSpan ElapsedGameTime);
        public event GameClockTickEventHandler OnGameClockTick;

        public delegate void GameCompletedEventHandler(object o, EventArgs args);
        public event GameCompletedEventHandler OnGameCompleted;
        public delegate void GameStateChangedEventHandler(object o, GameState NewState);
        public event GameStateChangedEventHandler OnGameStateChanged;

        #region ScoreSystem Events Delegation
        public delegate void ScoreChangedEventHandler(object o, ScoreSystem.Score Amount,
            double TotalScore);

        public event ScoreChangedEventHandler OnScoreIncreased;
        public event ScoreChangedEventHandler OnScoreDecreased;
        public event ScoreChangedEventHandler OnScoreChanged;
        #endregion
    }
}
