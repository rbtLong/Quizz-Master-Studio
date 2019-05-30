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
using LinqXMLTools;

namespace QuizzSettings
{
    public sealed partial class QuizzSettings : Settings
    {
        bool cannotBeChangedByUser = false;
        bool doesNotFinish = false;
        bool doNotRepeatQuestion = false;
        EnumQuestionGenerationMethod generationMethod = EnumQuestionGenerationMethod.LowestPercentFirst;

        bool enableRepeatQuizzUntil = false;
        int repeatQuizzUntilAmount = 3;

        bool enableTimedQuizz = false;
        TimeSpan timedQuizzLength = new TimeSpan(0, 0, 0, 60);

        #region Public Properties
        public bool CannotBeChangedByUser
        {
            get { return cannotBeChangedByUser; }
            set { cannotBeChangedByUser = value; }
        }

        public TimeSpan TimedQuizzLength
        {
            get { return timedQuizzLength; }
            set { timedQuizzLength = value; }
        }

        public bool EnableTimedQuizz
        {
            get { return enableTimedQuizz; }
            set { enableTimedQuizz = value; }
        }

        public bool EnableRepeatQuizzUntil
        {
            get { return enableRepeatQuizzUntil; }
            set { enableRepeatQuizzUntil = value; }
        }

        public int RepeatQuizzUntilAmount
        {
            get { return repeatQuizzUntilAmount; }
            set { repeatQuizzUntilAmount = value; }
        }

        public bool DoesNotFinish
        {
            get { return doesNotFinish; }
            set { doesNotFinish = value; }
        }

        public EnumQuestionGenerationMethod GenerationMethod
        {
            get { return generationMethod; }
            set { generationMethod = value; }
        }

        public bool DoNotRepeatQuestion
        {
            get { return doNotRepeatQuestion; }
            set { doNotRepeatQuestion = value; }
        }
        #endregion

        public QuizzSettings()
        {
            OnToXMLBinding += new BindingEventHandler(QuizzSettings_OnSaveBinding);
        }

        public QuizzSettings(XElement Problem)
        {
            OnToXMLBinding += new BindingEventHandler(QuizzSettings_OnSaveBinding);
            LoadQuizzSetting(Problem);
        }

        void QuizzSettings_OnSaveBinding(object sender, XElement Bindings)
        {
            Bindings.Add(new XAttribute("DoesNotFinish", doesNotFinish));
            Bindings.Add(new XAttribute("DoNotRepeatQuestion", doNotRepeatQuestion));
            Bindings.Add(new XAttribute("QuestionGenerationMethod", generationMethod));
            Bindings.Add(new XAttribute("RepeatQuizzUntil", enableRepeatQuizzUntil));
            Bindings.Add(new XAttribute("RepeatQuizzUntilAmount", repeatQuizzUntilAmount));
            Bindings.Add(new XAttribute("TimedQuizz", enableTimedQuestion));
            Bindings.Add(new XAttribute("TimedQuizzLength", timedQuizzLength.ToString()));
            Bindings.Add(new XAttribute("CannotBeChangedByUser", cannotBeChangedByUser));
        }

        public void LoadQuizzSetting(XElement xElm)
        {
            XElement gSettings = xElm.Element("GameSettings");
            if (gSettings != null)
            {
                ParseSettingsFromXElement(gSettings);
                XMLParser.ParseAttribute(gSettings, "DoesNotFinish", ref doesNotFinish);
                XMLParser.ParseAttribute(gSettings, "DoNotRepeatQuestoin", ref doNotRepeatQuestion);
                XMLParser.ParseAttribute(gSettings, "RepeatQuizzUntil", ref enableRepeatQuizzUntil);
                XMLParser.ParseAttribute(gSettings, "RepeatQuizzUntilAmount", ref repeatQuizzUntilAmount);
                XMLParser.ParseAttribute(gSettings, "TimedQuizz", ref enableTimedQuestion);
                XMLParser.ParseAttribute(gSettings, "TimedQuizzLength", ref timedQuizzLength);
                XMLParser.ParseAttribute(gSettings, "CannotBeChangedByUser", ref cannotBeChangedByUser);

                if(gSettings.Attribute("QuestionGenerationMethod") != null
                    && gSettings.Attribute("QuestionGenerationMethod").Value != null)
                    Enum.Parse(typeof(EnumQuestionGenerationMethod), 
                        gSettings.Attribute("QuestionGenerationMethod").Value); 
            }
        }

    }
}
