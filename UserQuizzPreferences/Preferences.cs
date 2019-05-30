using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserProfile;
using System.IO;

namespace UserQuizzPreferences
{
    public abstract class Preferences
    {
        Profile user;

        public Profile User
        {
            get { return user; }
            set { user = value; }
        }

        public Preferences(Profile User)
        {
            user = User;
        }

        public static string DefaultPreferencesLocation
        {
            get
            {
                string path = Path.Combine(Profile.DefaultFolder,
                    @"QuizzPreferences/");
                if (Directory.Exists(path))
                    Directory.CreateDirectory(path);

                return path;
            }
        }

        public static FileInfo[] GetAllQuizzPreferences
        {
            get
            {
                string[] paths = Directory.GetFiles(DefaultPreferencesLocation,
                    "UserPreferences.qmp",
                    SearchOption.AllDirectories);
                FileInfo[] fi = new FileInfo[paths.Length];
                for (int i = 0; i < paths.Length; ++i)
                    fi[i] = new FileInfo(paths[i]);

                return fi;
            }
        }

    }
}
