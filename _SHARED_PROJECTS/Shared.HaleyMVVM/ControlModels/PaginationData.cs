using Haley.Abstractions;
using Haley.Enums;
using Haley.Events;
using Haley.Models;
using Haley.MVVM;
using Haley.Utils;
using System;
using System.Collections.Generic;
using System.Text;


namespace Haley.Models
{
    public class PaginationData : ChangeNotifier
    {
        public int maxitemsperpage { get; set; }
        public int items_per_page { get; set; }
        public int total_pages { get; set; }

        private int _total_items;
        public int total_items
        {
            get { return _total_items; }
            set { SetProp(ref _total_items, value); }
        }

        private int _current_page;
        public int current_page
        {
            get { return _current_page; }
            set
            {
                if (value < total_pages + 1 && value > 0)
                {
                    _current_page = value;
                    onPropertyChanged();
                }
            }
        }

        private void _initiation(int _totalitemscount)
        {
            total_items = _totalitemscount;
            items_per_page = 10; //Start with 10

            int remainder_check = 0;
            //Get the remainder after dividing
            Math.DivRem(total_items, items_per_page, out remainder_check);
            total_pages = total_items / items_per_page;
            if (remainder_check != 0) total_pages++; //Increment by 1. Because, whatever the remainder is can be accommdaed in a single page.

            current_page = 1;
            maxitemsperpage = 15;
        }

        public PaginationData(int total_items_count)
        {
            _initiation(total_items_count);
        }

    }
}
