using Microsoft.AspNetCore.Mvc;

namespace Otel_TestBed.Controllers;

[ApiController]
[Route("[controller]")]
public class OtelTestBedController : ControllerBase
{
    private readonly ILogger<OtelTestBedController> _logger;
    private readonly ISampleService _sampleService;

    public OtelTestBedController(ILogger<OtelTestBedController> logger, ISampleService sampleService)
    {
        _logger = logger;
        _sampleService = sampleService;
    }

    [HttpGet(Name = "1")]
    public string Get()
    {
        try
        {
            return _sampleService.LowLevelRandomTryCatch().ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}