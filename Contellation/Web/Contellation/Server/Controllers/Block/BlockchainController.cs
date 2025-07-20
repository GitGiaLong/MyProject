using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blazor.Contellation.Server.Controllers.Block
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockchainController : ControllerBase
    {
        //private readonly Blockchain _blockchain;

        //public BlockchainController(Blockchain blockchain)
        //{
        //    _blockchain = blockchain;
        //}

        //[HttpGet]
        //public IActionResult Get(string userId)
        //{
        //    var user = _dbContext.Users.Find(userId);
        //    if (user == null) return Unauthorized();
        //    return Ok(_blockchain.GetChain(userId, user.Role));
        //}

        //[HttpPost]
        //public IActionResult Post([FromBody] PostRequest request)
        //{
        //    var user = _dbContext.Users.Find(request.UserId);
        //    if (user == null) return Unauthorized();

        //    var process = new Process
        //    {
        //        StartInfo = new ProcessStartInfo
        //        {
        //            FileName = "python",
        //            Arguments = $"../Services/AISecurity.py {request.UserId} 192.168.1.1",
        //            RedirectStandardOutput = true,
        //            UseShellExecute = false
        //        }
        //    };
        //    process.Start();
        //    string result = process.StandardOutput.ReadToEnd();
        //    process.WaitForExit();
        //    var securityResult = System.Text.Json.JsonSerializer.Deserialize<(bool, string)>(result);
        //    if (!securityResult.Item1) return BadRequest(securityResult.Item2);

        //    _blockchain.AddBlock(request.Data, request.UserId, request.Visibility);
        //    return Ok();
        //}

        //[HttpPost("login")]
        //public IActionResult Login([FromBody] LoginRequest request)
        //{
        //    var user = _dbContext.Users.Find(request.UserId);
        //    if (user == null)
        //    {
        //        user = new User { UserId = request.UserId, Role = "User", ProfileData = "{}" };
        //        _dbContext.Users.Add(user);
        //        _dbContext.SaveChanges();
        //    }
        //    return Ok(new { UserId = user.UserId, Role = user.Role });
        //}
    }

    public class PostRequest
    {
        public string Data { get; set; }
        public string UserId { get; set; }
        public string Visibility { get; set; }
    }

    public class LoginRequest
    {
        public string UserId { get; set; }
    }
}
