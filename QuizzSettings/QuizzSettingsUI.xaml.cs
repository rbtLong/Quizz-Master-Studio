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
using Microsoft.Win32;

namespace QuizzSettings
{
    /// <summary>
    /// Interaction logic for QuizzSettingsUI.xaml
    /// </summary>
    public partial class QuizzSettingsUI : UserControl
    {
        private Settings setting = new Settings();
        
        #region Public Members
        public Settings Setting
        {
            get { return setting; }
            set
            {
                if (!object.ReferenceEquals(value, setting))
                {
                    setting = value;
                    unregisterEvents();
                    updateForms();
                    registerEvents();
                }
            }
        }

        public CheckBox CbxAnswerButtonAfterInterval
        {
            get { return cbxAnswerButtonAfterInterval; }
        }

        public CheckBox CbxAnswerLimit
        {
            get { return cbxAnswerLimit; }
        }

        public CheckBox CbxAttempts
        {
            get { return cbxAttempts; }
        }

        public CheckBox CbxDeductPointsIntervalPenalty
        {
            get { return cbxDeductPointsIntervalPenalty; }
        }

        public CheckBox CbxEnableShowAnswer
        {
            get { return cbxEnableShowAnswer; }
        }

        public CheckBox CbxIgnoreCharCasing
        {
            get { return cbxIgnoreCharCasing; }
        }

        public CheckBox CbxIgnoreMistakes
        {
            get { return cbxIgnoreMistakes; }
        }

        public CheckBox CbxOnKeyPressed
        {
            get { return cbxOnKeyPressed; }
        }

        public CheckBox CbxPlaySongWhenWrong
        {
            get { return cbxPlaySongWhenWrong; }
        }

        public CheckBox CbxTimedQuestion
        {
            get { return cbxTimedQuestion; }
        }
        #endregion

        public QuizzSettingsUI()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(GameProperties_Loaded);
        }

