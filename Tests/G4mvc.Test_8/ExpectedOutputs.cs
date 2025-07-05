using G4mvc.TestBase;

namespace G4mvc.Test_8;

internal class ExpectedOutputs(string? mvcClassName = null, string? linksClassName = null, bool altRoot = false, bool additionalStatic = false, bool withVpp = false, bool vppForContent = false, bool classesInternal = false, bool enumerateSubDirectories = false, string? classNamespace = null, bool excludeIco = false, bool excludeCss = false, bool customJsName = false)
    : ExpectedOutputsBase(mvcClassName, linksClassName, altRoot, additionalStatic, withVpp, vppForContent, classesInternal, classNamespace, enumerateSubDirectories, excludeIco, excludeCss, customJsName)
{
    public override string SharedClass => $$"""
        #nullable enable

        namespace G4mvc.Routes;
        {{(ClassesInternal ? "internal" : "public")}} class SharedRoutes
        {
            public SharedViews Views { get; } = new();
            public class SharedViews
            {
                public SharedViewNames ViewNames { get; } = new();
                public string _Layout { get; } = "~/Views/Shared/_Layout.cshtml";
                public string _ValidationScriptsPartial { get; } = "~/Views/Shared/_ValidationScriptsPartial.cshtml";
                public string Error { get; } = "~/Views/Shared/Error.cshtml";

                public class SharedViewNames
                {
                    public string _Layout { get; } = nameof(_Layout);
                    public string _ValidationScriptsPartial { get; } = nameof(_ValidationScriptsPartial);
                    public string Error { get; } = nameof(Error);
                }
            }
        }
        """;

    public override string TestRoutesClass => $$"""
        using G4mvc;

        #nullable enable

        namespace G4mvc.Routes;
        {{(ClassesInternal ? "internal" : "public")}} class TestRoutes
        {
            public string Name { get; } = "Test";
            public TestActionNames ActionNames { get; } = new();
            public TestViews Views { get; } = new();
            public IndexParamsClass IndexParams { get; } = new();
            public PrivacyParamsClass PrivacyParams { get; } = new();
            public ErrorParamsClass ErrorParams { get; } = new();

            public G4mvcRouteValues Index()
            {
                return new(null, "Test", "Index");
            }

        #nullable disable

            public G4mvcRouteValues Index(string test)
            {
                G4mvcRouteValues route = Index();

                route["test"] = test;

                return route;
            }

        #nullable restore


            public G4mvcRouteValues Privacy()
            {
                return new(null, "Test", "Privacy");
            }

            public G4mvcRouteValues Error()
            {
                return new(null, "Test", "Error");
            }

            public class TestActionNames
            {
                public string Index { get; } = nameof(Index);
                public string Privacy { get; } = nameof(Privacy);
                public string Error { get; } = nameof(Error);
            }

            public class IndexParamsClass
            {
                public string test { get; } = nameof(test);
            }

            public class PrivacyParamsClass
            {
            }

            public class ErrorParamsClass
            {
            }

            public class TestViews
            {
                public TestViewNames ViewNames { get; } = new();
                public string Index { get; } = "~/Views/Test/Index.cshtml";
                public string Privacy { get; } = "~/Views/Test/Privacy.cshtml";

                public class TestViewNames
                {
                    public string Index { get; } = nameof(Index);
                    public string Privacy { get; } = nameof(Privacy);
                }

                {{(EnumerateSubDirectories ? """
                public SubDirViews SubDir { get; } = new();
                public class SubDirViews
                {
                    public SubDirViewNames ViewNames { get; } = new();
                    public string SubItem { get; } = "~/Views/Test/SubDir/SubItem.cshtml";

                    public class SubDirViewNames
                    {
                        public string SubItem { get; } = nameof(SubItem);
                    }
                }
                """ : null)}}
            }
        }
        """;

    public override string TestPartialRoutesClass => $$"""
        using G4mvc;

        #nullable enable

        namespace G4mvc.Routes;
        {{(ClassesInternal ? "internal" : "public")}} class TestPartialRoutes
        {
            public string Name { get; } = "TestPartial";
            public TestPartialActionNames ActionNames { get; } = new();
            public TestPartialViews Views { get; } = new();
            public IndexParamsClass IndexParams { get; } = new();
            public PrivacyParamsClass PrivacyParams { get; } = new();
            public ErrorParamsClass ErrorParams { get; } = new();

            public G4mvcRouteValues Index()
            {
                return new(null, "TestPartial", "Index");
            }

        #nullable disable

            public G4mvcRouteValues Index(string test)
            {
                G4mvcRouteValues route = Index();

                route["test"] = test;

                return route;
            }

        #nullable restore


            public G4mvcRouteValues Privacy()
            {
                return new(null, "TestPartial", "Privacy");
            }

            public G4mvcRouteValues Error()
            {
                return new(null, "TestPartial", "Error");
            }

            public class TestPartialActionNames
            {
                public string Index { get; } = nameof(Index);
                public string Privacy { get; } = nameof(Privacy);
                public string Error { get; } = nameof(Error);
            }

            public class IndexParamsClass
            {
                public string test { get; } = nameof(test);
            }

            public class PrivacyParamsClass
            {
            }

            public class ErrorParamsClass
            {
            }

            public class TestPartialViews
            {
            }
        }
        """;

    public override string TestPartialClass => $$"""
        using G4mvc;
        using Microsoft.AspNetCore.Mvc;
        {{(ClassNamespace is null ? "" : $"using {ClassNamespace};")}}

        #nullable enable

        namespace G4mvc.{{nameof(Test_8)}}.Controllers;
        public partial class TestPartialController
        {
            {{(ClassesInternal ? "private " : "")}}protected G4mvc.Routes.TestPartialRoutes.TestPartialViews Views { get; } = {{MvcClassName}}.TestPartial.Views;

            protected RedirectToRouteResult RedirectToAction(G4mvcRouteValues route)
            {
                return RedirectToRoute(route);
            }

            protected RedirectToRouteResult RedirectToActionPermanent(G4mvcRouteValues route)
            {
                return RedirectToRoutePermanent(route);
            }
        }

        """;

    public override string MvcClass => $$"""
        using G4mvc;
        using G4mvc.Routes;

        {{(ClassNamespace is null ? "" : $"namespace {ClassNamespace};\n")}}
        #nullable enable

        {{(ClassesInternal ? "internal" : "public")}} class {{MvcClassName}}
        {
            public static TestRoutes Test { get; } = new();
            public static TestPartialRoutes TestPartial { get; } = new();
            public static SharedRoutes Shared { get; } = new();
        }
        """;

    public override string LinksClass => WithVpp
        ? $$"""
        using G4mvc;

        {{(ClassNamespace is null ? "" : $"namespace {ClassNamespace};\n")}}
        #nullable enable

        internal static partial class VirtualPathProcessor
        {
            public static partial string Process(string path);
        }

        {{(ClassesInternal ? "internal" : "public")}} static partial class {{LinksClassName}}
        {
            //v1;
            public const string UrlPath = "~";
            public static readonly G4mvcContentLink @favicon_ico = new("~/favicon.ico", VirtualPathProcessor.Process, {{VppForContentStr}});

            public static partial class @css
            {
                public const string UrlPath = "~/css";
                public static readonly G4mvcContentLink @site_css = new("~/css/site.css", VirtualPathProcessor.Process, {{VppForContentStr}});
            }

            public static partial class @js
            {
                public const string UrlPath = "~/js";
                public static readonly G4mvcContentLink @site_js = new("~/js/site.js", VirtualPathProcessor.Process, {{VppForContentStr}});
            }

            public static partial class @lib
            {
                public const string UrlPath = "~/lib";

                public static partial class @bootstrap
                {
                    public const string UrlPath = "~/lib/bootstrap";
                    public static readonly G4mvcContentLink @LICENSE = new("~/lib/bootstrap/LICENSE", VirtualPathProcessor.Process, {{VppForContentStr}});

                    public static partial class @dist
                    {
                        public const string UrlPath = "~/lib/bootstrap/dist";

                        public static partial class @css
                        {
                            public const string UrlPath = "~/lib/bootstrap/dist/css";
                            public static readonly G4mvcContentLink @bootstrap_grid_css = new("~/lib/bootstrap/dist/css/bootstrap-grid.css", VirtualPathProcessor.Process, {{VppForContentStr}});
                            public static readonly G4mvcContentLink @bootstrap_grid_css_map = new("~/lib/bootstrap/dist/css/bootstrap-grid.css.map", VirtualPathProcessor.Process, {{VppForContentStr}});
                            public static readonly G4mvcContentLink @bootstrap_grid_min_css = new("~/lib/bootstrap/dist/css/bootstrap-grid.min.css", VirtualPathProcessor.Process, {{VppForContentStr}});
                            public static readonly G4mvcContentLink @bootstrap_grid_min_css_map = new("~/lib/bootstrap/dist/css/bootstrap-grid.min.css.map", VirtualPathProcessor.Process, {{VppForContentStr}});
                            public static readonly G4mvcContentLink @bootstrap_reboot_css = new("~/lib/bootstrap/dist/css/bootstrap-reboot.css", VirtualPathProcessor.Process, {{VppForContentStr}});
                            public static readonly G4mvcContentLink @bootstrap_reboot_css_map = new("~/lib/bootstrap/dist/css/bootstrap-reboot.css.map", VirtualPathProcessor.Process, {{VppForContentStr}});
                            public static readonly G4mvcContentLink @bootstrap_reboot_min_css = new("~/lib/bootstrap/dist/css/bootstrap-reboot.min.css", VirtualPathProcessor.Process, {{VppForContentStr}});
                            public static readonly G4mvcContentLink @bootstrap_reboot_min_css_map = new("~/lib/bootstrap/dist/css/bootstrap-reboot.min.css.map", VirtualPathProcessor.Process, {{VppForContentStr}});
                            public static readonly G4mvcContentLink @bootstrap_css = new("~/lib/bootstrap/dist/css/bootstrap.css", VirtualPathProcessor.Process, {{VppForContentStr}});
                            public static readonly G4mvcContentLink @bootstrap_css_map = new("~/lib/bootstrap/dist/css/bootstrap.css.map", VirtualPathProcessor.Process, {{VppForContentStr}});
                            public static readonly G4mvcContentLink @bootstrap_min_css = new("~/lib/bootstrap/dist/css/bootstrap.min.css", VirtualPathProcessor.Process, {{VppForContentStr}});
                            public static readonly G4mvcContentLink @bootstrap_min_css_map = new("~/lib/bootstrap/dist/css/bootstrap.min.css.map", VirtualPathProcessor.Process, {{VppForContentStr}});
                        }

                        public static partial class @js
                        {
                            public const string UrlPath = "~/lib/bootstrap/dist/js";
                            public static readonly G4mvcContentLink @bootstrap_bundle_js = new("~/lib/bootstrap/dist/js/bootstrap.bundle.js", VirtualPathProcessor.Process, {{VppForContentStr}});
                            public static readonly G4mvcContentLink @bootstrap_bundle_js_map = new("~/lib/bootstrap/dist/js/bootstrap.bundle.js.map", VirtualPathProcessor.Process, {{VppForContentStr}});
                            public static readonly G4mvcContentLink @bootstrap_bundle_min_js = new("~/lib/bootstrap/dist/js/bootstrap.bundle.min.js", VirtualPathProcessor.Process, {{VppForContentStr}});
                            public static readonly G4mvcContentLink @bootstrap_bundle_min_js_map = new("~/lib/bootstrap/dist/js/bootstrap.bundle.min.js.map", VirtualPathProcessor.Process, {{VppForContentStr}});
                            public static readonly G4mvcContentLink @bootstrap_js = new("~/lib/bootstrap/dist/js/bootstrap.js", VirtualPathProcessor.Process, {{VppForContentStr}});
                            public static readonly G4mvcContentLink @bootstrap_js_map = new("~/lib/bootstrap/dist/js/bootstrap.js.map", VirtualPathProcessor.Process, {{VppForContentStr}});
                            public static readonly G4mvcContentLink @bootstrap_min_js = new("~/lib/bootstrap/dist/js/bootstrap.min.js", VirtualPathProcessor.Process, {{VppForContentStr}});
                            public static readonly G4mvcContentLink @bootstrap_min_js_map = new("~/lib/bootstrap/dist/js/bootstrap.min.js.map", VirtualPathProcessor.Process, {{VppForContentStr}});
                        }
                    }
                }

                public static partial class @jquery
                {
                    public const string UrlPath = "~/lib/jquery";
                    public static readonly G4mvcContentLink @LICENSE_txt = new("~/lib/jquery/LICENSE.txt", VirtualPathProcessor.Process, {{VppForContentStr}});

                    public static partial class @dist
                    {
                        public const string UrlPath = "~/lib/jquery/dist";
                        public static readonly G4mvcContentLink @jquery_js = new("~/lib/jquery/dist/jquery.js", VirtualPathProcessor.Process, {{VppForContentStr}});
                        public static readonly G4mvcContentLink @jquery_min_js = new("~/lib/jquery/dist/jquery.min.js", VirtualPathProcessor.Process, {{VppForContentStr}});
                        public static readonly G4mvcContentLink @jquery_min_map = new("~/lib/jquery/dist/jquery.min.map", VirtualPathProcessor.Process, {{VppForContentStr}});
                    }
                }

                public static partial class @jquery_validation
                {
                    public const string UrlPath = "~/lib/jquery-validation";
                    public static readonly G4mvcContentLink @LICENSE_md = new("~/lib/jquery-validation/LICENSE.md", VirtualPathProcessor.Process, {{VppForContentStr}});

                    public static partial class @dist
                    {
                        public const string UrlPath = "~/lib/jquery-validation/dist";
                        public static readonly G4mvcContentLink @additional_methods_js = new("~/lib/jquery-validation/dist/additional-methods.js", VirtualPathProcessor.Process, {{VppForContentStr}});
                        public static readonly G4mvcContentLink @additional_methods_min_js = new("~/lib/jquery-validation/dist/additional-methods.min.js", VirtualPathProcessor.Process, {{VppForContentStr}});
                        public static readonly G4mvcContentLink @jquery_validate_js = new("~/lib/jquery-validation/dist/jquery.validate.js", VirtualPathProcessor.Process, {{VppForContentStr}});
                        public static readonly G4mvcContentLink @jquery_validate_min_js = new("~/lib/jquery-validation/dist/jquery.validate.min.js", VirtualPathProcessor.Process, {{VppForContentStr}});
                    }
                }

                public static partial class @jquery_validation_unobtrusive
                {
                    public const string UrlPath = "~/lib/jquery-validation-unobtrusive";
                    public static readonly G4mvcContentLink @jquery_validate_unobtrusive_js = new("~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js", VirtualPathProcessor.Process, {{VppForContentStr}});
                    public static readonly G4mvcContentLink @jquery_validate_unobtrusive_min_js = new("~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js", VirtualPathProcessor.Process, {{VppForContentStr}});
                    public static readonly G4mvcContentLink @LICENSE_txt = new("~/lib/jquery-validation-unobtrusive/LICENSE.txt", VirtualPathProcessor.Process, {{VppForContentStr}});
                }
            }
        }

        """
        : $$"""
        using G4mvc;

        {{(ClassNamespace is null ? "" : $"namespace {ClassNamespace};\n")}}
        #nullable enable

        {{(ClassesInternal ? "internal" : "public")}} static partial class {{LinksClassName}}
        {
            public const string UrlPath = "~";
            {{(ExcludeIco ? "" : $"""
                public static readonly G4mvcContentLink @{(AltRoot ? "alt" : "")}favicon_ico = new("~/{(AltRoot ? "alt" : "")}favicon.ico");
            """)}}

            {{(ExcludeCss ? "" : $$"""
                public static partial class @css
                {
                    public const string UrlPath = "~/css";
                    public static readonly G4mvcContentLink @{{(AltRoot ? "alt" : "")}}site_css = new("~/css/{{(AltRoot ? "alt" : "")}}site.css");
                }
                """)}}

            public static partial class @{{(CustomJsName ? "otherjs" : "js")}}
            {
                public const string UrlPath = "~/js";
                public static readonly G4mvcContentLink @{{(AltRoot ? "alt" : "")}}site_js = new("~/js/{{(AltRoot ? "alt" : "")}}site.js");
            }

            public static partial class @lib
            {
                public const string UrlPath = "~/lib";

                public static partial class @bootstrap
                {
                    public const string UrlPath = "~/lib/bootstrap";
                    public static readonly G4mvcContentLink @LICENSE = new("~/lib/bootstrap/LICENSE");

                    public static partial class @dist
                    {
                        public const string UrlPath = "~/lib/bootstrap/dist";

                        public static partial class @css
                        {
                            public const string UrlPath = "~/lib/bootstrap/dist/css";
                            public static readonly G4mvcContentLink @bootstrap_grid_css = new("~/lib/bootstrap/dist/css/bootstrap-grid.css");
                            public static readonly G4mvcContentLink @bootstrap_grid_css_map = new("~/lib/bootstrap/dist/css/bootstrap-grid.css.map");
                            public static readonly G4mvcContentLink @bootstrap_grid_min_css = new("~/lib/bootstrap/dist/css/bootstrap-grid.min.css");
                            public static readonly G4mvcContentLink @bootstrap_grid_min_css_map = new("~/lib/bootstrap/dist/css/bootstrap-grid.min.css.map");
                            public static readonly G4mvcContentLink @bootstrap_reboot_css = new("~/lib/bootstrap/dist/css/bootstrap-reboot.css");
                            public static readonly G4mvcContentLink @bootstrap_reboot_css_map = new("~/lib/bootstrap/dist/css/bootstrap-reboot.css.map");
                            public static readonly G4mvcContentLink @bootstrap_reboot_min_css = new("~/lib/bootstrap/dist/css/bootstrap-reboot.min.css");
                            public static readonly G4mvcContentLink @bootstrap_reboot_min_css_map = new("~/lib/bootstrap/dist/css/bootstrap-reboot.min.css.map");
                            public static readonly G4mvcContentLink @bootstrap_css = new("~/lib/bootstrap/dist/css/bootstrap.css");
                            public static readonly G4mvcContentLink @bootstrap_css_map = new("~/lib/bootstrap/dist/css/bootstrap.css.map");
                            public static readonly G4mvcContentLink @bootstrap_min_css = new("~/lib/bootstrap/dist/css/bootstrap.min.css");
                            public static readonly G4mvcContentLink @bootstrap_min_css_map = new("~/lib/bootstrap/dist/css/bootstrap.min.css.map");
                        }

                        public static partial class @js
                        {
                            public const string UrlPath = "~/lib/bootstrap/dist/js";
                            public static readonly G4mvcContentLink @bootstrap_bundle_js = new("~/lib/bootstrap/dist/js/bootstrap.bundle.js");
                            public static readonly G4mvcContentLink @bootstrap_bundle_js_map = new("~/lib/bootstrap/dist/js/bootstrap.bundle.js.map");
                            public static readonly G4mvcContentLink @bootstrap_bundle_min_js = new("~/lib/bootstrap/dist/js/bootstrap.bundle.min.js");
                            public static readonly G4mvcContentLink @bootstrap_bundle_min_js_map = new("~/lib/bootstrap/dist/js/bootstrap.bundle.min.js.map");
                            public static readonly G4mvcContentLink @bootstrap_js = new("~/lib/bootstrap/dist/js/bootstrap.js");
                            public static readonly G4mvcContentLink @bootstrap_js_map = new("~/lib/bootstrap/dist/js/bootstrap.js.map");
                            public static readonly G4mvcContentLink @bootstrap_min_js = new("~/lib/bootstrap/dist/js/bootstrap.min.js");
                            public static readonly G4mvcContentLink @bootstrap_min_js_map = new("~/lib/bootstrap/dist/js/bootstrap.min.js.map");
                        }
                    }
                }

                public static partial class @jquery
                {
                    public const string UrlPath = "~/lib/jquery";
                    public static readonly G4mvcContentLink @LICENSE_txt = new("~/lib/jquery/LICENSE.txt");
                    public static partial class @dist
                    {
                        public const string UrlPath = "~/lib/jquery/dist";
                        public static readonly G4mvcContentLink @jquery_js = new("~/lib/jquery/dist/jquery.js");
                        public static readonly G4mvcContentLink @jquery_min_js = new("~/lib/jquery/dist/jquery.min.js");
                        public static readonly G4mvcContentLink @jquery_min_map = new("~/lib/jquery/dist/jquery.min.map");
                    }
                }

                public static partial class @jquery_validation
                {
                    public const string UrlPath = "~/lib/jquery-validation";
                    public static readonly G4mvcContentLink @LICENSE_md = new("~/lib/jquery-validation/LICENSE.md");

                    public static partial class @dist
                    {
                        public const string UrlPath = "~/lib/jquery-validation/dist";
                        public static readonly G4mvcContentLink @additional_methods_js = new("~/lib/jquery-validation/dist/additional-methods.js");
                        public static readonly G4mvcContentLink @additional_methods_min_js = new("~/lib/jquery-validation/dist/additional-methods.min.js");
                        public static readonly G4mvcContentLink @jquery_validate_js = new("~/lib/jquery-validation/dist/jquery.validate.js");
                        public static readonly G4mvcContentLink @jquery_validate_min_js = new("~/lib/jquery-validation/dist/jquery.validate.min.js");
                    }
                }

                public static partial class @jquery_validation_unobtrusive
                {
                    public const string UrlPath = "~/lib/jquery-validation-unobtrusive";
                    public static readonly G4mvcContentLink @jquery_validate_unobtrusive_js = new("~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js");
                    public static readonly G4mvcContentLink @jquery_validate_unobtrusive_min_js = new("~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js");
                    public static readonly G4mvcContentLink @LICENSE_txt = new("~/lib/jquery-validation-unobtrusive/LICENSE.txt");
                }
            }

            {{(AdditionalStatic ? """
                public static partial class @alt
                {
                    public const string UrlPath = "~/alt";
                    public static readonly G4mvcContentLink @altfavicon_ico = new("~/alt/altfavicon.ico");

                    public static partial class @css
                    {
                        public const string UrlPath = "~/alt/css";
                        public static readonly G4mvcContentLink @altsite_css = new("~/alt/css/altsite.css");
                    }

                    public static partial class @js
                    {
                        public const string UrlPath = "~/alt/js";
                        public static readonly G4mvcContentLink @altsite_js = new("~/alt/js/altsite.js");
                    }

                    public static partial class @lib
                    {
                        public const string UrlPath = "~/alt/lib";

                        public static partial class @bootstrap
                        {
                            public const string UrlPath = "~/alt/lib/bootstrap";
                            public static readonly G4mvcContentLink @LICENSE = new("~/alt/lib/bootstrap/LICENSE");

                            public static partial class @dist
                            {
                                public const string UrlPath = "~/alt/lib/bootstrap/dist";

                                public static partial class @css
                                {
                                    public const string UrlPath = "~/alt/lib/bootstrap/dist/css";
                                    public static readonly G4mvcContentLink @bootstrap_grid_css = new("~/alt/lib/bootstrap/dist/css/bootstrap-grid.css");
                                    public static readonly G4mvcContentLink @bootstrap_grid_css_map = new("~/alt/lib/bootstrap/dist/css/bootstrap-grid.css.map");
                                    public static readonly G4mvcContentLink @bootstrap_grid_min_css = new("~/alt/lib/bootstrap/dist/css/bootstrap-grid.min.css");
                                    public static readonly G4mvcContentLink @bootstrap_grid_min_css_map = new("~/alt/lib/bootstrap/dist/css/bootstrap-grid.min.css.map");
                                    public static readonly G4mvcContentLink @bootstrap_reboot_css = new("~/alt/lib/bootstrap/dist/css/bootstrap-reboot.css");
                                    public static readonly G4mvcContentLink @bootstrap_reboot_css_map = new("~/alt/lib/bootstrap/dist/css/bootstrap-reboot.css.map");
                                    public static readonly G4mvcContentLink @bootstrap_reboot_min_css = new("~/alt/lib/bootstrap/dist/css/bootstrap-reboot.min.css");
                                    public static readonly G4mvcContentLink @bootstrap_reboot_min_css_map = new("~/alt/lib/bootstrap/dist/css/bootstrap-reboot.min.css.map");
                                    public static readonly G4mvcContentLink @bootstrap_css = new("~/alt/lib/bootstrap/dist/css/bootstrap.css");
                                    public static readonly G4mvcContentLink @bootstrap_css_map = new("~/alt/lib/bootstrap/dist/css/bootstrap.css.map");
                                    public static readonly G4mvcContentLink @bootstrap_min_css = new("~/alt/lib/bootstrap/dist/css/bootstrap.min.css");
                                    public static readonly G4mvcContentLink @bootstrap_min_css_map = new("~/alt/lib/bootstrap/dist/css/bootstrap.min.css.map");
                                }

                                public static partial class @js
                                {
                                    public const string UrlPath = "~/alt/lib/bootstrap/dist/js";
                                    public static readonly G4mvcContentLink @bootstrap_bundle_js = new("~/alt/lib/bootstrap/dist/js/bootstrap.bundle.js");
                                    public static readonly G4mvcContentLink @bootstrap_bundle_js_map = new("~/alt/lib/bootstrap/dist/js/bootstrap.bundle.js.map");
                                    public static readonly G4mvcContentLink @bootstrap_bundle_min_js = new("~/alt/lib/bootstrap/dist/js/bootstrap.bundle.min.js");
                                    public static readonly G4mvcContentLink @bootstrap_bundle_min_js_map = new("~/alt/lib/bootstrap/dist/js/bootstrap.bundle.min.js.map");
                                    public static readonly G4mvcContentLink @bootstrap_js = new("~/alt/lib/bootstrap/dist/js/bootstrap.js");
                                    public static readonly G4mvcContentLink @bootstrap_js_map = new("~/alt/lib/bootstrap/dist/js/bootstrap.js.map");
                                    public static readonly G4mvcContentLink @bootstrap_min_js = new("~/alt/lib/bootstrap/dist/js/bootstrap.min.js");
                                    public static readonly G4mvcContentLink @bootstrap_min_js_map = new("~/alt/lib/bootstrap/dist/js/bootstrap.min.js.map");
                                }
                            }
                        }

                        public static partial class @jquery
                        {
                            public const string UrlPath = "~/alt/lib/jquery";
                            public static readonly G4mvcContentLink @LICENSE_txt = new("~/alt/lib/jquery/LICENSE.txt");

                            public static partial class @dist
                            {
                                public const string UrlPath = "~/alt/lib/jquery/dist";
                                public static readonly G4mvcContentLink @jquery_js = new("~/alt/lib/jquery/dist/jquery.js");
                                public static readonly G4mvcContentLink @jquery_min_js = new("~/alt/lib/jquery/dist/jquery.min.js");
                                public static readonly G4mvcContentLink @jquery_min_map = new("~/alt/lib/jquery/dist/jquery.min.map");
                            }
                        }

                        public static partial class @jquery_validation
                        {
                            public const string UrlPath = "~/alt/lib/jquery-validation";
                            public static readonly G4mvcContentLink @LICENSE_md = new("~/alt/lib/jquery-validation/LICENSE.md");

                            public static partial class @dist
                            {
                                public const string UrlPath = "~/alt/lib/jquery-validation/dist";
                                public static readonly G4mvcContentLink @additional_methods_js = new("~/alt/lib/jquery-validation/dist/additional-methods.js");
                                public static readonly G4mvcContentLink @additional_methods_min_js = new("~/alt/lib/jquery-validation/dist/additional-methods.min.js");
                                public static readonly G4mvcContentLink @jquery_validate_js = new("~/alt/lib/jquery-validation/dist/jquery.validate.js");
                                public static readonly G4mvcContentLink @jquery_validate_min_js = new("~/alt/lib/jquery-validation/dist/jquery.validate.min.js");
                            }
                        }

                        public static partial class @jquery_validation_unobtrusive
                        {
                            public const string UrlPath = "~/alt/lib/jquery-validation-unobtrusive";
                            public static readonly G4mvcContentLink @jquery_validate_unobtrusive_js = new("~/alt/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js");
                            public static readonly G4mvcContentLink @jquery_validate_unobtrusive_min_js = new("~/alt/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js");
                            public static readonly G4mvcContentLink @LICENSE_txt = new("~/alt/lib/jquery-validation-unobtrusive/LICENSE.txt");
                        }
                    }
                }
                """ : "")}}
        }
        """;
}
