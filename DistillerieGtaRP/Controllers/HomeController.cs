﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DistillerieGtaRP.CustomQuery;
using DistillerieGtaRP.Data;
using DistillerieGtaRP.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DistillerieGtaRP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DistillerieGtaRP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly Data.Dapper.CustomQuery _customQuery;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, Data.Dapper.CustomQuery customQuery)
        {
            _logger = logger;
            _context = context;
            _customQuery = customQuery;
        }
        [Authorize(Roles = "Learner, Boss, CoBoss, Leader, Employee, Administration, Government")]
        [Route("", Name = "home.index")]
        public async Task<IActionResult> Index()
        {
            var cars = await _context.Car.Include(m => m.ApplicationUser).ToListAsync();
            
            var carsDictionary = new Dictionary<string, int>
            {
                { "Used", cars.Count(m => m.ApplicationUser != null) },
                { "Free", cars.Count(m => m.ApplicationUser is null) }
            };
            ViewBag.CarsDictionary = carsDictionary;

            var transactionHistoryExport = _context.Transaction.Where(m => m.Destination == Destination.Export && m.CreatedAt > DateTime.Now.AddDays(-7)).Sum(m => m.Quantity);
            var transactionHistoryStock = _context.Transaction.Where(m => m.Destination == Destination.Stock && m.CreatedAt > DateTime.Now.AddDays(-7)).Sum(m => m.Quantity);
            
            var transactionHistoryDictionary = new Dictionary<string, int>()
            {
                { "transactionHistoryExport", transactionHistoryExport},
                { "transactionHistoryStock", transactionHistoryStock }
            };
            ViewBag.TransactionDictionary = transactionHistoryDictionary;

            ViewBag.Parameter = _context.Parameters.First();

            
            var sqlTransaction = "SELECT TOP (7) SUM(Quantity) as Quantity, CONVERT(DATE,CreatedAt) as CreatedAt FROM [Transaction] GROUP BY CONVERT(DATE,CreatedAt) ORDER BY CONVERT(DATE,CreatedAt) DESC";
            var transactionWeek = await _customQuery.QueryAsync<TransactionCustomQuery>(sqlTransaction);
            ViewBag.TransactionWeek = transactionWeek;

            var sqlCommand = "SELECT TOP (14) SUM(Quantity) as Quantity, CONVERT(DATE,CreatedAt) as CreatedAt, Company.CompanyId, Company.Name FROM [Commands] as Command LEFT JOIN Companies Company on Command.CompanyId = Company.CompanyId GROUP BY CONVERT(DATE,CreatedAt), Company.CompanyId, Company.Name ORDER BY CONVERT(DATE,CreatedAt) DESC";
            var commandWeek = await _customQuery.QueryAsync<CommandCustomQuery>(sqlCommand);
            ViewBag.CommandWeek = commandWeek;
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}