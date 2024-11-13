using Blossom_BusinessObjects;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Blossom_RazorWeb.Pages.SellerChannel.SellerDashboard
{
    public class IndexModel : PageModel
    {
        private readonly IOrderDetailService _orderDetailService;

        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }
        public IndexModel(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }
        public decimal TotalRevenue {  get; set; }
        public int OrderTotal { get; set; }
        public int TotalViews { get; set; }

        public List<RevenueByDate> RevenueByDates { get; set; }

        public List<DateTime> dateTimes { get; set; }
        public void OnGet()
        {
            
        }

        public void OnPostCalculateRevenue()
        {
            TotalRevenue = _orderDetailService.GetTotalRevenueAsync(StartDate, EndDate);
            OrderTotal = _orderDetailService.GetTotalOrdersCountAsync(StartDate, EndDate);
            TotalViews = _orderDetailService.GetTotalFlowerViewsAsync(StartDate, EndDate);
            RevenueByDates =  _orderDetailService.GetDailyRevenueAsync(StartDate, EndDate);
            dateTimes = _orderDetailService.GetDateRangeList(StartDate, EndDate);
            ViewData["RevenueData"] = RevenueByDates;
            ViewData["DateTimes"] = dateTimes;

        }
    }
}
