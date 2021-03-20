using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.RazorWebsite.Models.Reports
{
    public class ReportViewModel
    {

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public string ChartData { get; set; }

        public int[] TicketSolveTimeCount { get; set; }

        public string AverageSolveTime { get; set; }

    }
}
