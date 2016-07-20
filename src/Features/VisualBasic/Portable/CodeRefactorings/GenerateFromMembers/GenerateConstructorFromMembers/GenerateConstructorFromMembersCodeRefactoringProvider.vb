' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports System.Composition
Imports Microsoft.CodeAnalysis.CodeRefactorings
Imports Microsoft.CodeAnalysis.GenerateFromMembers.GenerateConstructorFromMembers

Namespace Microsoft.CodeAnalysis.VisualBasic.CodeRefactorings.GenerateFromMembers.GenerateConstructorFromMembers
    <ExportCodeRefactoringProvider(LanguageNames.VisualBasic, Name:=PredefinedCodeRefactoringProviderNames.GenerateConstructorFromMembers), [Shared]>
    <ExtensionOrder(Before:=PredefinedCodeRefactoringProviderNames.AddConstructorParametersFromMembers)>
    Friend Class GenerateConstructorFromMembersCodeRefactoringProvider
        Inherits CodeRefactoringProvider

        Public Overrides Async Function ComputeRefactoringsAsync(context As CodeRefactoringContext) As Task
            Dim document = context.Document
            Dim textSpan = context.Span
            Dim cancellationToken = context.CancellationToken

            Dim workspace = document.Project.Solution.Workspace
            If workspace.Kind = WorkspaceKind.MiscellaneousFiles Then
                Return
            End If

            Dim service = document.GetLanguageService(Of IGenerateConstructorFromMembersService)()
            Dim actions = Await service.GenerateConstructorFromMembersAsync(document, textSpan, cancellationToken).ConfigureAwait(False)

            If Not actions.IsDefault Then
                context.RegisterRefactorings(actions)
            End If
        End Function
    End Class
End Namespace
