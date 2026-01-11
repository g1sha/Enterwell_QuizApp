using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ExporterController : Controller
{
    /// <summary>
    /// Get all available export formats
    /// </summary>
    /// <param name="factory"></param>
    /// <returns></returns>
    
    [Authorize]
    [HttpGet("api/export/formats")]
    public IActionResult GetExportFormats([FromServices] IExportServiceFactory factory)
    {
        var formats = factory.GetAllAvailableFormats();
        return Ok(formats);
    }
}