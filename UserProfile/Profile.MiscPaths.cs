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
using System.IO;

namespace UserProfile
{
    partial class Profile
    {

        //public static FileInfo[] GetAllQuizzPreferences
        //{
        //    get
        //    {
        //        string[] paths = Directory.GetFiles(DefaultPreferencesLocation,
        //            "Quizz.qmp",
        //            SearchOption.AllDirectories);
        //        FileInfo[] fi = new FileInfo[paths.Length];
        //        for (int i = 0; i < paths.Length; ++i)
        //            fi[i] = new FileInfo(paths[i]);

        //        return fi;
        //    }
        //}

        public string DefaultPreferencesLocation
        {
            get
            {
                string path = Path.Combine(DefaultUserFolder.FullName,
                    @"QuizzPreferences\");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                return path;
            }
        }

        public string DefaultGameResultLocation
        {
            get
            {
                string path = Path.Combine(DefaultUserFolder.FullName,
                    @"GameResult\");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                return path;
            }
        }

    }
}
