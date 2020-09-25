﻿using System.Diagnostics;
using System.Net;
using Guide.Models;
using Microsoft.AspNetCore.Mvc;

namespace Guide.Controllers
{
    public class ErrorsController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int code)
        {
            ViewBag.Error = code;
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}