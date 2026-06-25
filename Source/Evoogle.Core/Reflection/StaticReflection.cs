// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Linq.Expressions;

namespace Evoogle.Reflection;

/// <summary>
///     Static reflection class to get property or method names at compile time.
/// </summary>
public static class StaticReflection
{
    #region Methods
    /// <summary>
    ///     Gets the member name represented by a strongly-typed expression.
    /// </summary>
    /// <typeparam name="T">Type containing the member.</typeparam>
    /// <param name="expression">Expression selecting the member.</param>
    /// <param name="throwOnUnsupported">
    ///     If <see langword="true"/>, throws an <see cref="ArgumentException"/> when the expression type is unsupported;
    ///     otherwise, returns a placeholder name.
    /// </param>
    /// <returns>The name of the member.</returns>
    public static string GetMemberName<T>(Expression<Func<T, object>> expression, bool throwOnUnsupported = true) => GetMemberName(expression.Body, throwOnUnsupported);

    /// <summary>
    ///     Gets the member name represented by a strongly-typed expression.
    /// </summary>
    /// <typeparam name="T">Type containing the member.</typeparam>
    /// <typeparam name="TResult">Return type of the member.</typeparam>
    /// <param name="expression">Expression selecting the member.</param>
    /// <param name="throwOnUnsupported">
    ///     If <see langword="true"/>, throws an <see cref="ArgumentException"/> when the expression type is unsupported;
    ///     otherwise, returns a placeholder name.
    /// </param>
    /// <returns>The name of the member.</returns>
    public static string GetMemberName<T, TResult>(Expression<Func<T, TResult>> expression, bool throwOnUnsupported = true) => GetMemberName(expression.Body, throwOnUnsupported);

    /// <summary>
    ///     Gets the member name represented by a strongly-typed action expression.
    /// </summary>
    /// <typeparam name="T">Type containing the member.</typeparam>
    /// <param name="expression">Expression selecting the member.</param>
    /// <param name="throwOnUnsupported">
    ///     If <see langword="true"/>, throws an <see cref="ArgumentException"/> when the expression type is unsupported;
    ///     otherwise, returns a placeholder name.
    /// </param>
    /// <returns>The name of the member.</returns>
    public static string GetMemberName<T>(Expression<Action<T>> expression, bool throwOnUnsupported = true) => GetMemberName(expression.Body, throwOnUnsupported);

    /// <summary>
    ///     Gets the member name represented by a strongly-typed expression for the specified instance.
    /// </summary>
    /// <typeparam name="T">Type containing the member.</typeparam>
    /// <param name="_">Instance providing the generic type argument.</param>
    /// <param name="expression">Expression selecting the member.</param>
    /// <param name="throwOnUnsupported">
    ///     If <see langword="true"/>, throws an <see cref="ArgumentException"/> when the expression type is unsupported;
    ///     otherwise, returns a placeholder name.
    /// </param>
    /// <returns>The name of the member.</returns>
    public static string GetMemberName<T>(this T _, Expression<Func<T, object>> expression, bool throwOnUnsupported = true) => GetMemberName(expression.Body, throwOnUnsupported);

    /// <summary>
    ///     Gets the member name represented by a strongly-typed expression for the specified instance.
    /// </summary>
    /// <typeparam name="T">Type containing the member.</typeparam>
    /// <typeparam name="TResult">Return type of the member.</typeparam>
    /// <param name="_">Instance providing the generic type argument.</param>
    /// <param name="expression">Expression selecting the member.</param>
    /// <param name="throwOnUnsupported">
    ///     If <see langword="true"/>, throws an <see cref="ArgumentException"/> when the expression type is unsupported;
    ///     otherwise, returns a placeholder name.
    /// </param>
    /// <returns>The name of the member.</returns>
    public static string GetMemberName<T, TResult>(this T _, Expression<Func<T, TResult>> expression, bool throwOnUnsupported = true) => GetMemberName(expression.Body, throwOnUnsupported);

