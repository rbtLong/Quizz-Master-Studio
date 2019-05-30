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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuizzSettings;
using Microsoft.Win32;

namespace QuizzMasterStudio.QuizzGame
{
    /// <summary>
    /// Interaction logic for GameSettings.xaml
    /// </summary>
    public partial class GameSettings : UserControl
    {
        public event RoutedEventHandler FormChanged;

        public Settings Settings
        {
            get { return gmProperties.Setting; }
            set { gmProperties.Setting = value; }
        }

        public GameSettings()
        {
            InitializeComponent();
            Settings = new Settings();
            this.Loaded += new RoutedEventHandler(GameSettings_Loaded);
        }

        void GameSettings_Loaded(object sender, RoutedEventArgs e)
        {
            gmProperties.CbxAnswerButtonAfterInterval.Click += new RoutedEventHandler(cbxFormChange);
            gmProperties.CbxAnswerLimit.Click += new RoutedEventHandler(cbxFormChange);
            gmProperties.CbxAttempts.Click += new RoutedEventHandler(cbxFormChange);
            gmProperties.CbxDeductPointsIntervalPenalty.Click += new RoutedEventHandler(cbxFormChange);
            gmProperties.CbxEnableShowAnswer.Click += new RoutedEventHandler(cbxFormChange);
            gmProperties.CbxIgnoreCharCasing.Click += new RoutedEventHandler(cbxFormChange);
            gmProperties.CbxIgnoreMistakes.Click += new RoutedEventHandler(cbxFormChange);
            gmProperties.CbxOnKeyPressed.Click += new RoutedEventHandler(cbxFormChange);
            gmProperties.CbxPlaySongWhenWrong.Click += new RoutedEventHandler(cbxFormChange);
            gmProperties.CbxTimedQuestion.Click += new RoutedEventHandler(cbxFormChange);
        }

        void cbxFormChange(object sender, RoutedEventArgs e)
        {
            if (FormChanged != null)
                FormChanged(this, e);
        }

        public GameSettings(Settings SettingVariables)
            : base()
        {
            gmProperties.Setting = SettingVariables;
        }

        public void AddNewProperty(CheckBox Property)
        {
            Property.Margin = new Thickness(15,10,0,0);
            grdSettings.Children.Insert(0, Property);
        }

        public void LoadSettings(Settings settings)
        {
            gmProperties.Setting = settings;
        }
    }
}
