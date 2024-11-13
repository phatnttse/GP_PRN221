using Blossom_BusinessObjects;
using System.ComponentModel.DataAnnotations;

namespace Blossom_RazorWeb.ViewModels
{
    public class FeedbackViewModel
    {
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description can't be longer than 500 characters.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please select a rating.")]
        [EnumDataType(typeof(RatingEnum), ErrorMessage = "Invalid rating value.")]
        public RatingEnum Rating { get; set; }
    }
}
