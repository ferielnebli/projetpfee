using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Shared.VariableReader
{
    public class Configuration
    {
        #region Microservice

        #region ReferentialData
        private string? ReferentialDataPort { get; }
        private string? ReferentialDataAddress { get; } = string.Empty;
        public string? ReferentialData { get; } = string.Empty;
        #endregion

        #region Notification
        private string? NotificationPort { get; } = string.Empty;
        private string? NotificationAddress { get; } = string.Empty;
        public string? Notification { get; } = string.Empty;
        #endregion

        #region Payment
        private string? PaymentPort { get; } = string.Empty;
        private string? PaymentAddress { get; } = string.Empty;
        public string? Payment { get; } = string.Empty;
        #endregion

        #region Redis
        private string? RedisPort { get; } = string.Empty;
        private string? RedisIpAddress { get; } = string.Empty;
        public string? Redis { get; } = string.Empty;
        #endregion

        #endregion

        #region Token Variable
        public string? Key { get; } = string.Empty;
        public string? TokenLifeTime { get; } = string.Empty;
        public string? Issuer { get; } = string.Empty;
        public string? RefreshTokenLifeTime { get; } = string.Empty;
        public string? ValidIssuer { get; } = string.Empty;
        #endregion

        public Configuration()
        {
            string jsonFilePath = "../../../Configuration/config.json";

            if (File.Exists(jsonFilePath))
            {
                var json = File.ReadAllText(jsonFilePath);
                Config? jsonData = JsonConvert.DeserializeObject<Config>(json);

                #region Microservice

                ReferentialDataPort = jsonData?.ReferentialDataPort;
                ReferentialDataAddress = jsonData?.ReferentialDataAddress;
                ReferentialData = $"https://{ReferentialDataAddress}:{ReferentialDataPort}";

                NotificationPort = jsonData?.NotificationPort;
                NotificationAddress = jsonData?.NotificationAddress;
                Notification = $"https://{NotificationAddress}:{NotificationPort}";

                PaymentPort = jsonData?.PaymentPort;
                PaymentAddress = jsonData?.PaymentAddress;
                Payment = $"https://{PaymentAddress}:{PaymentPort}";

                RedisPort = jsonData?.RedisPort;
                RedisIpAddress = jsonData?.RedisIpAddress;
                Redis = $"{RedisIpAddress}:{RedisPort}";

                #endregion

                #region Token Variable

                Key = jsonData?.Key;
                TokenLifeTime = jsonData?.TokenLifeTime;
                Issuer = jsonData?.Issuer;
                RefreshTokenLifeTime = jsonData?.RefreshTokenLifeTime;
                ValidIssuer = jsonData?.ValidIssuer;

                #endregion
            }
            else
            {
                // Handle the case when the JSON file doesn't exist.
                // You can set default values or throw an exception.
            }
        }

        private class Config
        {
            #region Microservice

            #region ReferentialData
            public string ReferentialDataPort { get; set; } = string.Empty;
            public string ReferentialDataAddress { get; set; } = string.Empty;

            #endregion

            #region Notification
            public string NotificationPort { get; set; } = string.Empty;
            public string NotificationAddress { get; set; } = string.Empty;
            #endregion

            #region Payment
            public string PaymentPort { get; set; } = string.Empty;
            public string PaymentAddress { get; set; } = string.Empty;
            #endregion

            #region Redis
            public string RedisPort { get; set; } = string.Empty;
            public string RedisIpAddress { get; set; } = string.Empty;
            #endregion

            #endregion


            #region Token Variable
            public string Key { get; set; } = string.Empty;
            public string TokenLifeTime { get; set; } = string.Empty;
            public string Issuer { get; set; } = string.Empty;
            public string RefreshTokenLifeTime { get; set; } = string.Empty;
            public string ValidIssuer { get; set; } = string.Empty;
            #endregion

        }
    }

}
