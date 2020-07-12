using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentistCalendar.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Lütfen kullanıcı adı giriniz.")]
        [Display(Name = "Kullanıcı Adı:")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Lütfen  şifre giriniz.")]
        [Display(Name = "Şifre:")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")] 
        public bool RememberMe { get; set; }

    }

   

}
