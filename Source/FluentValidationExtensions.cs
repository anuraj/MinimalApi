using FluentValidation.Results;

namespace MinimalApi
{
    public static class FluentValidationExtensions
    {
        public static IDictionary<string, string[]> ToDictionary(this ValidationResult validationResult)
        {
            return validationResult.Errors
                  .GroupBy(x => x.PropertyName)
                  .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());
        }
    }
}
