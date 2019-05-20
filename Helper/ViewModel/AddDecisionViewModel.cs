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
    class AddDecisionViewModel:BaseVM
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

                        Text = Text.Replace("{fx}", originalValue);
                        Text = Text.Replace("{path}", Paths.PathToNewDecesion);
                        Text = Text.Replace("{a}", LowerSegmentValue);
                        Text = Text.Replace("{b}", UpperSegmentValue);
                        MessageBox.Show(Text);
                        using (StreamWriter sw = new StreamWriter(Paths.PathToBatchCalculateNewDecesion, false))
                        {
                            sw.Write(Text);
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
    }
}
