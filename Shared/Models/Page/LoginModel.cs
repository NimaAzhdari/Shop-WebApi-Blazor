using System.ComponentModel.DataAnnotations;

public class LoginModel
{
    [Required(ErrorMessage ="نام کاربری را وارد کنید")]
    public string UserName { get; set; }

    [Required(ErrorMessage ="پسورد را وارد کنید")]
    [MinLength(8, ErrorMessage = "حداقل هشت کاراکتر باید باشد")]
    public string Password { get; set; }
}