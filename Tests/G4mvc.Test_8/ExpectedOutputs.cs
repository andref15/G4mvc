using G4mvc.TestBase;

namespace G4mvc.Test_8;

internal class ExpectedOutputs(string? mvcClassName = null, string? linksClassName = null, bool altRoot = false, bool additionalStatic = false, bool withVpp = false, bool classesInternal = false, bool excludeIco = false, bool excludeCss = false, bool customJsName = false) : ExpectedOutputsBase(mvcClassName, linksClassName, altRoot, additionalStatic, withVpp, classesInternal, excludeIco, excludeCss, customJsName)
{
    public override string SharedClass => @$"#nullable enable

namespace G4mvc.Routes;
{(ClassesInternal ? "internal" : "public")} class SharedRoutes
{{
	public SharedViews Views {{ get; }} = new();
	public class SharedViews
	{{
		public SharedViewNames ViewNames {{ get; }} = new();
		public string _Layout {{ get; }} = ""~//Views/Shared/_Layout.cshtml"";
		public string _ValidationScriptsPartial {{ get; }} = ""~//Views/Shared/_ValidationScriptsPartial.cshtml"";
		public string Error {{ get; }} = ""~//Views/Shared/Error.cshtml"";

		public class SharedViewNames
		{{
			public string _Layout {{ get; }} = nameof(_Layout);
			public string _ValidationScriptsPartial {{ get; }} = nameof(_ValidationScriptsPartial);
			public string Error {{ get; }} = nameof(Error);
		}}
	}}
}}
";

    public override string TestRoutesClass => @$"using G4mvc;

#nullable enable

namespace G4mvc.Routes;
{(ClassesInternal ? "internal" : "public")} class TestRoutes
{{
	public string Name {{ get; }} = ""Test"";
	public TestActionNames ActionNames {{ get; }} = new();
	public TestViews Views {{ get; }} = new();
	public IndexParamsClass IndexParams {{ get; }} = new();
	public PrivacyParamsClass PrivacyParams {{ get; }} = new();
	public ErrorParamsClass ErrorParams {{ get; }} = new();

	public G4mvcRouteValues Index()
	{{
		return new(null, ""Test"", ""Index"");
	}}

#nullable disable

	public G4mvcRouteValues Index(string test)
	{{
		G4mvcRouteValues route = Index();

		route[""test""] = test;

		return route;
	}}

#nullable restore


	public G4mvcRouteValues Privacy()
	{{
		return new(null, ""Test"", ""Privacy"");
	}}

	public G4mvcRouteValues Error()
	{{
		return new(null, ""Test"", ""Error"");
	}}

	public class TestActionNames
	{{
		public string Index {{ get; }} = nameof(Index);
		public string Privacy {{ get; }} = nameof(Privacy);
		public string Error {{ get; }} = nameof(Error);
	}}

	public class IndexParamsClass
	{{
		public string test {{ get; }} = nameof(test);
	}}

	public class PrivacyParamsClass
	{{
	}}

	public class ErrorParamsClass
	{{
	}}

	public class TestViews
	{{
		public TestViewNames ViewNames {{ get; }} = new();
		public string Index {{ get; }} = ""~//Views/Test/Index.cshtml"";
		public string Privacy {{ get; }} = ""~//Views/Test/Privacy.cshtml"";

		public class TestViewNames
		{{
			public string Index {{ get; }} = nameof(Index);
			public string Privacy {{ get; }} = nameof(Privacy);
		}}
	}}
}}
";

