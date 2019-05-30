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
using UserProfile;
using System.IO;
using System.Xml.Linq;
using LinqXMLTools;

namespace UserQuizzPreferences
{
    public class QuizzPreferences : Preferences
    {
        QuizzSettings.QuizzSettings settings;
        DateTime lastChanged;
        string quizzId;

        #region Public Member
        public string QuizzId
        {
            get { return quizzId; }
            set { quizzId = value; }
        }

        public DateTime LastChanged
        {
            get { return lastChanged; }
            set { lastChanged = value; }
        }

        public QuizzSettings.QuizzSettings Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        //public string DefaultQuizzPreferencesFolder
        //{
        //    get
        //    {
        //        string path = Path.Combine(UserPreferences.DefaultPreferencesLocation,
        //            String.Format("{0}/", quizzId));
        //        if (!Directory.Exists(path))
        //            Directory.CreateDirectory(path);

        //        return path;
        //    }
        //}

        //public string DefaultQuizzPreferencesFile
        //{
        //    get
        //    {
        //        return Path.Combine(DefaultQuizzPreferencesFolder,
        //            "UserPreferences.qmp");
        //    }
        //}
        #endregion

        public QuizzPreferences(Profile User, string QuizzId, 
            QuizzSettings.QuizzSettings Settings)
            : base(User)
        {
            settings = Settings;
            lastChanged = DateTime.Now;
            this.quizzId = QuizzId;

            string fPath = Path.Combine(
                User.DefaultPreferencesLocation, quizzId + ".qsp");

            if (File.Exists(fPath))
            {
                XDocument doc = XDocument.Load(fPath);
                if (doc.Element("QuestionPreferences") != null)
                {
                    XElement uPref = doc.Element("UserPreferences");
                    if (!XMLParser.ParseAttribute(uPref, "LastChanged", ref lastChanged))
                        lastChanged = DateTime.Now;
                    if (uPref.Element("GameSettings") != null)
                        this.settings = new QuizzSettings.QuizzSettings(
                            uPref.Element("GameSettings"));
                }
            }
        }

        public XElement ToXElement()
        {
            return new XElement("QuizzPreferences", 
                new XAttribute("QuizzId", quizzId),
                new XAttribute("LastChanged", DateTime.Now),
                settings.ToXElement());
        }

        public void SaveToFile()
        {
            XDocument doc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                ToXElement());
            doc.Save(Path.Combine(User.DefaultPreferencesLocation, 
                string.Format("{0}.qsp", quizzId)));
        }
    }
}
