namespace TwoFactorAuthentication.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using Filters;

    [Authorize]
    [RoutePrefix("api/Transactions")]
    public class TransactionsController : ApiController
    {
        [Route("history")]
        public IHttpActionResult GetHistory()
        {
            return Ok(Transaction.CreateTransactions());
        }

        [Route("transfer")]
        [TwoFactorAuthorize]
        public IHttpActionResult PostTransfer(TransferModeyModel transferModeyModel)
        {
            return Ok();
        }
    }

    #region Helpers

    public class Transaction
    {
        public DateTime ActionDate { get; set; }

        public string Amount { get; set; }

        public string CustomerName { get; set; }

        public int Id { get; set; }

        public static List<Transaction> CreateTransactions()
        {
            var transactionList = new List<Transaction>
            {
                new Transaction
                {
                    Id = 10248,
                    CustomerName = "Taiseer Joudeh",
                    Amount = "$1,545.00",
                    ActionDate = DateTime.UtcNow.AddDays(-5)
                },
                new Transaction
                {
                    Id = 10249,
                    CustomerName = "Ahmad Hasan",
                    Amount = "$2,200.00",
                    ActionDate = DateTime.UtcNow.AddDays(-6)
                },
                new Transaction
                {
                    Id = 10250,
                    CustomerName = "Tamer Yaser",
                    Amount = "$300.00",
                    ActionDate = DateTime.UtcNow.AddDays(-7)
                },
                new Transaction
                {
                    Id = 10251,
                    CustomerName = "Lina Majed",
                    Amount = "$3,100.00",
                    ActionDate = DateTime.UtcNow.AddDays(-8)
                },
                new Transaction
                {
                    Id = 10252,
                    CustomerName = "Yasmeen Rami",
                    Amount = "$1,100.00",
                    ActionDate = DateTime.UtcNow.AddDays(-9)
                }
            };

            return transactionList;
        }
    }

    public class TransferModeyModel
    {
        public double Amount { get; set; }

        public string FromEmail { get; set; }

        public string ToEmail { get; set; }
    }

    #endregion
}