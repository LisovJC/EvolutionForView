using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution.Model
{
    public class BoardModel
    {
        public string Title { get; set; }

        public ObservableCollection<CardModel> Cards { get; set; }
    }
}
