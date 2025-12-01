// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace ClinicApp.Controllers
// {
//     public class DoctorAIController
//     {
        
//     }
// }
using System.IO;
using System.Threading.Tasks;
using ClinicApp.Models;
using CApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.Controllers
{
   public class DoctorAIController : Controller
{
    private readonly OpenAIService _aiService;

    public DoctorAIController(OpenAIService aiService)
    {
        _aiService = aiService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new DoctorAI());
    }

    [HttpPost]
    public async Task<IActionResult> Index(DoctorAI doctorAI)
    {
        if (!string.IsNullOrEmpty(doctorAI.SearchResult))
        {
            doctorAI.SearchResult = await _aiService.GetDoctorRecommendation(doctorAI.SearchResult);
        }

        return View(doctorAI);
    }
}

}
