namespace WebApiTesting.Interfaces
{
    public interface ICacheService
    {
        T GetData<T>(string key);
        bool SetData<T>(string key, T value, DateTimeOffset expirationTime);
        object RemoveData(string key);
        public bool UpdateData<T>(string key, T newValue, DateTimeOffset expirationTime);
    }
}
