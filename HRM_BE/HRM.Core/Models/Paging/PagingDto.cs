using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Core.Models.Paging
{
    public class PagingDto
    {
        public string Filter { get; set; }

        /// <summary>
        ///     Default 1
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        ///     Default 10, set 0 for unlimiteds
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
