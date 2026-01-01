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
/*
// KycProtector helps mask and hash KYC document numbers
public static class KycProtector
 {
 // secretPepper from a secure store; tenantSalt unique per tenant
 public static (string masked, string last4, string hashHex) Protect(
 string docType, string raw, Guid tenantId, string secretPepper, string tenantSalt)
 {
 var norm = Normalize(docType, raw);
 var last4 = new string(norm.TakeLast(4).ToArray());
 
 string masked = docType.ToUpperInvariant() switch
 {
 "AADHAAR" => $"XXXX-XXXX-{last4}",
 "PAN" => $"XXXXX{norm.Substring(5, 4)}X", // ABCDE1234F -> XXXXX1234X
 "PASSPORT" => MaskKeepLast(norm, 2, 4), // AB******1234
 "DL" => MaskKeepLast(norm, Math.Min(3, Math.Max(0, norm.Length - 4)), 4),
 _ => MaskKeepLast(norm, Math.Max(0, norm.Length - 4), 4)
 };
 
 var hashInput = $"{tenantId}|{docType.ToUpperInvariant()}|{norm}|{tenantSalt}";
 var hashHex = ToHmacSha256Hex(hashInput, secretPepper);
 return (masked, last4, hashHex);
 }
 
 static string Normalize(string docType, string raw)
 {
 var s = new string((raw ?? "").Where(char.IsLetterOrDigit).ToArray());
 s = s.ToUpperInvariant();
 if (docType.Equals("AADHAAR", StringComparison.OrdinalIgnoreCase) && s.Length >= 12)
 s = s[^12..]; // keep last 12 if someone sent full; you should prefer VID externally
 return s;
 }
 
 static string MaskKeepLast(string s, int keepHead, int keepTail)
 {
 if (s.Length <= keepHead + keepTail) return new string('X', s.Length);
 var head = s[..keepHead];
 var tail = s[^keepTail..];
 return head + new string('*', s.Length - keepHead - keepTail) + tail;
 }
 
 static string ToHmacSha256Hex(string data, string key)
 {
 using var h = new System.Security.Cryptography.HMACSHA256(
 System.Text.Encoding.UTF8.GetBytes(key));
 var bytes = h.ComputeHash(System.Text.Encoding.UTF8.GetBytes(data));
 return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
 }
 }
 */