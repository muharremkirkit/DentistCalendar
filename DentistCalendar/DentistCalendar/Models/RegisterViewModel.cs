using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentistCalendar.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Lütfen kullanıcı adı giriniz.")]
        [Display(Name="Kullanıcı Adı:")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Lütfen  adınızı giriniz.")]
        [Display(Name = "Ad:")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Lütfen  soyadınızı giriniz.")]
        [Display(Name = "Soyad:")]
        public string SurName { get; set; }
        [Required(ErrorMessage = "Lütfen  şifre giriniz.")]
        [Display(Name = "Şifre:")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Lütfen  e-mail giriniz.")]
        [Display(Name = "E-Mail:")]
        [EmailAddress(ErrorMessage ="Geçerli bir e-mail giriniz.")]
        public string Email { get; set; }
        
        [Display(Name = "Randevu Rengi:")]
        public string Color { get; set; }
        [Display(Name = "Doktorum")] //renk seçerse doktordur 
        public bool IsDentist { get; set; }


    }
}
