using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GuestBookProject.Models.ViewModel.Member
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "{0} 的長度至少必須為 {2} 個字元。", MinimumLength = 8)]
        [Display(Name = "帳號")]
        //[EmailAddress]
        [RegularExpression("^[0-9a-zA-Z_]+$",ErrorMessage = "輸入帳號必須為英文及數字")]
        public string Account { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        //[Display(Name = "記住我?")]
        //public bool RememberMe { get; set; }
    }
}