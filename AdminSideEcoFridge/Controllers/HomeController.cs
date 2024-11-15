using AdminSideEcoFridge.Contracts;
using AdminSideEcoFridge.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AdminSideEcoFridge.Repository;
using AdminSideEcoFridge.Utils;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace AdminSideEcoFridge.Controllers
{

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    public class HomeController : BaseController
    {
        public IActionResult Dashboard(string role = "all", string keyword = "")
        {
            if(!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            List<VwUsersRoleView> userList = _vwUsersRoleViewRepo.GetAll()
                                            .Where(user => user.UserId != UserId)
                                            .OrderByDescending(user => user.UserId)
                                            .ToList();
            List<VwUsersFoodItem> foodList = _vwUsersFoodItemRepo.GetAll();
            List<User> user = _userRepo.GetAll();

            userList = userList.ToList();

            if (!string.IsNullOrEmpty(keyword))
            {
                userList = _userSearchRepository.SearchUsers(keyword)
                    .ToList();
            }

            var roleCounts = userList
                .GroupBy(u => u.RoleName)
                .Select(group => new
                {
                    RoleName = group.Key,
                    Count = group.Count()
                })
                .ToDictionary(g => g.RoleName, g => g.Count);

            if (!string.IsNullOrEmpty(role) && role != "all")
            {
                userList = userList.Where(u => u.RoleName == role).ToList();
            }

            var viewModel = new DashboardViewModel
            {
                UserList = userList,
                FoodList = foodList,
                User = user,
                TotalUsers = userList.Count(),
                AdminCount = roleCounts.ContainsKey("admin") ? roleCounts["admin"] : 0,
                DonorCount = roleCounts.ContainsKey("personal") ? roleCounts["personal"] : 0,
                FoodBusinessCount = roleCounts.ContainsKey("food business") ? roleCounts["food business"] : 0,
                OrganizationCount = roleCounts.ContainsKey("donee organization") ? roleCounts["donee organization"] : 0
            };

            return View(viewModel);
        }

        public IActionResult GetFoodItemsByUserId(int userId)
        {
            List<VwUsersFoodItem> foodItems = _vwUsersFoodItemRepo.GetAll()
                                               .Where(f => f.UserId == userId)
                                               .ToList();
            return Json(foodItems);
        }

        public IActionResult GetUserById(int id)
        {
            var user = _userRepo.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            return Json(user);
        }

    }
}
