// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Text.Json.Serialization;
using System.Linq.Expressions;

using Evoogle.XUnit;
using Evoogle.Reflection;
using Evoogle.Json;
using Evoogle.Coercion.Internal;

using FluentAssertions;

namespace Evoogle.Coercion;

public class CoerceTest : XUnitTest
{
    #region Methods
    protected static string TypeAsString(Type type)
    {
        if (!TypeReflection.IsNullableType(type))
        {
            var typeName = type.Name;
            return typeName;
        }

        var underlyingType = Nullable.GetUnderlyingType(type) ?? throw new InvalidOperationException($"Unable to get the underlying type from given type {{Name={type.Name}}}");
        var underlyingTypeName = underlyingType.Name;
        var nullableTypeName = string.Format("Nullable<{0}>", underlyingTypeName);
        return nullableTypeName;
    }

    protected static string TypeAsString<T>()
    {
        var type = typeof(T);
        return TypeAsString(type)!;
    }

    protected static string ValueAsString<TValue>(TValue? value)
    {
        var valueAsString = value.SafeToString();
        return valueAsString;
    }
    #endregion
}

public abstract class CoerceTest<TInput, TOutput> : CoerceTest
{
    #region Calculated Properties
    private ITypeCoercion TypeCoercion { get; set; } = null!;
    private TypeCoercionContext TypeCoercionContext { get; set; } = null!;
    private bool ActualResult { get; set; }
    private TOutput? ActualOutput { get; set; }
    #endregion

    #region User Supplied Properties
    public TInput? Input { get; set; }

    [JsonConverter(typeof(ExpressionFuncJsonConverter<TypeCoercionContext>))]
    public Expression<Func<TypeCoercionContext>>? ContextFactoryExpression { get; set; }

    public bool ExpectedResult { get; set; }

    public TOutput? ExpectedOutput { get; set; }
    #endregion

    #region XUnitTest Methods
    protected override void Arrange()
    {
        this.TypeCoercion = new TypeCoercion();

        var context = default(TypeCoercionContext);
        if (this.ContextFactoryExpression != null)
        {
            var contextFactoryFunc = this.ContextFactoryExpression.Compile();
            context = contextFactoryFunc();
        }
        else
        {
            context = new TypeCoercionContext();
        }

        this.TypeCoercionContext = context;

        var inputAsString = ValueAsString(this.Input);
        var inputTypeAsString = TypeAsString<TInput>();

        this.WriteLine($"Input");
        this.WriteLine($"  Value: {inputAsString} ({inputTypeAsString})");
        this.WriteLine();

        this.WriteLine($"Expected Output");
        this.WriteLine($"  Result: {this.ExpectedResult}");

        if (this.ExpectedResult)
        {
            var expectedOutputAsString = ValueAsString(this.ExpectedOutput);
            var expectedOutputTypeAsString = TypeAsString<TOutput>();

            this.WriteLine($"  Value:  {expectedOutputAsString} ({expectedOutputTypeAsString})");
        }

        this.WriteLine();
    }

    protected override void Act()
    {
        var actualResult = default(bool);
        var actualOutput = default(TOutput?);
        try
        {
            actualOutput = this.CoerceImpl(this.TypeCoercion, this.Input, this.TypeCoercionContext);
            actualResult = true;
        }
        catch (Exception exception)
        {
            this.WriteLine($"Exception");
            this.WriteLine($"  Message: {exception.Message.SafeToString()}");
            this.WriteLine();

            actualResult = false;
        }

        this.ActualResult = actualResult;
        this.ActualOutput = actualOutput;

        this.WriteLine($"Actual Output");
        this.WriteLine($"  Result: {this.ActualResult}");
        if (this.ActualResult)
        {
            var actualOutputAsString = ValueAsString(actualOutput);
            var actualOutputTypeAsString = TypeAsString<TOutput>();

            this.WriteLine($"  Value:  {actualOutputAsString} ({actualOutputTypeAsString})");
        }
    }

    protected override void Assert()
    {
        this.ActualResult.Should().Be(this.ExpectedResult);
        if (this.ActualResult == false)
            return;

        AssertOutputForSuccessResult();
    }
    #endregion

    #region Methods
    protected abstract TOutput? CoerceImpl(ITypeCoercion typeCoercion, TInput? input, TypeCoercionContext context);

    private void AssertOutputForSuccessResult()
    {
        if (this.ExpectedOutput == null)
        {
            this.ActualOutput.Should().BeNull();
            return;
        }

        // Handle special cases:

        // .. Byte Array
        if (typeof(TOutput) == typeof(byte[]))
        {
            this.ActualOutput.Should().BeAssignableTo<TOutput>();

            var expectedOutput = this.ExpectedOutput as byte[];
            var actualOutput = this.ActualOutput as byte[];
            actualOutput.Should().ContainInOrder(expectedOutput);
            return;
        }

        // Handle nominal case here.
        this.ActualOutput.Should().BeAssignableTo<TOutput>();
        this.ActualOutput.Should().BeEquivalentTo(this.ExpectedOutput);
    }
    #endregion
}

