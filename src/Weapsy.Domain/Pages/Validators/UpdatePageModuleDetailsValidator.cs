﻿using FluentValidation;
using System;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Sites.Rules;

namespace Weapsy.Domain.Pages.Validators
{
    public class UpdatePageModuleDetailsValidator : AbstractValidator<UpdatePageModuleDetails>
    {
        private readonly ISiteRules _siteRules;
        private readonly IValidator<PageModuleLocalisation> _localisationValidator;

        public UpdatePageModuleDetailsValidator(ISiteRules siteRules, IValidator<PageModuleLocalisation> localisationValidator)
        {
            _siteRules = siteRules;
            _localisationValidator = localisationValidator;

            RuleFor(c => c.SiteId)
                .NotEmpty().WithMessage("Site id is required.")
                .Must(BeAnExistingSite).WithMessage("Site does not exist.");

            RuleFor(c => c.Title)
                .Length(1, 250).WithMessage("Title cannot have more than 250 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Title));

            RuleFor(c => c.PageModuleLocalisations)
                .SetCollectionValidator(_localisationValidator);
        }

        private bool BeAnExistingSite(Guid siteId)
        {
            return _siteRules.DoesSiteExist(siteId);
        }
    }
}
