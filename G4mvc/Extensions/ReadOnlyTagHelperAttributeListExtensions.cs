#if !NETSTANDARD
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace G4mvc.Extensions;

internal static class ReadOnlyTagHelperAttributeListExtensions
{
    extension(ReadOnlyTagHelperAttributeList attributes)
    {
        public bool AttributeHasValue(string name)
        {
            return attributes.TryGetAttribute(name, out var attribute) && !string.IsNullOrEmpty(attribute.Value?.ToString());
        }
    }
}

#endif