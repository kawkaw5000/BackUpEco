using AdminSideEcoFridge.Models;
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


    }
}
