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
using System.Timers;

namespace GameEngine
{
    partial class Core
    {
        DateTime lastQuestionGenerated;
        DateTime lastQuestionAnswered;

        DateTime gameStarted;
        DateTime gameEnded;

        TimeSpan timeLapsed;
        double clockRefreshRate = 10;
        Timer gameClock;

        #region Public Properties
        public TimeSpan TimeLapsed
        {
            get { return timeLapsed; }
        }

        public DateTime GameStarted
        {
            get { return gameStarted; }
        }

        public DateTime GameEnded
        {
            get { return gameEnded; }
        }

        #endregion

        void initializeTime()
        {
            timeLapsed = new TimeSpan(0);
            gameClock = new Timer(clockRefreshRate);
            gameClock.Enabled = true;
            gameClock.Stop();
            gameClock.Elapsed += (object o, ElapsedEventArgs e) =>
                {
                    timeLapsed = timeLapsed.Add(new TimeSpan(0, 0, 0, 0, 
                        (int)clockRefreshRate));

                    if(OnGameClockTick != null)
                        OnGameClockTick(this, timeLapsed);
                };
        }

        void stopAndRestGameTimer()
        {
            gameClock.Stop();
            timeLapsed = new TimeSpan(0);
        }
    }
}
