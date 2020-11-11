using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCWebSite.Models;
using MVCWebSite.Models.ViewModels;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace MVCWebSite.Controllers
{
    public class RegisterController : Controller
    {
        Entities db = new Entities();

        public ActionResult Index()
        {
            ViewBag.Professions = db.Professions;
            ViewBag.Achievements = db.Achievements.Select(m => m.AchievementName).ToList();
            ViewBag.Faculties = db.Faculties;
            ViewBag.Departments = db.Departments;

            List<SelectListItem> courses = new List<SelectListItem>();
            for (int i = 1; i < 7; i++)
            {
                courses.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = $"{i} курс"
                });
            }

            ViewBag.Courses = courses;

            return View(new ViewModelRegister());
        }

        public ActionResult Verify(ViewModelRegister userInfo, HttpPostedFileBase image)
        {
            User user = userInfo.GetUser();
            if (image != null)
            {
                ViewBag.Length = image.InputStream.Length;
                MemoryStream target = new MemoryStream();
                image.InputStream.CopyTo(target);
                byte[] data = target.ToArray();
                user.Avatar = data;
            }

            db.Users.Add(user);
            db.SaveChanges();

            if (userInfo.IsSport)
            {
                db.UserAchievements.Add(new UserAchievement { UserID = user.ID, AchievementID = 1});
            }
            if (userInfo.IsCandidate)
            {
                db.UserAchievements.Add(new UserAchievement { UserID = user.ID, AchievementID = 2 });
            }
            if (userInfo.IsDoctor)
            {
                db.UserAchievements.Add(new UserAchievement { UserID = user.ID, AchievementID = 3 });
            }

            db.SaveChanges();

            string digest = GenerateDigest();
            var model = new ViewModelVerify { ID = user.ID, GeneratedPass = digest };
            ViewBag.Error = false;
            try
            {
                SendEmail(user.Email, digest);
            }
            catch
            {
                ViewBag.Error = true;
            }

            return View(model);
        }

        private string GenerateDigest()
        {
            int length = 7;
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();
            char letter;
            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            return str_build.ToString();
        }

        private void SendEmail(string email, string body)
        {
            string subject = "Одноразовий пароль підтвердження";

            SmtpClient mailer = new SmtpClient("mail.univ.net.ua");
            mailer.Port = 25;
            mailer.Credentials = new NetworkCredential("Wresser1grr.la", "feldsher123");
            mailer.EnableSsl = false;
            mailer.Send(new MailMessage("Wresser1grr.la@univ.net.ua", email, subject, body));
        }

        public ActionResult Finish(ViewModelVerify info)
        {
            User user = db.Users.Where(u => u.ID == info.ID).FirstOrDefault();
            bool check = info.Correct();
            ViewBag.Correct = check;
            if (check)
            {
                user.IsValidated = true;
                db.SaveChanges();
                return View();
            }
            else
            {
                RemoveUser(user);
                return View();
            }
        }

        private void RemoveUser(User user)
        {
            var achievements = db.UserAchievements.Where(a => a.UserID == user.ID);
            db.Users.Remove(user);
            foreach (UserAchievement ach in achievements)
            {
                db.UserAchievements.Remove(ach);
            }
            db.SaveChanges();
        }

        private void WriteToLog(string message)
        {
            using (StreamWriter sw = new StreamWriter(@"D:\Programming\SysProg\Labs\Lab5\log.txt", true))
            {
                sw.WriteLine(DateTime.Now + " | " + message);
            }
        }
    }
}