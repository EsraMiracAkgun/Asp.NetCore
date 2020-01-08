using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreProject.Web.ViewModels.Identity
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Bu Alan Zorunludur")]
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Bu Alan Zorunludur")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        //[Required(ErrorMessage = "Bu Alan Zorunludur")]
        //[Display(Name = "TC Kimlik No")]
        ////[StringLength(11, ErrorMessage = "{0} {1} Karakter Olmalıdır"),MinLength(11)]
        //public int NationalIdNumber { get; set; }

        [Required(ErrorMessage = "Bu Alan Zorunludur")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage ="Lütfen Geçerli Bir Email Adresi Giriniz")] 
        public string Email { get; set; }

        [Required(ErrorMessage = "Bu Alan Zorunludur")]
        [Display(Name = "Parola")]
        [DataType(DataType.Password)]
        [StringLength(8,ErrorMessage ="{0} {1}-{2} Karakter Aralığında Olmalı"),MinLength(4)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bu Alan Zorunludur")]
        [Display(Name = "Parola Doğrulama")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Parolalar Aynı Değil")]
        public string ConfirmPassword { get; set; }
    }
}
