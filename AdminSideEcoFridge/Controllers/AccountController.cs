using AdminSideEcoFridge.Models;
using AdminSideEcoFridge.Models.CustomModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AdminSideEcoFridge.Utils;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Net;

namespace AdminSideEcoFridge.Controllers
{
    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    public class AccountController : BaseController
    {
        IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AccountController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

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

        public IActionResult ChangePass()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePass(User u)
        {
            if (u == null)
            {
                return NotFound();
            }

            var authUser = _userRepo.Get(UserId);
            if (authUser == null)
            {
                return NotFound();
            }

            if (u.CurrentPassword != authUser.Password)
            {
                ModelState.AddModelError("CurrentPassword", "Incorrect current password!");
            }

            if (u.NewPassword != u.ConfirmNewPassword)
            {
                ModelState.AddModelError("NewPassword", "New and Confirm password do not match!");
            }

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(u);
            }

            authUser.Password = u.NewPassword;
            var result = _userRepo.Update(authUser.UserId, authUser);

            if (result == ErrorCode.Success)
            {

                var sendersEmail = _configuration["EmailSettings:SendersEmail"];
                var sendersPassword = _configuration["EmailSettings:SendersPassword"];
                var noreplyEmail = "no-reply@ecofridge.com";
                var subject = "Change Password Notice";
                var body = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4;'>
                    <div style='max-width: 600px; margin: 0 auto; background-color: white; padding: 20px; border-radius: 10px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
                        <h2 style='color: #333;'>Change Password Confirmation</h2>
                        <p>Hello from EcoFridge,</p>
                        <p>We’re writing to confirm that your password has been successfully updated.</p>
                        <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;' />
                        <p>If you did not request this change, please contact our support team immediately.</p>
                        <p>Thank you,</p>
                        <p><strong>Team Snackers</strong></p>
                    </div>
                </div>";
                try
                {
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(noreplyEmail);
                        message.To.Add(authUser.Email); 
                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = true;

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.Credentials = new NetworkCredential(sendersEmail, sendersPassword);
                            smtp.EnableSsl = true;
                            smtp.Send(message);
                        }
                    }

                    TempData["Msg"] = $"Password for {authUser.Email} has been updated.";
                    return Json(new { success = true, message = "Sent successfully!" });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                    return Json(new { success = false, message = "Error sending approval email." });
                }
            }
            ModelState.AddModelError("", "An error occurred while updating the password.");
            return View(u);
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
                return NotFound();
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
                return NotFound();
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
                return NotFound();
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

        

        #endregion

        #region Account Creation -
        [Authorize(Policy = "AdminOrSuperAdminPolicy")]
        [HttpGet]
        public IActionResult AdminCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminCreate(User user, IFormFile? ProfilePicturePath)
        {
            const long MaxFileSize = 2 * 1024 * 1024; // 2 MB
            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var allowedEmailDomains = new[] { "gmail.com", "yahoo.com", "ymail.com" };

            // Generate a temporary password and set default values
            Guid guid = Guid.NewGuid();
            user.Password = guid.ToString("N").Substring(0, 8);
            user.FirstName = " ";
            user.LastName = " ";
            user.Gender = "M";
            user.Barangay = " ";
            user.City = " ";
            user.Province = " ";
            user.Birthdate = DateOnly.FromDateTime(DateTime.Now);
            user.AccountApproved = true;

            // Check if email already exists
            var existingEmail = _db.Users.FirstOrDefault(model => model.Email == user.Email);
            if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "Email is already taken.");
            }

            // Email domain validation
            var emailDomain = user.Email.Split('@').Last();
            if (!allowedEmailDomains.Contains(emailDomain))
            {
                ModelState.AddModelError("Email", "Please use a valid email.");
            }

            // Profile picture validation
            if (ProfilePicturePath != null && ProfilePicturePath.Length > 0)
            {
                var profileExtension = Path.GetExtension(ProfilePicturePath.FileName).ToLower();

                // Check file extension
                if (!allowedImageExtensions.Contains(profileExtension))
                {
                    ModelState.AddModelError("ProfilePicturePath", "Profile picture must be a .jpg, .jpeg, or .png file.");
                }

                // Check file size
                if (ProfilePicturePath.Length > MaxFileSize)
                {
                    ModelState.AddModelError("ProfilePicturePath", "Profile picture must be less than 2 MB.");
                }

                // Only attempt to save if the ModelState is still valid
                if (ModelState.IsValid)
                {
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
            }
            else
            {
                // No profile picture uploaded; set to default image
                user.ProfilePicturePath = "/images/profiles/default.png";
            }

            // Check if ModelState is valid before proceeding
            if (!ModelState.IsValid)
            {
                // Log the errors for debugging
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(user);
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

                if (user.Password != null)
                {
                    var sendersEmail = _configuration["EmailSettings:SendersEmail"];
                    var sendersPassword = _configuration["EmailSettings:SendersPassword"];
                    var noreplyEmail = "no-reply@ecofridge.com";
                    var subject = "Temporary Password";

                    var body = $@"
                            <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4;'>
                                <div style='max-width: 600px; margin: 0 auto; background-color: white; padding: 20px; border-radius: 10px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
                                    <h2 style='color: #333;'>Temporary Password</h2>
                                    <p>Hello,</p>
                                    <p>Your new temporary password is:</p>
                                    <p style='font-size: 18px; font-weight: bold; color: #307a59;'>{user.Password}</p>
                                    <p>You can change it in your profile settings once you log in.</p>
                                    <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;' />
                                    <p>If you didn't request this, please ignore this email or contact support.</p>
                                    <p>Thank you,</p>
                                    <p><strong>Team Snackers</strong></p>
                                </div>
                            </div>";

                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(noreplyEmail);
                        message.To.Add(user.Email);
                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = true;

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.Credentials = new NetworkCredential(sendersEmail, sendersPassword);
                            smtp.EnableSsl = true;
                            smtp.Send(message);
                        }
                    }
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
        public IActionResult RegularCreate(User user, IFormFile? ProfilePicturePath)
        {
            const long MaxFileSize = 2 * 1024 * 1024;
            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var allowedEmailDomains = new[] { "gmail.com", "yahoo.com", "ymail.com" };
            Guid guid = Guid.NewGuid();
            user.Password = guid.ToString("N").Substring(0, 8);

            var existingEmail = _db.Users.FirstOrDefault(model => model.Email == user.Email);
            if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "Email is already taken.");
            }


            var emailDomain = user.Email.Split('@').Last();
            if (!allowedEmailDomains.Contains(emailDomain))
            {
                ModelState.AddModelError("Email", "Please use a valid email.");
            }

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

                if (user.Password != null)
                {
                    var sendersEmail = _configuration["EmailSettings:SendersEmail"];
                    var sendersPassword = _configuration["EmailSettings:SendersPassword"];
                    var noreplyEmail = "no-reply@ecofridge.com";
                    var subject = "Temporary Password";

                    var body = $@"
                            <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4;'>
                                <div style='max-width: 600px; margin: 0 auto; background-color: white; padding: 20px; border-radius: 10px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
                                    <h2 style='color: #333;'>Temporary Password</h2>
                                    <p>Hello,{user.FirstName}</p>
                                    <p>Your new temporary password is:</p>
                                    <p style='font-size: 18px; font-weight: bold; color: #307a59;'>{user.Password}</p>
                                    <p>You can change it in your profile settings once you log in.</p>
                                    <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;' />
                                    <p>If you didn't request this, please ignore this email or contact support.</p>
                                    <p>Thank you,</p>
                                    <p><strong>Team Snackers</strong></p>
                                </div>
                            </div>";

                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(noreplyEmail);
                        message.To.Add(user.Email);
                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = true;

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.Credentials = new NetworkCredential(sendersEmail, sendersPassword);
                            smtp.EnableSsl = true;
                            smtp.Send(message);
                        }
                    }
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
        public IActionResult FoodBusinessCreate(User user, IFormFile? ProfilePicturePath, IFormFile ProofPicturePath)
        {
            const long MaxFileSize = 2 * 1024 * 1024; // 2 MB
            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var allowedEmailDomains = new[] { "gmail.com", "yahoo.com", "ymail.com" };
            Guid guid = Guid.NewGuid();
            user.Password = guid.ToString("N").Substring(0, 8);
            user.FirstName = " ";
            user.LastName = " ";
            user.Gender = "M";
            user.Birthdate = DateOnly.FromDateTime(DateTime.Now);
            user.AccountApproved = false;

            //Check if email already exists
            var existingEmail = _db.Users.FirstOrDefault(model => model.Email == user.Email);
            if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "Email is already taken.");
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

                if (user.Password != null)
                {
                    var sendersEmail = _configuration["EmailSettings:SendersEmail"];
                    var sendersPassword = _configuration["EmailSettings:SendersPassword"];
                    var noreplyEmail = "no-reply@ecofridge.com";
                    var subject = "Temporary Password";

                    var body = $@"
                            <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4;'>
                                <div style='max-width: 600px; margin: 0 auto; background-color: white; padding: 20px; border-radius: 10px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
                                    <h2 style='color: #333;'>Temporary Password</h2>
                                    <p>Hello,{user.FoodBusinessName}</p>
                                    <p>Your new temporary password is:</p>
                                    <p style='font-size: 18px; font-weight: bold; color: #307a59;'>{user.Password}</p>
                                    <p>You can change it in your profile settings once you log in.</p>
                                    <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;' />
                                    <p>If you didn't request this, please ignore this email or contact support.</p>
                                    <p>Thank you,</p>
                                    <p><strong>Team Snackers</strong></p>
                                </div>
                            </div>";

                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(noreplyEmail);
                        message.To.Add(user.Email);
                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = true;

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.Credentials = new NetworkCredential(sendersEmail, sendersPassword);
                            smtp.EnableSsl = true;
                            smtp.Send(message);
                        }
                    }
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
        public IActionResult OrganizationCreate(User user, IFormFile? ProfilePicturePath, IFormFile ProofPicturePath)
        {
            const long MaxFileSize = 2 * 1024 * 1024; // 2 MB
            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var allowedEmailDomains = new[] { "gmail.com", "yahoo.com", "ymail.com" };
            Guid guid = Guid.NewGuid();
            user.Password = guid.ToString("N").Substring(0, 8);
            user.FirstName = " ";
            user.LastName = " ";
            user.Gender = "M";
            user.Birthdate = DateOnly.FromDateTime(DateTime.Now);
            user.AccountApproved = false;

            //Check if email already exists
            var existingEmail = _db.Users.FirstOrDefault(model => model.Email == user.Email);
            if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "Email is already taken.");
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

                if (user.Password != null)
                {
                    var sendersEmail = _configuration["EmailSettings:SendersEmail"];
                    var sendersPassword = _configuration["EmailSettings:SendersPassword"];
                    var noreplyEmail = "no-reply@ecofridge.com";
                    var subject = "Temporary Password";

                    var body = $@"
                            <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4;'>
                                <div style='max-width: 600px; margin: 0 auto; background-color: white; padding: 20px; border-radius: 10px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
                                    <h2 style='color: #333;'>Temporary Password</h2>
                                    <p>Hello,{user.DoneeOrganizationName}</p>
                                    <p>Your new temporary password is:</p>
                                    <p style='font-size: 18px; font-weight: bold; color: #307a59;'>{user.Password}</p>
                                    <p>You can change it in your profile settings once you log in.</p>
                                    <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;' />
                                    <p>If you didn't request this, please ignore this email or contact support.</p>
                                    <p>Thank you,</p>
                                    <p><strong>Team Snackers</strong></p>
                                </div>
                            </div>";

                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(noreplyEmail);
                        message.To.Add(user.Email);
                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = true;

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.Credentials = new NetworkCredential(sendersEmail, sendersPassword);
                            smtp.EnableSsl = true;
                            smtp.Send(message);
                        }
                    }
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

        
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult RecordDelete(int userId)
        {
            // Retrieve the user by ID
            var user =_userManager.GetUserById(userId);
            if (user == null)
            {
                return NotFound(); 
            }

            try
            {
                var sendersEmail = _configuration["EmailSettings:SendersEmail"];
                var sendersPassword = _configuration["EmailSettings:SendersPassword"];
                var noreplyEmail = "no-reply@ecofridge.com";
                var subject = "Rejection Notice";
                var body = $@"
                            <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4;'>
                                <div style='max-width: 600px; margin: 0 auto; background-color: white; padding: 20px; border-radius: 10px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
                                    <h2 style='color: #333;'>Rejection Notice</h2>
                                    <p>Hello there this is from EcoFridge;</p>                                                                     
                                    <p>We regret to inform you that your request has been rejected.</p>
                                    <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;' />
                                    <p>If you didn't request this, please ignore this email or contact support.</p>
                                    <p>Thank you,</p>
                                    <p><strong>Team Snackers</strong></p>
                                </div>
                            </div>";

                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(noreplyEmail);
                    message.To.Add(user.Email);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential(sendersEmail, sendersPassword);
                        smtp.EnableSsl = true;
                        smtp.Send(message);
                    }
                }

                var result = _userRepo.Delete(user.UserId);
                if (result == ErrorCode.Success)
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during user deletion: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }

            return BadRequest("User deletion failed.");
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
                        if (user.AccountApproved == true)
                        {
                            var sendersEmail = _configuration["EmailSettings:SendersEmail"];
                            var sendersPassword = _configuration["EmailSettings:SendersPassword"];
                            var noreplyEmail = "no-reply@ecofridge.com";
                            var subject = "Approval Notice";
                            var body = $@"
                            <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4;'>
                                <div style='max-width: 600px; margin: 0 auto; background-color: white; padding: 20px; border-radius: 10px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
                                    <h2 style='color: #333;'>Approval Notice</h2>
                                    <p>Hello there this is from EcoFridge;</p>                                                                     
                                    <p>Congrats we are informing you that your request has been approved.</p>
                                    <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;' />
                                    <p>If you didn't request this, please ignore this email or contact support.</p>
                                    <p>Thank you,</p>
                                    <p><strong>Team Snackers</strong></p>
                                </div>
                            </div>";

                            try
                            {
                                using (MailMessage message = new MailMessage())
                                {
                                    message.From = new MailAddress(noreplyEmail);
                                    message.To.Add(user.Email);
                                    message.Subject = subject;
                                    message.Body = body;
                                    message.IsBodyHtml = true;

                                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                                    {
                                        smtp.Credentials = new NetworkCredential(sendersEmail, sendersPassword);
                                        smtp.EnableSsl = true;
                                        smtp.Send(message);
                                    }
                                }
                                return Json(new { success = true, message = "Sent successfully!" });
                            }
                            catch (Exception ex)
                            {
                                // Log error (ex) if necessary
                                return Json(new { success = false, message = "Error sending approval email." });
                            }
                        }

                        if (user.AccountApproved == false)
                        {
                            var sendersEmail = _configuration["EmailSettings:SendersEmail"];
                            var sendersPassword = _configuration["EmailSettings:SendersPassword"];
                            var noreplyEmail = "no-reply@ecofridge.com";
                            var subject = "Rejection Notice";
                            var body = $@"
                            <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4;'>
                                <div style='max-width: 600px; margin: 0 auto; background-color: white; padding: 20px; border-radius: 10px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
                                    <h2 style='color: #333;'>Rejection Notice</h2>
                                    <p>Hello there this is from EcoFridge;</p>                                                                     
                                    <p>We regret to inform you that your request has been rejected.</p>
                                    <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;' />
                                    <p>If you didn't request this, please ignore this email or contact support.</p>
                                    <p>Thank you,</p>
                                    <p><strong>Team Snackers</strong></p>
                                </div>
                            </div>";
                            try
                            {
                                using (MailMessage message = new MailMessage())
                                {
                                    message.From = new MailAddress(noreplyEmail);
                                    message.To.Add(user.Email);
                                    message.Subject = subject;
                                    message.Body = body;
                                    message.IsBodyHtml = true;

                                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                                    {
                                        smtp.Credentials = new NetworkCredential(sendersEmail, sendersPassword);
                                        smtp.EnableSsl = true;
                                        smtp.Send(message);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                // Log error (ex) if necessary
                                return Json(new { success = false, message = "Error sending approval email." });
                            }
                        }

                        if (user.AccountApproved == false)
                        {

                            if (_userRepo.Delete(parsedUserId) == ErrorCode.Success)
                            {
                                return Json(new { success = false, message = "deleted." });
                            }
                        }

                        return Json(new { success = true, message = isApproved ? "User approved" : "User declined" });
                    }

                    return Json(new { success = false, message = "Error updating user" });
                }

                return Json(new { success = false, message = "User not found" });
            }

            return Json(new { success = false, message = "Invalid user ID" });
        }

    }
}
