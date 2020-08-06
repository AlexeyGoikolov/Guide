using System;
using Microsoft.AspNetCore.Mvc;

namespace Guide.Controllers
{
    public class ValidationController : Controller
    {
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
    }
}