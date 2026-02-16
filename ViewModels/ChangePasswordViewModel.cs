using System.ComponentModel.DataAnnotations;

public class ChangePasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Aktuálne heslo")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Nové heslo")]
    public string NewPassword { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Potvrdiť nové heslo")]
    [Compare("NewPassword", ErrorMessage = "Heslá sa nezhodujú.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}

