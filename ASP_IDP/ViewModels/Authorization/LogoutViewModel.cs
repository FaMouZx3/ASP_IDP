using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ASP_IDP.ViewModels.Authorization
{
    public class LogoutViewModel
    {
        [BindNever]
        public string? RequestId { get; set; }
    }
}
