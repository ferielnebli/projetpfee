using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Shared.Cache
{
    public interface IRedisCacheService
    {
        T? GetData<T>(string key);
        bool SetData<T>(string key, T value, DateTimeOffset expirationTime);
        bool UpdateData<T>(string key, T value, DateTimeOffset expirationTime);
        bool RemoveData(string key);
        DateTime GetRemainingTime(string key);
    }
}
