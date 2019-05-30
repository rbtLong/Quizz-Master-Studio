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
using System.Xml.Linq;
using LinqXMLTools;

namespace QuizzSettings
{
    public class AnswerSettings
    {
        bool ignoreMistakes = false;
        double answerCreditPercent = 100;
        bool enablePlaySoundWhenWrong = false;

        #region Public Properties

        public bool IgnoreMistakes
        {
            get { return ignoreMistakes; }
            set { ignoreMistakes = value; }
        }

        public double AnswerCreditPercent
        {
            get { return answerCreditPercent; }
            set { answerCreditPercent = value; }
        }

        public bool EnablePlaySoundWhenWrong
        {
            get { return enablePlaySoundWhenWrong; }
            set { enablePlaySoundWhenWrong = value; }
        }
        #endregion

        public AnswerSettings()
        {

        }

        public AnswerSettings(XElement xElm)
        {
            XMLParser.ParseAttribute(xElm, "IgnoreMistakes", ref ignoreMistakes);
            XMLParser.ParseAttribute(xElm, "AnswerCreditPercent", ref answerCreditPercent);
            XMLParser.ParseAttribute(xElm, "EnablePlaySoundWhenWrong", ref enablePlaySoundWhenWrong);
        }
    }
}
