using OpenTelemetry.Instrumentation.Digma.Helpers.Attributes;

namespace Otel_TestBed;
public enum TestResult
{
    OkResult,
    Exception
}

public interface ISampleService
{
    string OnlyOneFunction();
    TestResult LowLevelRandomTryCatch();
}
public class SampleService : ISampleService
{
    private readonly ILogger<SampleService> _logger;

    public SampleService(ILogger<SampleService> logger)
    {
        _logger = logger;
    }
    public string OnlyOneFunction()
    {
        return "This function returns only one level of result";
    }


    [ActivitiesAttributes("expenseType:Capex", "businessUnit:Dev")]
    public TestResult LowLevelRandomTryCatch()
    {
        try
        {
            var random = System.Random.Shared.Next(100);
            if (random % 2 == 0)
            {
                return TestResult.OkResult;
            }

            throw new Exception(TestResult.Exception.ToString());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Caught Exception");
            return TestResult.Exception;
        }

    }


}
