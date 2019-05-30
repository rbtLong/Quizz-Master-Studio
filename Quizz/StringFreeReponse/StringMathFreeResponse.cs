using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Quizz.StringFreeReponse
{
    public class StringMathFreeResponse : StringFreeResponseProblem
    {
        protected string description = string.Empty;

        #region Public Property
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public static bool operator ==(StringMathFreeResponse First,
            StringMathFreeResponse Second)
        {
            if (!object.ReferenceEquals(First, Second))
                return false;
            else if (!(First is StringMathFreeResponse)
                && !(Second is StringMathFreeResponse))
                return false;
            else if (First == null || Second == null)
                return false;

            return First.Equals(Second);
        }

        public static bool operator !=(StringMathFreeResponse First,
            StringMathFreeResponse Second)
        {
            return !(First==Second);
        }
        #endregion

        public StringMathFreeResponse()
        {
            this.problemKind = ProblemType.StringMathFreeResponse;
        }

        public StringMathFreeResponse(string Description, string Question, string[] Answers)
            : base(Question, Answers)
        {
            this.problemKind = ProblemType.StringMathFreeResponse;
            this.answer = Answers;
            this.question = Question;
            this.description = Description;
        }

        public StringMathFreeResponse(XElement XmlElement)
        {
            this.problemKind = ProblemType.StringMathFreeResponse;
            ParseXElement(XmlElement);
        }

        public override bool CheckAnswer(string Answer)
        {
            foreach (string itm in answer)
                if (itm.Trim().ToLower() == Answer.Trim().ToLower())
                    return true;
            return false;
        }

        public override string ToString()
        {
            return String.Format("Math Problem: {0}", 
                description == string.Empty ?
                "<NO DESCRIPT>" : description);
        }

        public override System.Xml.Linq.XElement ToXML()
        {
            XElement answers = new XElement("Answers");
            foreach (string itm in answer)
                answers.Add(new XElement("Answer", itm));

            return new XElement(ProblemKind.ToString(),
                new XAttribute("Description", Description),
                new XAttribute("Question", question),
                new XAttribute("Id", id == null
                    ? (DateTime.Now.GetHashCode().ToString("x") + System.Guid.NewGuid().ToString()).Replace("-","")
                    : id),
                answers);
        }

        public override void ParseXElement(XElement elmMathFreeResponse)
        {
            base.ParseXElement(elmMathFreeResponse);
            string descript = description;

            try
            {
                description = elmMathFreeResponse.Attribute("Description").Value;
            }
            catch (System.Xml.XmlException e)
            {
                System.Diagnostics.Debug.WriteLine("Error occurred when trying to parse a free response question [obj: {0} @ line: {1}]\n{2}]",
                    e.Source, e.LineNumber, e.StackTrace);

                description = descript;
            }
        }

        public override bool Equals(object obj)
        {
            return (obj is StringMathFreeResponse) ?
                base.Equals(obj) : false;
        }

        public bool Equals(StringMathFreeResponse other)
        {
            if ((this.answer == null && other == null) 
                || (other.answer != null && this.answer.Length != other.answer.Length))
                return false;

            if (this.answer != null && other.answer != null)
                for (int i = 0; i < this.answer.Length; ++i)
                    if ((this.answer[i] != null && other.answer != null)
                        && answer[i] != other.answer[i])
                        return false;

            return (other != null
                && this.question == other.question
                && this.ProblemKind == other.problemKind);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
