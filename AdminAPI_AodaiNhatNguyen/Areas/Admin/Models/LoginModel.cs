using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AdminAPI_AodaiNhatNguyen.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Hãy nhập Email")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Hãy nhập mật khẩu")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}