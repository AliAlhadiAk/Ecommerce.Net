using System.ComponentModel.DataAnnotations;

namespace WebApiTesting.Model
{
    public class Fan
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
