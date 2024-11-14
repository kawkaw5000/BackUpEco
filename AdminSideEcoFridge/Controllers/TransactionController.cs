using AdminSideEcoFridge.Models;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
namespace AdminSideEcoFridge.Controllers
{
    public class TransactionController : BaseController
    {
        private string GetSubscriberName(User user)
        {
            if (user == null) return "Unknown Name";

            if (!string.IsNullOrEmpty(user.DoneeOrganizationName) && !string.IsNullOrEmpty(user.FoodBusinessName))
            {
                return user.FirstName ?? "Unknown Name";
            }
            else if (string.IsNullOrEmpty(user.DoneeOrganizationName) && string.IsNullOrEmpty(user.FirstName))
            {
                return user.FoodBusinessName ?? "Unknown Name";
            }
            else if (string.IsNullOrEmpty(user.FoodBusinessName) && string.IsNullOrEmpty(user.FirstName))
            {
                return user.DoneeOrganizationName ?? "Unknown Name";
            }
            else
            {
                return user.FirstName ?? user.FoodBusinessName ?? user.DoneeOrganizationName ?? "Unknown Name";
            }
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
        private List<VwDonationTransactionMasterUserView> SortDonations(List<VwDonationTransactionMasterUserView> donationList, string sortColumn, bool ascending)
        {
            switch (sortColumn.ToLower())
            {
                case "id":
                    donationList = ascending ? donationList.OrderBy(x => x.DonationTransactionMasterId).ToList() :
                                               donationList.OrderByDescending(x => x.DonationTransactionMasterId).ToList();
                    break;
                case "donorname":
                    donationList = ascending ? donationList.OrderBy(x => x.FirstName).ToList() :
                                               donationList.OrderByDescending(x => x.FirstName).ToList();
                    break;
                case "doneeorganizationname":
                    donationList = ascending ? donationList.OrderBy(x => x.DoneeOrgName).ToList() :
                                               donationList.OrderByDescending(x => x.DoneeOrgName).ToList();
                    break;
                case "status":
                    donationList = ascending ? donationList.OrderBy(x => x.Status).ToList() :
                                               donationList.OrderByDescending(x => x.Status).ToList();
                    break;
                case "accounttype":
                    donationList = ascending ? donationList.OrderBy(x => x.AccountType).ToList() :
                                               donationList.OrderByDescending(x => x.AccountType).ToList();
                    break;
                case "transactiondate":
                    donationList = ascending ? donationList.OrderBy(x => x.TransactionDate).ToList() :
                                               donationList.OrderByDescending(x => x.TransactionDate).ToList();
                    break;
                default:
                    break;
            }
            return donationList;
        }

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
        public IActionResult GenerateSalesReport(string period, int year)
        {
            var userSubscribers = _userPlanMgr.GetAll();
            IEnumerable<UserPlan> filteredSubscribers;

            decimal yearSum = 0;
            decimal monthSum = 0;

            if (period == "yearly")
            {
                filteredSubscribers = userSubscribers.Where(plan => plan.SubscriptionDate.Year == year);
                foreach (var plan in filteredSubscribers)
                {
                    decimal planPrice = (plan.StoragePlan != null) ? plan.StoragePlan.Price : 0.0m;
                    yearSum += planPrice;
                }
            }
            else
            {
                filteredSubscribers = userSubscribers.Where(plan =>
                    plan.SubscriptionDate.Year == year && plan.SubscriptionDate.Month == DateTime.Now.Month);
                foreach (var plan in filteredSubscribers)
                {
                    decimal planPrice = (plan.StoragePlan != null) ? plan.StoragePlan.Price : 0.0m;
                    monthSum += planPrice;
                }
            }

            decimal totalSales = (period == "yearly") ? yearSum : monthSum;

            var htmlContent = "<h1>Sales Report</h1>";
            htmlContent += $"<p>Period: {period} - {year}</p>";
            htmlContent += $"<h1>Total Sales: {totalSales:C}</h1>";

            htmlContent += "<table border='1'><tr><th>Subscriber Name</th><th>Plan</th><th>Plan Price</th><th>Subscription Date</th></tr>";

            foreach (var plan in filteredSubscribers)
            {
                string name = "Unknown Name"; 

                if (plan.User != null)
                {
                    name = GetSubscriberName(plan.User);  
                }

                string planName = plan.StoragePlan?.StoragePlanName ?? "Unknown Plan";
                decimal planPrice = plan.StoragePlan?.Price ?? 0.0m;
                string subscriptionDate = plan.SubscriptionDate.ToShortDateString();

                htmlContent += $"<tr><td>{name}</td><td>{planName}</td><td>{planPrice:C}</td><td>{subscriptionDate}</td></tr>";
            }

            htmlContent += "</table>";

            if (string.IsNullOrEmpty(htmlContent))
            {
                htmlContent = "<h1>Sales Report is empty.</h1>"; 
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    var writer = new PdfWriter(memoryStream);
                    using (var pdfDoc = new iText.Kernel.Pdf.PdfDocument(writer))
                    {
                        pdfDoc.SetTagged();
                        var converterProperties = new ConverterProperties();

                        if (!string.IsNullOrEmpty(htmlContent))
                        {
                            HtmlConverter.ConvertToPdf(htmlContent, pdfDoc, converterProperties);
                        }
                        else
                        {                         
                            HtmlConverter.ConvertToPdf("<h1>No Data Available</h1>", pdfDoc, converterProperties);
                        }
                    }

                    return File(memoryStream.ToArray(), "application/pdf", "SalesReport.pdf");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating PDF: {ex.Message}");
                return BadRequest("An error occurred while generating the report.");
            }
        }

        public IActionResult DonationTransaction(string keyword = "", string sortColumn = "TransactionDate", string sortDirection = "asc")
        {
            // Get the list of donations, either filtered by the keyword or unfiltered
            var donationList = _userSearchRepository.SearchDonation(keyword);

            // Apply sorting based on the column and direction
            if (sortDirection == "asc")
            {
                donationList = SortDonations(donationList, sortColumn, true);  // Ascending
            }
            else
            {
                donationList = SortDonations(donationList, sortColumn, false); // Descending
            }

            return View(donationList);
        }      
    }
}
