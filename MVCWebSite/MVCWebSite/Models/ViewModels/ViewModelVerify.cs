using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCWebSite.Models;

namespace MVCWebSite.Models.ViewModels
{
    public class ViewModelVerify
    {
        public int ID { get; set; }
        public string GeneratedPass {get; set;}
        public string UserPass { get; set; }
        public bool Correct()
        {
            return GeneratedPass == UserPass;
        }
    }
}