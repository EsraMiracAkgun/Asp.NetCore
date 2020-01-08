using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreProject.Web.ViewModels.Identity
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Bu Alan Zorunludur")]
        [Display(Name= "Kullanıcı Adı")]
        [EmailAddress(ErrorMessage ="Lütfen Geçerli Bir Email Adresi Girin")]
        public string UserName { get; set; } 
        
        [Required (ErrorMessage ="Bu Alan Zorunludur")]
        [Display(Name ="Parola")]
        [DataType(DataType.Password)]
        [StringLength(8,ErrorMessage ="{0} {1} - {2} Karakter Aralığında Olmalıdır."),MinLength(4)]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; } = false;
    }
}
