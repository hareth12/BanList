using System.Linq;
using System.Net.Http;
using TwoFactorAuthentication.API.Services;

namespace TwoFactorAuthentication.API.Helpers
{
    public static class OtpHelper
    {
        private const string otpHeader = "X-OTP";

        public static bool HasValidTotp(this HttpRequestMessage request, string key)
        {
            if (!request.Headers.Contains(otpHeader))
            {
                return false;
            }
            string otp = request.Headers.GetValues(otpHeader).First();

            // We need to check the passcode against the past, current, and future passcodes

            return !string.IsNullOrWhiteSpace(otp) && TimeSensitivePassCode.GetListOfOtp(key).Any(t => t.Equals(otp));
        }
    }
}