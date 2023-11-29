using G4mvc.Generator;
using G4mvc.TestBase;
using G4mvc.TestBase.Utils;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace G4mvc.Test_6;

[TestClass]
public class G4mvcTests : G4mvcTestBase
{
    public G4mvcTests() : base(LanguageVersion.CSharp10) { }

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

        var outputCompilation = BaseTest(new Configuration.JsonConfigClass
        {
            HelperClassName = mvc,
            LinksClassName = links
        });

        var expectedOutputs = new ExpectedOutputs(mvcClassName: mvc, linksClassName: links);

        AssertExpectedSyntaxTrees(expectedOutputs, outputCompilation.SyntaxTrees);
    }

    [TestMethod]
    public void CustomOptions_UseVirtualPathProcessor()
    {
        const string vppImplementation = @"internal static partial class VirtualPathProcessor
{
    public static partial string Process(string path)
    {
        return path;
    }
}";

        var vppSyntaxTree = SyntaxUtils.ToSyntaxTree(vppImplementation, ParseOptions);

        var outputCompilation = BaseTest(new Configuration.JsonConfigClass
        {
            UseVirtualPathProcessor = true
        }, EnumerableUtils.Create(vppSyntaxTree));

        var expectedOutputs = new ExpectedOutputs(withVpp: true);

        AssertExpectedSyntaxTrees(expectedOutputs, outputCompilation.SyntaxTrees, 10);
    }

    [TestMethod]
    public void CustomOptions_MakeGeneratedClassesInternal()
    {
        var outputCompilation = BaseTest(new Configuration.JsonConfigClass
        {
            MakeGeneratedClassesInternal = true
        });

        var expectedOutputs = new ExpectedOutputs(classesInternal: true);

        AssertExpectedSyntaxTrees(expectedOutputs, outputCompilation.SyntaxTrees);
    }

    [TestMethod]
    public void CustomOptions_StaticFilesPath()
    {
        var outputCompilation = BaseTest(new Configuration.JsonConfigClass
        {
            StaticFilesPath = "wwwrootAlt"
        });

        var expectedOutputs = new ExpectedOutputs(altRoot: true);

        AssertExpectedSyntaxTrees(expectedOutputs, outputCompilation.SyntaxTrees);
    }

    [TestMethod]
    public void CustomOptions_ExcludedStaticFileExtensions()
    {
        var outputCompilation = BaseTest(new Configuration.JsonConfigClass
        {
            ExcludedStaticFileExtensions = new()
            {
                ".ico"
            }
        });

        var expectedOutputs = new ExpectedOutputs(excludeIco: true);

        AssertExpectedSyntaxTrees(expectedOutputs, outputCompilation.SyntaxTrees);
    }

    [TestMethod]
    public void CustomOptions_ExcludedStaticFileDirectories()
    {
        var outputCompilation = BaseTest(new Configuration.JsonConfigClass
        {
            ExcludedStaticFileDirectories = new()
            {
                "wwwroot/css"
            }
        });

        var expectedOutputs = new ExpectedOutputs(excludeCss: true);

        AssertExpectedSyntaxTrees(expectedOutputs, outputCompilation.SyntaxTrees);
    }

    [TestMethod]
    public void CustomOptions_AdditionalStaticFilesPaths()
    {
        var outputCompilation = BaseTest(new Configuration.JsonConfigClass
        {
            AdditionalStaticFilesPaths = new()
            {
                ["wwwrootAlt"] = "wwwrootAlt"
            }
        });

        var expectedOutputs = new ExpectedOutputs(additionalStatic: true);

        AssertExpectedSyntaxTrees(expectedOutputs, outputCompilation.SyntaxTrees);
    }

    [TestMethod]
    public void CustomOptions_CustomStaticFileDirectoryAlias()
    {
        var outputCompilation = BaseTest(new Configuration.JsonConfigClass
        {
            CustomStaticFileDirectoryAlias = new()
            {
                ["wwwroot/js"] = "otherjs"
            }
        });

        var expectedOutputs = new ExpectedOutputs(customJsName: true);

        AssertExpectedSyntaxTrees(expectedOutputs, outputCompilation.SyntaxTrees);
    }
}