        public QuizzSettingsUI(Settings variables)
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(GameProperties_Loaded);
            Setting = variables;
        }

        void GameProperties_Loaded(object sender, RoutedEventArgs e)
        {
            registerEvents();
        }

        private void unregisterEvents()
        {
            cbxAnswerButtonAfterInterval.Click -= new RoutedEventHandler(cbxAnswerButtonAfterInterval_Click);
            cbxAnswerLimit.Click -= new RoutedEventHandler(cbxAnswerLimit_Click);
            cbxAttempts.Click += new RoutedEventHandler(cbxAttempts_Click);
            cbxDeductPointsIntervalPenalty.Click -= new RoutedEventHandler(cbxDeductPointsIntervalPenalty_Click);
            cbxEnableShowAnswer.Click -= new RoutedEventHandler(cbxEnableShowAnswer_Click);
            cbxIgnoreCharCasing.Click += new RoutedEventHandler(cbxIgnoreCharCasing_Click);
            cbxOnKeyPressed.Click -= new RoutedEventHandler(cbxOnKeyPressed_Click);
            cbxPlaySongWhenWrong.Click -= new RoutedEventHandler(cbxPlaySongWhenWrong_Click);
            cbxTimedQuestion.Click -= new RoutedEventHandler(cbxTimedQuestion_Click);
            cbxIgnoreMistakes.Click -= new RoutedEventHandler(cbxIgnoreMistakes_Click);

            tbxAnswerLimit.TextChanged -= new TextChangedEventHandler(tbxAnswerLimit_TextChanged);
            tbxDecayTimerInMS.TextChanged -= new TextChangedEventHandler(tbxDecayTimerInMS_TextChanged);
            tbxShowAnswerAttempts.TextChanged -= new TextChangedEventHandler(tbxShowAnswerAttempts_TextChanged);
            tbxShowAnswerTime.TextChanged -= new TextChangedEventHandler(tbxShowAnswerTime_TextChanged);
            tbxTimedPointsDecay.TextChanged -= new TextChangedEventHandler(tbxTimedPointsDecay_TextChanged);
            tbxTimeToAnswerInMS.TextChanged -= new TextChangedEventHandler(tbxTimeToAnswerInMS_TextChanged);
        }

        private void registerEvents()
        {
            cbxAnswerButtonAfterInterval.Click += new RoutedEventHandler(cbxAnswerButtonAfterInterval_Click);
            cbxAnswerLimit.Click += new RoutedEventHandler(cbxAnswerLimit_Click);
            cbxAttempts.Click += new RoutedEventHandler(cbxAttempts_Click);
            cbxDeductPointsIntervalPenalty.Click += new RoutedEventHandler(cbxDeductPointsIntervalPenalty_Click);
            cbxEnableShowAnswer.Click += new RoutedEventHandler(cbxEnableShowAnswer_Click);
            cbxIgnoreCharCasing.Click += new RoutedEventHandler(cbxIgnoreCharCasing_Click);
            cbxOnKeyPressed.Click += new RoutedEventHandler(cbxOnKeyPressed_Click);
            cbxPlaySongWhenWrong.Click += new RoutedEventHandler(cbxPlaySongWhenWrong_Click);
            cbxTimedQuestion.Click += new RoutedEventHandler(cbxTimedQuestion_Click);
            cbxIgnoreMistakes.Click += new RoutedEventHandler(cbxIgnoreMistakes_Click);

            tbxAnswerLimit.TextChanged += new TextChangedEventHandler(tbxAnswerLimit_TextChanged);
            tbxDecayTimerInMS.TextChanged += new TextChangedEventHandler(tbxDecayTimerInMS_TextChanged);
            tbxShowAnswerAttempts.TextChanged += new TextChangedEventHandler(tbxShowAnswerAttempts_TextChanged);
            tbxShowAnswerTime.TextChanged += new TextChangedEventHandler(tbxShowAnswerTime_TextChanged);
            tbxTimedPointsDecay.TextChanged += new TextChangedEventHandler(tbxTimedPointsDecay_TextChanged);
            tbxTimeToAnswerInMS.TextChanged += new TextChangedEventHandler(tbxTimeToAnswerInMS_TextChanged);
        }

        private void updateForms()
        {
            tbxTimeToAnswerInMS.Text = setting.TimedQuestionInterval.TotalMilliseconds.ToString();
            tbxTimedPointsDecay.Text = setting.PointsToDeduct.ToString();
            tbxShowAnswerTime.Text = setting.ShowEnableAnswerButtonOverInterval.TotalMilliseconds.ToString();
            tbxShowAnswerAttempts.Text = setting.AnswerButtonAttempts.ToString();
            tbxDecayTimerInMS.Text = setting.DeductPointsInterval.ToString();
            tbxAnswerLimit.Text = setting.AnswerLimits.ToString();
            cbxTimedQuestion.IsChecked = setting.EnableTimedQuestion;
            cbxPlaySongWhenWrong.IsChecked = setting.EnablePlaySoundWhenWrong;
            cbxOnKeyPressed.IsChecked = setting.VerifyOnKeyPress;
            cbxEnableShowAnswer.IsChecked = setting.EnableShowAnswerButton;
            cbxIgnoreCharCasing.IsChecked = setting.IgnoreAnswerCharacterCasing;
            cbxDeductPointsIntervalPenalty.IsChecked = setting.EnableDeductPointsOverInterval;
            cbxAttempts.IsChecked = setting.EnableAnswerButtonOverAttempts;
            cbxAnswerLimit.IsChecked = setting.EnableAnswerLimits;
            cbxAnswerButtonAfterInterval.IsChecked = setting.EnableShowAnswerButtonOverInterval;
            cbxIgnoreMistakes.IsChecked = setting.IgnoreMistakes;
        }

        void cbxIgnoreMistakes_Click(object sender, RoutedEventArgs e)
        {
            setting.IgnoreMistakes = cbxIgnoreMistakes.IsChecked.Value;
        }

        void tbxTimeToAnswerInMS_TextChanged(object sender, TextChangedEventArgs e)
        {
            setting.TimedQuestionInterval = new TimeSpan(0, 0, 0, 0, int.Parse(tbxTimeToAnswerInMS.Text));
        }

        void tbxTimedPointsDecay_TextChanged(object sender, TextChangedEventArgs e)
        {
            setting.PointsToDeduct = double.Parse(tbxTimedPointsDecay.Text);
        }

        void tbxShowAnswerTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            setting.ShowEnableAnswerButtonOverInterval = new TimeSpan(0, 0, 0, 0, int.Parse(tbxShowAnswerTime.Text));
        }

        void tbxShowAnswerAttempts_TextChanged(object sender, TextChangedEventArgs e)
        {
            setting.AnswerButtonAttempts = int.Parse(tbxShowAnswerAttempts.Text);
        }

        void tbxDecayTimerInMS_TextChanged(object sender, TextChangedEventArgs e)
        {
            setting.DeductPointsInterval = new TimeSpan(0, 0, 0, 0, int.Parse(tbxTimedPointsDecay.Text));
        }

        void tbxAnswerLimit_TextChanged(object sender, TextChangedEventArgs e)
        {
            setting.AnswerLimits = int.Parse(tbxAnswerLimit.Text);
        }

        void cbxTimedQuestion_Click(object sender, RoutedEventArgs e)
        {
            setting.EnableTimedQuestion = cbxTimedQuestion.IsChecked.Value;
        }

        void cbxPlaySongWhenWrong_Click(object sender, RoutedEventArgs e)
        {
            setting.EnablePlaySoundWhenWrong = cbxPlaySongWhenWrong.IsChecked.Value;
        }

        void cbxOnKeyPressed_Click(object sender, RoutedEventArgs e)
        {
            setting.VerifyOnKeyPress = cbxOnKeyPressed.IsChecked.Value;
        }

        void cbxEnableShowAnswer_Click(object sender, RoutedEventArgs e)
        {
            setting.EnableShowAnswerButton = cbxEnableShowAnswer.IsChecked.Value;
            if (setting.EnableShowAnswerButton)
                cbxAttempts.IsChecked =
                cbxAnswerButtonAfterInterval.IsChecked = false;
        }

        void cbxIgnoreCharCasing_Click(object sender, RoutedEventArgs e)
        {
            setting.IgnoreAnswerCharacterCasing = cbxIgnoreCharCasing.IsChecked.Value;
        }

        void cbxDeductPointsIntervalPenalty_Click(object sender, RoutedEventArgs e)
        {
            setting.EnableDeductPointsOverInterval = cbxDeductPointsIntervalPenalty.IsChecked.Value;
        }

        void cbxAttempts_Click(object sender, RoutedEventArgs e)
        {
            setting.EnableAnswerButtonOverAttempts = cbxAttempts.IsChecked.Value;
            if (setting.EnableAnswerButtonOverAttempts)
                cbxEnableShowAnswer.IsChecked = false;
        }

        void cbxAnswerLimit_Click(object sender, RoutedEventArgs e)
        {
            setting.EnableAnswerLimits = cbxAnswerLimit.IsChecked.Value;
        }

        void cbxAnswerButtonAfterInterval_Click(object sender, RoutedEventArgs e)
        {
            setting.EnableShowAnswerButtonOverInterval = cbxAnswerButtonAfterInterval.IsChecked.Value;
            if (setting.EnableShowAnswerButtonOverInterval)
                cbxEnableShowAnswer.IsChecked = false;
        }

        private void cmbSongPath_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSongPath.SelectedIndex == cmbSongPath.Items.Count - 1)
            {
                OpenFileDialog oFi = new OpenFileDialog();
                oFi.Multiselect = false;
                oFi.Filter = "Sound Media Files | *.wav; *.mp3; *.aac; *.aiff; *.wma | All Files | *.*";
                bool? dial = oFi.ShowDialog();
                if (dial.HasValue && dial.Value && oFi.CheckFileExists)
                {
                    cmbSongPath.Items.Insert(0, oFi.FileName);
                    cmbSongPath.SelectedIndex = 0;
                }
            }
        }
    }
}
