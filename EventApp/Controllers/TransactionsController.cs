using EventApp.Services.EventTransactionServ;
using EventApp.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionServ _transactionService;

        public TransactionsController(ITransactionServ transactionService)
        {
            _transactionService = transactionService;
        }

        // ✅ Get transactions for the currently logged-in user
        [HttpGet("my-transactions")]
        [Authorize]
        public async Task<ActionResult<List<EventTransaction>>> GetUserTransactions()
        {
            // get userId from JWT claims (use the same claim type as in your events endpoint)
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

            var transactions = await _transactionService.GetUserTransactionsAsync(userId);
            return Ok(transactions);
        }

        // ✅ Admin: get all transactions
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<EventTransaction>>> GetAllTransactions()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            return Ok(transactions);
        }
    }
}
