using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Model
{
    public class Decision:BaseVM
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DecisionID { get; set; }
        [Required]
        public string InputedValue { get; set; }
        public OriginalValue originalValue;
        public Coefficient_a0 coefficient_a0;
        public Coefficient_an coefficient_an;
        public Coefficient_bn coefficient_bn;
        public FourierSeries fourierSeries;
        public PartialSum_k partialSum_k;
        public string lowerSegmentValue;
        public string upperSegmentValue;
        public string halfPeriod;
        public DateTime creationTime;

        public ICollection<User> Users { get; set; }

        [Required]
        public OriginalValue OriginalValue
        {
            get { return originalValue; }
            set
            {
                originalValue = value;
                OnPropertyChanged("OriginalValue");
            }
        }
        [Required]
        public Coefficient_a0 Coefficient_a0
        {
            get { return coefficient_a0; }
            set
            {
                coefficient_a0 = value;
                OnPropertyChanged("Coefficient_a0");
            }
        }
        [Required]
        public Coefficient_an Coefficient_an
        {
            get { return coefficient_an; }
            set
            {
                coefficient_an = value;
                OnPropertyChanged("Coefficient_an");
            }
        }
        [Required]
        public Coefficient_bn Coefficient_bn
        {
            get { return coefficient_bn; }
            set
            {
                coefficient_bn = value;
                OnPropertyChanged("Coefficient_bn");
            }
        }
        [Required]
        public FourierSeries FourierSeries
        {
            get { return fourierSeries; }
            set
            {
                fourierSeries = value;
                OnPropertyChanged("FourierSeries");
            }
        }
        [Required]
        public PartialSum_k PartialSum_k
        {
            get { return partialSum_k; }
            set
            {
                partialSum_k = value;
                OnPropertyChanged("PartialSum_k");
            }
        }
        [Required]
        public string LowerSegmentValue
        {
            get { return lowerSegmentValue.Replace("%pi", @"\pi"); }
            set
            {
                lowerSegmentValue = value;
                OnPropertyChanged("LowerSegmentValue");
            }
        }
        [Required]
        public string UpperSegmentValue
        {
            get { return upperSegmentValue.Replace("%pi",@"\pi"); }
            set
            {
                upperSegmentValue = value;
                OnPropertyChanged("UpperSegmentValue");
            }
        }
        [Required]
        public string HalfPeriod
        {
            get { return halfPeriod; }
            set
            {
                halfPeriod = value;
                OnPropertyChanged("HalfPeriod");
            }
        }
        [Required]
        public DateTime СreationTime
        {
            get { return creationTime; }
            set
            {
                creationTime = value;
                OnPropertyChanged("СreationTime");
            }
        }

        public Decision(string InputedValue, OriginalValue oV, string lsv, string usv, string hp, Coefficient_a0 ca0, Coefficient_an can, Coefficient_bn cbn, FourierSeries fs, PartialSum_k ps_k, DateTime ct)
        {
            this.OriginalValue = oV as OriginalValue;
            this.LowerSegmentValue = lsv;
            this.UpperSegmentValue = usv;
            this.HalfPeriod = hp;
            this.Coefficient_a0 = ca0 as Coefficient_a0;
            this.Coefficient_an = can as Coefficient_an;
            this.Coefficient_bn = cbn as Coefficient_bn;
            this.FourierSeries = fs as FourierSeries;
            this.СreationTime = ct;
            this.PartialSum_k = ps_k as PartialSum_k;
        }

        public Decision()
        {
            this.OriginalValue = new OriginalValue();
            this.Coefficient_a0 = new Coefficient_a0();
            this.Coefficient_an = new Coefficient_an();
            this.Coefficient_bn = new Coefficient_bn();
            this.FourierSeries = new FourierSeries();
            this.PartialSum_k = new PartialSum_k();
            this.СreationTime = DateTime.Now;
        }
    }

    public class OriginalValue : SymbolicExpretion
    {
        [Key]
        public int DecisionID { get; set; }
        public OriginalValue(SymbolicExpretion symbolicExpretion) : base(symbolicExpretion.SymbolicValue.Replace(@"\pi", @"%pi"), symbolicExpretion.LaTeXValue) { }
        public OriginalValue() { }
    }

    public class Coefficient_a0 : SymbolicExpretion
    {
        [Key]
        public int DecisionID { get; set; }
        public Coefficient_a0() { }
        public Coefficient_a0(SymbolicExpretion symbolicExpretion) : base(symbolicExpretion.SymbolicValue.Replace(@"\pi", @"%pi"), symbolicExpretion.LaTeXValue) { }
    }

    public class Coefficient_an : SymbolicExpretion
    {
        [Key]
        public int DecisionID { get; set; }
        public Coefficient_an() { }
        public Coefficient_an(SymbolicExpretion symbolicExpretion) : base(symbolicExpretion.SymbolicValue.Replace(@"\pi", @"%pi"), symbolicExpretion.LaTeXValue) { }
    }

    public class Coefficient_bn : SymbolicExpretion
    {
        [Key]
        public int DecisionID { get; set; }
        public Coefficient_bn() { }
        public Coefficient_bn(SymbolicExpretion symbolicExpretion) : base(symbolicExpretion.SymbolicValue.Replace(@"\pi", @"%pi"), symbolicExpretion.LaTeXValue) { }
    }

    public class FourierSeries : SymbolicExpretion
    {
        [Key]
        public int DecisionID { get; set; }
        public FourierSeries() { }
        public FourierSeries(string SymbolicValue, string LaTeXValue) : base(SymbolicValue.Replace(@"\pi", @"%pi"), LaTeXValue) { }
    }

    public class PartialSum_k : SymbolicExpretion
    {
        [Key]
        public int DecisionID { get; set; }
        public PartialSum_k() { }
        public PartialSum_k(SymbolicExpretion symbolicExpretion) : base(symbolicExpretion.SymbolicValue.Replace(@"\pi", @"%pi"), symbolicExpretion.LaTeXValue) { }
    }
}
