using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Core.Models.Paging
{
    public class Paging
    {
        public string Filter { get; set; }
        public string Sort { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int TotalPages => PageSize == 0 ? 1 : (Total + PageSize - 1) / PageSize;
        public int StartIndex => (Page - 1) * PageSize;
        public int EndIndex => StartIndex + PageSize > Total ? Total - 1 : StartIndex + PageSize - 1;
    }
}
