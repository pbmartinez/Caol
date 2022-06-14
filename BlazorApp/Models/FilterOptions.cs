using Domain.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models
{
    public class FilterOptions
    {
        [Required]
        public DateTime? StartDate { get; set; }

        [Required]
        [AttributeGreaterThan(nameof(StartDate),ErrorMessage = "EndDate should be greated then StartDate")]
        public DateTime? EndDate { get; set; }

        [Required]
        public string SelectedValues { get; set; }
    }
}
