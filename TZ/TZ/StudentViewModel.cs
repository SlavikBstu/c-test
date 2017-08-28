using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TZ
{
    class StudentViewModel : INotifyPropertyChanged
    {
        private Student _student;

        public StudentViewModel(Student student)
        {
            _student = student;
        }

        public int Id
        {
            get
            {
                return _student.Id;
            }

            set
            {
                _student.Id = value;
                OnPropertyChanged("Id");
            }
        }

        public string FirstName
        {
            get
            {
                return _student.FirstName;
            }

            set
            {
                _student.FirstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        public string Last
        {
            get
            {
                return _student.Last;
            }

            set
            {
                _student.Last = value;
                OnPropertyChanged("Last");
            }
        }


        public string Age
        {
            get
            {
                return _student.Age;
            }

            set
            {
                _student.Age = value;
                OnPropertyChanged("Age");
            }
        }


        public string Gender
        {
            get
            {
                return _student.Gender;
            }

            set
            {
                _student.Gender = value;
                OnPropertyChanged("Gender");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
