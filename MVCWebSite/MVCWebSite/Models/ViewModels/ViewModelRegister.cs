using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCWebSite.Models;

namespace MVCWebSite.Models.ViewModels
{
    public class ViewModelRegister
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Nullable<int> ProfessionID { get; set; }
        public Nullable<int> Course { get; set; }
        public Nullable<int> FacultyID { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public byte[] Avatar { get; set; }
        public bool IsValidated { get; set; }
        public bool IsSport { get; set; }
        public bool IsCandidate { get; set; }
        public bool IsDoctor { get; set; }
        public User GetUser()
        {
            return new User {
                ID = this.ID,
                Login = this.Login,
                Password = this.Password,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                ProfessionID = this.ProfessionID,
                Course = this.Course,
                FacultyID = this.FacultyID,
                DepartmentID = this.DepartmentID,
                Avatar = this.Avatar,
                IsValidated = false,
            };
        }
    }
}