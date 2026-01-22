#if NET8_0
using G4mvc.Generator.Compilation;

namespace G4mvc.Test.OutputComparisson;

internal partial class ExpectedOutputs
{
    public partial Dictionary<string, string> Get()
    {
        return new()
        {
            [Path.Join("G4mvc.Generator", "G4mvc.Generator.G4mvcGenerator", $"{_linksHelperClassName}.generated.cs")] = _withVpp
                ? $$"""
                using {{nameof(G4mvc)}};

                #nullable enable
                {{(_classNamespace is null ? "" : $"namespace {_classNamespace};{Environment.NewLine}")}}
                internal static partial class {{Configuration.VppClassName}}
                {
                    public static partial string {{Configuration.VppMethodName}}(string path);
                }

                {{(_classesInternal ? "internal" : "public")}} static partial class {{_linksHelperClassName}}
                {
                    //v1;
                    public const string UrlPath = "~";
                    public static readonly {{nameof(G4mvcContentLink)}} @favicon_ico = new("~/favicon.ico", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});

                    public static partial class @css
                    {
                        public const string UrlPath = "~/css";
                        public static readonly {{nameof(G4mvcContentLink)}} @site_css = new("~/css/site.css", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                    }

                    public static partial class @js
                    {
                        public const string UrlPath = "~/js";
                        public static readonly {{nameof(G4mvcContentLink)}} @site_js = new("~/js/site.js", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                    }

                    public static partial class @lib
                    {
                        public const string UrlPath = "~/lib";

                        public static partial class @bootstrap
                        {
                            public const string UrlPath = "~/lib/bootstrap";
                            public static readonly {{nameof(G4mvcContentLink)}} @LICENSE = new("~/lib/bootstrap/LICENSE", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});

                            public static partial class @dist
                            {
                                public const string UrlPath = "~/lib/bootstrap/dist";

                                public static partial class @css
                                {
                                    public const string UrlPath = "~/lib/bootstrap/dist/css";
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_grid_css = new("~/lib/bootstrap/dist/css/bootstrap-grid.css", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_grid_css_map = new("~/lib/bootstrap/dist/css/bootstrap-grid.css.map", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_grid_min_css = new("~/lib/bootstrap/dist/css/bootstrap-grid.min.css", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_grid_min_css_map = new("~/lib/bootstrap/dist/css/bootstrap-grid.min.css.map", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_reboot_css = new("~/lib/bootstrap/dist/css/bootstrap-reboot.css", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_reboot_css_map = new("~/lib/bootstrap/dist/css/bootstrap-reboot.css.map", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_reboot_min_css = new("~/lib/bootstrap/dist/css/bootstrap-reboot.min.css", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_reboot_min_css_map = new("~/lib/bootstrap/dist/css/bootstrap-reboot.min.css.map", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_css = new("~/lib/bootstrap/dist/css/bootstrap.css", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_css_map = new("~/lib/bootstrap/dist/css/bootstrap.css.map", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_min_css = new("~/lib/bootstrap/dist/css/bootstrap.min.css", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_min_css_map = new("~/lib/bootstrap/dist/css/bootstrap.min.css.map", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                }

                                public static partial class @js
                                {
                                    public const string UrlPath = "~/lib/bootstrap/dist/js";
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_bundle_js = new("~/lib/bootstrap/dist/js/bootstrap.bundle.js", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_bundle_js_map = new("~/lib/bootstrap/dist/js/bootstrap.bundle.js.map", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_bundle_min_js = new("~/lib/bootstrap/dist/js/bootstrap.bundle.min.js", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_bundle_min_js_map = new("~/lib/bootstrap/dist/js/bootstrap.bundle.min.js.map", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_js = new("~/lib/bootstrap/dist/js/bootstrap.js", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_js_map = new("~/lib/bootstrap/dist/js/bootstrap.js.map", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_min_js = new("~/lib/bootstrap/dist/js/bootstrap.min.js", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_min_js_map = new("~/lib/bootstrap/dist/js/bootstrap.min.js.map", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                }
                            }
                        }

                        public static partial class @jquery
                        {
                            public const string UrlPath = "~/lib/jquery";
                            public static readonly {{nameof(G4mvcContentLink)}} @LICENSE_txt = new("~/lib/jquery/LICENSE.txt", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});

                            public static partial class @dist
                            {
                                public const string UrlPath = "~/lib/jquery/dist";
                                public static readonly {{nameof(G4mvcContentLink)}} @jquery_js = new("~/lib/jquery/dist/jquery.js", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                public static readonly {{nameof(G4mvcContentLink)}} @jquery_min_js = new("~/lib/jquery/dist/jquery.min.js", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                public static readonly {{nameof(G4mvcContentLink)}} @jquery_min_map = new("~/lib/jquery/dist/jquery.min.map", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                            }
                        }

                        public static partial class @jquery_validation
                        {
                            public const string UrlPath = "~/lib/jquery-validation";
                            public static readonly {{nameof(G4mvcContentLink)}} @LICENSE_md = new("~/lib/jquery-validation/LICENSE.md", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});

                            public static partial class @dist
                            {
                                public const string UrlPath = "~/lib/jquery-validation/dist";
                                public static readonly {{nameof(G4mvcContentLink)}} @additional_methods_js = new("~/lib/jquery-validation/dist/additional-methods.js", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                public static readonly {{nameof(G4mvcContentLink)}} @additional_methods_min_js = new("~/lib/jquery-validation/dist/additional-methods.min.js", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                public static readonly {{nameof(G4mvcContentLink)}} @jquery_validate_js = new("~/lib/jquery-validation/dist/jquery.validate.js", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                                public static readonly {{nameof(G4mvcContentLink)}} @jquery_validate_min_js = new("~/lib/jquery-validation/dist/jquery.validate.min.js", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                            }
                        }

                        public static partial class @jquery_validation_unobtrusive
                        {
                            public const string UrlPath = "~/lib/jquery-validation-unobtrusive";
                            public static readonly {{nameof(G4mvcContentLink)}} @jquery_validate_unobtrusive_js = new("~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                            public static readonly {{nameof(G4mvcContentLink)}} @jquery_validate_unobtrusive_min_js = new("~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                            public static readonly {{nameof(G4mvcContentLink)}} @LICENSE_txt = new("~/lib/jquery-validation-unobtrusive/LICENSE.txt", {{Configuration.VppClassName}}.{{Configuration.VppMethodName}}, {{_vppForContentStr}});
                        }
                    }
                }

                """
                : $$"""
                using {{nameof(G4mvc)}};

                #nullable enable
                {{(_classNamespace is null ? "" : $"{Environment.NewLine}namespace {_classNamespace};{Environment.NewLine}")}}
                {{(_classesInternal ? "internal" : "public")}} static partial class {{_linksHelperClassName}}
                {
                    //v1;
                    public const string UrlPath = "~";
                {{(_excludeIco ? "" : $"""
                        public static readonly {nameof(G4mvcContentLink)} @{(_altRoot ? "alt" : "")}favicon_ico = new("~/{(_altRoot ? "alt" : "")}favicon.ico");

                    """
                    )}}{{(_excludeCss ? "" : $$"""

                        public static partial class @css
                        {
                            public const string UrlPath = "~/css";
                            public static readonly {{nameof(G4mvcContentLink)}} @{{(_altRoot ? "alt" : "")}}site_css = new("~/css/{{(_altRoot ? "alt" : "")}}site.css");
                        }

                    """)}}
                    public static partial class @{{(_customJsName ? "otherjs" : "js")}}
                    {
                        public const string UrlPath = "~/js";
                        public static readonly {{nameof(G4mvcContentLink)}} @{{(_altRoot ? "alt" : "")}}site_js = new("~/js/{{(_altRoot ? "alt" : "")}}site.js");
                    }

                    public static partial class @lib
                    {
                        public const string UrlPath = "~/lib";

                        public static partial class @bootstrap
                        {
                            public const string UrlPath = "~/lib/bootstrap";
                            public static readonly {{nameof(G4mvcContentLink)}} @LICENSE = new("~/lib/bootstrap/LICENSE");

                            public static partial class @dist
                            {
                                public const string UrlPath = "~/lib/bootstrap/dist";

                                public static partial class @css
                                {
                                    public const string UrlPath = "~/lib/bootstrap/dist/css";
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_grid_css = new("~/lib/bootstrap/dist/css/bootstrap-grid.css");
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_grid_css_map = new("~/lib/bootstrap/dist/css/bootstrap-grid.css.map");
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_grid_min_css = new("~/lib/bootstrap/dist/css/bootstrap-grid.min.css");
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_grid_min_css_map = new("~/lib/bootstrap/dist/css/bootstrap-grid.min.css.map");
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_reboot_css = new("~/lib/bootstrap/dist/css/bootstrap-reboot.css");
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_reboot_css_map = new("~/lib/bootstrap/dist/css/bootstrap-reboot.css.map");
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_reboot_min_css = new("~/lib/bootstrap/dist/css/bootstrap-reboot.min.css");
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_reboot_min_css_map = new("~/lib/bootstrap/dist/css/bootstrap-reboot.min.css.map");
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_css = new("~/lib/bootstrap/dist/css/bootstrap.css");
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_css_map = new("~/lib/bootstrap/dist/css/bootstrap.css.map");
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_min_css = new("~/lib/bootstrap/dist/css/bootstrap.min.css");
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_min_css_map = new("~/lib/bootstrap/dist/css/bootstrap.min.css.map");
                                }

                                public static partial class @js
                                {
                                    public const string UrlPath = "~/lib/bootstrap/dist/js";
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_bundle_js = new("~/lib/bootstrap/dist/js/bootstrap.bundle.js");
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_bundle_js_map = new("~/lib/bootstrap/dist/js/bootstrap.bundle.js.map");
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_bundle_min_js = new("~/lib/bootstrap/dist/js/bootstrap.bundle.min.js");
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_bundle_min_js_map = new("~/lib/bootstrap/dist/js/bootstrap.bundle.min.js.map");
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_js = new("~/lib/bootstrap/dist/js/bootstrap.js");
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_js_map = new("~/lib/bootstrap/dist/js/bootstrap.js.map");
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_min_js = new("~/lib/bootstrap/dist/js/bootstrap.min.js");
                                    public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_min_js_map = new("~/lib/bootstrap/dist/js/bootstrap.min.js.map");
                                }
                            }
                        }

                        public static partial class @jquery
                        {
                            public const string UrlPath = "~/lib/jquery";
                            public static readonly {{nameof(G4mvcContentLink)}} @LICENSE_txt = new("~/lib/jquery/LICENSE.txt");
                
                            public static partial class @dist
                            {
                                public const string UrlPath = "~/lib/jquery/dist";
                                public static readonly {{nameof(G4mvcContentLink)}} @jquery_js = new("~/lib/jquery/dist/jquery.js");
                                public static readonly {{nameof(G4mvcContentLink)}} @jquery_min_js = new("~/lib/jquery/dist/jquery.min.js");
                                public static readonly {{nameof(G4mvcContentLink)}} @jquery_min_map = new("~/lib/jquery/dist/jquery.min.map");
                            }
                        }

                        public static partial class @jquery_validation
                        {
                            public const string UrlPath = "~/lib/jquery-validation";
                            public static readonly {{nameof(G4mvcContentLink)}} @LICENSE_md = new("~/lib/jquery-validation/LICENSE.md");

                            public static partial class @dist
                            {
                                public const string UrlPath = "~/lib/jquery-validation/dist";
                                public static readonly {{nameof(G4mvcContentLink)}} @additional_methods_js = new("~/lib/jquery-validation/dist/additional-methods.js");
                                public static readonly {{nameof(G4mvcContentLink)}} @additional_methods_min_js = new("~/lib/jquery-validation/dist/additional-methods.min.js");
                                public static readonly {{nameof(G4mvcContentLink)}} @jquery_validate_js = new("~/lib/jquery-validation/dist/jquery.validate.js");
                                public static readonly {{nameof(G4mvcContentLink)}} @jquery_validate_min_js = new("~/lib/jquery-validation/dist/jquery.validate.min.js");
                            }
                        }

                        public static partial class @jquery_validation_unobtrusive
                        {
                            public const string UrlPath = "~/lib/jquery-validation-unobtrusive";
                            public static readonly {{nameof(G4mvcContentLink)}} @jquery_validate_unobtrusive_js = new("~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js");
                            public static readonly {{nameof(G4mvcContentLink)}} @jquery_validate_unobtrusive_min_js = new("~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js");
                            public static readonly {{nameof(G4mvcContentLink)}} @LICENSE_txt = new("~/lib/jquery-validation-unobtrusive/LICENSE.txt");
                        }
                    }
                {{(_additionalStatic ? $$"""

                        public static partial class @alt
                        {
                            public const string UrlPath = "~/alt";
                            public static readonly {{nameof(G4mvcContentLink)}} @altfavicon_ico = new("~/alt/altfavicon.ico");

                            public static partial class @css
                            {
                                public const string UrlPath = "~/alt/css";
                                public static readonly {{nameof(G4mvcContentLink)}} @altsite_css = new("~/alt/css/altsite.css");
                            }

                            public static partial class @js
                            {
                                public const string UrlPath = "~/alt/js";
                                public static readonly {{nameof(G4mvcContentLink)}} @altsite_js = new("~/alt/js/altsite.js");
                            }

                            public static partial class @lib
                            {
                                public const string UrlPath = "~/alt/lib";

                                public static partial class @bootstrap
                                {
                                    public const string UrlPath = "~/alt/lib/bootstrap";
                                    public static readonly {{nameof(G4mvcContentLink)}} @LICENSE = new("~/alt/lib/bootstrap/LICENSE");

                                    public static partial class @dist
                                    {
                                        public const string UrlPath = "~/alt/lib/bootstrap/dist";

                                        public static partial class @css
                                        {
                                            public const string UrlPath = "~/alt/lib/bootstrap/dist/css";
                                            public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_grid_css = new("~/alt/lib/bootstrap/dist/css/bootstrap-grid.css");
                                            public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_grid_css_map = new("~/alt/lib/bootstrap/dist/css/bootstrap-grid.css.map");
                                            public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_grid_min_css = new("~/alt/lib/bootstrap/dist/css/bootstrap-grid.min.css");
                                            public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_grid_min_css_map = new("~/alt/lib/bootstrap/dist/css/bootstrap-grid.min.css.map");
                                            public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_reboot_css = new("~/alt/lib/bootstrap/dist/css/bootstrap-reboot.css");
                                            public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_reboot_css_map = new("~/alt/lib/bootstrap/dist/css/bootstrap-reboot.css.map");
                                            public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_reboot_min_css = new("~/alt/lib/bootstrap/dist/css/bootstrap-reboot.min.css");
                                            public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_reboot_min_css_map = new("~/alt/lib/bootstrap/dist/css/bootstrap-reboot.min.css.map");
                                            public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_css = new("~/alt/lib/bootstrap/dist/css/bootstrap.css");
                                            public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_css_map = new("~/alt/lib/bootstrap/dist/css/bootstrap.css.map");
                                            public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_min_css = new("~/alt/lib/bootstrap/dist/css/bootstrap.min.css");
                                            public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_min_css_map = new("~/alt/lib/bootstrap/dist/css/bootstrap.min.css.map");
                                        }

                                        public static partial class @js
                                        {
                                            public const string UrlPath = "~/alt/lib/bootstrap/dist/js";
                                            public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_bundle_js = new("~/alt/lib/bootstrap/dist/js/bootstrap.bundle.js");
                                            public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_bundle_js_map = new("~/alt/lib/bootstrap/dist/js/bootstrap.bundle.js.map");
                                            public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_bundle_min_js = new("~/alt/lib/bootstrap/dist/js/bootstrap.bundle.min.js");
                                            public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_bundle_min_js_map = new("~/alt/lib/bootstrap/dist/js/bootstrap.bundle.min.js.map");
                                            public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_js = new("~/alt/lib/bootstrap/dist/js/bootstrap.js");
                                            public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_js_map = new("~/alt/lib/bootstrap/dist/js/bootstrap.js.map");
                                            public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_min_js = new("~/alt/lib/bootstrap/dist/js/bootstrap.min.js");
                                            public static readonly {{nameof(G4mvcContentLink)}} @bootstrap_min_js_map = new("~/alt/lib/bootstrap/dist/js/bootstrap.min.js.map");
                                        }
                                    }
                                }

                                public static partial class @jquery
                                {
                                    public const string UrlPath = "~/alt/lib/jquery";
                                    public static readonly {{nameof(G4mvcContentLink)}} @LICENSE_txt = new("~/alt/lib/jquery/LICENSE.txt");

                                    public static partial class @dist
                                    {
                                        public const string UrlPath = "~/alt/lib/jquery/dist";
                                        public static readonly {{nameof(G4mvcContentLink)}} @jquery_js = new("~/alt/lib/jquery/dist/jquery.js");
                                        public static readonly {{nameof(G4mvcContentLink)}} @jquery_min_js = new("~/alt/lib/jquery/dist/jquery.min.js");
                                        public static readonly {{nameof(G4mvcContentLink)}} @jquery_min_map = new("~/alt/lib/jquery/dist/jquery.min.map");
                                    }
                                }

                                public static partial class @jquery_validation
                                {
                                    public const string UrlPath = "~/alt/lib/jquery-validation";
                                    public static readonly {{nameof(G4mvcContentLink)}} @LICENSE_md = new("~/alt/lib/jquery-validation/LICENSE.md");

                                    public static partial class @dist
                                    {
                                        public const string UrlPath = "~/alt/lib/jquery-validation/dist";
                                        public static readonly {{nameof(G4mvcContentLink)}} @additional_methods_js = new("~/alt/lib/jquery-validation/dist/additional-methods.js");
                                        public static readonly {{nameof(G4mvcContentLink)}} @additional_methods_min_js = new("~/alt/lib/jquery-validation/dist/additional-methods.min.js");
                                        public static readonly {{nameof(G4mvcContentLink)}} @jquery_validate_js = new("~/alt/lib/jquery-validation/dist/jquery.validate.js");
                                        public static readonly {{nameof(G4mvcContentLink)}} @jquery_validate_min_js = new("~/alt/lib/jquery-validation/dist/jquery.validate.min.js");
                                    }
                                }

                                public static partial class @jquery_validation_unobtrusive
                                {
                                    public const string UrlPath = "~/alt/lib/jquery-validation-unobtrusive";
                                    public static readonly {{nameof(G4mvcContentLink)}} @jquery_validate_unobtrusive_js = new("~/alt/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js");
                                    public static readonly {{nameof(G4mvcContentLink)}} @jquery_validate_unobtrusive_min_js = new("~/alt/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js");
                                    public static readonly {{nameof(G4mvcContentLink)}} @LICENSE_txt = new("~/alt/lib/jquery-validation-unobtrusive/LICENSE.txt");
                                }
                            }
                        }

                    """ : "")}}}

                """,
            [Path.Join("G4mvc.Generator", "G4mvc.Generator.G4mvcGenerator", "TestRoutes.generated.cs")] = $$"""
                using G4mvc;

                #nullable enable
                
                namespace {{nameof(G4mvc)}}.{{nameof(Test)}}.{{nameof(G4mvc)}}.Mvc;
                {{(_classesInternal ? "internal" : "public")}} partial class TestRoutes
                {
                    public string Name { get; } = "Test";
                    public TestActionNames ActionNames { get; } = new();
                    public TestViews Views { get; } = new();
                    public IndexParamsClass IndexParams { get; } = new();
                    public PrivacyParamsClass PrivacyParams { get; } = new();
                    public ErrorParamsClass ErrorParams { get; } = new();

                    public {{nameof(G4mvcActionRouteValues)}} Index()
                    {
                        return new(null, "Test", "Index");
                    }

                #nullable disable
                
                    public {{nameof(G4mvcActionRouteValues)}} Index(string test)
                    {
                        var route = Index();

                        route["test"] = test;

                        return route;
                    }

                #nullable restore


                    public {{nameof(G4mvcActionRouteValues)}} Privacy()
                    {
                        return new(null, "Test", "Privacy");
                    }

                    public {{nameof(G4mvcActionRouteValues)}} Error()
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
                        }{{(_enumerateSubDirectories ? """


                            public @SubDirViews @SubDir { get; } = new();
                            public class @SubDirViews
                            {
                                public @SubDirViewNames ViewNames { get; } = new();
                                public string SubItem { get; } = "~/Views/Test/SubDir/SubItem.cshtml";

                                public class @SubDirViewNames
                                {
                                    public string SubItem { get; } = nameof(SubItem);
                                }
                            }
                    """ : "")}}
                    }
                }

                """,
            [Path.Join("G4mvc.Generator", "G4mvc.Generator.G4mvcGenerator", "TestPartialRoutes.generated.cs")] = $$"""
                using G4mvc;

                #nullable enable

                namespace {{nameof(G4mvc)}}.{{nameof(Test)}}.{{nameof(G4mvc)}}.Mvc;
                {{(_classesInternal ? "internal" : "public")}} partial class TestPartialRoutes
                {
                    public string Name { get; } = "TestPartial";
                    public TestPartialActionNames ActionNames { get; } = new();
                    public TestPartialViews Views { get; } = new();
                    public IndexParamsClass IndexParams { get; } = new();
                    public PrivacyParamsClass PrivacyParams { get; } = new();
                    public ErrorParamsClass ErrorParams { get; } = new();

                    public {{nameof(G4mvcActionRouteValues)}} Index()
                    {
                        return new(null, "TestPartial", "Index");
                    }

                #nullable disable

                    public {{nameof(G4mvcActionRouteValues)}} Index(string test)
                    {
                        var route = Index();

                        route["test"] = test;

                        return route;
                    }

                #nullable restore


                    public {{nameof(G4mvcActionRouteValues)}} Privacy()
                    {
                        return new(null, "TestPartial", "Privacy");
                    }

                    public {{nameof(G4mvcActionRouteValues)}} Error()
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

                """,
            [Path.Join("G4mvc.Generator", "G4mvc.Generator.G4mvcGenerator", "TestPartialController.generated.cs")] = $$"""
                using {{nameof(G4mvc)}};
                using Microsoft.AspNetCore.Mvc;
                {{(_classNamespace is null ? "" : $"using {_classNamespace};{Environment.NewLine}")}}
                #nullable enable

                namespace {{nameof(G4mvc)}}.{{nameof(Test)}}.{{nameof(Controllers)}};
                public partial class TestPartialController
                {
                    {{(_classesInternal ? "private " : "")}}protected global::{{nameof(G4mvc)}}.{{nameof(Test)}}.{{nameof(G4mvc)}}.Mvc.TestPartialRoutes.TestPartialViews Views { get; } = {{_mvcClassName}}.TestPartial.Views;

                    protected RedirectToRouteResult RedirectToAction({{nameof(G4mvcActionRouteValues)}} route)
                    {
                        return RedirectToRoute(route);
                    }

                    protected RedirectToRouteResult RedirectToActionPermanent({{nameof(G4mvcActionRouteValues)}} route)
                    {
                        return RedirectToRoutePermanent(route);
                    }
                }

                """,
            [Path.Join("G4mvc.Generator", "G4mvc.Generator.G4mvcGenerator", "SharedRoutes.generated.cs")] = $$"""
                #nullable enable

                namespace {{nameof(G4mvc)}}.{{nameof(Test)}}.{{nameof(G4mvc)}}.Mvc;
                {{(_classesInternal ? "internal" : "public")}} class SharedRoutes
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

                """,
            [Path.Join("G4mvc.Generator", "G4mvc.Generator.G4mvcGenerator", $"{_mvcClassName}.generated.cs")] = $$"""
                using {{nameof(G4mvc)}}.{{nameof(Test)}}.{{nameof(G4mvc)}}.Mvc;

                #nullable enable
                {{(_classNamespace is null ? "" : $"{Environment.NewLine}namespace {_classNamespace};{Environment.NewLine}")}}
                {{(_classesInternal ? "internal" : "public")}} class {{_mvcClassName}}
                {
                    //v1;
                    public static TestRoutes Test { get; } = new();
                    public static TestPartialRoutes TestPartial { get; } = new();
                    public static SharedRoutes Shared { get; } = new();
                }

                """,
            [Path.Join("G4mvc.Generator", "G4mvc.Generator.G4mvcGenerator", "IndexRoutes.generated.cs")] = $$"""
                using {{nameof(G4mvc)}};

                #nullable enable

                namespace {{nameof(G4mvc)}}.{{nameof(Test)}}.{{nameof(G4mvc)}}.RazorPages;
                {{(_classesInternal ? "internal" : "public")}} partial class IndexRoutes
                {
                    public string Name { get; } = "Index";
                    public IndexMethodNames HttpMethods { get; } = new();
                    public IndexView View { get; } = new();
                    public GetParamsClass GetParams { get; } = new();
                    public PostParamsClass PostParams { get; } = new();
                    public PostAlternativeParamsClass PostAlternativeParams { get; } = new();

                    public {{nameof(G4mvcPageRouteValues)}} Get()
                    {
                        return new(null, "/Index", null, "GET");
                    }

                    public {{nameof(G4mvcPageRouteValues)}} Post()
                    {
                        return new(null, "/Index", null, "POST");
                    }

                    public {{nameof(G4mvcPageRouteValues)}} Post(global::{{nameof(G4mvc)}}.{{nameof(Test)}}.{{nameof(Pages)}}.TestFormModel? testModel)
                    {
                        var route = Post();

                        route["testModel"] = testModel;

                        return route;
                    }
                    public {{nameof(G4mvcPageRouteValues)}} PostAlternative()
                    {
                        return new(null, "/Index", "Alternative", "POST");
                    }

                    public {{nameof(G4mvcPageRouteValues)}} PostAlternative(global::{{nameof(G4mvc)}}.{{nameof(Test)}}.{{nameof(Pages)}}.TestFormModel? testModel)
                    {
                        var route = PostAlternative();

                        route["testModel"] = testModel;

                        return route;
                    }
                    public class IndexMethodNames
                    {
                        public string Get { get; } = nameof(Get);
                        public string Post { get; } = nameof(Post);
                        public string PostAlternative { get; } = nameof(PostAlternative);
                    }

                    public class GetParamsClass
                    {
                    }

                    public class PostParamsClass
                    {
                        public string testModel { get; } = nameof(testModel);
                    }

                    public class PostAlternativeParamsClass
                    {
                        public string testModel { get; } = nameof(testModel);
                    }
                    public class IndexView
                    {
                        public string Name { get; } = "Index";
                        public string AppPath { get; } = "~/Pages/Index.cshtml";
                    }
                }

                """,
            [Path.Join("G4mvc.Generator", "G4mvc.Generator.G4mvcGenerator", "Sample.IndexRoutes.generated.cs")] = $$"""
                using {{nameof(G4mvc)}};
        
                #nullable enable

                namespace {{nameof(G4mvc)}}.{{nameof(Test)}}.{{nameof(G4mvc)}}.RazorPages;
                {{(_classesInternal ? "internal" : "public")}} partial class SampleRoutes
                {
                    public IndexRoutes Index { get; } = new();

                    {{(_classesInternal ? "internal" : "public")}} partial class IndexRoutes
                    {
                        public string Name { get; } = "Index";
                        public IndexMethodNames HttpMethods { get; } = new();
                        public IndexView View { get; } = new();
                        public GetParamsClass GetParams { get; } = new();
                        public PostParamsClass PostParams { get; } = new();
                        public PostAlternativeParamsClass PostAlternativeParams { get; } = new();
            
                        public {{nameof(G4mvcPageRouteValues)}} Get()
                        {
                            return new(null, "/Sample/Index", null, "GET");
                        }
            
                        public {{nameof(G4mvcPageRouteValues)}} Post()
                        {
                            return new(null, "/Sample/Index", null, "POST");
                        }
            
                        public {{nameof(G4mvcPageRouteValues)}} Post(global::{{nameof(G4mvc)}}.{{nameof(Test)}}.{{nameof(Pages)}}.Sample.TestFormModel? testModel)
                        {
                            var route = Post();
            
                            route["testModel"] = testModel;
            
                            return route;
                        }
                        public {{nameof(G4mvcPageRouteValues)}} PostAlternative()
                        {
                            return new(null, "/Sample/Index", "Alternative", "POST");
                        }
            
                        public {{nameof(G4mvcPageRouteValues)}} PostAlternative(global::{{nameof(G4mvc)}}.{{nameof(Test)}}.{{nameof(Pages)}}.Sample.TestFormModel? testModel)
                        {
                            var route = PostAlternative();
            
                            route["testModel"] = testModel;
            
                            return route;
                        }
                        public class IndexMethodNames
                        {
                            public string Get { get; } = nameof(Get);
                            public string Post { get; } = nameof(Post);
                            public string PostAlternative { get; } = nameof(PostAlternative);
                        }
            
                        public class GetParamsClass
                        {
                        }
            
                        public class PostParamsClass
                        {
                            public string testModel { get; } = nameof(testModel);
                        }
            
                        public class PostAlternativeParamsClass
                        {
                            public string testModel { get; } = nameof(testModel);
                        }
                        public class IndexView
                        {
                            public string Name { get; } = "Index";
                            public string AppPath { get; } = "~/Pages/Sample/Index.cshtml";
                        }
                    }
                }

                """,
            [Path.Join("G4mvc.Generator", "G4mvc.Generator.G4mvcGenerator", $"{_pageHelperClassName}.generated.cs")] = $$"""
                using {{nameof(G4mvc)}}.{{nameof(Test)}}.{{nameof(G4mvc)}}.RazorPages;

                #nullable enable
                {{(_classNamespace is null ? "" : $"{Environment.NewLine}namespace {_classNamespace};{Environment.NewLine}")}}
                {{(_classesInternal ? "internal" : "public")}} class {{_pageHelperClassName}}
                {
                    //v1;
                    public static IndexRoutes Index { get; } = new();
                    public static SampleRoutes Sample { get; } = new();
                }

                """
        };
    }
}

#endif