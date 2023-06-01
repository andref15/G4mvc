﻿using System;
using System.Collections.Generic;
using System.Text;

namespace G4mvc.Generator.Helpers;
internal static class CSharpCompilationExtensions
{
    public static bool IsNullableEnabled(this CSharpCompilation compilation)
        => compilation.Options.NullableContextOptions is not NullableContextOptions.Disable;
}
