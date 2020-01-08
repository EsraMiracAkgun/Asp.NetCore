using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreProject.Web.ViewModels.Manage
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [DisplayName("Ad")]
        public string FirstName { get; set; }
        [DisplayName("Soyad")]
        public string LastName { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
       
    }
}
