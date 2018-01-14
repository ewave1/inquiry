using System.ComponentModel.DataAnnotations;

namespace SmartSSO.Models
{
    public class HomeLoginRequest
    {
        public HomeLoginRequest()
        {
            UserName = string.Empty;
            Password = string.Empty;
        }

        [StringLength(32)]
        [Required] 
        public string UserName { get; set; }

        [StringLength(32)]
        [Required]
        [Display(Name = "登录密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public void Trim()
        {
            if (!string.IsNullOrEmpty(UserName)) UserName = UserName.Trim();

            if (!string.IsNullOrEmpty(Password)) Password = Password.Trim();

        }
    }
}