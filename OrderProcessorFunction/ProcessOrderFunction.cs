using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace OrderProcessorFunction
{
    public class ProcessOrderFunction
    {
        [FunctionName("ProcessOrderFunction")]
        public void Run(
            [ServiceBusTrigger("order-queue", Connection = "ServiceBusConnection")] string myQueueItem,
            ILogger log)
        {
            log.LogInformation($"RAW MESSAGE: {myQueueItem}");

            // Retry and DLQ(Dead Letter Queue)
            try
            {
                if (myQueueItem.ToLower().Contains("\"product\":\"fail\""))
                // sample json in post for failure
                // {
                //   "product": "fail",
                //   "amount": 100
                // }
                {
                    throw new Exception("Simulated failure ❌");
                }

                log.LogInformation("Processing successful ✅");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Processing failed ❌");
                throw;
            }
        }
    }
}