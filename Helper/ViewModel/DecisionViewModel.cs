using DevExpress.Mvvm;
using Helper.Model;
using Helper.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Helper.ViewModel
{
    public class DecisionViewModel:BaseVM
    {
        Decision selectedDecision;
        public ObservableCollection<Decision> Decisions { get; set; }

        bool decisionsIsEmpty;
        public Decision SelectedDecision
        {
            get { return selectedDecision; }
            set
            {
                selectedDecision = value;
                OnPropertyChanged("SelectedDecision");
            }
        }

        public Visibility DecisionsIsEmpty
        {
            get
            {
                if (decisionsIsEmpty)
                    return Visibility.Hidden;
                else
                    return Visibility.Visible;
            }
            set
            {
                decisionsIsEmpty= !(value == Visibility.Visible);
                OnPropertyChanged("DecisionsIsEmpty");
            }
        }

        public DecisionViewModel()
        {
            SymbolicExpretion fx = new SymbolicExpretion("x^2+2", @"{\frac{2 \left(\pi^3+6 \pi\right)}{3 \pi}}");
            SymbolicExpretion a0 = new SymbolicExpretion("(2*(%pi ^ 3 + 6 *% pi)) / (3 *% pi)", @"{\frac{2 \left(\pi^3+6 \pi\right)}{3 \pi}}");
            SymbolicExpretion an = new SymbolicExpretion("(4 * (-1) ^ n) / n ^ 2", @"{\frac{4 \left(-1\right)^{n}}{n^2}}");
            SymbolicExpretion bn = new SymbolicExpretion("0", "0");
            SymbolicExpretion fs = new SymbolicExpretion("", @"4 \sum_{n=1}^{k}{{\frac{\left(-1\right)^{n} \cos \left(n x\right) }{n^2}}}+{\frac{\pi^3+6 \pi}{3 \pi}}");
            Decision decision = new Decision(fx, @"-\pi", @"\pi", @"\pi", a0, an, bn, fs, DateTime.Now);

            Decisions = new ObservableCollection<Decision>();
            Decisions.Add(decision);
            decisionsIsEmpty = Decisions.Count == 0;

            //SelectedDecision = Decisions.FirstOrDefault();
        }

        public DelegateCommand AddItem
        {
            get
            {
                decisionsIsEmpty = Decisions.Count == 0;

                return new DelegateCommand(() =>
                {
                    try
                    {
                        AddDecision addDecision = new AddDecision() { DataContext=new AddDecisionViewModel()};
                        addDecision.ShowDialog();

                        string path = AppDomain.CurrentDomain.BaseDirectory;
                        path = path.Replace(@"bin\Debug\", @"WorkWithMaxima\");
                        path = path.Replace(@"\", @"/");
                        Process pr=Process.Start("D:/Helper/Helper/WorkWithMaxima/maxima.bat", "-q -b " + '"' + "D:/Helper/Helper/WorkWithMaxima/batch.txt" + '"');

                        pr.WaitForExit(10000);

                        path += "newDecision/";

                    
                        SymbolicExpretion OriginalValue = CreateSymbolicExpretion(path + "func.txt");

                        SymbolicExpretion Coefficient_a0 = CreateSymbolicExpretion(path + "a0.txt");
                        SymbolicExpretion Coefficient_an = CreateSymbolicExpretion(path + "an.txt");
                        SymbolicExpretion Coefficient_bn = CreateSymbolicExpretion(path + "bn.txt");
                        SymbolicExpretion FourierSeries = CreateSymbolicExpretion(path + "FS.txt");
                        DateTime СreationTime = DateTime.Now;

                        string[] values;

                        using (StreamReader sr = new StreamReader(path + "SegmentValues.txt"))
                        {
                            values = sr.ReadToEnd().Split(';');
                        }

                        string LowerSegmentValue = MaximaTeXParser(values[0]);
                        string UpperSegmentValue = MaximaTeXParser(values[1]);
                        string HalfPeriod = MaximaTeXParser(values[2]);

                        Decision decision = new Decision(OriginalValue, LowerSegmentValue, UpperSegmentValue, HalfPeriod, Coefficient_a0, Coefficient_an, Coefficient_bn, FourierSeries, СreationTime);

                        
                        DecisionsIsEmpty = Visibility.Visible;

                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
            }
        }

        #region AddItem functions
        SymbolicExpretion CreateSymbolicExpretion(string FileName)
        {
            SymbolicExpretion expretion;
            string[] values;
            string val;
            string LaTeXval;

            using (StreamReader sr = new StreamReader(FileName))
            {
                values=sr.ReadToEnd().Split(';');
            }

            val = MaximaTeXParser(values[0]);
            LaTeXval = MaximaTeXParser(values[1]);
            expretion = new SymbolicExpretion(val, LaTeXval);
            return expretion;
        }

        string MaximaTeXParser(string str)
        {
            string resstr = str;
            resstr = resstr.Replace("\n", "");
            resstr = resstr.Replace("\"", "");
            resstr = resstr.Replace("$", "");
            resstr = resstr.Replace(@"\\,", " ");
            resstr = resstr.Replace(@"\\", @"\");

            resstr = replaceOverToFrac(resstr);

            return resstr;
        }

        string replaceOverToFrac(string str)
        {
            string result = str;
            string findsubstr = @"\over";
            string inputsubstr = @"\frac";
            int openCount = 0;
            int closeCount = 0;

            int index = result.IndexOf(findsubstr);

            while (index != -1)
            {
                openCount = 0;
                closeCount = 0;

                result = result.Remove(index, findsubstr.Length);
                for (index = index - 1; ; index--)
                {
                    if (str[index] == '}')
                        closeCount++;
                    if (str[index] == '{')
                        openCount++;
                    if (openCount == closeCount)
                        break;
                }
                result = result.Insert(index, inputsubstr);
                index = result.IndexOf(findsubstr);
            }

            return result;
        }
        #endregion
    }
}
