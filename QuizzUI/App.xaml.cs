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
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;
using UserProfile;
using GameEngine.GameQuizzDataTypes;

namespace QuizzUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Profile User = null;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                MessageBox.Show(e.Args[0]);
                foreach (string itm in e.Args)
                {
                    if (itm.IndexOf("--load ") > -1)
                    {
                        string dest = itm.Split(' ')[1].Replace("\"", "");
                        if (File.Exists(dest))
                        {
                            GameQuizzSet gm = new GameQuizzSet(new FileInfo(dest));
                            QuizzContainer qc = new QuizzContainer()
                            {
                                QSet = gm
                            };
                            qc.Show();
                        }
                        else
                            MessageBox.Show(string.Format("The path '{0}' is invalid.",
                                dest));

                        return;
                    }
                }
            }
            global::QuizzUI.MainWindow main = new MainWindow();
            main.Show();
        }
    }
}
