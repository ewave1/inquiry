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
        [DataType(DataType.Password)]
        [Display(Name = "账号")]
        public string UserName { get; set; }

        [MinLength(6)]
        [MaxLength(12)]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "登录密码")]
        public string Password { get; set; }

        [MaxLength(50)]
        [Required]
        [Display(Name = "姓名")]
        public string Nick { get; set; }
         
        [Required]
        [Display(Name = "折扣")]
        public decimal Discount { get; set; }

        [Required]
        [Display(Name = "是否管理员")]

        public bool IsAdmin { get; set; }

        public bool IsAdd { get; set; }
    }
}