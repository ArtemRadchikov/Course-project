using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Model
{
    public class SymbolicExpretion : BaseVM, ICloneable
    {
        string symbolicValue;
        string laTeXValue;

        [Required]
        public string SymbolicValue
        {
            get { return symbolicValue; }
            set
            {
                symbolicValue = value;
                OnPropertyChanged("SymbolicValue");
            }
        }

        [Required]
        public string LaTeXValue
        {
            get { return laTeXValue; }
            set
            {
                laTeXValue = value;
                OnPropertyChanged("LaTeXValue");
            }
        }

        public SymbolicExpretion(string SymbolicValue, string LaTeXValue)
        {
            this.SymbolicValue = SymbolicValue;
            this.LaTeXValue = LaTeXValue;
        }

        public SymbolicExpretion() { }

        public object Clone()
        {
            return new SymbolicExpretion(this.SymbolicValue, this.LaTeXValue);
        }

    }
}
