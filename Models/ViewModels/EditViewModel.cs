using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MurkyPluse.Models.ViewModels
{
    public class EditViewModel
    {
        public BlogPost Post { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Categories { get; set; }
        [ValidateNever]
        public IFormFile? FeatureImage { get; set; }
    }
}
