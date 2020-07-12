using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentistCalendar.Data;
using DentistCalendar.Data.Entity;
using DentistCalendar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentistCalendar.Controllers
{
    public class AppointmentController : Controller
    {
        private ApplicationDbContext _context;
        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public JsonResult GetAppointments()
        {
            var model = _context.Appointments
                //include ilişkili user tablosuyla join yapıyor , ondan sonra yeni bir randevu modeli oluşturcaz.
                .Include(x => x.User).Select(x => new AppointmentViewModel() 
                {
                    Id=x.Id,
                    Dentist= x.User.Name + " " + x.User.Surname,
                    PatientName= x.PatientName,
                    PatientSurname=x.PatientSurName,
                    StartDate = x.StartDate,
                    EndDate=x.EndDate,
                    Description=x.Description,
                    Color=x.User.Color,
                    UserId=x.UserId
                });
            return Json(model);
        }

        public JsonResult GetAppointmentsByDentist(string userId="") //userId gelirse veya boş olursa 
        {
            var model = _context.Appointments.Where(x=>x.UserId==userId) //ıd gelirse userıd ile eşleşen kayıtları getir
                //include ilişkili user tablosuyla join yapıyor , ondan sonra yeni bir randevu modeli oluşturcaz.
                .Include(x => x.User).Select(x => new AppointmentViewModel()
                {
                    Id = x.Id,
                    Dentist = x.User.Name + " " + x.User.Surname,
                    PatientName = x.PatientName,
                    PatientSurname = x.PatientSurName,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Description = x.Description,
                    Color = x.User.Color,
                    UserId = x.UserId
                });
            return Json(model);
        }

        [HttpPost]
        public JsonResult AddOrUpdateAppointment(AddOrUpdateAppointmentViewModel model)
        {
            //validasyon
            if (model.Id == 0) //eğer ıd o sa ekleme işlemi değilse güncelleme
            {
                Appointment entity = new Appointment()
                {
                    CreatedDate = DateTime.Now,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    PatientName = model.PatientName,
                    PatientSurName = model.PatientSurName,
                    Description = model.Description,
                    UserId = model.UserId
                };
                _context.Add(entity);
                _context.SaveChanges();
            }
            else
            {   //context ten ıd sine eşit olan randevuyu getirip güncellicez
                var entity = _context.Appointments.SingleOrDefault(x => x.Id == model.Id);
                if (entity==null)
                {
                    return Json("Güncellenecek veri bulunamadı.");
                }
                entity.UpdatedDate = DateTime.Now;
                entity.PatientName = model.PatientName;
                entity.PatientSurName = model.PatientSurName;
                entity.Description = model.Description;
                entity.StartDate = model.StartDate;
                entity.EndDate = model.EndDate;
                entity.UserId = model.UserId;

                _context.Update(entity);
                _context.SaveChanges();
            }
            return Json("200"); //ekleme işlemi başarılı olmuştur
        }

        public JsonResult DeleteAppointment(int id=0)
        {
            var entity = _context.Appointments.SingleOrDefault(x => x.Id == id);
            if (entity==null)
            {
                return Json("Kayıt Bulunamadı");
            }
            _context.Remove(entity);
            _context.SaveChanges();
            return Json("200"); //işlem başarılı ise
        }
    }
}