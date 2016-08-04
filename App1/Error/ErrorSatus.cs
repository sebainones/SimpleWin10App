using System.ComponentModel.DataAnnotations;

namespace RateApp.Error
{
    public enum ErrorStatus
    {
        [Display(Description = "Información")]
        Info,
        [Display(Description = "Advertencia")]
        Warning,
        [Display(Description = "Error")]
        Error,
        [Display(Description = "Crítico")]
        Critical              
    }
}
