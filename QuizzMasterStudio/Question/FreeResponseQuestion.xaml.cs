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
using Quiz = global::Quizz;
using System.IO;
using Microsoft.Win32;
using QuizzSettings;
using Quizz.StringFreeReponse;
using GameEngine.GameQuizzDataTypes;
using UserQuizzPreferences;

namespace QuizzMasterStudio.Question
{
    /// <summary>
    /// Interaction logic for FreeResponseQuestion.xaml
    /// </summary>
    public partial class FreeResponseQuestion : UserControl
    {
        bool isNew = false;
        bool isSettingsChange = false;
        bool isProbNameChanged = false;
        bool isAnswerChanged = false;
        bool isQuestionChanged = false;
        bool isChanged = false;
        bool isRtfDoc = false;
        GameProblem sfrProb = null;
        ProblemPreferences prefs = null;

        public event CancelClickedEventHandler CancelClicked;
        public delegate void CancelClickedEventHandler(object sender);

        public event ChangesMadeEventHandler ChangesMade;
        public delegate void ChangesMadeEventHandler(object sender);

        public event ApplyClickedEventHandler SaveAsClicked;
        public event ApplyClickedEventHandler AppliedClicked;
        public delegate void ApplyClickedEventHandler(object sender, GameProblem Problem);

        #region Public Properties
        public bool IsSettingsChange
        {
            get { return isSettingsChange; }
            set { isSettingsChange = value; }
        }

        public bool IsNew
        {
            get { return isNew; }
            set 
            { 
                isNew = value;
                if (isNew)
                    cmbEditMode.SelectedIndex = 1;
            }
        }

        public bool IsRtfDoc
        {
            get { return isRtfDoc; }
            set { isRtfDoc = value; }
        }
        public bool AnswerChanged
        {
            get { return isAnswerChanged; }
            set { isAnswerChanged = value; }
        }

        public bool QuestionChanged
        {
            get { return isQuestionChanged; }
            set { isQuestionChanged = value; }
        }

        public bool IsChanged
        {
            get { return isChanged; }
            set 
            {
                bool difference = (isChanged != value);
                isChanged = value;
                btnApply.IsEnabled = isChanged;

                if (ChangesMade != null)
                    ChangesMade(this);
            }
        }

        public GameProblem GameProblem
        {
            get { return sfrProb; }
            set 
            { 
                sfrProb = value;
                cbxProbSettings.IsChecked = sfrProb.Settings.IsEnabled;
                prefs = new ProblemPreferences(App.User, sfrProb.Id, sfrProb.Settings);
                checkEditMode();
            }
        }
        #endregion

        public FreeResponseQuestion()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(FreeResponseQuestion_Loaded);
        }

        public FreeResponseQuestion(GameProblem Prob, bool IsNew)
            : this()
        {
            sfrProb = Prob;
            prefs = new ProblemPreferences(App.User, sfrProb.Id, sfrProb.Settings);
            this.isNew = IsNew;
            this.Loaded += new RoutedEventHandler(FreeResponseQuestion_Loaded);
        }

        void FreeResponseQuestion_Loaded(object sender, RoutedEventArgs e)
        {
            cmbEditMode.SelectionChanged += new SelectionChangedEventHandler(cbxEditMode_SelectionChanged);
            alAnswers.AnswerAdded += new AnswerList.AnswerListAddedEventHandler(alAnswers_AnswerAdded);
            alAnswers.AnswerDeleted += new AnswerList.AnswerListDeletedEventHandler(alAnswers_AnswerDeleted);
            alAnswers.AnswerEdited += new AnswerList.AnswerListEditedEventHandler(alAnswers_AnswerEdited);

            SetRichTextbox();
            ftbQuestion.rtbDoc.TextChanged += new TextChangedEventHandler(rtbDoc_TextChanged);
            tbxProbName.TextChanged += new TextChangedEventHandler(tbxProbName_TextChanged);

            initializeFormCreationHandlers();
            loadSettings();

            if (!IsNew)
                IsChanged = false;
            checkEditMode();
        }

        private void loadSettings()
        {
            gmSettings.Settings = sfrProb.Settings.Clone() as Settings;
        }

        private void initializeFormCreationHandlers()
        {
            gmSettings.FormChanged += new RoutedEventHandler(cbxFormChange);
        }

        private void SetRichTextbox()
        {
            if (sfrProb != null)
            {
                if (sfrProb.Question.IndexOf("{\\rtf1\\") == 0)
                {
                    if (ftbQuestion.rtbDoc.Document == null)
                        ftbQuestion.rtbDoc.Document = new FlowDocument();

                    byte[] buff = new byte[sfrProb.Question.Length];
                    for (int i = 0; i < buff.Length; ++i)
                        buff[i] = Convert.ToByte(sfrProb.Question[i]);

                    TextRange trData = new TextRange(ftbQuestion.rtbDoc.Document.ContentStart,
                        ftbQuestion.rtbDoc.Document.ContentEnd);
                    trData.Load(new MemoryStream(buff, true), DataFormats.Rtf);
                    isRtfDoc = true;
                }
                else
                {
                    FlowDocument doc = new FlowDocument();
                    doc.Blocks.Add(new Paragraph(new Run(sfrProb.Question)));
                    this.ftbQuestion.rtbDoc.Document = doc;
                    isRtfDoc = false;
                }
                this.alAnswers.lbxAnswers.ItemsSource = sfrProb.Answer;

                string pName = string.Empty;
                if (sfrProb.ToString().Length == 0)
                {
                    if (sfrProb == null || sfrProb.Name == string.Empty)
                        pName = isRtfDoc ? "<Unnamed Question>" : sfrProb.Question;
                    else
                        pName = sfrProb.Name;
                }
                else
                    pName = sfrProb.ToString();
                tbxProbName.Text = pName;
            }
        }