    /// <summary>
    ///     Gets the member name represented by a strongly-typed action expression for the specified instance.
    /// </summary>
    /// <typeparam name="T">Type containing the member.</typeparam>
    /// <param name="_">Instance providing the generic type argument.</param>
    /// <param name="expression">Expression selecting the member.</param>
    /// <param name="throwOnUnsupported">
    ///     If <see langword="true"/>, throws an <see cref="ArgumentException"/> when the expression type is unsupported;
    ///     otherwise, returns a placeholder name.
    /// </param>
    /// <returns>The name of the member.</returns>
    public static string GetMemberName<T>(this T _, Expression<Action<T>> expression, bool throwOnUnsupported = true) => GetMemberName(expression.Body, throwOnUnsupported);

    /// <summary>
    ///     Gets the ordered chain of member names from root to leaf represented by a dot-navigated expression.
    /// </summary>
    /// <typeparam name="T">The root type.</typeparam>
    /// <typeparam name="TResult">The return type of the final member.</typeparam>
    /// <param name="expression">A lambda expression consisting only of chained member access (e.g. <c>x => x.Address.City</c>).</param>
    /// <returns>An ordered array of member names from root to leaf, e.g. <c>["Address", "City"]</c>.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when the expression body contains anything other than chained member access on the lambda parameter.
    /// </exception>
    public static string[] GetMemberPath<T, TResult>(Expression<Func<T, TResult>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        var names = new Stack<string>();
        var node = UnwrapConversion(expression.Body);

        while (node is MemberExpression member)
        {
            names.Push(member.Member.Name);
            node = UnwrapConversion(member.Expression);
        }

        if (node is not ParameterExpression)
        {
            throw new ArgumentException
            (
                $"Expression must consist only of chained member access on the lambda parameter (e.g. x => x.A.B.C). " +
                $"Unsupported node type: {node?.GetType().Name ?? "null"}.",
                nameof(expression)
            );
        }

        if (names.Count == 0)
        {
            throw new ArgumentException
            (
                "Expression must contain at least one member access (e.g. x => x.Property).",
                nameof(expression)
            );
        }

        return [.. names];
    }

    /// <summary>
    ///     Removes compiler-inserted conversion nodes so object-returning lambdas over value types are still treated
    ///     as simple member-access expressions.
    /// </summary>
    /// <param name="expression">Expression tree node to normalize.</param>
    /// <returns>The operand for conversion expressions; otherwise, the original expression.</returns>
    private static Expression? UnwrapConversion(Expression? expression)
    {
        while (expression is UnaryExpression { NodeType: ExpressionType.Convert or ExpressionType.ConvertChecked } unaryExpression)
        {
            expression = unaryExpression.Operand;
        }

        return expression;
    }

    /// <summary>
    ///     Core logic that extracts the member name from an expression tree.
    /// </summary>
    /// <param name="expression">Expression tree to analyze.</param>
    /// <param name="throwOnUnsupported">
    ///     If <see langword="true"/>, throws an <see cref="ArgumentException"/> when the expression type is unsupported;
    ///     otherwise, returns a placeholder name.
    /// </param>
    /// <returns>The name of the member represented by the expression.</returns>
    internal static string GetMemberName(Expression expression, bool throwOnUnsupported)
    {
        switch (expression)
        {
            case MemberExpression memberExpression:
                return memberExpression.Member.Name;

            case MethodCallExpression methodCallExpression:
                return methodCallExpression.Method.Name;

            case UnaryExpression unaryExpression:
                return GetMemberName(unaryExpression.Operand, throwOnUnsupported);

            case BinaryExpression binaryExpression:
                if (binaryExpression.Left is MemberExpression leftMember)
                {
                    return leftMember.Member.Name;
                }

                if (!throwOnUnsupported)
                {
                    return $"Unsupported_{expression.GetType().Name}";
                }

                throw new ArgumentException("Unsupported binary expression without a left member access.");

            case ConstantExpression constantExpression:
                return $"Constant_{constantExpression.Value?.ToString() ?? "null"}";

            case ParameterExpression parameterExpression:
                return parameterExpression.Name ?? "parameter";

            default:
                if (!throwOnUnsupported)
                {
                    return $"Unsupported_{expression.GetType().Name}";
                }

                throw new ArgumentException($"Unsupported expression type: {expression.GetType().Name}.");
        }
    }
    #endregion
}
