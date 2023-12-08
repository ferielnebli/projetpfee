using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Platform.Shared.Cache
{
    public class RedisCacheService : IRedisCacheService
    {
        private IDatabase? _cacheDb;
        private readonly ILogger<RedisCacheService> _logger;
        private readonly Configuration _configuration;

        public RedisCacheService(ILogger<RedisCacheService> logger,
            Configuration configuration)
        {
            _configuration = configuration;
            _logger = logger;

            try
            {
                /*var redis = ConnectionMultiplexer.Connect(_configuration.Redis);
                _cacheDb = redis.GetDatabase();*/
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(RedisCacheService)}");
            }
        }

        #region Get
        public T? GetData<T>(string key)
        {
            RedisValue value = string.Empty;
            if (_cacheDb != null)
            {
                value = _cacheDb.StringGet(key);
                if (!string.IsNullOrEmpty(value))
                {
                    Console.WriteLine($"Data for key {key} was fetched from cache");
                    return JsonSerializer.Deserialize<T>(value);
                }
            }
            return default;
        }
        public DateTime GetRemainingTime(string key)
        {
            if (_cacheDb != null)
            {
                var remainingTime = _cacheDb.KeyTimeToLive(key);
                if (remainingTime.HasValue)
                {
                    var expirationTime = DateTime.UtcNow.Add(remainingTime.Value);
                    return expirationTime;
                }
            }

            return DateTime.MinValue;
        }
        #endregion

        #region Set
        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            if (_cacheDb != null)
            {
                var expiryTime = expirationTime - DateTimeOffset.Now;
                return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expiryTime);
            }
            else
                return false;
        }
        #endregion

        #region Update 
        public bool UpdateData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            if (_cacheDb != null)
            {
                var expiryTime = expirationTime - DateTimeOffset.Now;
                var result = _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expiryTime);
                return result;
            }
            else
                return false;
        }
        #endregion

        #region Delete
        public bool RemoveData(string key)
        {
            if (_cacheDb != null)
            {
                var cacheRemoveResult = _cacheDb.KeyDelete(key);
                return cacheRemoveResult;
            }
            else
                return false;
        }
        #endregion

    }
}
