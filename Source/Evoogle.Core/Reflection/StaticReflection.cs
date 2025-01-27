// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Linq.Expressions;

namespace Evoogle.Reflection;

/// <summary>
///     Static reflection class to get property or method names at compile time.
/// </summary>
public static class StaticReflection
{
    #region Methods
    public static string GetMemberName<T>(Expression<Func<T, object>> expression)
    {
        return GetMemberName(expression.Body);
    }

    public static string GetMemberName<T, TResult>(Expression<Func<T, TResult>> expression)
    {
        return GetMemberName(expression.Body);
    }

    public static string GetMemberName<T>(Expression<Action<T>> expression)
    {
        return GetMemberName(expression.Body);
    }

    public static string GetMemberName<T>(this T _, Expression<Func<T, object>> expression)
    {
        return GetMemberName(expression.Body);
    }

    public static string GetMemberName<T, TResult>(this T _, Expression<Func<T, TResult>> expression)
    {
        return GetMemberName(expression.Body);
    }

    public static string GetMemberName<T>(this T _, Expression<Action<T>> expression)
    {
        return GetMemberName(expression.Body);
    }

    private static string GetMemberName(Expression expression)
    {
        switch (expression)
        {
            case MemberExpression memberExpression:
                // Reference type property or field
                return memberExpression.Member.Name;
            case MethodCallExpression methodCallExpression:
                // Reference type method
                return methodCallExpression.Method.Name;
            case UnaryExpression unaryExpression:
                // Property, field of method returning value type
                return GetMemberName(unaryExpression);
            default:
                throw new ArgumentException($"Invalid expression, must be either a {nameof(MemberExpression)}, {nameof(MethodCallExpression)}, or {nameof(UnaryExpression)}.");
        }
    }

    private static string GetMemberName(UnaryExpression unaryExpression)
    {
        return unaryExpression.Operand is MethodCallExpression operand
            ? operand.Method.Name
            : ((MemberExpression)unaryExpression.Operand).Member.Name;
    }
    #endregion
}