﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meditatr.Enums;
using Meditatr.Infrastructure;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Meditatr.Services
{
    public class ClassService
    {
        private CompilationUnitSyntax _compilationUnitSyntax;
        private NamespaceDeclarationSyntax _namespaceDeclarationSyntax;
        private ClassDeclarationSyntax _classDeclarationSyntax;

        public void Create(string projectName, string handlerProjectName, string modelName, string actionName, OperationType operationType, string returnType)
        {
            _compilationUnitSyntax = SyntaxFactory.CompilationUnit();
            var operation = operationType == OperationType.Command ? "Command" : "Query";
            
            var cqClassName = $"{actionName}{modelName}{operation}";
            AddUsings();
            var cqClassNamespace = CreateNamespace(projectName, modelName, operationType);
            AddNamespace(cqClassNamespace);
            var baseType = $"IRequest<{returnType}>";
            CreateClassStructure(cqClassName, baseType);
            GenerateClass(projectName, modelName, operationType, cqClassName);

            CleanUp();


            _compilationUnitSyntax = SyntaxFactory.CompilationUnit();

            var handlerClassname = $"{cqClassName}Handler";
            AddUsings(true, cqClassNamespace);
            var handlerNamespace = CreateNamespace(handlerProjectName, modelName, operationType);
            AddNamespace(handlerNamespace);
            baseType = $"IRequestHandler<{cqClassName}, {returnType}>";

            CreateClassStructure(handlerClassname, baseType, true, returnType, cqClassName);
            
            GenerateClass(handlerProjectName, modelName, operationType, handlerClassname);
        }

        private void AddUsings(bool forHandler = false, string cqClassNamespace = "")
        {
            _compilationUnitSyntax = _compilationUnitSyntax.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("MediatR")));
            if (forHandler)
            {
                _compilationUnitSyntax = _compilationUnitSyntax.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(cqClassNamespace)));
                _compilationUnitSyntax = _compilationUnitSyntax.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Threading")));
                _compilationUnitSyntax = _compilationUnitSyntax.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Threading.Tasks")));
            }
        }

        private string CreateNamespace(string projectName, string modelName, OperationType operationType)
        {
            var pluralizeOperationType = operationType == OperationType.Command ? "Commands" : "Queries";

            return $"{projectName}.{pluralizeOperationType}.{modelName}{pluralizeOperationType}";
        }

        private void AddNamespace(string name)
        {
            _namespaceDeclarationSyntax = SyntaxFactory.NamespaceDeclaration(
                    SyntaxFactory.ParseName(name))
                .NormalizeWhitespace();
        }

        
        private void CreateClassStructure(string className, string baseType = "", bool isHandler = false, string handlerReturnType = "", string handlerRequestType = "")
        {
            _classDeclarationSyntax = SyntaxFactory.ClassDeclaration(className)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            if (!string.IsNullOrWhiteSpace(baseType))
            {
                _classDeclarationSyntax = _classDeclarationSyntax.AddBaseListTypes(SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(baseType)));

            }

            if (isHandler)
            {
                var body = SyntaxFactory.ParseStatement("throw new System.NotImplementedException();");
                var methodParams = new[]
                {
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("request")).WithType(SyntaxFactory.ParseTypeName(handlerRequestType)),
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("cancellationToken")).WithType(SyntaxFactory.ParseTypeName("CancellationToken")),
                };

                var methodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName($"Task<{handlerReturnType}>"), "Handle")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddParameterListParameters(methodParams)
                    .WithBody(SyntaxFactory.Block(body));

                _classDeclarationSyntax = _classDeclarationSyntax.AddMembers(methodDeclaration);
            }
        }

        private void GenerateClass(string projectName, string modelName, OperationType operationType, string className)
        {
            var pluralizeOperationType = operationType == OperationType.Command ? "Commands" : "Queries";

            _namespaceDeclarationSyntax = _namespaceDeclarationSyntax.AddMembers(_classDeclarationSyntax);

            _compilationUnitSyntax = _compilationUnitSyntax.AddMembers(_namespaceDeclarationSyntax);

            var data = _compilationUnitSyntax
                .NormalizeWhitespace()
                .ToFullString();

            var projectAbsolutePath = IoHelper.GetProjectAbsolutePath(projectName);

            IoHelper.CreateDirectory(new[] { projectAbsolutePath, pluralizeOperationType, $"{modelName}{pluralizeOperationType}" });
            IoHelper.CreateFile(new[] { projectAbsolutePath, pluralizeOperationType, $"{modelName}{pluralizeOperationType}", className }, data);
        }

        private void CleanUp()
        {
            _classDeclarationSyntax = null;
            _namespaceDeclarationSyntax = null;
            _compilationUnitSyntax = null;
        }
    }
}
