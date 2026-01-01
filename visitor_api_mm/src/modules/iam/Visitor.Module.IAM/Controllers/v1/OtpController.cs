using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace Visitor.Module.IAM.Controllers.v1;

[ApiController]
[Route("v{version:apiVersion}/[module]/[controller]")]
[SwaggerTag("Otp Controller")]
public class OtpController : BaseController
{
    private static readonly ConcurrentDictionary<string, (string Otp, DateTime Expiry, string TransactionId)> otpStore = new();
    [HttpPost("otp/generate")]
    public IActionResult GenerateOtp([FromQuery][Required] string address, [Required] string transactionId)
    {
        string otp = GenerateRandomOtp();
        DateTime expiry = DateTime.UtcNow.AddMinutes(5);
        otpStore[address] = (otp, expiry, transactionId);
        return Ok(new
        {
            Address = address,
            Otp = otp,
            Expiry = expiry,
            TransactionId = transactionId
        });
    }
    [HttpGet("otp/validate")]
    public IActionResult ValidateOtp([FromQuery][Required] string address, [Required] string otp, [Required] string transactionId)
    {

        if (otpStore.TryGetValue(address, out var storedOtp))
        {
            if (storedOtp.TransactionId == transactionId && storedOtp.Otp == otp && storedOtp.Expiry > DateTime.UtcNow)
            {
                otpStore.TryRemove(address, out _);
                return Ok("OTP is valid.");
            }
            else if (storedOtp.Expiry <= DateTime.UtcNow)
            {
                return BadRequest("OTP has expired.");
            }
            else
            {
                return BadRequest("Invalid OTP.");
            }
        }
        return NotFound("OTP not found for the given address.");
    }
    private string GenerateRandomOtp()
    {
        using var rng = new RNGCryptoServiceProvider();
        byte[] data = new byte[4];
        rng.GetBytes(data);
        int value = BitConverter.ToInt32(data, 0) % 1000000;
        return Math.Abs(value).ToString("D6");
    }
}
