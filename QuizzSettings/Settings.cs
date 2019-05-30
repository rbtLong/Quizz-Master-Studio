using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using LinqXMLTools;

namespace QuizzSettings
{
    public class Settings : ICloneable, IEquatable<Settings>
    {
        protected event BindingEventHandler OnToXMLBinding;
        protected delegate void BindingEventHandler(object sender, XElement Bindings);

        public event SettingsSavingDelegate SettingsSaving;
        public delegate void SettingsSavingDelegate(object sender);

        protected bool isEnabled = false;
        protected bool enableShowAnswerButton = false;

        protected bool ignoreAnswerCharacterCasing = false;
        protected bool verifyOnKeyPress = false;
        protected bool ignoreMistakes = false;
        protected bool answerButtonIsEnabled = true;

        protected bool answerLimitsIsEnabled = false;
        protected int answerLimits = 1;

        protected bool enableTimedQuestion = false;
        protected TimeSpan timedQuestionInterval = new TimeSpan(0, 0, 0, 0, 3000);

        protected bool enableDeductPointsOverInterval = false;
        protected TimeSpan deductPointsInterval = new TimeSpan(0, 0, 0, 0, 1000);
        private double pointsToDeduct = 1;

        protected bool enableShowAnswerButtonOverInterval = false;
        protected TimeSpan showAnswButtonInterval = new TimeSpan(0, 0, 0, 0, 3000);

        protected bool enableAnswerButtonOverAttempts = false;
        private int answerButtonAttempts = 3;

        protected bool enablePlaySoundWhenWrong = false;
        protected string songPath = string.Empty;

        #region Public Properties
        public bool EnableShowAnswerButton
        {
            get { return enableShowAnswerButton; }
            set { enableShowAnswerButton = value; }
        }

