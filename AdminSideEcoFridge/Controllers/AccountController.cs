using AdminSideEcoFridge.Models;
using AdminSideEcoFridge.Models.CustomModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AdminSideEcoFridge.Utils;
using AdminSideEcoFridge.Repository;
using Microsoft.AspNetCore.Authorization;

namespace AdminSideEcoFridge.Controllers
{
    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    public class AccountController : BaseController
    {
        #region Login Authentication -
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Home");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(CustomUserModelForLogIn user)
        {
           
            var userObj = _db.Users.Where(model => (model.Email == user.Email || model.Email == user.Email)).FirstOrDefault();

            if (userObj == null || userObj.EmailConfirmed == false)
            {
                ViewData["ErrorMessage"] = "Incorrect Password or User does not exist.";
                return View();
            }

            if (user.Password != userObj.Password)
            {
                ViewData["ErrorMessage"] = "Incorrect Password or User does not exist.";
                return View();
            }

            var userRoleVw = _db.VwUsersRoleViews.Where(m => m.UserId == userObj.UserId).FirstOrDefault();

            if (userRoleVw == null || String.IsNullOrEmpty(userRoleVw.RoleName))
            {
                return View();
            }

            ViewData["ErrorMessage"] = null;

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.GivenName, userObj.FirstName),
                new Claim("ProfilePicture", userObj.ProfilePicturePath ?? "/images/default-profile.png"),
                new Claim(ClaimsIdentity.DefaultNameClaimType, Convert.ToString(userObj.UserId)),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, userRoleVw.RoleName)
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
        #endregion

        #region Edit Section -
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _userRepo.Get(id);            

            if (user == null)
            {
                return NotFound();
            }

