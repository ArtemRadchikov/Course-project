using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm;
using Helper.Model;

namespace Helper.ViewModel
{
    class AdddecisionViewModel:BaseVM
    {
        string originalValue;
        string lowerSegmentValue;
        string upperSegmentValue;

        public string OriginalValue
        {
            get => originalValue;
            set
            {
                originalValue = value;
                OnPropertyChanged("OriginalValue");
            }
        }

        public string LowerSegmentValue
        {
            get => lowerSegmentValue;
            set
            {
                lowerSegmentValue = value;
                OnPropertyChanged("LowerSegmentValue");
            }
        }

        public string UpperSegmentValue
        {
            get => upperSegmentValue;
            set
            {
                upperSegmentValue = value;
                OnPropertyChanged("UpperSegmentValue");
            }
        }

        public ICommand CreateFile
        {
            get
            {
                return new DelegateCommand(() =>
                    {
                        string Text;
                        using (StreamReader sr = new StreamReader(Paths.PathToBatchExample))
                        {
                            Text = sr.ReadToEnd();
                        }

                        Text = Text.Replace("{fx}", CorrectPi(originalValue));
                        Text = Text.Replace("{path}", Paths.PathToNewDecision);
                        Text = Text.Replace("{a}", CorrectPi(LowerSegmentValue));
                        Text = Text.Replace("{b}", CorrectPi(UpperSegmentValue));

                        using (StreamWriter sw = new StreamWriter(Paths.PathToBatchCalculateNewDecision, false))
                        {
                            sw.Write(Text);
                        }

                        using (StreamWriter sw = new StreamWriter(Paths.PathToNewDecision + "inputedValue.txt", false))
                        {
                            sw.WriteLine(CorrectPi(originalValue));
                        }

                        foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
                        {
                            if (window.DataContext == this)
                            {
                                window.Close();
                            }
                        }
                    }
                );
            }
        }
        string CorrectPi(string input)
        {
            string output = input.Replace(@"\pi", @"%pi");
            return output;
        }
    }
}
