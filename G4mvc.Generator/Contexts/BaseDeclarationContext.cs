using System.Reflection;

namespace G4mvc.Generator.Contexts;
internal abstract class BaseDeclarationContext
{
    public SemanticModel Model { get; }
    public bool NullableEnabled { get; private set; }

    protected BaseDeclarationContext(SemanticModel model, int syntaxSpanStart)
    {
        Model = model;

        NullableEnabled = Model.GetNullableContext(syntaxSpanStart) switch
        {
            NullableContext.Disabled => false,
            NullableContext.ContextInherited => Configuration.Instance.GlobalNullable,
            _ => true
        };
    }
}
