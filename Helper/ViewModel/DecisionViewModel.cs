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
using System.Data.Entity;


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
            Decisions = new ObservableCollection<Decision>();
            if (AppUser.GetRoll().ToLower()!="гость")
                using (HelperContext helper = new HelperContext())
                {
                    foreach (Decision d in helper.Decisions.Include(d=>d.Coefficient_a0)
                                                            .Include(d => d.Coefficient_an)
                                                            .Include(d => d.Coefficient_bn)
                                                            .Include(d => d.FourierSeries)
                                                            .Include(d => d.PartialSum_k)
                                                            .Include(d => d.OriginalValue).ToList())
                    {
                        
                        Decisions.Add(d);
                    }
                }
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

        public ICommand DeleteDecision
        {
            get
            {
                return new DelegateCommand<Decision>((decesion) =>
                {
                    Decisions.Remove(decesion);
                    SelectedDecesion = Decisions.FirstOrDefault();

                }, (decesion) => decesion != null);
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

                        string func;
                        using (StreamReader sr = new StreamReader(Paths.PathToBatchCalculateNewDecision))
                        {
                            func = sr.ReadLine();
                        }
                        func = func.Replace("f(x):=", "");
                        func = func.Replace(";", "");
                        func = func.Replace("\n", "");
                        func = func.Replace(" ", "");

                        HelperContext helperContext = new HelperContext();
                        Decision decision;

                        if (helperContext.Decisions.Any(d => d.OriginalValue.SymbolicValue.Replace("\n", "").Replace(" ", "").Equals(func)))
                        {
                            decision = helperContext.Decisions.Include(d => d.FourierSeries)
                                    .Include(d => d.OriginalValue)
                                    .Include(d => d.Coefficient_a0)
                                    .Include(d => d.Coefficient_an)
                                    .Include(d => d.Coefficient_bn)
                                    .Include(d => d.PartialSum_k)
                                    .FirstOrDefault(d => d.OriginalValue.SymbolicValue.Equals(func));
                            Decisions.Add(decision);
                        }
                        else
                        {
                            using (Loading loading = new Loading(() => StartCalculationProcces(Paths.PathToBatchCalculateNewDecision)))
                            {
                                loading.ShowDialog();
                            }

                            #region Get values and create object

                            OriginalValue OriginalValue = new OriginalValue(CreateSymbolicExpretion(Paths.PathToNewDecisionFunctionValues));
                            Coefficient_a0 Coefficient_a0 = new Coefficient_a0(CreateSymbolicExpretion(Paths.PathToNewDecisionСoefficientValues_a0));
                            Coefficient_an Coefficient_an = new Coefficient_an(CreateSymbolicExpretion(Paths.PathToNewDecisionСoefficientValues_an));
                            Coefficient_bn Coefficient_bn = new Coefficient_bn(CreateSymbolicExpretion(Paths.PathToNewDecisionСoefficientValues_bn));
                            PartialSum_k PartialSum_k = new PartialSum_k(CreateSymbolicExpretion(Paths.PathToNewDecisionFourierSeriesValues));
                            FourierSeries FourierSeries = new FourierSeries(PartialSum_k.SymbolicValue.Replace("k", @"+\infty"), PartialSum_k.LaTeXValue.Replace("k", @"+\infty"));


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

                            decision = new Decision(inputedValue, OriginalValue, LowerSegmentValue, UpperSegmentValue, HalfPeriod, Coefficient_a0, Coefficient_an, Coefficient_bn, FourierSeries, PartialSum_k, СreationTime);
                            #endregion

                            Decisions.Add(decision);
                            helperContext.Decisions.Add(decision);
                            helperContext.SaveChanges();
                            helperContext.Dispose();
                            //SaveDecisionAsync(decision).Wait();
                        }                     
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Уважаемый пользователь, при добавлении возникла ошибка, попробуйте повторить операцию.");
                    }
                });
            }
        }

        private static async Task SaveDecisionAsync(Decision decision)
        {
            using (HelperContext helperContext = new HelperContext())
            {
                helperContext.Decisions.Add(decision);
                await helperContext.SaveChangesAsync();
            }
        }

        public ICommand CalculateKbyE
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        string Text;
                        using (StreamReader sr = new StreamReader(Paths.PathToMaximaLogs+ "/CalculateExample.txt"))
                        {
                            Text = sr.ReadToEnd();
                        }

                        Text = Text.Replace("{E}", EForCalculation.ToString().Replace(",","."));
                        Text = Text.Replace("{x}", XForCalculation.ToString());
                        Text = Text.Replace("{fx}", SelectedDecesion.OriginalValue.SymbolicValue);
                        Text = Text.Replace("{sxk}", SelectedDecesion.PartialSum_k.SymbolicValue);
                        Text = Text.Replace("{a}", SelectedDecesion.LowerSegmentValue.Replace(@"\", "%"));
                        Text = Text.Replace("{b}", SelectedDecesion.UpperSegmentValue.Replace(@"\", "%"));
                        Text = Text.Replace("{path}", Paths.PathToMaximaLogs);

                        using (StreamWriter sw = new StreamWriter(Paths.PathToMaximaLogs + "/CalculateK.txt", false))
                        {
                            sw.Write(Text);
                        }

                        using (Loading loading = new Loading(() => StartCalculationProcces(Paths.PathToMaximaLogs + "/CalculateK.txt")))
                        {
                            loading.ShowDialog();
                        }
                        string k;
                        using (StreamReader sr = new StreamReader(Paths.PathToMaximaLogs + "K.txt"))
                            k = sr.ReadToEnd();

                        MessageBox.Show(" Для достижения точности E= "+EForCalculation.ToString()+"\n Достаточно суммы "+ k.Replace("\"","").Replace(";","") +" порядка");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }, ()=> ChekE(EForCalculation));
            }
        }

        private bool ChekE(double E)
        {
            if (E == 0)
                return false;
            if (E > 1)
                return false;
            
            return true;
        }

        public double xForCalculation;
        public double XForCalculation
        {
            get =>xForCalculation;
            set
            {
                xForCalculation = value;
                OnPropertyChanged("XForCalculation");
            }
        }

        public double eForCalculation;
        public double EForCalculation
        {
            get => eForCalculation;
            set
            {
                eForCalculation = value;
                OnPropertyChanged("EForCalculation");
            }
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
                        using (StreamReader sr = new StreamReader(Paths.PathToMaximaLogs + "/plotExample.txt"))
                        {
                            Text = sr.ReadToEnd();
                        }
                        int k = Convert.ToInt32(ks);

                        Text = Text.Replace("{fx}", SelectedDecesion.OriginalValue.SymbolicValue);
                        Text = Text.Replace("{gx}", SelectedDecesion.PartialSum_k.SymbolicValue);
                        Text = Text.Replace("{k}", k.ToString());
                        Text = Text.Replace("{a}", SelectedDecesion.LowerSegmentValue.Replace(@"\", "%"));
                        Text = Text.Replace("{b}", SelectedDecesion.UpperSegmentValue.Replace(@"\", "%"));
                        Text = Text.Replace("{path}", Paths.PathToMaximaLogs);


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
                }, (ks) => int.TryParse(ks.ToString(), out int ds));
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
            resstr = resstr.Replace(@"%pi", @"\pi");
            resstr = resstr.Replace("$", "");
            resstr = resstr.Replace(@"\\,", " ");
            resstr = resstr.Replace(@"\\", @"\");
            resstr = resstr.Replace("'sum", "sum");

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
