using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace MurkyPluse.Models.ViewModels
{
    public class PostViewModel
    {
            public BlogPost Post { get; set; }
             [ValidateNever]
            public IEnumerable<SelectListItem> Categories { get; set; }
            public IFormFile? FeatureImage { get; set; }
        
    }
}
