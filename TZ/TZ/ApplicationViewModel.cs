using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Xml;
using System.Windows;
using System.Xml.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace TZ
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Student> Students { get; set; }
        public Student student = new Student();
                
        public string xPath = "Students.xml";

        private Student _seletedStudent;
        private RelayCommand _addCommand;
        private RelayCommand _removeCommand;

        private static int CountElements(string s)
        {
            return Regex.Matches(s, @"Id").Count;
        }


        public Student SelectedStudent
        {
            get
            {
                return _seletedStudent;
            }

            set
            {
                _seletedStudent = value;
                OnPropertyChanged("SelectedStudent");
            }
        }
        // для добавления необходимо нажать кнопку "Добавить", ввести значения в поля и слова нажать данную кнопку
        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand ??
                    (_addCommand = new RelayCommand(obj =>
                    {
                        int _countStudents = 0;
                        foreach(Student stud in Students)
                        {
                            if (stud.Id > _countStudents)
                                _countStudents = stud.Id;

                        }
                                       
                        try
                        {
                            Students.Insert(0, student);
                           

                            XmlDocument xDoc = new XmlDocument();
                            xDoc.Load("Students.xml");
                            XmlElement xRoot = xDoc.DocumentElement;
                            XmlElement studentElem = xDoc.CreateElement("Student");
                            XmlAttribute idAttr = xDoc.CreateAttribute("Id");
                            XmlElement firstNameElem = xDoc.CreateElement("FirstName");
                            XmlElement lastElem = xDoc.CreateElement("Last");
                            XmlElement ageElem = xDoc.CreateElement("Age");
                            XmlElement genderElem = xDoc.CreateElement("Gender");

                            XmlText idText = xDoc.CreateTextNode((++_countStudents).ToString());
                            XmlText firstNameText = xDoc.CreateTextNode(SelectedStudent.FirstName);
                            XmlText lastText = xDoc.CreateTextNode(SelectedStudent.Last);
                            XmlText ageText = xDoc.CreateTextNode(SelectedStudent.Age);
                            XmlText genderText = xDoc.CreateTextNode(SelectedStudent.Gender);

                            idAttr.AppendChild(idText);
                            firstNameElem.AppendChild(firstNameText);
                            lastElem.AppendChild(lastText);
                            ageElem.AppendChild(ageText);
                            genderElem.AppendChild(genderText);
                            studentElem.Attributes.Append(idAttr);
                            studentElem.AppendChild(firstNameElem);
                            studentElem.AppendChild(lastElem);
                            studentElem.AppendChild(ageElem);
                            studentElem.AppendChild(genderElem);

                            xRoot.AppendChild(studentElem);
                            xDoc.Save("Students.xml");
                                                                  
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                        SelectedStudent = student;

                    }));
            }
        }

        // для обновления необходимо выделить запись, поменять значения в полях (обязательно убрать строковые значения в полях возраста и пола) и нажать кнопку "Изменить", при этом старая запись останется прежней и добавится новая с новыми данными
        public RelayCommand UpdateCommand
        {
            get
            {
                return _addCommand ??
                    (_addCommand = new RelayCommand(obj =>
                    {
                        

                        Student stud = obj as Student;
                        if (stud != null)
                        {
                            XDocument xDoc = XDocument.Load(xPath);
                            foreach (XElement node in xDoc.Root.Nodes())
                            {
                                if (node.Attribute("Id").Value == stud.Id.ToString())
                                {
                                    node.Remove();
                                    xDoc.Save(xPath);
                                }

                            }

                            Students.Remove(stud);

                        }

                        XmlDocument doc = new XmlDocument();
                        doc.Load(xPath);
                        Student student = new Student();
                        foreach (XmlNode xn in doc.GetElementsByTagName("Student"))
                        {
                            if (xn.Attributes["Id"].Value == student.Id.ToString())
                            {
                                student.Id = Convert.ToInt32(xn.Value);

                            }
                            foreach (XmlNode childEl in xn.ChildNodes)
                            {
                                if (childEl.Name == "FirstName")
                                    student.FirstName = student.FirstName;

                                if (childEl.Name == "Last")
                                    student.Last = student.Last;

                                if (childEl.Name == "Age")
                                {
                                    student.Age = student.Age;
                                }


                                if (childEl.Name == "Gender")
                                {
                                    if (student.Gender == "Мужской")
                                    {
                                        student.Gender = "0";
                                    }
                                    else
                                    {
                                        student.Gender = "1";
                                    }
                                }
                            }                                                                                  
                        }
                        doc.Save(xPath);
                    }));
            }
        }

        //для даления необходимо выделить элемент и нажать кнопку "Удалить"
        public RelayCommand RemoveCommand
        {
            get
            {
                return _removeCommand ??
                  (_removeCommand = new RelayCommand(obj =>
                  {
                      Student student = obj as Student;
                      if (student != null)
                      {
                          XDocument doc = XDocument.Load(xPath);
                          foreach (XElement node in doc.Root.Nodes())
                          {
                              if (node.Attribute("Id").Value == student.Id.ToString())
                              {
                                  node.Remove();
                                  doc.Save(xPath);
                              }

                          }

                          Students.Remove(student);

                      }
                  },
                 (obj) => Students.Count > 0));
            }
        }

        public ApplicationViewModel()
        {
            

            Students = new ObservableCollection<Student>();

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xPath);

                XmlElement element = doc.DocumentElement;

                foreach (XmlElement el in element)
                {
                    Student student = new Student();

                    XmlNode attribute = el.Attributes.GetNamedItem("Id");
                    if (attribute != null)
                    {
                        student.Id = Convert.ToInt32(attribute.Value);
                    }

                    foreach (XmlNode childEl in el.ChildNodes)
                    {
                        if (childEl.Name == "FirstName")
                            student.FirstName = childEl.InnerText;

                        if (childEl.Name == "Last")
                            student.Last = childEl.InnerText;

                        if (childEl.Name == "Age")
                        {
                            if (Int32.Parse(childEl.InnerText) % 100 == 0 ||
                                Int32.Parse(childEl.InnerText) % 10 >= 5 ||
                                Int32.Parse(childEl.InnerText) % 10 == 0)
                            {
                                student.Age = childEl.InnerText + " лет";
                            }
                            if (Int32.Parse(childEl.InnerText) % 10 > 1 &&
                                Int32.Parse(childEl.InnerText) % 10 < 5)
                            {
                                student.Age = childEl.InnerText + " года";
                            }
                            if (Int32.Parse(childEl.InnerText) % 10 == 1)
                            {
                                student.Age = childEl.InnerText + " год";
                            }
                        }


                        if (childEl.Name == "Gender")
                        {
                            if (Int32.Parse(childEl.InnerText) == 0)
                            {
                                student.Gender = "Мужской";
                            }
                            else
                            {
                                student.Gender = "Женский";
                            }
                        }
                    }
                    Students.Add(student);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка: " + e.Message);

            }
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
