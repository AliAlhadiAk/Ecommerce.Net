namespace WebApiTesting.Model
{
    public interface IFanService
    {
        public Task<List<string>> GetAllFans();
    }
}