    public override string TestPartialRoutesClass => @$"using G4mvc;

#nullable enable

namespace G4mvc.Routes;
{(ClassesInternal ? "internal" : "public")} class TestPartialRoutes
{{
	public string Name {{ get; }} = ""TestPartial"";
	public TestPartialActionNames ActionNames {{ get; }} = new();
	public TestPartialViews Views {{ get; }} = new();
	public IndexParamsClass IndexParams {{ get; }} = new();
	public PrivacyParamsClass PrivacyParams {{ get; }} = new();
	public ErrorParamsClass ErrorParams {{ get; }} = new();

	public G4mvcRouteValues Index()
	{{
		return new(null, ""TestPartial"", ""Index"");
	}}

#nullable disable

	public G4mvcRouteValues Index(string test)
	{{
		G4mvcRouteValues route = Index();

		route[""test""] = test;

		return route;
	}}

#nullable restore


	public G4mvcRouteValues Privacy()
	{{
		return new(null, ""TestPartial"", ""Privacy"");
	}}

	public G4mvcRouteValues Error()
	{{
		return new(null, ""TestPartial"", ""Error"");
	}}

	public class TestPartialActionNames
	{{
		public string Index {{ get; }} = nameof(Index);
		public string Privacy {{ get; }} = nameof(Privacy);
		public string Error {{ get; }} = nameof(Error);
	}}

	public class IndexParamsClass
	{{
		public string test {{ get; }} = nameof(test);
	}}

	public class PrivacyParamsClass
	{{
	}}

	public class ErrorParamsClass
	{{
	}}

	public class TestPartialViews
	{{
		public TestPartialViewNames ViewNames {{ get; }} = new();

		public class TestPartialViewNames
		{{
		}}
	}}
}}
";

    public override string TestPartialClass => @$"using G4mvc;
using Microsoft.AspNetCore.Mvc;

#nullable enable

namespace G4mvc.{nameof(Test_8)}.Controllers;
public partial class TestPartialController
{{
	{(ClassesInternal ? "private " : "")}protected G4mvc.Routes.TestPartialRoutes.TestPartialViews Views {{ get; }} = {MvcClassName}.TestPartial.Views;

	protected RedirectToRouteResult RedirectToAction(G4mvcRouteValues route)
	{{
		return RedirectToRoute(route);
	}}

	protected RedirectToRouteResult RedirectToActionPermanent(G4mvcRouteValues route)
	{{
		return RedirectToRoutePermanent(route);
	}}
}}

";

    public override string MvcClass => @$"using G4mvc;
using G4mvc.Routes;

#nullable enable

{(ClassesInternal ? "internal" : "public")} class {MvcClassName}
{{
	public static SharedRoutes Shared {{ get; }} = new();
	public static TestRoutes Test {{ get; }} = new();
	public static TestPartialRoutes TestPartial {{ get; }} = new();
}}
";

    public override string LinksClass => WithVpp
        ? @"#nullable enable

internal static partial class VirtualPathProcessor
{
	public static partial string Process(string path);
}

public static partial class Links
{
	//v1;
	public const string UrlPath = ""~"";
	public static readonly string @favicon_ico = VirtualPathProcessor.Process(""~/favicon.ico"");

	public static partial class @css
	{
		public const string UrlPath = ""~/css"";
		public static readonly string @site_css = VirtualPathProcessor.Process(""~/css/site.css"");
	}

	public static partial class @js
	{
		public const string UrlPath = ""~/js"";
		public static readonly string @site_js = VirtualPathProcessor.Process(""~/js/site.js"");
	}

	public static partial class @lib
	{
		public const string UrlPath = ""~/lib"";

		public static partial class @bootstrap
		{
			public const string UrlPath = ""~/lib/bootstrap"";
			public static readonly string @LICENSE = VirtualPathProcessor.Process(""~/lib/bootstrap/LICENSE"");

			public static partial class @dist
			{
				public const string UrlPath = ""~/lib/bootstrap/dist"";