        void cbxFormChange(object sender, RoutedEventArgs e)
        {
            IsChanged = isSettingsChange = true;
        }

        void rtbDoc_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsChanged = isQuestionChanged = true;
        }

        public string SaveQuestionFromRTF()
        {
            string destination = string.Empty;
            TextRange tr = new TextRange(ftbQuestion.rtbDoc.Document.ContentStart,
                ftbQuestion.rtbDoc.Document.ContentEnd);
            using (MemoryStream stream = new MemoryStream())
            {
                tr.Save(stream, DataFormats.Rtf);
                stream.Seek(0, SeekOrigin.Begin);

                using (StreamReader reader = new StreamReader(stream))
                {
                    destination = reader.ReadToEnd();
                }
            }
            return destination;
        }

        public void ApplyQuestionChanges()
        {
            sfrProb.Question = SaveQuestionFromRTF();
            isQuestionChanged = false;
        }

        void alAnswers_AnswerEdited(object sender, int Position, string OldAnswer, string NewAnswer)
        {
            List<string> ans = sfrProb.Answer.ToList();
            ans[Position] = NewAnswer;
            alAnswers.lbxAnswers.ItemsSource = ans;
            IsChanged = isAnswerChanged = true;
        }

        void alAnswers_AnswerDeleted(object sender, int Index, string Answer)
        {
            List<string> ans = sfrProb.Answer.ToList();
            ans.RemoveAt(Index);
            alAnswers.lbxAnswers.ItemsSource = ans;
            IsChanged = isAnswerChanged = true;
        }

        void alAnswers_AnswerAdded(object sender, string Answer)
        {
            List<string> ans = sfrProb.Answer.ToList();
            ans.Add(Answer);
            alAnswers.lbxAnswers.ItemsSource = ans;
            IsChanged = isAnswerChanged = true;
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            ApplyChanges();
        }

        public void ApplyChanges()
        {
            if (sfrProb != null)
            {
                if (validateForm(true))
                {
                    if (isQuestionChanged)
                        ApplyQuestionChanges();
                    if (isAnswerChanged)
                        ApplyAnswerChanges();
                    if (isProbNameChanged)
                        ProbNameChange();
                    if (isSettingsChange)
                        ApplySettingsChange();

                    IsChanged = false;

                    if (AppliedClicked != null)
                        AppliedClicked(this, sfrProb);
                }
            }
            else
                MessageBox.Show("Error: No quizzes associated with this document.",
                    "No Quizzes Associated");
        }

        private void ApplySettingsChange()
        {
            switch (cmbEditMode.SelectedIndex)
            {
                case 0:
                    prefs.Settings.CopyFrom(gmSettings.Settings);
                    prefs.SaveToFile();
                    break;

                case 1:
                    sfrProb.Settings.CopyFrom(gmSettings.Settings);
                    break;
            }
            isSettingsChange = false;
        }

        private bool validateForm(bool DisplayMessage)
        {
            string msg = "The following errors must be corrected before continuing:\n",
                   err = msg;

            if (alAnswers.lbxAnswers.Items.Count < 1)
                err += "\t- You must have an Answer entry\n";
            if (tbxProbName.Text.Length < 1)
                err += "\t- You must have a Name for the problem\n";

            if (err != msg)
            {
                if (DisplayMessage)
                    MessageBox.Show(err);

                return false;
            }
            return true;

        }

        private void ProbNameChange()
        {
            sfrProb.Name = tbxProbName.Text;
        }

        public void ApplyAnswerChanges()
        {
            List<string> ans = new List<string>();
            foreach (string itm in alAnswers.lbxAnswers.Items)
                ans.Add(itm);
            sfrProb.Answer = ans.ToArray();
            isAnswerChanged = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (CancelClicked != null)
                CancelClicked(this);
        }

        private void btnSaveOptions_Click(object sender, RoutedEventArgs e)
        {
            if (SaveAsClicked != null)
                SaveAsClicked(this, sfrProb);

            if (sfrProb != null)
            {
                if (SaveAsClicked != null)
                    SaveAsClicked(this, sfrProb);
                IsChanged = false;
            }
            else
                MessageBox.Show("Error: No quizzes associated with this document.",
                    "No Quizzes Associated");
        }

        private void tbxProbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsChanged = isProbNameChanged = true;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            gmSettings.grdSettings.IsEnabled = gmSettings.Settings.IsEnabled = true;
            IsChanged = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            gmSettings.grdSettings.IsEnabled = gmSettings.Settings.IsEnabled = false;
            IsChanged = true;
        }

        private void cbxEditMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool preserveApplyState = isChanged;
            if (cmbEditMode.SelectedIndex > -1)
            {
                //if (isSettingsChange)
                //    switch(MessageBox.Show("Are you sure you want to Discard unsaved settings?",
                //        "Discard Unsaved Settings?", MessageBoxButton.OKCancel))
                //    {
                //        case MessageBoxResult.Cancel:
                //            e.Handled = false;
                //            return;
                //    }

                checkEditMode();
            }
            if (!preserveApplyState)
                IsChanged = false;
        }

        private void checkEditMode()
        {
            switch (cmbEditMode.SelectedIndex)
            {
                case 0:
                    gmSettings.LoadSettings(prefs.Settings);
                    cbxProbSettings.IsChecked = gmSettings.grdSettings.IsEnabled
                        = prefs.Settings.IsEnabled;
                    break;

                case 1:
                    gmSettings.LoadSettings(sfrProb.Settings);
                    cbxProbSettings.IsChecked =
                        gmSettings.grdSettings.IsEnabled = sfrProb.Settings.IsEnabled;
                    break;
            }
        }

    }
}
