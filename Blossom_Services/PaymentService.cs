using Blossom_BusinessObjects.Configurations;
using Blossom_BusinessObjects.Entities;
using Blossom_BusinessObjects.Enums;
using Blossom_Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Blossom_Services
{
    public class PaymentService : IPaymentService
    {
        private readonly MomoConfig _momoConfig;

        public PaymentService(IOptions<MomoConfig> momoConfig)
        {
            _momoConfig = momoConfig.Value;
        }

        public async Task<string> CreateMomoPayment(long amount, string userId)
        {           
            var requestId = Guid.NewGuid().ToString();
            var orderId = Guid.NewGuid().ToString();
            var rawHash = $"accessKey={_momoConfig.AccessKey}" +
              $"&amount={amount}" +
              $"&extraData=" +  
              $"&ipnUrl={_momoConfig.IpnUrl}" +
              $"&orderId={orderId}" +
              $"&orderInfo=Topup Blossom Wallet" +
              $"&partnerCode={_momoConfig.PartnerCode}" +
              $"&redirectUrl={_momoConfig.ReturnUrl}" +
              $"&requestId={requestId}" +
              $"&requestType=payWithATM";

            var signature = this.HmacSHA256(rawHash, _momoConfig.SecretKey);

            var requestBody = new
            {
                partnerCode = _momoConfig.PartnerCode,
                partnerName = "BlossomApp",
                autoCapture = false,
                requestId,
                amount = amount.ToString(),
                orderId,
                orderInfo = "Topup Blossom Wallet",
                storeId = "BlossomApp",
                redirectUrl = _momoConfig.ReturnUrl,
                ipnUrl = _momoConfig.IpnUrl,
                requestType = "payWithATM",
                extraData="",
                lang= "vi",
                signature
            };

            var client = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(_momoConfig.PaymentUrl, content);
            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);

            if (jsonResponse.resultCode == 0)
            {
                return jsonResponse.payUrl;
            }
            return null;
        }

        public async Task<bool> HandleMomoCallback(string transactionId, string userId, long amount, bool success)
        {
            if (success)
            {
                // Add balance to the user
                // Add a wallet log
                // Implement your database logic here.
                return true;
            }
            return false;
        }

        private string HmacSHA256(string inputData, string key)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData));
                StringBuilder hexString = new StringBuilder();

                foreach (byte hashByte in hashBytes)
                {
                    hexString.Append(hashByte.ToString("x2"));
                }

                return hexString.ToString();
            }
        }
    }
}
