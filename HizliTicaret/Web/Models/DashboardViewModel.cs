using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class DashboardViewModel
    {
        public List<Product> MostSales { get; set; }
        public List<Product> MostAddsToCart { get; set; }
        public List<Category> MostSalesCategories { get; set; }
        public List<Category> MostSalesMainCategories { get; set; }

        public int DailyVisitor { get; set; }
        public int MonthlyVisitor { get; set; }
        public int TotalVisitor { get; set; }

        public int DailySales { get; set; }
        public int MonthlySales { get; set; }
        public int TotalSales { get; set; }

        public int DailyMembers { get; set; }
        public int MonthlyMembers { get; set; }
        public int TotalMembers { get; set; }
    }
}
