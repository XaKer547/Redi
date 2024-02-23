using System.ComponentModel.DataAnnotations;

namespace Redi.Domain.DataAnnotation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class LowerCaseOnlyAttribute : ValidationAttribute
    {
        public LowerCaseOnlyAttribute() : base("string must be lowercase") { }
        public bool AllowEmptyStrings { get; set; }
        public override bool IsValid(object? value)
        {
            return AllowEmptyStrings || value is not string stringValue || !string.IsNullOrWhiteSpace(stringValue) || !stringValue.Any(c => char.IsUpper(c));
        }
    }
}
