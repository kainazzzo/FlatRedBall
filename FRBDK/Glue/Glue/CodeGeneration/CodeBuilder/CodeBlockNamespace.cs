﻿namespace FlatRedBall.Glue.CodeGeneration.CodeBuilder
{
    public class CodeBlockNamespace : CodeBlockBase
    {
        public CodeBlockNamespace(ICodeBlock pParent, string value) : base(pParent)
        {
            PreCodeLines.Add(new CodeLine("namespace " + (string.IsNullOrEmpty(value) ? "" : value)));
            PreCodeLines.Add(new CodeLine("{"));
            PostCodeLines.Add(new CodeLine("}"));
        }
    }

    public static class CodeBlockNamespaceExtensions
    {
        public static CodeBlockNamespace Namespace(this ICodeBlock pCodeBase, string pValue)
        {
            return new CodeBlockNamespace(pCodeBase, pValue);
        }
    }
}
