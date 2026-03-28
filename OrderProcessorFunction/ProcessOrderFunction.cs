using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Order.Domain.Entities;

namespace OrderProcessorFunction
{
    public class ProcessOrderFunction
    {
        // In-memory store (Idempotency demo only)
        private static readonly HashSet<string> processed = new();

        [FunctionName("ProcessOrderFunction")]
        public async Task Run(
            [ServiceBusTrigger("order-queue", Connection = "ServiceBusConnection")] string myQueueItem,
            ILogger log)
        {
            log.LogInformation($"RAW MESSAGE: {myQueueItem}");

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
                if (await AlreadyProcessed(orderId))
                {
                    log.LogInformation($"Order {orderId} already processed. Skipping...");
                    return;
                }

                // =====================================
                // STEP 3: SIMULATE FAILURE (FOR RETRY/DLQ)
                // =====================================
                log.LogInformation($"Product value: {order.Product.ToLower()}");
                if (order.Product.ToLower() == "fail")
                {
                    throw new Exception("Simulated failure ❌");
                }

                // =====================================
                // STEP 4: BUSINESS LOGIC
                // =====================================
                log.LogInformation($"Processing Order: {orderId}");

                // (Example: payment, inventory, notification)

                // =====================================
                // STEP 5: MARK AS PROCESSED
                // =====================================
                await MarkAsProcessed(orderId);

                log.LogInformation("Processing successful ✅");
            }
            catch (Exception ex)
            {
                // =====================================
                // STEP 6: RETRY + DLQ TRIGGER
                // =====================================
                log.LogError(ex, "Processing failed ❌");

                throw; // 🔥 Enables retry and DLQ
            }
        }

        // =====================================
        // IDEMPOTENCY HELPERS
        // =====================================

        private Task<bool> AlreadyProcessed(string orderId)
        {
            return Task.FromResult(processed.Contains(orderId));
        }

        private Task MarkAsProcessed(string orderId)
        {
            processed.Add(orderId);
            return Task.CompletedTask;
        }
    }
}