
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.Caching;
using System.Security.AccessControl;

namespace WebApiTesting.Interfaces
{
    public class CacheService : ICacheService
    {
        private ObjectCache _memoryCache = System.Runtime.Caching.MemoryCache.Default;
        public T GetData<T>(string key)
        {
            try
            {
                T item = (T)_memoryCache.Get(key);
                return item;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public object RemoveData(string key)
        {
            var res = true;
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    _memoryCache.Remove(key);
                }
                else
                    res = false;

                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public bool SetData<T>(string key, T value, DateTimeOffset expirationtime)
        {
            var res = true;
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    _memoryCache.Set(key, value, expirationtime);
                }
                else
                    res = false;

                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public bool UpdateData<T>(string key, T newValue, DateTimeOffset expirationTime)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    // Check if the key exists in the cache
                    if (_memoryCache.Contains(key))
                    {
                        // Update the existing cached item with new value and expiration time
                        _memoryCache.Set(key, newValue, expirationTime);
                        return true;
                    }
                    else
                    {
                        throw new KeyNotFoundException($"Key '{key}' not found in cache");
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating data in cache", ex);
            }
        }
    }
}
