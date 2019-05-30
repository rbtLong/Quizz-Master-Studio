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

namespace UserProfile
{
    public partial class Profile
    {
        DateTime dateCreated;
        DateTime lastAccessed;
        DateTime currentlyAccessed;
        string id;
        string name;
        Uri iconPath;
        string fileDirectory;
        int logInCount;

        public event SaveCompleteEventHandler SaveComplete;
        public delegate void SaveCompleteEventHandler(object sender, Profile Person);


        public static string DefaultFolder
        {
            get 
            {
                string path = Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.MyDocuments),
                    @"Quizz Master Studio\Users\");
                if (!Directory.Exists(path))
                   Directory.CreateDirectory(path);
                return path;
            } 
        }

        public static string[] GetProfileLocationsFromFolder()
        {
            return Directory.GetFiles(DefaultFolder, "*.upl", SearchOption.AllDirectories); 
        }

        public static Profile[] GetProfilesFromFolder()
        {
            List<Profile> profiles = new List<Profile>();
            foreach(string itm in GetProfileLocationsFromFolder())
                profiles.Add(new Profile(XDocument.Load(itm)));
            return profiles.ToArray();
        }

        public static void DeleteProfile(Profile User)
        {
            if (File.Exists(Path.Combine(User.FileDirectory, User.name + ".upl")))
            {
                File.Delete(Path.Combine(User.FileDirectory, User.name + ".upl"));
                try { Directory.Delete(User.FileDirectory); } //deletes if no other files are left
                catch { }
            }
        }

        public static void DeleteProfileDirectory(Profile User)
        {
            if (Directory.Exists(User.fileDirectory))
                Directory.Delete(User.fileDirectory);
        }

        #region Public Property
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public Uri IconPath
        {
            get { return iconPath; }
            set { iconPath = value; }
        }

        public DirectoryInfo DefaultUserFolder
        {
            get 
            {
                string path = DefaultFolder + @"\" + name;
                DirectoryInfo dir = new DirectoryInfo(path);
                if (!dir.Exists)
                    Directory.CreateDirectory(path);
                return dir; 
            }
        }

        public DateTime CurrentlyAccessed
        {
            get { return currentlyAccessed; }
            set { currentlyAccessed = value; }
        }

        public int LogInCount
        {
            get { return logInCount; }
        }

        public DateTime LastAccessed
        {
            get { return lastAccessed; }
            set { lastAccessed = value; }
        }

        public DateTime DateCreated
        {
            get { return dateCreated; }
        }

        public string Name
        {
            get { return name; }
        }

        public string FileDirectory
        {
            get { return fileDirectory; }
        }
        #endregion

        public Profile(string Name, string FileDirectory, Uri IconPath)
        {
            name = Name;
            fileDirectory = FileDirectory;
            dateCreated = lastAccessed = DateTime.Now;
            this.iconPath = IconPath;
            logInCount = 0;
            if (!Directory.Exists(fileDirectory))
                Directory.CreateDirectory(fileDirectory);
        }

        public Profile(string Name, Uri IconPath)
        {
            name = Name;
            dateCreated = lastAccessed = DateTime.Now;
            logInCount = 0;
            iconPath = IconPath;
            fileDirectory = Path.Combine(DefaultFolder, Name);
            if (!Directory.Exists(fileDirectory))
                Directory.CreateDirectory(fileDirectory);
        }

        public Profile(string Name)
        {
            name = Name;
            dateCreated = lastAccessed = DateTime.Now;
            logInCount = 0;
            iconPath = null;
            fileDirectory = Path.Combine(DefaultFolder, Name);
            if (!Directory.Exists(fileDirectory))
                Directory.CreateDirectory(fileDirectory);
        }

        public Profile(XDocument Profile)
        {
            XElement profile = Profile.Element("UserProfile");
            if (profile != null)
            {
                name = profile.Attribute("Name").Value;
                if (name == null)
                    throw new ProfileExceptions.XMLNoNameException();
                fileDirectory = Path.Combine(DefaultFolder, Name);
                if (fileDirectory == null)
                    throw new ProfileExceptions.XMLNoDirectoryFound();

                currentlyAccessed = DateTime.Now;

                logInCount = int.Parse(profile.Attribute("LogInCount").Value == null
                    ? "0"
                    : profile.Attribute("LogInCount").Value);

                lastAccessed = DateTime.Parse(profile.Attribute("CurrentlyAccessed").Value 
                    ?? profile.Attribute("CurrentlyAccessed").Value);

                dateCreated = DateTime.Parse(profile.Attribute("DateCreated").Value
                    ?? profile.Attribute("DateCreated").Value);

                iconPath = profile.Attribute("IconPath").Value != "-None-"
                    ? new Uri(profile.Attribute("IconPath").Value, UriKind.RelativeOrAbsolute)
                    : null;

                if (profile.Attribute("IdToken") != null
                    && profile.Attribute("IdToken").Value != null)
                    id = profile.Attribute("IdToken").Value;
                else
                    id = (DateTime.Now.GetHashCode().ToString("x") + System.Guid.NewGuid().ToString());

                logInCount++;
            }
            
        }

        public XElement ToXML()
        {
            XElement QuizzEntries = new XElement("QuizzEntries");

            XElement xProfile = new XElement("UserProfile",
                new XAttribute("Name", name),
                new XAttribute("FileDirectory", fileDirectory),
                new XAttribute("CurrentlyAccessed", currentlyAccessed.ToString()),
                new XAttribute("LastAccessed", currentlyAccessed.ToString()),
                new XAttribute("DateCreated", dateCreated.ToString()),
                new XAttribute("LogInCount", logInCount.ToString()),
                new XAttribute("IconPath", iconPath != null 
                    ? iconPath.OriginalString
                    : "-None-"),
                new XAttribute("IdToken", id 
                    ?? (DateTime.Now.GetHashCode().ToString("x") + System.Guid.NewGuid().ToString())),
                QuizzEntries);

            return xProfile;
        }

        public void Save()
        {
            SaveAs(Path.Combine(fileDirectory, name+".upl"));

            if (SaveComplete != null)
                SaveComplete(this, this);
        }

        public void SaveAs(string path)
        {
            XDocument xDoc = new XDocument(
                new XDeclaration("1.0", Encoding.UTF8.ToString(), "yes"),
                ToXML());

            xDoc.Save(path, SaveOptions.None);

            if (SaveComplete != null)
                SaveComplete(this, this);
        }

        public override string ToString()
        {
            return name;
        }
    }
}