        public double PointsToDeduct
        {
            get { return pointsToDeduct; }
            set { pointsToDeduct = value; }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        public int AnswerLimits
        {
            get { return answerLimits; }
            set { answerLimits = value; }
        }

        public TimeSpan DeductPointsInterval
        {
            get { return deductPointsInterval; }
            set { deductPointsInterval = value; }
        }

        public bool EnableDeductPointsOverInterval
        {
            get { return enableDeductPointsOverInterval; }
            set { enableDeductPointsOverInterval = value; }
        }

        public bool EnableTimedQuestion
        {
            get { return enableTimedQuestion; }
            set { enableTimedQuestion = value; }
        }

        public TimeSpan TimedQuestionInterval
        {
            get { return timedQuestionInterval; }
            set { timedQuestionInterval = value; }
        }

        public bool EnableAnswerLimits
        {
            get { return answerLimitsIsEnabled; }
            set { answerLimitsIsEnabled = value; }
        }

        public bool IgnoreAnswerCharacterCasing
        {
            get { return ignoreAnswerCharacterCasing; }
            set { ignoreAnswerCharacterCasing = value; }
        }

        public bool VerifyOnKeyPress
        {
            get { return verifyOnKeyPress; }
            set { verifyOnKeyPress = value; }
        }

        public bool IgnoreMistakes
        {
            get { return ignoreMistakes; }
            set { ignoreMistakes = value; }
        }

        public bool AnswerButtonIsEnabled
        {
            get { return answerButtonIsEnabled; }
            set
            {
                answerButtonIsEnabled = value;
                if (value)
                    EnableAnswerButtonOverAttempts
                        = EnableShowAnswerButtonOverInterval
                        = false;
            }
        }

        public bool EnableShowAnswerButtonOverInterval
        {
            get { return enableShowAnswerButtonOverInterval; }
            set
            {
                enableShowAnswerButtonOverInterval = value;
                if (value)
                    EnableShowAnswerButton = false;
            }
        }

        public TimeSpan ShowEnableAnswerButtonOverInterval
        {
            get { return showAnswButtonInterval; }
            set { showAnswButtonInterval = value; }
        }


        public bool EnableAnswerButtonOverAttempts
        {
            get { return enableAnswerButtonOverAttempts; }
            set
            {
                enableAnswerButtonOverAttempts = value;
                if (value)
                    EnableShowAnswerButton = false;
            }
        }

        public bool EnablePlaySoundWhenWrong
        {
            get { return enablePlaySoundWhenWrong; }
            set { enablePlaySoundWhenWrong = value; }
        }
        public string SongPath
        {
            get { return songPath; }
            set { songPath = value; }
        }


        public int AnswerButtonAttempts
        {
            get { return answerButtonAttempts; }
            set { answerButtonAttempts = value; }
        }
        #endregion

        public XElement ToXElement()
        {
            XElement gSettings = new XElement("GameSettings",
                    new XAttribute("IsEnabled", isEnabled),
                    new XAttribute("IgnoreCharacterCasing", ignoreAnswerCharacterCasing),
                    new XAttribute("IgnoreMistakes", ignoreMistakes),
                    new XAttribute("AnswerButtonIsEnabled", answerButtonIsEnabled),
                    new XAttribute("VerifyOnKeyPress", verifyOnKeyPress),
                    new XAttribute("EnabledAnswerLimits", answerLimitsIsEnabled),
                    new XAttribute("AnswerLimits", answerLimits),
                    new XAttribute("EnabledTimeQuestion", enableTimedQuestion),
                    new XAttribute("TimeQuestionInterval", timedQuestionInterval.ToString()),
                    new XAttribute("DeductPointsInterval", deductPointsInterval.ToString()),
                    new XAttribute("ShowAnswerButtonAttempts", answerButtonAttempts),
                    new XAttribute("EnableShowAnswerButtonAttempts", enableAnswerButtonOverAttempts),
                    new XAttribute("EnableShowAnswerButtonOverInterval", enableShowAnswerButtonOverInterval),
                    new XAttribute("ShowAnswerButtonInterval", showAnswButtonInterval.ToString()),
                    new XAttribute("EnablePlaySoundWhenWrong", enablePlaySoundWhenWrong),
                    new XAttribute("SongPath", songPath),
                    new XAttribute("PointsToDeduct", pointsToDeduct),
                    new XAttribute("EnableDeductPointsOverInterval", enableDeductPointsOverInterval),
                    new XAttribute("EnableShowAnswButton", enableShowAnswerButton));

            if (OnToXMLBinding != null)
                OnToXMLBinding(this, gSettings);

            return gSettings;
        }

        public void ParseSettingsFromXElement(XElement gSettings)
        {
            XMLParser.ParseAttribute(gSettings, "IsEnabled", ref isEnabled);
            XMLParser.ParseAttribute(gSettings, "EnableShowAnswButton", ref enableShowAnswerButton);
            XMLParser.ParseAttribute(gSettings, "IgnoreMistakes", ref ignoreMistakes);
            XMLParser.ParseAttribute(gSettings, "IgnoreCharacterCasing", ref ignoreAnswerCharacterCasing);

            XMLParser.ParseAttribute(gSettings, "AnswerButtonIsEnabled", ref answerButtonIsEnabled);
            XMLParser.ParseAttribute(gSettings, "VerifyOnKeyPress", ref verifyOnKeyPress);

            XMLParser.ParseAttribute(gSettings, "EnabledAnswerLimits", ref answerLimitsIsEnabled);
            XMLParser.ParseAttribute(gSettings, "AnswerLimits", ref answerLimits);

            XMLParser.ParseAttribute(gSettings, "EnabledTimeQuestion", ref enableTimedQuestion);
            XMLParser.ParseAttribute(gSettings, "TimeQuestionInterval", ref timedQuestionInterval);

            XMLParser.ParseAttribute(gSettings, "EnableDeductPointsOverInterval", ref enableDeductPointsOverInterval);
            XMLParser.ParseAttribute(gSettings, "DeductPointsInterval", ref deductPointsInterval);
            XMLParser.ParseAttribute(gSettings, "PointsToDeduct", ref pointsToDeduct);

            XMLParser.ParseAttribute(gSettings, "EnableShowAnswerButtonOverInterval", ref enableShowAnswerButtonOverInterval);
            XMLParser.ParseAttribute(gSettings, "ShowAnswerButtonInterval", ref showAnswButtonInterval);

            XMLParser.ParseAttribute(gSettings, "ShowAnswerButtonAttempts", ref answerButtonAttempts);
            XMLParser.ParseAttribute(gSettings, "EnableShowAnswerButtonAttempts", ref enableAnswerButtonOverAttempts);

            XMLParser.ParseAttribute(gSettings, "EnablePlaySoundWhenWrong", ref enablePlaySoundWhenWrong);
            XMLParser.ParseAttribute(gSettings, "SongPath", ref songPath);
        }

        public object Clone()
        {
            return new Settings()
            {
                IsEnabled = isEnabled,
                AnswerButtonIsEnabled = answerButtonIsEnabled,
                AnswerButtonAttempts = answerButtonAttempts,
                AnswerLimits = answerLimits,
                EnableAnswerLimits = answerLimitsIsEnabled,
                EnableAnswerButtonOverAttempts = enableAnswerButtonOverAttempts,
                EnableDeductPointsOverInterval = enableDeductPointsOverInterval,
                TimedQuestionInterval = new TimeSpan(0,0,0,0, Convert.ToInt32(timedQuestionInterval.TotalMilliseconds)),
                EnableTimedQuestion = enableTimedQuestion,
                IgnoreAnswerCharacterCasing = ignoreAnswerCharacterCasing,
                IgnoreMistakes = ignoreMistakes,
                EnableShowAnswerButtonOverInterval = enableShowAnswerButtonOverInterval,
                ShowEnableAnswerButtonOverInterval = new TimeSpan(0,0,0,0, Convert.ToInt32(showAnswButtonInterval.TotalMilliseconds)),
                EnablePlaySoundWhenWrong = enablePlaySoundWhenWrong,
                SongPath = string.Copy(songPath),
                DeductPointsInterval = new TimeSpan(0,0,0,0, Convert.ToInt32(deductPointsInterval.TotalMilliseconds)),
                VerifyOnKeyPress = verifyOnKeyPress,
                PointsToDeduct = pointsToDeduct,
                EnableShowAnswerButton = EnableShowAnswerButton,
            };
        }

        public bool Equals(Settings other)
        {
            return (this.isEnabled == other.isEnabled
                && this.answerButtonAttempts == other.answerButtonAttempts
                && this.enableDeductPointsOverInterval == other.enableDeductPointsOverInterval
                && this.ignoreMistakes == other.ignoreMistakes
                && this.answerButtonIsEnabled == other.answerButtonIsEnabled
                && this.answerLimits == other.answerLimits
                && this.answerLimitsIsEnabled == other.answerLimitsIsEnabled
                && this.deductPointsInterval == other.deductPointsInterval
                && this.enableAnswerButtonOverAttempts == other.enableAnswerButtonOverAttempts
                && this.showAnswButtonInterval == other.showAnswButtonInterval
                && this.enableShowAnswerButtonOverInterval == other.enableShowAnswerButtonOverInterval
                && this.ignoreAnswerCharacterCasing == other.ignoreAnswerCharacterCasing
                && this.enablePlaySoundWhenWrong == other.enablePlaySoundWhenWrong
                && this.songPath == other.songPath
                && this.timedQuestionInterval == other.timedQuestionInterval
                && this.enableTimedQuestion == other.enableTimedQuestion
                && this.verifyOnKeyPress == other.verifyOnKeyPress
                && this.pointsToDeduct == other.pointsToDeduct
                && this.enableShowAnswerButton == other.enableShowAnswerButton);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator==(Settings first, Settings second)
        {
            if (object.ReferenceEquals(first, second))
                return true;
            if (first == null && second == null)
                return true;
            if (first == null || second == null)
                return false;

            return first.Equals(second);
        }

        public static bool operator !=(Settings first, Settings second)
        {
            return !(first == second);
        }

        public void CopyFrom(Settings other)
        {
            this.isEnabled = other.isEnabled;
            this.answerButtonAttempts = other.answerButtonAttempts;
            this.enableDeductPointsOverInterval = other.enableDeductPointsOverInterval;
            this.ignoreMistakes = other.ignoreMistakes;
            this.answerButtonIsEnabled = other.answerButtonIsEnabled;
            this.answerLimits = other.answerLimits;
            this.answerLimitsIsEnabled = other.answerLimitsIsEnabled;
            this.deductPointsInterval = other.deductPointsInterval;
            this.enableAnswerButtonOverAttempts = other.enableAnswerButtonOverAttempts;
            this.showAnswButtonInterval = other.showAnswButtonInterval;
            this.enableShowAnswerButtonOverInterval = other.enableShowAnswerButtonOverInterval;
            this.ignoreAnswerCharacterCasing = other.ignoreAnswerCharacterCasing;
            this.enablePlaySoundWhenWrong = other.enablePlaySoundWhenWrong;
            this.songPath = other.songPath;
            this.timedQuestionInterval = other.timedQuestionInterval;
            this.enableTimedQuestion = other.enableTimedQuestion;
            this.verifyOnKeyPress = other.verifyOnKeyPress;
            this.pointsToDeduct = other.pointsToDeduct;
            this.enableShowAnswerButton = other.enableShowAnswerButton;
        }
    }
}
