using AdminSideEcoFridge.Models;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Globalization;
namespace AdminSideEcoFridge.Controllers
{
    //[Route("api/[controller]")]
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

            var user = _db.Users.Where(m => m.StorageSize != 5).ToList();

            if (user.Count <= 1)
            {
                colors.Add($"hsl(120, 19%, 42%)");
                return colors.ToArray();
            }
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

        [HttpGet]
        public JsonResult FilterSubscriptions(DateTime startDate, DateTime endDate)
        {
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);

            var subscriptionDates = _userPlanMgr.GetAll()
                .Where(plan => plan.SubscriptionDate >= startDate && plan.SubscriptionDate <= endDate)
                .GroupBy(plan => plan.SubscriptionDate.ToString("yyyy-MM-dd"))
                .ToDictionary(g => g.Key, g => g.Count());

            return Json(subscriptionDates);
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

        public IActionResult GenerateSalesReport(string period, int? year = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var userSubscribers = _userPlanMgr.GetAll();
            IEnumerable<UserPlan> filteredSubscribers;
            decimal totalSum = 0;
            var cultureInfo = new CultureInfo("fil-PH");

            if (period == "yearly" && year.HasValue)
            {
                filteredSubscribers = userSubscribers.Where(plan => plan.SubscriptionDate.Year == year.Value);
            }
            else if (period == "daterange" && startDate.HasValue && endDate.HasValue)
            {
                filteredSubscribers = userSubscribers.Where(plan =>
                    plan.SubscriptionDate.Date >= startDate.Value.Date && plan.SubscriptionDate.Date <= endDate.Value.Date);
            }
            else
            {
                return BadRequest("Invalid period or missing parameters.");
            }

            foreach (var plan in filteredSubscribers)
            {
                totalSum += plan.StoragePlan?.Price ?? 0.0m;
            }

            decimal totalSales = totalSum;

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "EcoNewLogo.svg");
            var imageSrc = $"file:///{imagePath}";
            var htmlContent = $@"
                <div style='text-align: center; margin-bottom: 20px;'>
                    <img src='{imageSrc}' alt='Company Logo' style='width: 60px; height: 60px; margin-bottom: 10px;' />
                    <h1>Sales Report</h1>
                </div>";

            if (period == "yearly" && year.HasValue)
            {
                htmlContent += $"<p style='text-align: center;'>Period: Yearly - {year}</p>";
            }
            else if (period == "daterange" && startDate.HasValue && endDate.HasValue)
            {
                htmlContent += $"<p style='text-align: center;'>Period: {startDate.Value:MM/dd/yyyy} to {endDate.Value:MM/dd/yyyy}</p>";
            }

            htmlContent += $"<h2 style='text-align: center;'>Total Sales: {totalSales.ToString("C", cultureInfo)}</h2>";

            htmlContent += @"
                <table border='1' style='width: 100%; border-collapse: collapse; text-align: left;'>
                    <tr style='background-color: #f2f2f2;'>
                        <th style='padding: 10px;'>Subscriber Name</th>
                        <th style='padding: 10px;'>Plan</th>
                        <th style='padding: 10px;'>Plan Price</th>
                        <th style='padding: 10px;'>Subscription Date</th>
                    </tr>";

            foreach (var plan in filteredSubscribers)
            {
                string name = plan.User != null ? GetSubscriberName(plan.User) : "Unknown Name";
                string planName = plan.StoragePlan?.StoragePlanName ?? "Unknown Plan";
                decimal planPrice = plan.StoragePlan?.Price ?? 0.0m;
                string subscriptionDate = plan.SubscriptionDate.ToShortDateString();

                htmlContent += $@"
                    <tr>
                        <td style='padding: 10px;'>{name}</td>
                        <td style='padding: 10px;'>{planName}</td>
                        <td style='padding: 10px;'>{planPrice.ToString("C", cultureInfo)}</td>
                        <td style='padding: 10px;'>{subscriptionDate}</td>
                    </tr>";
            }

            htmlContent += "</table>";

            try
            {
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating PDF: {ex.Message}");
                return BadRequest("An error occurred while generating the report.");
            }
        }

        public IActionResult DonationTransaction(string keyword = "", string sortColumn = "TransactionDate", string sortDirection = "asc")
        {
            var donationList = _userSearchRepository.SearchDonation(keyword);

            if (sortDirection == "asc")
            {
                donationList = SortDonations(donationList, sortColumn, true);
            }
            else
            {
                donationList = SortDonations(donationList, sortColumn, false);
            }

            return View(donationList);
        }

        [HttpGet]
        public IActionResult GetDonationByDonoMasterId(int donorId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest(new { message = "User is not Authenticated." });
            }

            var donatedItems = _donationTransaction.donationItemsDetails(donorId);

            return Json(donatedItems);
        }
    }
}
