using System;
using System.Linq;
using Guide.Models;
using Guide.Models.Data;
using Microsoft.AspNetCore.Mvc;
namespace Guide.Controllers
{
    public class ValidationController : Controller
    {

        private readonly GuideContext _db;

        public ValidationController(GuideContext db)
        {
            _db = db;
        }

        public bool CheckYear(string YearOfWriting)
        {
            try
            {
                DateTime data = DateTime.Now;
                int newYear = data.Year;
                int year = Convert.ToInt32(YearOfWriting);
                if (year <= newYear && year > 1000)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }

        public bool CheckEmail(string email)
        {
            string emailUpper = email.ToUpper();
            User user = _db.Users.FirstOrDefault(u => u.NormalizedEmail == emailUpper);
            if (user != null)
                return false;
            return true;
        }
    }
}