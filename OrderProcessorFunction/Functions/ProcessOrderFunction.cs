// using Microsoft.Extensions.Logging;

// public class ProcessOrderFunction
// {
//     private readonly ILogger _logger;

//     public ProcessOrderFunction(ILoggerFactory loggerFactory)
//     {
//         _logger = loggerFactory.CreateLogger<ProcessOrderFunction>();
//     }

//     [FunctionName("ProcessOrderFunction")]
//     public void Run(
//         [ServiceBusTrigger("order-queue", Connection = "ServiceBusConnection")]
//         string message)
//     {
//         _logger.LogInformation($"Processing Order: {message}");
//     }
// }