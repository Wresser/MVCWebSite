using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCWebSite.Models;
using MVCWebSite.Models.ViewModels;

namespace MVCWebSite.Controllers
{
    

    public class HomeController : Controller
    {
        Entities db = new Entities();

        public ActionResult Index(bool? error)
        {
            if (error.HasValue && error.Value)
            {
                ViewBag.ErrorText = "Помилка авторизації, перевірте правильність логіну і паролю";
            }
            else if (error.HasValue && !error.Value)
            {
                ViewBag.ErrorText = "Цей акаунт не було проверифіковано";
            }

            return View(new ViewModelLogin());
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

        [HttpPost]
        public ActionResult Login(ViewModelLogin info)
        {
            var user = db.Users.Where(m => m.Login == info.Login).FirstOrDefault();
            if (user != null && !user.IsValidated)
            {
                RemoveUser(user);
                return RedirectToAction("Index", new { error = false });
            }

            if (user != null && user.Password == info.Password)
            {
                return View(user);
            }

            return RedirectToAction("Index", new { error = true });
        }

        public ActionResult Show(int id)
        {
            var imageData = db.Users.Where(d => d.ID == id).Select(m => m.Avatar).FirstOrDefault();
            return File(imageData, "image/jpg");
        }
    }
}