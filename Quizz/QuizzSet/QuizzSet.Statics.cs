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
using System.Xml.Linq;
using System.Xml;

namespace Quizz.QuizzSet
{
    partial class QuizzSet
    {
        public static string DefaultLocalPath
        {
            get 
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    @"Quizz Master Studio\QuizzSets\");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }

        public static string FullFilePath(string FileName)
        {
            return System.IO.Path.Combine(DefaultLocalPath, FileName+".qzm");
        }

        public static string[] GetQuizzFilePaths(string Path)
        {
            string fullPath = System.IO.Path.GetFullPath(Path);
            return Directory.GetFiles(fullPath, "*.qzm", SearchOption.AllDirectories);
        }

        public static IEnumerable<QuizzSet> GetQuizzSet(string Path)
        {
            string[] paths = GetQuizzFilePaths(Path);
            List<QuizzSet> sets = new List<QuizzSet>(paths.Length);
            foreach(string itm in paths)
            {
                try
                {
                    sets.Add(new QuizzSet(new FileInfo(itm)));
                }
                catch (IOException)
                {
                    throw new Exception("An error occurred while trying to access the file: \"" + itm + "\". (Perhaps it is being used by another application?)");
                }
                catch (XmlException exception2)
                {
                    throw new Exception(exception2.Message);
                }
            }

            return sets;
        }
    }
}
