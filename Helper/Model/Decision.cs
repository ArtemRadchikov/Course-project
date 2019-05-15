using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Model
{
    public class Decision:BaseVM
    {
        public SymbolicExpretion originalValue;
        public SymbolicExpretion coefficient_a0;
        public SymbolicExpretion coefficient_an;
        public SymbolicExpretion coefficient_bn;
        public SymbolicExpretion fourierSeries;
        public string lowerSegmentValue;
        public string upperSegmentValue;
        public string halfPeriod;
        public DateTime creationTime;


        public SymbolicExpretion OriginalValue
        {
            get { return originalValue; }
            set
            {
                originalValue = value;
                OnPropertyChanged("OriginalValue");
            }
        }
        public SymbolicExpretion Coefficient_a0
        {
            get { return coefficient_a0; }
            set
            {
                coefficient_a0 = value;
                OnPropertyChanged("Coefficient_a0");
            }
        }
        public SymbolicExpretion Coefficient_an
        {
            get { return coefficient_an; }
            set
            {
                coefficient_an = value;
                OnPropertyChanged("Coefficient_an");
            }
        }
        public SymbolicExpretion Coefficient_bn
        {
            get { return coefficient_bn; }
            set
            {
                coefficient_bn = value;
                OnPropertyChanged("Coefficient_bn");
            }
        }
        public SymbolicExpretion FourierSeries
        {
            get { return fourierSeries; }
            set
            {
                fourierSeries = value;
                OnPropertyChanged("FourierSeries");
            }
        }
        public string LowerSegmentValue
        {
            get { return lowerSegmentValue; }
            set
            {
                lowerSegmentValue = value;
                OnPropertyChanged("LowerSegmentValue");
            }
        }
        public string UpperSegmentValue
        {
            get { return upperSegmentValue; }
            set
            {
                upperSegmentValue = value;
                OnPropertyChanged("UpperSegmentValue");
            }
        }
        public string HalfPeriod
        {
            get { return halfPeriod; }
            set
            {
                halfPeriod = value;
                OnPropertyChanged("HalfPeriod");
            }
        }
        public DateTime СreationTime
        {
            get { return creationTime; }
            set
            {
                creationTime = value;
                OnPropertyChanged("СreationTime");
            }
        }

        public Decision(SymbolicExpretion oV, string lsv, string usv, string hp, SymbolicExpretion ca0, SymbolicExpretion can, SymbolicExpretion cbn, SymbolicExpretion fs, DateTime ct)
        {
            this.OriginalValue = oV;
            this.LowerSegmentValue = lsv;
            this.UpperSegmentValue = usv;
            this.HalfPeriod = hp;
            this.Coefficient_a0 = ca0;
            this.Coefficient_an = can;
            this.Coefficient_bn = cbn;
            this.FourierSeries = fs;
            this.СreationTime = ct;
        }

        public Decision()
        {
            this.OriginalValue = new SymbolicExpretion();
            this.Coefficient_a0 = new SymbolicExpretion();
            this.Coefficient_an = new SymbolicExpretion();
            this.Coefficient_bn = new SymbolicExpretion();
            this.FourierSeries = new SymbolicExpretion();
            this.СreationTime = DateTime.Now;
        }
    }
}
