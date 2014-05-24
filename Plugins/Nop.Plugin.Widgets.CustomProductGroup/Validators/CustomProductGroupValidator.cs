using System;
using FluentValidation;
using Nop.Services.Localization;
using Nop.Plugin.Widgets.CustomProductGroup.Models;

namespace Nop.Plugin.Widgets.CustomProductGroup.Validators
{
    public class CustomProductGroupValidator : AbstractValidator<CustomProductGroupModel>
    {
        public CustomProductGroupValidator(ILocalizationService localizationService)
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage(localizationService.GetResource("Blog.Comments.CommentText.Required")); 
            RuleFor(x => x.Title).NotEmpty().WithMessage(localizationService.GetResource("Plugin.Widgets.CustomProductGroup.Title.Required"));
            RuleFor(x => x.Style).NotEmpty().WithMessage(localizationService.GetResource("Plugin.Widgets.CustomProductGroup.Style.Required"));
            RuleFor(x => x.MoreLink).NotEmpty().WithMessage(localizationService.GetResource("Plugin.Widgets.CustomProductGroup.MoreLink.Required"));
            RuleFor(x => x.SubCategoryLinks).NotEmpty().WithMessage(localizationService.GetResource("Plugin.Widgets.CustomProductGroup.SubCategoryLinks.Required"));
        }
    }
}
