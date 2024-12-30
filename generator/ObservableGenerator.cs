using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace generator
{
    [Generator]
    public class ObservableGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterSourceOutput(context.CompilationProvider, GenerateSource);
        }

        private void GenerateSource(SourceProductionContext context, Compilation compilation)
        {
            IEnumerable<SyntaxTree> syntaxTrees = compilation.SyntaxTrees;

            foreach (SyntaxTree? tree in syntaxTrees)
            {
                SyntaxNode root = tree.GetRoot();
                IEnumerable<ClassDeclarationSyntax> classDeclarations = root.DescendantNodes()
                    .OfType<Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax>();

                foreach (ClassDeclarationSyntax classDeclaration in classDeclarations)
                {
                    INamedTypeSymbol? classSymbol = (INamedTypeSymbol?)
                        compilation.GetSemanticModel(tree).GetDeclaredSymbol(classDeclaration);

                    if (classSymbol is not { BaseType.Name: "BindingBase" })
                        continue;

                    List<(string Name, string Type)> observableFields = GetObservableFields(
                        classSymbol
                    );

                    if (!observableFields.Any())
                        continue;

                    string className = classSymbol.Name;
                    string? namespaceName = classSymbol.ContainingNamespace.ToString();

                    string generatedCode = GeneratePartialClass(
                        namespaceName,
                        className,
                        observableFields
                    );

                    context.AddSource(
                        $"{className}_Generated.cs",
                        SourceText.From(generatedCode, Encoding.UTF8)
                    );
                }
            }
        }

        private List<(string Name, string Type)> GetObservableFields(INamedTypeSymbol classSymbol)
        {
            List<(string Name, string Type)> observableFields =
                new List<(string Name, string Type)>();

            foreach (ISymbol? member in classSymbol.GetMembers())
            {
                if (
                    member is IFieldSymbol field
                    && field
                        .GetAttributes()
                        .Any(attr => attr.AttributeClass?.Name == "ObservableAttribute")
                )
                {
                    observableFields.Add((field.Name, field.Type.Name));
                }
            }

            return observableFields;
        }

        private string GeneratePartialClass(
            string namespaceName,
            string className,
            List<(string Name, string Type)> observableFields
        )
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(
                @$"
namespace {namespaceName};
public partial class {className}
{{
"
            );

            foreach ((string fieldName, string fieldType) in observableFields)
            {
                sb.Append(
                    @$"
    public {fieldType} G{fieldName}
    {{
        get=>{fieldName};
        set=>SetField(ref {fieldName},value);
    }}
"
                );
            }

            sb.Append(
                @$"
}}
"
            );
            return sb.ToString();
        }
    }
}
