﻿using FlatsLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CianWeb.Controllers
{
    public class HomeController: Controller
    {
        private readonly ImapConfig _settings;

        public HomeController(IOptions<ImapConfig> options)
        {
            _settings = options.Value;
        }

        public ViewResult Index()
        {
            return View();
        }

        public JsonResult GetAllFlats()
        {
            IFlatsSource source = new ImapFlatsSource(_settings);
            IEnumerable<Flat> flats = source.GetAll();
            flats = flats
                .OrderByDescending(flat => flat.MailDate)
                .ThenByDescending(flat => flat.EscapeDate)
                .Distinct()
                .ToList();
            return Json(flats);
        }
    }
}
