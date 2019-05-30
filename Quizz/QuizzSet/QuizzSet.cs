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
using Quizz.StringFreeReponse;
using Quizz.StringFreeReponse.MathFormulaResponse;
using System.IO;

namespace Quizz.QuizzSet
{
    public partial class QuizzSet : IEquatable<QuizzSet>, ICloneable
    {
        string name = string.Empty;
        List<StringFreeResponseProblem> qstStrFreeRsp;
        string id = null;
        FileInfo fileId;

        protected event BindingEventHandler ToXmlBinding;
        protected delegate void BindingEventHandler(object sender, List<XElement> Bindings);

        public event QuizzSetLoadedEventHandler OnQuizzLoaded;
        public delegate void QuizzSetLoadedEventHandler(object sender, QuizzSet QSet, XElement Quizz);

        public event QuestionLoadedEventHandler OnQuestionLoaded;
        public delegate void QuestionLoadedEventHandler(object sender, QuizzSet qSet, XElement Question,
            StringFreeResponseProblem Problem);

        #region Public Property
        public FileInfo FileId
        {
            get { return fileId; }
            set { fileId = value; }
        }

        public string Id
        {
            get { return id; }
        }

        public virtual List<StringFreeResponseProblem> Problems
        {
            get { return qstStrFreeRsp; }
            set { qstStrFreeRsp = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public StringFreeResponseProblem this[int pos]
        {
            get 
            {
                return qstStrFreeRsp[pos];
            }
        }
        #endregion

        public QuizzSet(string Name)
        {
            name = Name;

            string path = Path.Combine(QuizzSet.DefaultLocalPath, name + ".qzm");
            for (int i = 0; File.Exists(path); ++i)
                path = Path.Combine(QuizzSet.DefaultLocalPath,
                    string.Format("{0}{1:0}.qzm", name, i));

            fileId = new FileInfo(path);
            qstStrFreeRsp = new List<StringFreeResponseProblem>();
            id = (DateTime.Now.GetHashCode().ToString("x") + System.Guid.NewGuid().ToString()).Replace("-", "");
        }

        public QuizzSet(FileInfo File)
            : this(XDocument.Load(File.FullName))
        {
            this.fileId = File;
        }

        public QuizzSet(FileInfo File, bool AutoLoad)
        {
            this.fileId = File;
            if (AutoLoad)
                LoadXDoc(XDocument.Load(File.FullName));
        }

        protected QuizzSet(XDocument xDoc)
        {
            LoadXDoc(xDoc);
        }

        protected QuizzSet(XDocument xDoc, bool AutoLoad)
        {
            if(AutoLoad)
                LoadXDoc(xDoc);
        }

        protected void LoadXDoc(XDocument xDoc)
        {
            qstStrFreeRsp = new List<StringFreeResponseProblem>();
            try
            {
                XElement qSets = xDoc.Element("StringQuizzMaster").Element("QuizzSet");

                if (qSets == null)
                    throw new System.Xml.XmlException("The xDoc inputted does not contain any quizzes.");
                else
                {
                    name = qSets.Attribute("Name").Value;

                    if (qSets.Attribute("Id") != null
                        && qSets.Attribute("Id").Value != null)
                        id = qSets.Attribute("Id").Value;
                    else
                        id = (DateTime.Now.GetHashCode().ToString("x") + System.Guid.NewGuid().ToString()).Replace("-", "");

                    List<XElement> problems = qSets.Elements(
                        ProblemType.FreeResponseProblem.ToString()).ToList<XElement>();
                    problems.AddRange(qSets.Elements(
                        ProblemType.MathFormulaProblem.ToString()).ToList<XElement>());

                    foreach (XElement itm in problems)
                    {
                        StringFreeResponseProblem prob;
                        if (itm.Name == ProblemType.MathFormulaProblem.ToString())
                        {
                            StringMathFreeResponse mProb = new StringMathFreeResponse(itm);
                            prob = new StringFreeResponseProblem(
                                mProb.Description,
                                mProb.Question,
                                mProb.Answer,
                                mProb.Id);
                        }
                        else
                            prob = new StringFreeResponseProblem(itm);
                        qstStrFreeRsp.Add(prob);

                        if (OnQuestionLoaded != null)
                            OnQuestionLoaded(this, this, itm, prob);
                    }

                    if(OnQuizzLoaded != null)
                        OnQuizzLoaded(this, this, qSets);
                }
            }
            catch (System.Xml.XmlException err)
            {
                throw new System.IO.IOException("Could not parse one of the .qzm files (probably corruption).",
                    err);
            }
        }

        public void Remove(int Pos)
        {
            qstStrFreeRsp.RemoveAt(Pos);
        }

        public void Remove(StringFreeResponseProblem Prob)
        {
            qstStrFreeRsp.Remove(Prob);
        }

        public virtual XElement ToElement()
        {
            List<XElement> bindings = new List<XElement>();

            if (ToXmlBinding != null)
                ToXmlBinding(this, bindings);

            foreach (StringFreeResponseProblem itm in qstStrFreeRsp)
                bindings.Add(itm.ToXML());

            return new XElement("QuizzSet",
                new XAttribute("Name", name),
                new XAttribute("Id", id == null
                    ? (DateTime.Now.GetHashCode().ToString("x") + System.Guid.NewGuid().ToString()).Replace("-","")
                    : id),
                bindings);
        }

        public virtual void SaveAsFile(string FilePath)
        {
            XDocument xDoc = new XDocument(new XElement("StringQuizzMaster",
                ToElement()));
            xDoc.Save(FilePath);
        }

        public virtual void SaveFile()
        {
            XDocument xDoc = new XDocument(new XElement("StringQuizzMaster",
                ToElement()));
            xDoc.Save(fileId.FullName);
       } 

        public virtual void CopyFrom(QuizzSet Item)
        {
            this.name = Item.name;
            foreach (StringFreeResponseProblem itm in Item.qstStrFreeRsp)
                this.qstStrFreeRsp.Add(itm.Clone() as StringFreeResponseProblem);
        }

        #region IEquatable Implimentation

        public override string ToString()
        {
            return string.Format("{0} [{1}]", 
                name, qstStrFreeRsp != null ? qstStrFreeRsp.Count.ToString() : "EMPTY SET");
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                base.Equals(obj);

            return (obj is QuizzSet
                && this.Equals(obj as QuizzSet));
        }

        public bool Equals(QuizzSet other)
        {
            if (other == null)
                return false;
            else if (Problems.Count
                != other.Problems.Count)
                return false;

            for (int i = 0; i < Problems.Count; ++i)
                if (!Problems[i].Equals(
                    other.Problems[i]))
                    return false;

            return (this.name == other.Name);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(QuizzSet first, QuizzSet other)
        {
            if (!Object.ReferenceEquals(first, other))
                return false;

            if ((first as object) == null
                || (other as object) == null)
                return false;

            return first.Equals(other);
        }

        public static bool operator !=(QuizzSet first, QuizzSet other)
        {
            return !(first == other);
        }
        
        public object Clone()
        {
            List<StringFreeResponseProblem> probs = new List<StringFreeResponseProblem>();
            StringFreeResponseProblem[] dest = new StringFreeResponseProblem[qstStrFreeRsp.Count];
            for (int i = 0; i < dest.Length; i++)
                dest[i] = qstStrFreeRsp[0].Clone() as StringFreeResponseProblem;

            return new QuizzSet(name)
            {
                qstStrFreeRsp = dest.ToList<StringFreeResponseProblem>(),
                id = this.id,
                fileId = fileId,
            };
        }

        #endregion
    }
}
