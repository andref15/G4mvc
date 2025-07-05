using G4mvc.Generator;
using G4mvc.TestBase;
using G4mvc.TestBase.Utils;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace G4mvc.Test_8;

[TestClass]
public class G4mvcTests : G4mvcTestBase
{
    public G4mvcTests() : base(LanguageVersion.CSharp12) { }

    [TestMethod]
    public void DefaultOptions()
    {
        var outputCompilation = BaseTest();

        var expectedOutputs = new ExpectedOutputs();

        AssertExpectedSyntaxTrees(expectedOutputs, outputCompilation.SyntaxTrees);
    }

    [TestMethod]
    public void CustomOptions_ClassNames()
    {
        const string mvc = "TestMvc";
        const string links = "TestLinks";

        var outputCompilation = BaseTest(Configuration.JsonConfigModel.Create(helperClassName: mvc, linksClassName: links));

        var expectedOutputs = new ExpectedOutputs(mvcClassName: mvc, linksClassName: links);

        AssertExpectedSyntaxTrees(expectedOutputs, outputCompilation.SyntaxTrees);
    }

    [TestMethod]
    [DataRow(null, DisplayName = $"{nameof(Configuration.JsonConfig.UseProcessedPathForContentLink)} = null")]
    [DataRow(true, DisplayName = $"{nameof(Configuration.JsonConfig.UseProcessedPathForContentLink)} = true")]
    [DataRow(false, DisplayName = $"{nameof(Configuration.JsonConfig.UseProcessedPathForContentLink)} = false")]
    public void CustomOptions_UseVirtualPathProcessor(bool? useProcessedPathForContentLink)
    {
        const string vppImplementation = """
            internal static partial class VirtualPathProcessor
            {
                public static partial string Process(string path)
                {
                    return path;
                }
            }
            """;

        var vppSyntaxTree = SyntaxUtils.ToSyntaxTree(vppImplementation, ParseOptions);

        var outputCompilation = BaseTest(Configuration.JsonConfigModel.Create(useVirtualPathProcessor: true, useProcessedPathForContentLink: useProcessedPathForContentLink), EnumerableUtils.Create(vppSyntaxTree));

        var expectedOutputs = new ExpectedOutputs(withVpp: true, vppForContent: useProcessedPathForContentLink ?? true);

        AssertExpectedSyntaxTrees(expectedOutputs, outputCompilation.SyntaxTrees, 10);
    }

    [TestMethod]
    public void CustomOptions_MakeGeneratedClassesInternal()
    {
        var outputCompilation = BaseTest(Configuration.JsonConfigModel.Create(makeGeneratedClassesInternal: true));

        var expectedOutputs = new ExpectedOutputs(classesInternal: true);

        AssertExpectedSyntaxTrees(expectedOutputs, outputCompilation.SyntaxTrees);
    }

    [TestMethod]
    public void CustomOptions_GeneratedClassNamespace()
    {
        const string classNamespace = $"{nameof(G4mvc)}.{nameof(Test_8)}.Routes";

        var outputCompilation = BaseTest(Configuration.JsonConfigModel.Create(generatedClassNamespace: classNamespace));

        var expectedOutputs = new ExpectedOutputs(classNamespace: classNamespace);

        AssertExpectedSyntaxTrees(expectedOutputs, outputCompilation.SyntaxTrees);
    }

    [TestMethod]
    public void CustomOptions_EnableSubfoldersInViews()
    {
        var outputCompilation = BaseTest(Configuration.JsonConfigModel.Create(enableSubfoldersInViews: true));

        var expectedOutputs = new ExpectedOutputs(enumerateSubDirectories: true);

        AssertExpectedSyntaxTrees(expectedOutputs, outputCompilation.SyntaxTrees);
    }

    [TestMethod]
    public void CustomOptions_StaticFilesPath()
    {
        var outputCompilation = BaseTest(Configuration.JsonConfigModel.Create(staticFilesPath: "wwwrootAlt"));

        var expectedOutputs = new ExpectedOutputs(altRoot: true);

        AssertExpectedSyntaxTrees(expectedOutputs, outputCompilation.SyntaxTrees);
    }

    [TestMethod]
    public void CustomOptions_ExcludedStaticFileExtensions()
    {
        var outputCompilation = BaseTest(Configuration.JsonConfigModel.Create(excludedStaticFileExtensions: [".ico"]));

        var expectedOutputs = new ExpectedOutputs(excludeIco: true);

        AssertExpectedSyntaxTrees(expectedOutputs, outputCompilation.SyntaxTrees);
    }

    [TestMethod]
    public void CustomOptions_ExcludedStaticFileDirectories()
    {
        var outputCompilation = BaseTest(Configuration.JsonConfigModel.Create(excludedStaticFileDirectories: ["wwwroot/css"]));

        var expectedOutputs = new ExpectedOutputs(excludeCss: true);

        AssertExpectedSyntaxTrees(expectedOutputs, outputCompilation.SyntaxTrees);
    }

    [TestMethod]
    public void CustomOptions_AdditionalStaticFilesPaths()
    {
        var outputCompilation = BaseTest(Configuration.JsonConfigModel.Create(additionalStaticFilesPaths: new Dictionary<string, string>
        {
            ["wwwrootAlt"] = "alt"
        }));

        var expectedOutputs = new ExpectedOutputs(additionalStatic: true);

        AssertExpectedSyntaxTrees(expectedOutputs, outputCompilation.SyntaxTrees);
    }

    [TestMethod]
    public void CustomOptions_CustomStaticFileDirectoryAlias()
    {
        var outputCompilation = BaseTest(Configuration.JsonConfigModel.Create(customStaticFileDirectoryAlias: new Dictionary<string, string>
        {
            ["wwwroot/js"] = "otherjs"
        }));

        var expectedOutputs = new ExpectedOutputs(customJsName: true);

        AssertExpectedSyntaxTrees(expectedOutputs, outputCompilation.SyntaxTrees);
    }
}
