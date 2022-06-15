using Domain.DataAnnotations;
using Domain.Localization;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models
{
    public class FilterOptions
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "validation_FieldRequired")]
        [Display(ResourceType = typeof(Resource), Name = $"{nameof(FilterOptions)}{nameof(StartDate)}")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "validation_FieldRequired")]
        [Display(ResourceType = typeof(Resource), Name = $"{nameof(FilterOptions)}{nameof(EndDate)}")]
        [AttributeGreaterThan(nameof(StartDate), ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "validation_OnDateComparison")]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "validation_FieldRequired")]
        [Display(ResourceType = typeof(Resource), Name = $"{nameof(FilterOptions)}{nameof(SelectedValues)}")]
        public string SelectedValues { get; set; } = string.Empty;
    }
}
