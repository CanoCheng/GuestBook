using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace GuestBookProject.Models.ViewModel.Member
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50,ErrorMessage = "{0} 的長度至少必須為 {2} 個字元。",MinimumLength = 8)]
        [Display(Name = "帳號")]
        [RegularExpression("^[0-9a-zA-Z_]+$",ErrorMessage = "輸入帳號必須為英文及數字")]
        public string Account { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 的長度至少必須為 {2} 個字元。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "確認密碼")]
        [Compare("Password", ErrorMessage = "密碼和確認密碼不相符。")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(10,ErrorMessage = "{0} 的字數至少必須為  {2}~{1} 個字以內。", MinimumLength = 1)]
        [Display(Name ="暱稱")]
        [RegularExpression("^[a-zA-Z0-9\u4e00-\u9fa5]+$", ErrorMessage = "輸入名稱必須為中文或英文")]
        public string NickName { get; set; }

        //public int Role { get; set; }
    }
}