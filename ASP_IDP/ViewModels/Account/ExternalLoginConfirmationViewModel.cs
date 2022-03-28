using System.ComponentModel.DataAnnotations;

namespace ASP_IDP.ViewModels.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
