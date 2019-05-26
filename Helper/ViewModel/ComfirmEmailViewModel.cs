using Helper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.ViewModel
{
    class ComfirmEmailViewModel:BaseVM
    {
        string correctString;
        int attemptsNumber;

        public ComfirmEmailViewModel(string CorrectString)
        {
            correctString = CorrectString;
            attemptsNumber = 3;
        }

        public int AttemptsNumber
        {
            get => attemptsNumber;
            set
            {
                attemptsNumber = value;
                OnPropertyChanged("AttemptsNumber");
            }
        }
    }
}
