using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Order.Domain.Entities;

namespace OrderProcessorFunction
{
    public class ProcessOrderFunction
    {
        private readonly ILogger _logger;

        // In-memory store (Idempotency demo only)
        private static readonly HashSet<string> processed = new();

        public ProcessOrderFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ProcessOrderFunction>();
        }

        [Function("ProcessOrderFunction")]
        public async Task Run(
            [ServiceBusTrigger("order-queue", Connection = "ServiceBusConnection")] string myQueueItem)
        {
            _logger.LogInformation($"RAW MESSAGE: {myQueueItem}");

            try
            {
                // ==============================
                // STEP 1: Deserialize Message
                // ==============================
                var order = JsonConvert.DeserializeObject<Order.Domain.Entities.Order>(myQueueItem);
                var orderId = order.OrderId;

                // =====================================
                // STEP 2: IDEMPOTENCY CHECK
                // =====================================
                if (processed.Contains(orderId))
                {
                    _logger.LogInformation($"Order {orderId} already processed. Skipping...");
                    return;
                }

                // =====================================
                // STEP 3: SIMULATE FAILURE (FOR RETRY/DLQ)
                // =====================================
                if (order.Product?.ToLower() == "fail")
                {
                    throw new Exception("Simulated failure ❌");
                }

                // =====================================
                // STEP 4: BUSINESS LOGIC
                // =====================================
                _logger.LogInformation($"Processing Order: {orderId}");

                // (Example: payment, inventory, notification)

                // =====================================
                // STEP 5: MARK AS PROCESSED
                // =====================================
                processed.Add(orderId);

                _logger.LogInformation("Processing successful ✅");
            }
            catch (Exception ex)
            {
                // =====================================
                // STEP 6: RETRY + DLQ TRIGGER
                // =====================================
                _logger.LogError(ex, "Processing failed ❌");

                throw; // 🔥 Enables retry + DLQ
            }
        }
    }
}