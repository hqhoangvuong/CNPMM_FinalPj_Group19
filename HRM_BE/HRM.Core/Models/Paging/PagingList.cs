using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Core.Models.Paging
{
    public class PagingList<T>
    {
        public Paging Paging { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
