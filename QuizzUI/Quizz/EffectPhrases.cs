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

namespace QuizzUI.Quizz
{
    public static class EffectPhrases
    {
        public static readonly string[] CorrectPhrases = new string[]
        { 
            "Correct", "That's Right", "Awesome", "Nice",
            "Affirmative", "Good", "Yep", "Indeed",
            "Very Good", "Good Job", "Well Done",
            "That's It", "Right", "Fantastic", "Right On",
            "Super",
        };

        public static readonly string[] WrongPhrases = new string[]
        {
            "Not Quite", "Wrong", "Nope", "Negative",
            "Try Again", "Bad Answer", "Incorrect",
            "Not Right", "That's Off", "Sorry, No",
        };

        public static readonly string[] PositiveExpressionMarks = new string[]
        {
            "!", "!!!", ".", "! !", ""
        };


        public static readonly string[] NegativeExpressionMarks = new string[] 
        { 
            "...", ".", "", "!"
        };

        public static string GenerateRandomCorrectAnswerPhrase()
        {
            return string.Format("{0}{1}",
                CorrectPhrases[new Random().Next(0, CorrectPhrases.Length)],
                PositiveExpressionMarks[new Random().Next(0, PositiveExpressionMarks.Length)]);
        }

        public static string GenerateRandomWrongAnswerPhrase()
        {
            return string.Format("{0}{1}",
                CorrectPhrases[new Random().Next(0, CorrectPhrases.Length)],
                PositiveExpressionMarks[new Random().Next(0, PositiveExpressionMarks.Length)]);
        }
    }
}
