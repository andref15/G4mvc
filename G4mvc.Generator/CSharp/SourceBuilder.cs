using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace G4mvc.Generator.CSharp;
internal class SourceBuilder
{
    private readonly StringBuilder _stringBuilder = new();
    private readonly LanguageVersion _languageVersion;
    private readonly List<string> _globalUsings;
    private int _indentCounter = 0;
    private NamespaceBlock? _currentNamespace;

    public SourceBuilder(LanguageVersion languageVersion, List<string> globalUsings)
    {
        _languageVersion = languageVersion;
        _globalUsings = globalUsings;
    }

    public SourceBuilder AppendLine()
    {
        _stringBuilder.AppendLine();
        return this;
    }

    public SourceBuilder AppendLine(string line)
    {
        AppendIndentation();
        _stringBuilder.Append(line).AppendLine(";");

        return this;
    }

    public SourceBuilder AppendConst(string modifiers, string type, string name, string value)
    {
        AppendIndentation();

        _stringBuilder.AppendLine($"{modifiers} const {type} {name} = {value};");

        return this;
    }

    public SourceBuilder AppendProperty(string modifiers, string type, string name, string get, string? set, string? assignmnet = null)
    {
        AppendIndentation();
        _stringBuilder.Append($"{modifiers} {type} {name} {{ {get}; {(set is null ? null : $"{set}; ")}}}");

        if (assignmnet is not null)
        {
            if (assignmnet is SourceCode.NewCtor && _languageVersion < LanguageVersion.CSharp9)
            {
                assignmnet = $"new {type}()";
            }

            _stringBuilder.Append($" = {assignmnet};");
        }

        _stringBuilder.AppendLine();

        return this;
    }

    public SourceBuilder AppendProperties(string modifier, Dictionary<string, string> propertyDefinitions, string get, string? set, string? assignmnet = null)
    {
        foreach (KeyValuePair<string, string> propertyDefinition in propertyDefinitions)
        {
            AppendProperty(modifier, propertyDefinition.Key, propertyDefinition.Value, get, set, assignmnet);
        }

        return this;
    }

    public SourceBuilder AppendReturn(string @return)
    {
        AppendLine($"return {@return}");

        return this;
    }

    public SourceBuilder AppendReturnCtor(string type, params string[] parameters)
    {
        string ctorCall = _languageVersion >= LanguageVersion.CSharp9 ? "new" : $"new {type}";

        AppendReturn($"{ctorCall}({string.Join(", ", parameters)})");

        return this;
    }

    public NamespaceBlock BeginNamespace(string @namespace, bool fileScoped)
    {
        fileScoped &= _languageVersion >= LanguageVersion.CSharp10;

        return new(this, @namespace, fileScoped);
    }

    public SourceBuilderBlock BeginClass(string modifiers, string className)
    {
        AppendIndentation();

        _stringBuilder.AppendLine($"{modifiers} class {className}");

        return new(this);
    }

    public SourceBuilderBlock BeginMethod(string modifiers, string returnType, string methodName, string? parameters = null)
    {
        AppendIndentation();
        _stringBuilder.AppendLine($"{modifiers} {returnType} {methodName}({parameters})");

        return new(this);
    }

    public SourceBuilderBlock BeginObjectInitializer(string type, bool @return, params string[] parameters)
    {
        string ctorCall = _languageVersion >= LanguageVersion.CSharp9 ? "new" : $"new {type}";

        AppendIndentation();

        _stringBuilder.AppendLine($"{(@return ? "return " : null)}{ctorCall}({string.Join(", ", parameters)})");

        return new(this);
    }

    public NullableBlock BeginNullable(bool enable)
        => new(this, enable);

    public SourceBuilder Nullable(bool enable)
    {
        _stringBuilder.AppendLine($"#nullable {(enable ? "enable" : "disable")}").AppendLine();

        return this;
    }

    public SourceBuilder Using(params string[] usings)
        => Using((IEnumerable<string>)usings);

    public SourceBuilder Using(IEnumerable<string> usings)
    {
        foreach (string @using in usings)
        {
            if (_globalUsings.Contains(@using))
            {
                continue;
            }

            _stringBuilder.AppendLine($"using {@using};");
        }

        return this;
    }

    private void AppendIndentation()
        => _stringBuilder.Append(string.Empty.PadRight(_indentCounter, '\t'));

    public override string ToString()
        => _stringBuilder.ToString();

    public SourceText ToSourceText()
        => SourceText.From(ToString(), Encoding.UTF8);

    public class NullableBlock : IDisposable
    {
        private readonly SourceBuilder _sourceBuilder;

        public NullableBlock(SourceBuilder sourceBuilder, bool enable)
        {
            _sourceBuilder = sourceBuilder;
            sourceBuilder.Nullable(enable);
        }

        public void Dispose()
            => _sourceBuilder._stringBuilder.AppendLine().AppendLine($"#nullable restore").AppendLine();
    }

    public class SourceBuilderBlock : IDisposable
    {
        private readonly SourceBuilder _sourceBuilder;

        public SourceBuilderBlock(SourceBuilder sourceBuilder)
        {
            sourceBuilder.AppendIndentation();
            sourceBuilder._stringBuilder.AppendLine("{");
            sourceBuilder._indentCounter++;
            _sourceBuilder = sourceBuilder;
        }

        public void Dispose()
        {
            _sourceBuilder._indentCounter--;
            _sourceBuilder.AppendIndentation();
            _sourceBuilder._stringBuilder.AppendLine("}");
        }
    }

    public class NamespaceBlock : IDisposable
    {
        private readonly SourceBuilder _sourceBuilder;
        private readonly NamespaceBlock? _parent;
        private readonly bool _fileScoped;

        public NamespaceBlock(SourceBuilder sourceBuilder, string @namespace, bool fileScoped)
        {
            _parent = sourceBuilder._currentNamespace;

            if (fileScoped && _parent is not null)
            {
                fileScoped = false;
            }

            sourceBuilder.AppendIndentation();

            sourceBuilder._stringBuilder.Append($"namespace {@namespace}");

            if (fileScoped)
            {
                sourceBuilder._stringBuilder.AppendLine(";");
            }
            else
            {
                sourceBuilder._stringBuilder.AppendLine().AppendLine("{");
                sourceBuilder._indentCounter++;
            }

            sourceBuilder._currentNamespace = this;

            _fileScoped = fileScoped;
            _sourceBuilder = sourceBuilder;
        }

        public void Dispose()
        {
            _sourceBuilder._currentNamespace = _parent;

            if (!_fileScoped)
            {
                _sourceBuilder._indentCounter--;
                _sourceBuilder.AppendIndentation();
                _sourceBuilder._stringBuilder.AppendLine("}");
            }
        }
    }
}
