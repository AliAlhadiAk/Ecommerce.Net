
using Microsoft.VisualBasic;

namespace WebApiTesting.Model
{
    public class FanService : IFanService
    {
        public async Task<List<string>> GetAllFans()
        {
            List<string> list = new List<string>();
            list.Add("Sara");
            list.Add("Eman");
            list.Add("7amoodi");
            list.Add("C# Developer");
            list.Add("Sara Al m2boora");
            return list;
        }
    }
}