            return PartialView("_EditUserPartialView", user);
        }

        [HttpPost]
        public IActionResult Edit(User u)
        {
            if (u == null)
            {
                return RedirectToAction("Login");
            }

            var existingUser = _userRepo.Get(u.UserId);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.FirstName = u.FirstName;
            existingUser.LastName = u.LastName;
            existingUser.Gender = u.Gender;
            existingUser.Birthdate = u.Birthdate;
            existingUser.Barangay = u.Barangay;
            existingUser.City = u.City;
            existingUser.Province = u.Province;

            if (!string.IsNullOrEmpty(u.ProfilePicturePath))
            {
                existingUser.ProfilePicturePath = u.ProfilePicturePath;
            }

            var result = _userRepo.Update(existingUser.UserId, existingUser);

            if (result == ErrorCode.Success)
            {
                TempData["Msg"] = $"User {u.Email} Updated";
                return RedirectToAction("Dashboard", "Home");
            }

            ModelState.AddModelError("", "Unable to update user. Please try again.");
            return View(u);
        }


        [HttpGet]
        public IActionResult EditOrg(int id)
        {
            var user = _userRepo.Get(id);

            if (user == null)
            {
                return NotFound();
            }


            return PartialView("_EditOrgPartialView", user);
        }

        [HttpPost]
        public IActionResult EditOrg(User u)
        {
            if (u == null)
            {
                return RedirectToAction("Login");
            }

            var existingUser = _userRepo.Get(u.UserId);
            if (existingUser == null)
            {
                return NotFound();
            }

            if (u.AccountApproved != null)
            {
                existingUser.AccountApproved = u.AccountApproved;
            }

            existingUser.FirstName = u.FirstName;
            existingUser.LastName = u.LastName;
            existingUser.Gender = u.Gender;
            existingUser.Birthdate = u.Birthdate;
            existingUser.Barangay = u.Barangay;
            existingUser.City = u.City;
            existingUser.Province = u.Province;
            existingUser.DoneeOrganizationName = u.DoneeOrganizationName;

            if (!string.IsNullOrEmpty(u.ProfilePicturePath))
            {
                existingUser.ProfilePicturePath = u.ProfilePicturePath;
            }

            if (!string.IsNullOrEmpty(u.ProofPicturePath))
            {
                existingUser.ProofPicturePath = u.ProofPicturePath;
            }

            var result = _userRepo.Update(existingUser.UserId, existingUser);

            if (result == ErrorCode.Success)
            {
                TempData["Msg"] = $"User {u.Email} Updated";
                return RedirectToAction("Dashboard", "Home");
            }

            ModelState.AddModelError("", "Unable to update user. Please try again.");
            return View(u);
        }


        [HttpGet]
        public IActionResult EditFoodResto(int id)
        {
            var user = _userRepo.Get(id);

            if (user == null)
            {
                return NotFound();
            }


            return PartialView("_EditFoodOrgPartialView", user);
        }

        [HttpPost]
        public IActionResult EditFoodResto(User u)
        {

            if (u == null)
            {
                return RedirectToAction("Login");
            }

            var existingUser = _userRepo.Get(u.UserId);
            if (existingUser == null)
            {
                return NotFound();
            }

            if (u.AccountApproved != null)
            {
                existingUser.AccountApproved = u.AccountApproved;
            }

            existingUser.FirstName = u.FirstName;
            existingUser.LastName = u.LastName;
            existingUser.Gender = u.Gender;
            existingUser.Birthdate = u.Birthdate;
            existingUser.Barangay = u.Barangay;
            existingUser.City = u.City;
            existingUser.Province = u.Province;
            existingUser.FoodBusinessName = u.FoodBusinessName;
           

            if (!string.IsNullOrEmpty(u.ProfilePicturePath))
            {
                existingUser.ProfilePicturePath = u.ProfilePicturePath;
            }

            if (!string.IsNullOrEmpty(u.ProofPicturePath))
            {
                existingUser.ProfilePicturePath = u.ProfilePicturePath;
            }

          

            var result = _userRepo.Update(existingUser.UserId, existingUser);

            if (result == ErrorCode.Success)
            {
                TempData["Msg"] = $"User {u.Email} Updated";
                return RedirectToAction("Dashboard", "Home");
            }

            ModelState.AddModelError("", "Unable to update user. Please try again.");
            return View(u);
        }

        [HttpPost]
        public JsonResult UpdateAccountApproval(string userId, bool isApproved)
        {
            if (int.TryParse(userId, out int parsedUserId))
            {     
                var user = _userRepo.Get(parsedUserId);

                if (user != null)
                {
                    user.AccountApproved = isApproved;

                    var result = _userRepo.Update(user.UserId, user);

                    if (result == ErrorCode.Success)
                    {
                        return Json(new { success = true, message = isApproved ? "User approved" : "User declined" });
                    }

                    return Json(new { success = false, message = "Error updating user" });
                }

                return Json(new { success = false, message = "User not found" });
            }

            return Json(new { success = false, message = "Invalid user ID" });
        }


        #endregion

        #region Account Creation -
        [Authorize(Policy = "AdminOrSuperAdminPolicy")]
        [HttpGet]
        public IActionResult AdminCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminCreate(User user, IFormFile ProfilePicturePath)
        {
            const long MaxFileSize = 2 * 1024 * 1024; // 2 MB
            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var allowedEmailDomains = new[] { "gmail.com", "yahoo.com", "ymail.com" };

            //Check if email already exists
            var existingEmail = _db.Users.FirstOrDefault(model => model.Email == user.Email);
            if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "Email is already taken.");
            }

            //Check if password and confirm password match
            if (user.Password != user.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Password and Confirm Password don't match.");
            }

            //Email domain validation
            var emailDomain = user.Email.Split('@').Last();
            if (!allowedEmailDomains.Contains(emailDomain))
            {
                ModelState.AddModelError("Email", "Please use a valid email.");
            }

            //Profile picture validation
            if (ProfilePicturePath != null && ProfilePicturePath.Length > 0)
            {
                var profileExtension = Path.GetExtension(ProfilePicturePath.FileName).ToLower();
                if (!allowedImageExtensions.Contains(profileExtension))
                {
                    ModelState.AddModelError("ProfilePicturePath", "Profile picture must be a .jpg, .jpeg, or .png file.");
                }
                if (ProfilePicturePath.Length > MaxFileSize)
                {
                    ModelState.AddModelError("ProfilePicturePath", "Profile picture must be less than 2 MB.");
                }
            }

            //Return to view if validation fails
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            //Set default values for fields
            user.FirstName = " ";
            user.LastName = " ";
            user.Gender = "M";
            user.Barangay = " ";
            user.City = " ";
            user.Province = " ";
            user.Birthdate = DateOnly.FromDateTime(DateTime.Now);
            user.AccountApproved = true;

            //Profile picture upload logic
            if (ProfilePicturePath != null && ProfilePicturePath.Length > 0)
            {
                var profileExtension = Path.GetExtension(ProfilePicturePath.FileName).ToLower();
                var profileFileName = "profile_" + Guid.NewGuid() + profileExtension;
                var profileFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles/adminProfiles/", profileFileName);

                try
                {
                    using (var stream = new FileStream(profileFilePath, FileMode.Create))
                    {
                        ProfilePicturePath.CopyTo(stream);
                    }
                    user.ProfilePicturePath = "/images/profiles/adminProfiles/" + profileFileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ProfilePicturePath", "Error uploading profile picture: " + ex.Message);
                    return View(user);
                }
            }
            else
            {
                user.ProfilePicturePath = "/images/profiles/default.png";
            }

            //Save user and assign role
            if (_userRepo.Create(user) == ErrorCode.Success)
            {
                var adminRole = _roleRepo.GetAll().FirstOrDefault(r => r.RoleName == "admin");
                if (adminRole != null)
                {
                    var userRole = new UserRole
                    {
                        UserId = user.UserId,
                        RoleId = adminRole.RoleId
                    };
                    _userRoleRepo.Create(userRole);
                }
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                ViewData["ErrorMessage"] = "An error occurred while creating the admin.";
                return View();
            }
        }

        [Authorize(Policy = "AdminOrSuperAdminPolicy")]
        [HttpGet]
        public IActionResult RegularCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegularCreate(User user, IFormFile ProfilePicturePath)
        {
            const long MaxFileSize = 2 * 1024 * 1024; // 2 MB
            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var allowedEmailDomains = new[] { "gmail.com", "yahoo.com", "ymail.com" };

            //Check if email already exists
            var existingEmail = _db.Users.FirstOrDefault(model => model.Email == user.Email);
            if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "Email is already taken.");
            }

            //Check if password and confirm password match
            if (user.Password != user.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Password and Confirm Password don't match.");
            }
     
            //Email domain validation
            var emailDomain = user.Email.Split('@').Last();
            if (!allowedEmailDomains.Contains(emailDomain))
            {
                ModelState.AddModelError("Email", "Please use a valid email.");
            }

            //Profile picture validation
            if (ProfilePicturePath != null && ProfilePicturePath.Length > 0)
            {
                var profileExtension = Path.GetExtension(ProfilePicturePath.FileName).ToLower();
                if (!allowedImageExtensions.Contains(profileExtension))
                {
                    ModelState.AddModelError("ProfilePicturePath", "Profile picture must be a .jpg, .jpeg, or .png file.");
                }
                if (ProfilePicturePath.Length > MaxFileSize)
                {
                    ModelState.AddModelError("ProfilePicturePath", "Profile picture must be less than 2 MB.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(user);
            }
            //Set blank inputs
            //user.FirstName = " ";
            //user.LastName = " ";
            //user.Gender = "M";
            //user.Barangay = " ";
            //user.City = " ";
            //user.Street = " ";
            //user.Province = " ";
            //user.Birthdate = DateOnly.FromDateTime(DateTime.Now);
            user.AccountApproved = true;

            //Upload File Trappings
            if (ProfilePicturePath != null && ProfilePicturePath.Length > 0)
            {
                var profileExtension = Path.GetExtension(ProfilePicturePath.FileName).ToLower();
                var profileFileName = "profile_" + Guid.NewGuid() + profileExtension;
                var profileFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles/userProfiles/", profileFileName);

                try
                {
                    using (var stream = new FileStream(profileFilePath, FileMode.Create))
                    {
                        ProfilePicturePath.CopyTo(stream);
                    }
                    user.ProfilePicturePath = "/images/profiles/userProfiles/" + profileFileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ProfilePicturePath", "Error uploading profile picture: " + ex.Message);
                    return View(user);
                }
            }
            else
            {
                user.ProfilePicturePath = "/images/profiles/default.png";
            }

            //Save user and assign role
            if (_userRepo.Create(user) == ErrorCode.Success)
            {
                var adminRole = _roleRepo.GetAll().FirstOrDefault(r => r.RoleName == "personal");
                if (adminRole != null)
                {
                    var userRole = new UserRole
                    {
                        UserId = user.UserId,
                        RoleId = adminRole.RoleId
                    };
                    _userRoleRepo.Create(userRole);
                }
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                ViewData["ErrorMessage"] = "An error occurred while creating the personal.";
                return View();
            }
        }

        [Authorize(Policy = "AdminOrSuperAdminPolicy")]
        [HttpGet]
        public IActionResult FoodBusinessCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FoodBusinessCreate(User user, IFormFile ProfilePicturePath, IFormFile ProofPicturePath)
        {
            const long MaxFileSize = 2 * 1024 * 1024; // 2 MB
            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var allowedEmailDomains = new[] { "gmail.com", "yahoo.com", "ymail.com" };

            //Check if email already exists
            var existingEmail = _db.Users.FirstOrDefault(model => model.Email == user.Email);
            if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "Email is already taken.");
            }

            //Check if password and confirm password match
            if (user.Password != user.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Password and Confirm Password don't match.");
            }

            //Email domain validation
            var emailDomain = user.Email.Split('@').Last();
            if (!allowedEmailDomains.Contains(emailDomain))
            {
                ModelState.AddModelError("Email", "Please use a valid email.");
            }

            //Check if Food Business Name already exists
            var existFoodBusinessName = _db.Users.FirstOrDefault(model => model.FoodBusinessName == user.FoodBusinessName);
            if (existFoodBusinessName != null)
            {
                ModelState.AddModelError("FoodBusinessName", "Food Business Name already exists.");
            }

            //Profile picture validation
            if (ProfilePicturePath != null && ProfilePicturePath.Length > 0)
            {
                var profileExtension = Path.GetExtension(ProfilePicturePath.FileName).ToLower();
                if (!allowedImageExtensions.Contains(profileExtension))
                {
                    ModelState.AddModelError("ProfilePicturePath", "must be a .jpg, .jpeg, or .png file.");
                }
                if (ProfilePicturePath.Length > MaxFileSize)
                {
                    ModelState.AddModelError("ProfilePicturePath", "must be less than 2 MB.");
                }
            }

            //Proof picture validation
            if (ProofPicturePath != null && ProofPicturePath.Length > 0)
            {
                var proofExtension = Path.GetExtension(ProofPicturePath.FileName).ToLower();
                if (!allowedImageExtensions.Contains(proofExtension))
                {
                    ModelState.AddModelError("ProofPicturePath", "must be a .jpg, .jpeg, or .png file.");
                }
                if (ProofPicturePath.Length > MaxFileSize)
                {
                    ModelState.AddModelError("ProofPicturePath", "must be less than 2 MB.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            //Set blank inputs
            user.FirstName = " ";
            user.LastName = " ";
            user.Gender = "M";
            user.Birthdate = DateOnly.FromDateTime(DateTime.Now);

            //Handle Profile Picture upload
            if (ProfilePicturePath != null && ProfilePicturePath.Length > 0)
            {
                var profileExtension = Path.GetExtension(ProfilePicturePath.FileName).ToLower();
                var profileFileName = "profile_" + Guid.NewGuid() + profileExtension;
                var profileFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles/foodProfiles/", profileFileName);

                try
                {
                    using (var stream = new FileStream(profileFilePath, FileMode.Create))
                    {
                        ProfilePicturePath.CopyTo(stream);
                    }
                    user.ProfilePicturePath = "/images/profiles/foodProfiles/" + profileFileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ProfilePicturePath", "Error uploading profile picture: " + ex.Message);
                    return View(user);
                }
            }
            else
            {
                user.ProfilePicturePath = "/images/profiles/default.png";
            }

            //Handle Proof Picture upload
            if (ProofPicturePath != null && ProofPicturePath.Length > 0)
            {
                var proofExtension = Path.GetExtension(ProofPicturePath.FileName).ToLower();
                var proofFileName = "proof_" + Guid.NewGuid() + proofExtension;
                var proofFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/proofs/foodProofs/", proofFileName);

                try
                {
                    using (var stream = new FileStream(proofFilePath, FileMode.Create))
                    {
                        ProofPicturePath.CopyTo(stream);
                    }
                    user.ProofPicturePath = "/images/proofs/foodProofs/" + proofFileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ProofPicturePath", "Error uploading proof picture: " + ex.Message);
                    return View(user);
                }
            }
            else
            {
                user.ProofPicturePath = "/images/proofs/default.png";
            }

            //Save user and assign role
            if (_userRepo.Create(user) == ErrorCode.Success)
            {
                var adminRole = _roleRepo.GetAll().FirstOrDefault(r => r.RoleName == "food business");
                if (adminRole != null)
                {
                    var userRole = new UserRole
                    {
                        UserId = user.UserId,
                        RoleId = adminRole.RoleId
                    };
                    _userRoleRepo.Create(userRole);
                }
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                ViewData["ErrorMessage"] = "An error occurred while creating the food business account.";
                return View(user);
            }
        }

        [Authorize(Policy = "AdminOrSuperAdminPolicy")]
        [HttpGet]
        public IActionResult OrganizationCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OrganizationCreate(User user, IFormFile ProfilePicturePath, IFormFile ProofPicturePath)
        {
            const long MaxFileSize = 2 * 1024 * 1024; // 2 MB
            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var allowedEmailDomains = new[] { "gmail.com", "yahoo.com", "ymail.com" };

            //Check if email already exists
            var existingEmail = _db.Users.FirstOrDefault(model => model.Email == user.Email);
            if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "Email is already taken.");
            }

            //Check if password and confirm password match
            if (user.Password != user.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Password and Confirm Password don't match.");
            }

            //Email domain validation
            var emailDomain = user.Email.Split('@').Last();
            if (!allowedEmailDomains.Contains(emailDomain))
            {
                ModelState.AddModelError("Email", "Please use a valid email.");
            }

            //Check if Food Business Name already exists
            var existDoneeOrganizationName = _db.Users.FirstOrDefault(model => model.DoneeOrganizationName == user.DoneeOrganizationName);
            if (existDoneeOrganizationName != null)
            {
                ModelState.AddModelError("DoneeOrganizationName", "Organization Name already exists.");
            }

            //Profile picture validation
            if (ProfilePicturePath != null && ProfilePicturePath.Length > 0)
            {
                var profileExtension = Path.GetExtension(ProfilePicturePath.FileName).ToLower();
                if (!allowedImageExtensions.Contains(profileExtension))
                {
                    ModelState.AddModelError("ProfilePicturePath", "must be a .jpg, .jpeg, or .png file.");
                }
                if (ProfilePicturePath.Length > MaxFileSize)
                {
                    ModelState.AddModelError("ProfilePicturePath", "must be less than 2 MB.");
                }
            }

            //Proof picture validation
            if (ProofPicturePath != null && ProofPicturePath.Length > 0)
            {
                var proofExtension = Path.GetExtension(ProofPicturePath.FileName).ToLower();
                if (!allowedImageExtensions.Contains(proofExtension))
                {
                    ModelState.AddModelError("ProofPicturePath", "must be a .jpg, .jpeg, or .png file.");
                }
                if (ProofPicturePath.Length > MaxFileSize)
                {
                    ModelState.AddModelError("ProofPicturePath", "must be less than 2 MB.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            //Set blank inputs
            user.FirstName = " ";
            user.LastName = " ";
            user.Gender = "M";
            user.Birthdate = DateOnly.FromDateTime(DateTime.Now);

            //Handle Profile Picture upload
            if (ProfilePicturePath != null && ProfilePicturePath.Length > 0)
            {
                var profileExtension = Path.GetExtension(ProfilePicturePath.FileName).ToLower();
                var profileFileName = "profile_" + Guid.NewGuid() + profileExtension;
                var profileFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles/orgProfiles/", profileFileName);

                try
                {
                    using (var stream = new FileStream(profileFilePath, FileMode.Create))
                    {
                        ProfilePicturePath.CopyTo(stream);
                    }
                    user.ProfilePicturePath = "/images/profiles/orgProfiles/" + profileFileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ProfilePicturePath", "Error uploading profile picture: " + ex.Message);
                    return View(user);
                }
            }
            else
            {
                user.ProfilePicturePath = "/images/profiles/default.png";
            }

            //Handle Proof Picture upload
            if (ProofPicturePath != null && ProofPicturePath.Length > 0)
            {
                var proofExtension = Path.GetExtension(ProofPicturePath.FileName).ToLower();
                var proofFileName = "proof_" + Guid.NewGuid() + proofExtension;
                var proofFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/proofs/organizationProofs/", proofFileName);

                try
                {
                    using (var stream = new FileStream(proofFilePath, FileMode.Create))
                    {
                        ProofPicturePath.CopyTo(stream);
                    }
                    user.ProofPicturePath = "/images/proofs/organizationProofs/" + proofFileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ProofPicturePath", "Error uploading proof picture: " + ex.Message);
                    return View(user);
                }
            }
            else
            {
                user.ProofPicturePath = "/images/proofs/default.png";
            }

            //Save user and assign role
            if (_userRepo.Create(user) == ErrorCode.Success)
            {
                var adminRole = _roleRepo.GetAll().FirstOrDefault(r => r.RoleName == "donee organization");
                if (adminRole != null)
                {
                    var userRole = new UserRole
                    {
                        UserId = user.UserId,
                        RoleId = adminRole.RoleId
                    };
                    _userRoleRepo.Create(userRole);
                }
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                ViewData["ErrorMessage"] = "An error occurred while creating the donee organization account.";
                return View(user);
            }
        }
        #endregion

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (_userRepo.Delete(id) == ErrorCode.Success)
            {
                return Ok();
            }
            return RedirectToAction("Dashboard", "Home");
        }

    }
}
