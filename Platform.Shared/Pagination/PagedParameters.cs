﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Shared.Pagination
{
    public class PagedParameters
    {
        const int maxPageSize = int.MaxValue;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = int.MaxValue;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
