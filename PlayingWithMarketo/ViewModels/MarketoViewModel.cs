using PagedList;
using System;

namespace PlayingWithMarketo.ViewModels
{
    public class MarketoViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IPagedList<ActivityViewModel> Data { get; set; }
    }
}
