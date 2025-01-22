// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Linq.Expressions;

namespace Evoogle.Json.Internal;

/// <summary>
///     This API supports the Evoogle.Core infrastructure and is not intended to be used directly from your code.
///     This API may change or be removed in future releases.
/// </summary>
internal static class ExpressionUtils
{
    #region Methods
    public static string GetExpressionBodyString(Expression expressionBody)
    {
        var expressionBodyNodeType = expressionBody.NodeType;

        switch (expressionBodyNodeType)
        {
            case ExpressionType.Call:
                return GetCallExpressionBodyString(expressionBody);

            default:
                return expressionBody.ToString();
        }
    }

    public static Type[]? GetParameterTypes(LambdaExpression lambdaExpression)
    {
        var parameters = lambdaExpression.Parameters;
        if (parameters == null || parameters.Any() == false)
            return null;

        var parameterTypes = parameters.Select(x => x.Type).ToArray();
        return parameterTypes;
    }

    private static string GetCallExpressionBodyString(Expression expressionBody)
    {
        var methodCallExpressionBody = (MethodCallExpression)expressionBody;

        var method = methodCallExpressionBody.Method;
        if (!method.IsStatic)
        {
            // This is an instance method call, leave expression body with a parameter expression as-is.
            return $"{expressionBody}";
        }

        // Add the declaring type to the static method call within the expression body string with C# dot method calling syntax.
        var staticMethodDeclaringType = method.DeclaringType;
        var staticMethodDeclaringTypeName = staticMethodDeclaringType!.Name;

        var expressionBodyString = $"{staticMethodDeclaringTypeName}.{expressionBody}";
        return expressionBodyString;
    }
    #endregion
}
