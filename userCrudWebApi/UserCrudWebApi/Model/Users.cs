using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserCrudWebApi.Model
{
    [Table("tbl_user")]
    public class Users
    {
        [Key, Column("id")]
        public int id { get; set; }

        [Required, Column("username")]
        public string username { get; set; }

        [Required, Column("password")]
        public string password { get; set; }

        [Column("isAdmin")]
        public bool isAdmin { get; set; }

        [Required, Column("age")]
        public int age { get; set; }

        [Required, Column("hobbies")]
        public string? hobbies { get; set; }
    }
}