				public static partial class @css
				{
					public const string UrlPath = ""~/lib/bootstrap/dist/css"";
					public static readonly string @bootstrap_grid_css = VirtualPathProcessor.Process(""~/lib/bootstrap/dist/css/bootstrap-grid.css"");
					public static readonly string @bootstrap_grid_css_map = VirtualPathProcessor.Process(""~/lib/bootstrap/dist/css/bootstrap-grid.css.map"");
					public static readonly string @bootstrap_grid_min_css = VirtualPathProcessor.Process(""~/lib/bootstrap/dist/css/bootstrap-grid.min.css"");
					public static readonly string @bootstrap_grid_min_css_map = VirtualPathProcessor.Process(""~/lib/bootstrap/dist/css/bootstrap-grid.min.css.map"");
					public static readonly string @bootstrap_reboot_css = VirtualPathProcessor.Process(""~/lib/bootstrap/dist/css/bootstrap-reboot.css"");
					public static readonly string @bootstrap_reboot_css_map = VirtualPathProcessor.Process(""~/lib/bootstrap/dist/css/bootstrap-reboot.css.map"");
					public static readonly string @bootstrap_reboot_min_css = VirtualPathProcessor.Process(""~/lib/bootstrap/dist/css/bootstrap-reboot.min.css"");
					public static readonly string @bootstrap_reboot_min_css_map = VirtualPathProcessor.Process(""~/lib/bootstrap/dist/css/bootstrap-reboot.min.css.map"");
					public static readonly string @bootstrap_css = VirtualPathProcessor.Process(""~/lib/bootstrap/dist/css/bootstrap.css"");
					public static readonly string @bootstrap_css_map = VirtualPathProcessor.Process(""~/lib/bootstrap/dist/css/bootstrap.css.map"");
					public static readonly string @bootstrap_min_css = VirtualPathProcessor.Process(""~/lib/bootstrap/dist/css/bootstrap.min.css"");
					public static readonly string @bootstrap_min_css_map = VirtualPathProcessor.Process(""~/lib/bootstrap/dist/css/bootstrap.min.css.map"");
				}

				public static partial class @js
				{
					public const string UrlPath = ""~/lib/bootstrap/dist/js"";
					public static readonly string @bootstrap_bundle_js = VirtualPathProcessor.Process(""~/lib/bootstrap/dist/js/bootstrap.bundle.js"");
					public static readonly string @bootstrap_bundle_js_map = VirtualPathProcessor.Process(""~/lib/bootstrap/dist/js/bootstrap.bundle.js.map"");
					public static readonly string @bootstrap_bundle_min_js = VirtualPathProcessor.Process(""~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"");
					public static readonly string @bootstrap_bundle_min_js_map = VirtualPathProcessor.Process(""~/lib/bootstrap/dist/js/bootstrap.bundle.min.js.map"");
					public static readonly string @bootstrap_js = VirtualPathProcessor.Process(""~/lib/bootstrap/dist/js/bootstrap.js"");
					public static readonly string @bootstrap_js_map = VirtualPathProcessor.Process(""~/lib/bootstrap/dist/js/bootstrap.js.map"");
					public static readonly string @bootstrap_min_js = VirtualPathProcessor.Process(""~/lib/bootstrap/dist/js/bootstrap.min.js"");
					public static readonly string @bootstrap_min_js_map = VirtualPathProcessor.Process(""~/lib/bootstrap/dist/js/bootstrap.min.js.map"");
				}
			}
		}

		public static partial class @jquery
		{
			public const string UrlPath = ""~/lib/jquery"";
			public static readonly string @LICENSE_txt = VirtualPathProcessor.Process(""~/lib/jquery/LICENSE.txt"");

			public static partial class @dist
			{
				public const string UrlPath = ""~/lib/jquery/dist"";
				public static readonly string @jquery_js = VirtualPathProcessor.Process(""~/lib/jquery/dist/jquery.js"");
				public static readonly string @jquery_min_js = VirtualPathProcessor.Process(""~/lib/jquery/dist/jquery.min.js"");
				public static readonly string @jquery_min_map = VirtualPathProcessor.Process(""~/lib/jquery/dist/jquery.min.map"");
			}
		}

		public static partial class @jquery_validation
		{
			public const string UrlPath = ""~/lib/jquery-validation"";
			public static readonly string @LICENSE_md = VirtualPathProcessor.Process(""~/lib/jquery-validation/LICENSE.md"");

			public static partial class @dist
			{
				public const string UrlPath = ""~/lib/jquery-validation/dist"";
				public static readonly string @additional_methods_js = VirtualPathProcessor.Process(""~/lib/jquery-validation/dist/additional-methods.js"");
				public static readonly string @additional_methods_min_js = VirtualPathProcessor.Process(""~/lib/jquery-validation/dist/additional-methods.min.js"");
				public static readonly string @jquery_validate_js = VirtualPathProcessor.Process(""~/lib/jquery-validation/dist/jquery.validate.js"");
				public static readonly string @jquery_validate_min_js = VirtualPathProcessor.Process(""~/lib/jquery-validation/dist/jquery.validate.min.js"");
			}
		}

