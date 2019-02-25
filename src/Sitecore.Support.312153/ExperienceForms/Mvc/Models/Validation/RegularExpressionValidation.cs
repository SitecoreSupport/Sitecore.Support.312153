using Sitecore.ExperienceForms.Mvc.Models.Fields;
using Sitecore.ExperienceForms.Mvc.Models.Validation;
using Sitecore.ExperienceForms.Mvc.Models.Validation.Parameters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Sitecore.Support.ExperienceForms.Mvc.Models.Validation
{
  public class RegularExpressionValidation : ValidationElement<RegularExpressionParameters>
  {
    public RegularExpressionValidation(ValidationDataModel validationItem)
        : base(validationItem)
    {
    }

    protected virtual string RegularExpression { get; set; }

    protected virtual string Title { get; set; }

    public override IEnumerable<ModelClientValidationRule> ClientValidationRules
    {
      get
      {
        if (string.IsNullOrEmpty(RegularExpression))
        {
          yield break;
        }

        var regex = new Regex(RegularExpression, RegexOptions.ExplicitCapture | RegexOptions.Compiled);

        yield return new ModelClientValidationRegexRule(FormatMessage(Title), regex.ToString());
      }
    }

    public override void Initialize(object validationModel)
    {
      base.Initialize(validationModel);

      var obj = validationModel as StringInputViewModel;
      if (obj != null)
      {
        Title = obj.Title;
      }

      RegularExpression = Parameters?.RegularExpression ?? string.Empty;
    }

    public override ValidationResult Validate(object value)
    {
      if (value == null || string.IsNullOrEmpty(RegularExpression))
      {
        return ValidationResult.Success;
      }

      var regex = new Regex(RegularExpression, RegexOptions.ExplicitCapture | RegexOptions.Compiled);

      var stringValue = (string)value;
      if (string.IsNullOrEmpty(stringValue) || regex.IsMatch(stringValue))
      {
        return ValidationResult.Success;
      }

      return new ValidationResult(FormatMessage(Title));
    }
  }
}