using Evolution.Command;
using Evolution.Core;
using System;

namespace Evolution.Model
{
    public class Category : ObservableObject
    {
		private bool _isSelect = false;

		public bool isSelect
        {
			get => _isSelect;
			set => Set(ref _isSelect, value);
		}

        private string _categoryName = "";

        public string CategoryName
        {
            get => _categoryName;
            set => Set(ref _categoryName, value);
        }      

    }
}
