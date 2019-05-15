using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Helper.Model
{
    class Book : BaseVM
    {
        public string Name { get; set; }
        public ObservableCollection<Author> Authors { get; set; } = new ObservableCollection<Author>();
        ObservableCollection<KeyWordItem> keyWords = new ObservableCollection<KeyWordItem>();
        public ObservableCollection<KeyWordItem> KeyWords
        {
            get => keyWords;
            set
            {
                keyWords = value;
                OnPropertyChanged("KeyWords");
            }
        }
        public int PublishDate { get; set; }
        public string Publisher { get; set; }
        public string BibliographicDescription { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
}
