namespace G4mvc.Generator.CSharp;
internal static class TypeNames
{
    public const string AreaAttribute = $"{Namespaces.MicrosoftAspNetCoreMvc}.AreaAttribute";
    public const string CancellationToken = $"{Namespaces.SystemThreading}.CancellationToken";
    public const string Controller = $"{Namespaces.MicrosoftAspNetCoreMvc}.Controller";
    public const string HttpDeleteAttribute = $"{Namespaces.MicrosoftAspNetCoreMvc}.HttpDeleteAttribute";
    public const string HttpGetAttribute = $"{Namespaces.MicrosoftAspNetCoreMvc}.HttpGetAttribute";
    public const string HttpMethodAttribute = $"{Namespaces.MicrosoftAspNetCoreMvcRouting}.HttpMethodAttribute";
    public const string HttpPatchAttribute = $"{Namespaces.MicrosoftAspNetCoreMvc}.HttpPatchAttribute";
    public const string HttpPostAttribute = $"{Namespaces.MicrosoftAspNetCoreMvc}.HttpPostAttribute";
    public const string HttpPutAttribute = $"{Namespaces.MicrosoftAspNetCoreMvc}.HttpPutAttribute";
    public const string IActionResult = $"{Namespaces.MicrosoftAspNetCoreMvc}.IActionResult";
    public const string IConvertToActionResult = $"{Namespaces.MicrosoftAspNetCoreMvcInfrastructure}.IConvertToActionResult";
    public const string PageModel = $"{Namespaces.MicrosoftAspNetCoreMvcRazorPages}.PageModel";
    public const string PageModelAttribute = $"{Namespaces.MicrosoftAspNetCoreMvcRazorPagesInfrastructure}.PageModelAttribute";
    public const string Task = $"{Namespaces.SystemThreadingTasks}.Task";

    public static class NonControllerAttribute
    {
        public const string Name = "NonControllerAttribute";
        public const string ShortName = "NonController";
        public const string FullName = $"{Namespaces.MicrosoftAspNetCoreMvc}.{Name}";
    }

    public static class NonActionAttribute
    {
        public const string Name = "NonActionAttribute";
        public const string ShortName = "NonAttribute";
        public const string FullName = $"{Namespaces.MicrosoftAspNetCoreMvc}.{Name}";
    }
}
