using AdminSideEcoFridge.Models;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Mvc;
namespace AdminSideEcoFridge.Controllers
{
    public class TransactionController : BaseController
    {
        public IActionResult Subscribers()
        {
            var userSubscribers = _userPlanMgr.GetAll();

            DateTime today = DateTime.Now;
            int activeSubscribersCount = userSubscribers.Count(plan => plan.ExpiryDate > DateTime.Now);
            int currentMonthSubscribersCount = userSubscribers.Count(plan =>
                plan.SubscriptionDate.Year == today.Year &&
                plan.SubscriptionDate.Month == today.Month &&
                plan.ExpiryDate > today);
            int currentYearSubscribersCount = userSubscribers.Count(plan =>
                plan.SubscriptionDate.Year == today.Year &&
                plan.ExpiryDate > today);
            int[] yearlyCounts = new int[7];

            var storagePlanCounts = new Dictionary<string, int>(); 

            foreach (var plan in userSubscribers)
            {
                if (plan.SubscriptionDate.Year >= 2024 && plan.SubscriptionDate.Year <= 2030)
                {
                    yearlyCounts[plan.SubscriptionDate.Year - 2024]++;

                    if (plan.StoragePlan != null) 
                    {
                        string storagePlanName = plan.StoragePlan.StoragePlanName; 

                        if (!storagePlanCounts.ContainsKey(storagePlanName))
                        {
                            storagePlanCounts[storagePlanName] = 0;
                        }
                        storagePlanCounts[storagePlanName]++;
                    }
                }
            }

            var pieChartData = new
            {
                labels = storagePlanCounts.Keys.Select(k => k.ToString()).ToArray(),
                data = storagePlanCounts.Values.ToArray(),
                colors = GenerateColors(storagePlanCounts.Count)
            };

            var monthlyCounts = new Dictionary<int, int[]>();

            for (int year = 2024; year <= 2030; year++)
            {
                monthlyCounts[year] = new int[12];
            }

            foreach (var plan in userSubscribers)
            {
                if (plan.SubscriptionDate.Year >= 2024 && plan.SubscriptionDate.Year <= 2030)
                {
                    int month = plan.SubscriptionDate.Month - 1; 
                    if (plan.SubscriptionDate.Month >= 1 && plan.SubscriptionDate.Month <= 12) 
                    {
                        monthlyCounts[plan.SubscriptionDate.Year][month]++;
                    }
                }
            }

            var model = new
            {

                YearlyCounts = yearlyCounts,
                MonthlyCounts = monthlyCounts.ToDictionary(x => x.Key, x => x.Value), 
                PieChartData = pieChartData,
                CurrentMonthSubscribersCount = currentMonthSubscribersCount,
                CurrentYearSubscribersCount = currentYearSubscribersCount,
                ActiveSubscribersCount = activeSubscribersCount
            };

            return View(model);
        }

        private string[] GenerateColors(int count)
        {
            var colors = new List<string>();
            for (int i = 0; i < count; i++)
            {
                var lightness = 80 - (i * 60 / (count - 1));
                var saturation = 30 + (i * 40 / (count - 1));

                colors.Add($"hsl(120, {saturation}%, {lightness}%)");
            }
            return colors.ToArray();
        }

        public IActionResult GenerateSalesReport(string period, int year)
        {

            var userSubscribers = _userPlanMgr.GetAll();
            IEnumerable<UserPlan> filteredSubscribers;

            if (period == "yearly")
            {
                filteredSubscribers = userSubscribers.Where(plan => plan.SubscriptionDate.Year == year);
            }
            else
            {
                filteredSubscribers = userSubscribers.Where(plan =>
                    plan.SubscriptionDate.Year == year && plan.SubscriptionDate.Month == DateTime.Now.Month);
            }

            var htmlContent = "<h1>Sales Report</h1>";
            htmlContent += $"<p>Period: {period} - {year}</p>";
            htmlContent += "<table border='1'><tr><th>Subscriber Name</th><th>Plan</th><th>Plan Price</th><th>Subscription Date</th></tr>";

            foreach (var plan in filteredSubscribers)
            {
                string name;

                if (!string.IsNullOrEmpty(plan.User?.DoneeOrganizationName) && !string.IsNullOrEmpty(plan.User?.FoodBusinessName))
                {
                    name = plan.User.FirstName;
                }
                else if (string.IsNullOrEmpty(plan.User?.DoneeOrganizationName) && string.IsNullOrEmpty(plan.User?.FirstName))
                {
                    name = plan.User.FoodBusinessName;
                }
                else if (string.IsNullOrEmpty(plan.User?.FoodBusinessName) && string.IsNullOrEmpty(plan.User?.FirstName))
                {
                    name = plan.User.DoneeOrganizationName;
                }
                else
                {
                    name = !string.IsNullOrEmpty(plan.User?.FirstName)
                        ? plan.User.FirstName
                        : (!string.IsNullOrEmpty(plan.User?.FoodBusinessName)
                            ? plan.User.FoodBusinessName
                            : plan.User.DoneeOrganizationName);
                }

                htmlContent += $"<tr><td>{name}</td><td>{plan.StoragePlan?.StoragePlanName}</td><td>{plan.StoragePlan?.Price}</td><td>{plan.SubscriptionDate.ToShortDateString()}</td></tr>";
            }

            htmlContent += "</table>";

            using (var memoryStream = new MemoryStream())
            {
                var writer = new PdfWriter(memoryStream);
                using (var pdfDoc = new iText.Kernel.Pdf.PdfDocument(writer))
                {
                    pdfDoc.SetTagged();
                    var converterProperties = new ConverterProperties();
                    HtmlConverter.ConvertToPdf(htmlContent, pdfDoc, converterProperties);
                }

                return File(memoryStream.ToArray(), "application/pdf", "SalesReport.pdf");
            }
        }
    }
}
