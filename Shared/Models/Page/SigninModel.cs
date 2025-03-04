using System.ComponentModel.DataAnnotations;

public class SigninModel
{

[Required(ErrorMessage ="نام کاربری را وارد کنید")]
 public string UserName { get; set; }

 [Required(ErrorMessage ="پسورد را وارد کنید")]
 [MinLength(8, ErrorMessage = "حداقل هشت کاراکتر باید باشد")]
 public string Password { get; set; }

 [Required(ErrorMessage = "شماره همراه را وارد کنید")]
 [StringLength(11, MinimumLength = 11, ErrorMessage="شماره همراه را درست وارد کنید")]
 public string PhoneNumber { get; set; }
}