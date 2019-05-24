﻿using DevExpress.Mvvm;
using Helper.Model;
using Helper.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Helper.ViewModel
{
    public class DecisionViewModel:BaseVM
    {
        Decision selectedDecesion;
        public ObservableCollection<Decision> Decisions { get; set; }
        public ICollectionView DecisionsView { get; set; }

        public Decision SelectedDecesion
        {
            get { return selectedDecesion; }
            set
            {
                selectedDecesion = value;
                OnPropertyChanged("SelectedDecesion");
            }
        }

        public DecisionViewModel()
        {
            OriginalValue fx = new OriginalValue("x^2+2", @"{\frac{2 \left(\pi^3+6 \pi\right)}{3 \pi}}");
            Coefficient_a0 a0 = new Coefficient_a0("(2*(%pi ^ 3 + 6 *% pi)) / (3 *% pi)", @"{\frac{2 \left(\pi^3+6 \pi\right)}{3 \pi}}");
            Coefficient_an an = new Coefficient_an("(4 * (-1) ^ n) / n ^ 2", @"{\frac{4 \left(-1\right)^{n}}{n^2}}");
            Coefficient_bn bn = new Coefficient_bn("0", "0");
            FourierSeries fs = new FourierSeries("25-(6*'sum(((-1)^n*sin((%pi*n*x)/3))/n,n,1,k))/%pi", @"4 \sum_{n=1}^{k}{{\frac{\left(-1\right)^{n} \cos \left(n x\right) }{n^2}}}+{\frac{\pi^3+6 \pi}{3 \pi}}");
            PartialSum_k ps_k = new PartialSum_k("25-(6*'sum(((-1)^n*sin((%pi*n*x)/3))/n,n,1,k))/%pi", @"4 \sum_{n=1}^{k}{{\frac{\left(-1\right)^{n} \cos \left(n x\right) }{n^2}}}+{\frac{\pi^3+6 \pi}{3 \pi}}");
            Decision decesion = new Decision("x^2+2", fx, @"-\pi", @"\pi", @"\pi", a0, an, bn, fs,ps_k, DateTime.Now);
            Decisions = new ObservableCollection<Decision>();
            Decisions.Add(decesion);
            Thread.Sleep(5000);
            Decisions.Add(decesion);
            Thread.Sleep(5000);

            Decisions.Add(decesion);
            DecisionsView = CollectionViewSource.GetDefaultView(Decisions);

            //SelectedDecesion = Decisions.FirstOrDefault();
        }

        public ICommand Sort
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (DecisionsView.SortDescriptions.Count > 0)
                    {
                        DecisionsView.SortDescriptions.Clear();
                    }
                    else
                    {
                        DecisionsView.SortDescriptions.Add(new SortDescription("СreationTime", ListSortDirection.Ascending));
                    }
                }, DecisionsView.SortDescriptions.Count != 0);
            }
        }
        public ICommand DeleteDecesion
        {
            get
            {
                return new DelegateCommand<Decision>((decesion) =>
                {
                    Decisions.Remove(decesion);
                    SelectedDecesion = Decisions.FirstOrDefault();

                }, (book) => book != null);
            }
        }
        public DelegateCommand AddItem
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        AddDecision addDecesion = new AddDecision();
                        addDecesion.ShowDialog();

                        string path = Paths.PathToMaximaLogs;

                        using (Loading loading = new Loading(()=>StartCalculationProcces(Paths.PathToBatchCalculateNewDecision)))
                        {
                            loading.ShowDialog();
                        }

                        #region Get values and create object
                        OriginalValue OriginalValue = CreateSymbolicExpretion(Paths.PathToNewDecisionFunctionValues) as OriginalValue;
                        Coefficient_a0 Coefficient_a0 = CreateSymbolicExpretion(Paths.PathToNewDecisionСoefficientValues_a0) as Coefficient_a0;
                        Coefficient_an Coefficient_an = CreateSymbolicExpretion(Paths.PathToNewDecisionСoefficientValues_an) as Coefficient_an;
                        Coefficient_bn Coefficient_bn = CreateSymbolicExpretion(Paths.PathToNewDecisionСoefficientValues_bn) as Coefficient_bn;
                        PartialSum_k PartialSum_k = CreateSymbolicExpretion(Paths.PathToNewDecisionFourierSeriesValues) as PartialSum_k;
                        FourierSeries FourierSeries = new FourierSeries(PartialSum_k.LaTeXValue.Replace("k", @"+\infty"), PartialSum_k.SymbolicValue.Replace("k", @"+\infty"));


                        DateTime СreationTime = DateTime.Now;

                        string[] values;

                        using (StreamReader sr = new StreamReader(Paths.PathToNewDecisionsegmentValues))
                        {
                            values = sr.ReadToEnd().Split(';');
                        }

                        string LowerSegmentValue = MaximaTeXParser(values[0]);
                        string UpperSegmentValue = MaximaTeXParser(values[1]);
                        string HalfPeriod = MaximaTeXParser(values[2]);

                        string inputedValue;
                        using (StreamReader sw = new StreamReader(Paths.PathToNewDecision + "inputedValue.txt"))
                        {
                            inputedValue = sw.ReadLine();
                        }

                        Decision Decesion = new Decision(inputedValue,OriginalValue, LowerSegmentValue, UpperSegmentValue, HalfPeriod, Coefficient_a0, Coefficient_an, Coefficient_bn, FourierSeries, PartialSum_k, СreationTime);

                        Decisions.Add(Decesion);
                        #endregion
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
            }
        }

        void StartCalculationProcces(string path)
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = Paths.PathMaximaCMD,
                Arguments = "-q -b " + '"' + path + '"',  // Путь к приложению
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process pr = Process.Start(startInfo);

            pr.WaitForExit();
        }

        public ICommand AddPlot
        {
            get
            {
                return new DelegateCommand<string>((ks) =>
                {
                    try
                    {
                        string Text;
                        using (StreamReader sr = new StreamReader(Paths.PathToMaximaLogs+ "/plotExample.txt"))
                        {
                            Text = sr.ReadToEnd();
                        }

                        int k = Convert.ToInt32(ks);
                        Text = Text.Replace("{fx}", SelectedDecesion.OriginalValue.SymbolicValue);
                        Text = Text.Replace("{gx}", SelectedDecesion.PartialSum_k.SymbolicValue);
                        Text = Text.Replace("{k}", k.ToString());
                    Text = Text.Replace("{a}", SelectedDecesion.LowerSegmentValue.Replace(@"\","%"));
                        Text = Text.Replace("{b}", SelectedDecesion.UpperSegmentValue.Replace(@"\", "%"));

                        using (StreamWriter sw = new StreamWriter(Paths.PathToMaximaLogs + "/plot.txt", false))
                        {
                            sw.Write(Text);
                        }

                        using (Loading loading = new Loading(() => StartCalculationProcces(Paths.PathToMaximaLogs + "/plot.txt")))
                        {
                            loading.ShowDialog();
                        }
                        //Paths.PathToBatchCalculateNewDecesion


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
            resstr = resstr.Replace(@"\pi", @"%pi");
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
