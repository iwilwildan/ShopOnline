using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Models.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Username is Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Password is Required")]
        public string Password { get; set; }
        public string Email { get; set; }
    }

    public class UserSignedDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
