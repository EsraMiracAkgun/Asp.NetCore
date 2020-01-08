using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreProject.Web.ViewModels.Manage
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Display(Name="Rol Adı")]
        public string Name { get; set; }
    }
}
