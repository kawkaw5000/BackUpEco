using AdminSideEcoFridge.Models.CustomModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using AdminSideEcoFridge.Utils;

namespace AdminSideEcoFridge.Controllers
{
    public class MailController : BaseController
    {
        IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public MailController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public IActionResult SendApproveMessage([FromBody] MessageModel model)
        {
            try
            {
                var sendersEmail = _configuration["EmailSettings:SendersEmail"];
                var sendersPassword = _configuration["EmailSettings:SendersPassword"];
                var noreplyEmail = "no-reply@ecofridge.com";
                var subject = "Approval Notice";

                var body = $@"
                    <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4;'>
                        <div style='max-width: 600px; margin: 0 auto; background-color: white; padding: 20px; border-radius: 10px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
                            <h2 style='color: #000000;'>Approval Notice</h2>
                            <p style='color: #000000;'>{model.Message.Replace("\n", "<br/>")}</p>
                            <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;' />
                            <p style='color: #000000;'>Best regards,</p>
                            <p style='color: #000000;'><strong>Management: TeamSnackers</strong></p>
                        </div>
                    </div>";

                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(noreplyEmail);
                    message.To.Add(model.Email);
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
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while sending the message." });
            }
        }

        [HttpPost]
        public IActionResult SendRejectionMessage([FromBody] MessageModel model)
        {
            try
            {
                var sendersEmail = _configuration["EmailSettings:SendersEmail"];
                var sendersPassword = _configuration["EmailSettings:SendersPassword"];
                var noreplyEmail = "no-reply@ecofridge.com";
                var subject = "Rejection Notice";

                var body = $@"
                    <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4;'>
                        <div style='max-width: 600px; margin: 0 auto; background-color: white; padding: 20px; border-radius: 10px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
                            <h2 style='color: #000000;'>Rejection Notice</h2>
                            <p style='color: #000000;'>{model.Message.Replace("\n", "<br/>")}</p>
                            <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;' />
                            <p style='color: #000000;'>Best regards,</p>
                            <p style='color: #000000;'><strong>Management: TeamSnackers</strong></p>
                        </div>
                    </div>";

                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(noreplyEmail);
                    message.To.Add(model.Email);
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
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while sending the message." });
            }
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var user = _db.Users.Where(m => m.Email == model.Email).FirstOrDefault();

            if (user == null)
            {
                return Json(new { success = false, message = "Email not found." });
            }

            try
            {
                var sendersEmail = _configuration["EmailSettings:SendersEmail"];
                var sendersPassword = _configuration["EmailSettings:SendersPassword"];
                var noreplyEmail = "no-reply@ecofridge.com";
                var subject = "Forgot Password";

                Guid guid = Guid.NewGuid();
                var temporaryPassword = guid.ToString("N").Substring(0, 8);
                var body = $@"
                            <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4;'>
                                <div style='max-width: 600px; margin: 0 auto; background-color: white; padding: 20px; border-radius: 10px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
                                    <h2 style='color: #333;'>Password Reset Request</h2>
                                    <p>Hello,</p>
                                    <p>Your new temporary password is:</p>
                                    <p style='font-size: 18px; font-weight: bold; color: #307a59;'>{temporaryPassword}</p>
                                    <p>You can change it in your profile settings once you log in.</p>
                                    <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;' />
                                    <p>If you didn't request this, please ignore this email or contact support.</p>
                                    <p>Thank you,</p>
                                    <p><strong>Team Snackers</strong></p>
                                </div>
                            </div>";
                user.Password = temporaryPassword;

                if (_userRepo.Update(user.UserId, user) == ErrorCode.Success)
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
                    return Json(new { success = true, message = "Temporary password sent to your email." });
                }

                return Json(new { success = false, message = "Error updating user." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while processing your request." });
            }
        }
    }
}
