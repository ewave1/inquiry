using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartSSO.Models
{

    public class PassportLoginRequest
    {
        [MaxLength(50)]
        [Required] 
        public string UserName { get; set; }

        [MinLength(6)]
        [MaxLength(12)]
        [Required]
        [DisplayName("登录密码")]
        public string Password { get; set; }

        [MinLength(32)]
        [MaxLength(32)]
        [Required]
        [Display(Name = "应用标识")]
        public string AppKey { get; set; }

        public void Trim()
        {
            UserName = UserName.Trim();
            Password = Password.Trim();
            AppKey = AppKey.Trim();
        }
    }
}