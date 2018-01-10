using System.ComponentModel.DataAnnotations;

namespace SmartSSO.Models
{
    public class UserCreateRequest
    {
        public UserCreateRequest()
        {
            UserName = string.Empty;
            Password = string.Empty;
        }

        [MaxLength(50)]
        [Required] 
        public string UserName { get; set; }

        [MinLength(6)]
        [MaxLength(12)]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "登录密码")]
        public string Password { get; set; }

        [MaxLength(50)]
        [Required]
        [Display(Name = "真实姓名")]
        public string Nick { get; set; }
    }
}