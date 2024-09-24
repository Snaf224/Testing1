using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Models
{
    public class Users
    {
        [Key]
        public int id { get; set; }

        public string username { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }

        public string phone {  get; set; }

        public string status { get; set; }
    }
}
