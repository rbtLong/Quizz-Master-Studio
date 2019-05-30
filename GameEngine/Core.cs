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
    public partial class Core
    {
        bool proceedOnWrongAnswer = false;
        GameQuizzSet qSet;

        /// <summary>
        /// Do not set value directly! Use the 'PlayState'
        /// property.
        /// </summary>
        private GameState playState;
        private GameState PlayState
        {
            get { return playState; }
            set 
            {
                if (playState != value)
                {
                    playState = value;
                    if (OnGameStateChanged != null)
                        OnGameStateChanged(this, playState);

                    switch (value)
                    {
                        case GameState.Completed:
                            if (OnGameCompleted != null)
                                OnGameCompleted(this, new EventArgs());
                            break;
                    }
                }

            }
        }

        public int GetEnabledQuestions
        {
            get
            {
                int enabled = 0;
                foreach (GameProblem itm in qSet.Problems)
                    if (itm.IsEnabled)
                        enabled++;
                return enabled;
            }
        }

        public int DisabledQuestions
        {
            get 
            {
                int disabled = 0;
                foreach (GameProblem itm in qSet.Problems)
                    if(!itm.IsEnabled)
                        disabled++;
                return disabled; 
            }
        }

        public GameState CurrentGameState
        {
            get { return playState; }
        }

        public bool ProceedOnWrongAnswer
        {
            get { return proceedOnWrongAnswer; }
            set { proceedOnWrongAnswer = value; }
        }

        public Core(GameQuizzSet QuizSet)
        {
            PlayState = GameState.NeverStarted;
            qSet = QuizSet;
            initializeScoreSystem();
            initializeQuestionGenerator();
            initializeTime();
        }

        public Core(GameQuizzSet QuizSet,
            ScoreSystem.IScoreSystemConfiguration ScoreConfig)
            : this(QuizSet)
        {
            initializeScoreSystem(ScoreConfig);
        }

    }
}