		public static partial class @jquery_validation_unobtrusive
		{
			public const string UrlPath = ""~/lib/jquery-validation-unobtrusive"";
			public static readonly string @jquery_validate_unobtrusive_js = VirtualPathProcessor.Process(""~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js"");
			public static readonly string @jquery_validate_unobtrusive_min_js = VirtualPathProcessor.Process(""~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js"");
			public static readonly string @LICENSE_txt = VirtualPathProcessor.Process(""~/lib/jquery-validation-unobtrusive/LICENSE.txt"");
		}
	}
}
"
        : @$"#nullable enable

{(ClassesInternal ? "internal" : "public")} static partial class {LinksClassName}
{{
	public const string UrlPath = ""~"";
	{(ExcludeIco ? "" : @$"public const string @{(AltRoot ? "alt" : "")}favicon_ico = ""~/{(AltRoot ? "alt" : "")}favicon.ico"";")}

{(ExcludeCss ? "" : @$"public static partial class @css
	{{
		public const string UrlPath = ""~/css"";
		public const string @{(AltRoot ? "alt" : "")}site_css = ""~/css/{(AltRoot ? "alt" : "")}site.css"";
	}}")}

	public static partial class @{(CustomJsName ? "otherjs" : "js")}
	{{
		public const string UrlPath = ""~/js"";
		public const string @{(AltRoot ? "alt" : "")}site_js = ""~/js/{(AltRoot ? "alt" : "")}site.js"";
	}}

	public static partial class @lib
	{{
		public const string UrlPath = ""~/lib"";

		public static partial class @bootstrap
		{{
			public const string UrlPath = ""~/lib/bootstrap"";
			public const string @LICENSE = ""~/lib/bootstrap/LICENSE"";

			public static partial class @dist
			{{
				public const string UrlPath = ""~/lib/bootstrap/dist"";

				public static partial class @css
				{{
					public const string UrlPath = ""~/lib/bootstrap/dist/css"";
					public const string @bootstrap_grid_css = ""~/lib/bootstrap/dist/css/bootstrap-grid.css"";
					public const string @bootstrap_grid_css_map = ""~/lib/bootstrap/dist/css/bootstrap-grid.css.map"";
					public const string @bootstrap_grid_min_css = ""~/lib/bootstrap/dist/css/bootstrap-grid.min.css"";
					public const string @bootstrap_grid_min_css_map = ""~/lib/bootstrap/dist/css/bootstrap-grid.min.css.map"";
					public const string @bootstrap_reboot_css = ""~/lib/bootstrap/dist/css/bootstrap-reboot.css"";
					public const string @bootstrap_reboot_css_map = ""~/lib/bootstrap/dist/css/bootstrap-reboot.css.map"";
					public const string @bootstrap_reboot_min_css = ""~/lib/bootstrap/dist/css/bootstrap-reboot.min.css"";
					public const string @bootstrap_reboot_min_css_map = ""~/lib/bootstrap/dist/css/bootstrap-reboot.min.css.map"";
					public const string @bootstrap_css = ""~/lib/bootstrap/dist/css/bootstrap.css"";
					public const string @bootstrap_css_map = ""~/lib/bootstrap/dist/css/bootstrap.css.map"";
					public const string @bootstrap_min_css = ""~/lib/bootstrap/dist/css/bootstrap.min.css"";
					public const string @bootstrap_min_css_map = ""~/lib/bootstrap/dist/css/bootstrap.min.css.map"";
				}}

				public static partial class @js
				{{
					public const string UrlPath = ""~/lib/bootstrap/dist/js"";
					public const string @bootstrap_bundle_js = ""~/lib/bootstrap/dist/js/bootstrap.bundle.js"";
					public const string @bootstrap_bundle_js_map = ""~/lib/bootstrap/dist/js/bootstrap.bundle.js.map"";
					public const string @bootstrap_bundle_min_js = ""~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"";
					public const string @bootstrap_bundle_min_js_map = ""~/lib/bootstrap/dist/js/bootstrap.bundle.min.js.map"";
					public const string @bootstrap_js = ""~/lib/bootstrap/dist/js/bootstrap.js"";
					public const string @bootstrap_js_map = ""~/lib/bootstrap/dist/js/bootstrap.js.map"";
					public const string @bootstrap_min_js = ""~/lib/bootstrap/dist/js/bootstrap.min.js"";
					public const string @bootstrap_min_js_map = ""~/lib/bootstrap/dist/js/bootstrap.min.js.map"";
				}}
			}}
		}}

		public static partial class @jquery
		{{
			public const string UrlPath = ""~/lib/jquery"";
			public const string @LICENSE_txt = ""~/lib/jquery/LICENSE.txt"";

			public static partial class @dist
			{{
				public const string UrlPath = ""~/lib/jquery/dist"";
				public const string @jquery_js = ""~/lib/jquery/dist/jquery.js"";
				public const string @jquery_min_js = ""~/lib/jquery/dist/jquery.min.js"";
				public const string @jquery_min_map = ""~/lib/jquery/dist/jquery.min.map"";
			}}
		}}

		public static partial class @jquery_validation
		{{
			public const string UrlPath = ""~/lib/jquery-validation"";
			public const string @LICENSE_md = ""~/lib/jquery-validation/LICENSE.md"";

			public static partial class @dist
			{{
				public const string UrlPath = ""~/lib/jquery-validation/dist"";
				public const string @additional_methods_js = ""~/lib/jquery-validation/dist/additional-methods.js"";
				public const string @additional_methods_min_js = ""~/lib/jquery-validation/dist/additional-methods.min.js"";
				public const string @jquery_validate_js = ""~/lib/jquery-validation/dist/jquery.validate.js"";
				public const string @jquery_validate_min_js = ""~/lib/jquery-validation/dist/jquery.validate.min.js"";
			}}
		}}

		public static partial class @jquery_validation_unobtrusive
		{{
			public const string UrlPath = ""~/lib/jquery-validation-unobtrusive"";
			public const string @jquery_validate_unobtrusive_js = ""~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js"";
			public const string @jquery_validate_unobtrusive_min_js = ""~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js"";
			public const string @LICENSE_txt = ""~/lib/jquery-validation-unobtrusive/LICENSE.txt"";
		}}
	}}

	{(AdditionalStatic ? @"public static partial class @wwwrootAlt
	{
		public const string UrlPath = ""~/wwwrootAlt"";
		public const string @altfavicon_ico = ""~/wwwrootAlt/altfavicon.ico"";

		public static partial class @css
		{
			public const string UrlPath = ""~/wwwrootAlt/css"";
			public const string @altsite_css = ""~/wwwrootAlt/css/altsite.css"";
		}

		public static partial class @js
		{
			public const string UrlPath = ""~/wwwrootAlt/js"";
			public const string @altsite_js = ""~/wwwrootAlt/js/altsite.js"";
		}

		public static partial class @lib
		{
			public const string UrlPath = ""~/wwwrootAlt/lib"";

			public static partial class @bootstrap
			{
				public const string UrlPath = ""~/wwwrootAlt/lib/bootstrap"";
				public const string @LICENSE = ""~/wwwrootAlt/lib/bootstrap/LICENSE"";

				public static partial class @dist
				{
					public const string UrlPath = ""~/wwwrootAlt/lib/bootstrap/dist"";

					public static partial class @css
					{
						public const string UrlPath = ""~/wwwrootAlt/lib/bootstrap/dist/css"";
						public const string @bootstrap_grid_css = ""~/wwwrootAlt/lib/bootstrap/dist/css/bootstrap-grid.css"";
						public const string @bootstrap_grid_css_map = ""~/wwwrootAlt/lib/bootstrap/dist/css/bootstrap-grid.css.map"";
						public const string @bootstrap_grid_min_css = ""~/wwwrootAlt/lib/bootstrap/dist/css/bootstrap-grid.min.css"";
						public const string @bootstrap_grid_min_css_map = ""~/wwwrootAlt/lib/bootstrap/dist/css/bootstrap-grid.min.css.map"";
						public const string @bootstrap_reboot_css = ""~/wwwrootAlt/lib/bootstrap/dist/css/bootstrap-reboot.css"";
						public const string @bootstrap_reboot_css_map = ""~/wwwrootAlt/lib/bootstrap/dist/css/bootstrap-reboot.css.map"";
						public const string @bootstrap_reboot_min_css = ""~/wwwrootAlt/lib/bootstrap/dist/css/bootstrap-reboot.min.css"";
						public const string @bootstrap_reboot_min_css_map = ""~/wwwrootAlt/lib/bootstrap/dist/css/bootstrap-reboot.min.css.map"";
						public const string @bootstrap_css = ""~/wwwrootAlt/lib/bootstrap/dist/css/bootstrap.css"";
						public const string @bootstrap_css_map = ""~/wwwrootAlt/lib/bootstrap/dist/css/bootstrap.css.map"";
						public const string @bootstrap_min_css = ""~/wwwrootAlt/lib/bootstrap/dist/css/bootstrap.min.css"";
						public const string @bootstrap_min_css_map = ""~/wwwrootAlt/lib/bootstrap/dist/css/bootstrap.min.css.map"";
					}

					public static partial class @js
					{
						public const string UrlPath = ""~/wwwrootAlt/lib/bootstrap/dist/js"";
						public const string @bootstrap_bundle_js = ""~/wwwrootAlt/lib/bootstrap/dist/js/bootstrap.bundle.js"";
						public const string @bootstrap_bundle_js_map = ""~/wwwrootAlt/lib/bootstrap/dist/js/bootstrap.bundle.js.map"";
						public const string @bootstrap_bundle_min_js = ""~/wwwrootAlt/lib/bootstrap/dist/js/bootstrap.bundle.min.js"";
						public const string @bootstrap_bundle_min_js_map = ""~/wwwrootAlt/lib/bootstrap/dist/js/bootstrap.bundle.min.js.map"";
						public const string @bootstrap_js = ""~/wwwrootAlt/lib/bootstrap/dist/js/bootstrap.js"";
						public const string @bootstrap_js_map = ""~/wwwrootAlt/lib/bootstrap/dist/js/bootstrap.js.map"";
						public const string @bootstrap_min_js = ""~/wwwrootAlt/lib/bootstrap/dist/js/bootstrap.min.js"";
						public const string @bootstrap_min_js_map = ""~/wwwrootAlt/lib/bootstrap/dist/js/bootstrap.min.js.map"";
					}
				}
			}

			public static partial class @jquery
			{
				public const string UrlPath = ""~/wwwrootAlt/lib/jquery"";
				public const string @LICENSE_txt = ""~/wwwrootAlt/lib/jquery/LICENSE.txt"";

				public static partial class @dist
				{
					public const string UrlPath = ""~/wwwrootAlt/lib/jquery/dist"";
					public const string @jquery_js = ""~/wwwrootAlt/lib/jquery/dist/jquery.js"";
					public const string @jquery_min_js = ""~/wwwrootAlt/lib/jquery/dist/jquery.min.js"";
					public const string @jquery_min_map = ""~/wwwrootAlt/lib/jquery/dist/jquery.min.map"";
				}
			}

			public static partial class @jquery_validation
			{
				public const string UrlPath = ""~/wwwrootAlt/lib/jquery-validation"";
				public const string @LICENSE_md = ""~/wwwrootAlt/lib/jquery-validation/LICENSE.md"";

				public static partial class @dist
				{
					public const string UrlPath = ""~/wwwrootAlt/lib/jquery-validation/dist"";
					public const string @additional_methods_js = ""~/wwwrootAlt/lib/jquery-validation/dist/additional-methods.js"";
					public const string @additional_methods_min_js = ""~/wwwrootAlt/lib/jquery-validation/dist/additional-methods.min.js"";
					public const string @jquery_validate_js = ""~/wwwrootAlt/lib/jquery-validation/dist/jquery.validate.js"";
					public const string @jquery_validate_min_js = ""~/wwwrootAlt/lib/jquery-validation/dist/jquery.validate.min.js"";
				}
			}

			public static partial class @jquery_validation_unobtrusive
			{
				public const string UrlPath = ""~/wwwrootAlt/lib/jquery-validation-unobtrusive"";
				public const string @jquery_validate_unobtrusive_js = ""~/wwwrootAlt/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js"";
				public const string @jquery_validate_unobtrusive_min_js = ""~/wwwrootAlt/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js"";
				public const string @LICENSE_txt = ""~/wwwrootAlt/lib/jquery-validation-unobtrusive/LICENSE.txt"";
			}
		}
	}" : "")}
}}
";
}
