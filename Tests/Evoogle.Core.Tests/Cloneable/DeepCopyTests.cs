// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

using Evoogle.Json;
using Evoogle.XUnit;

using FluentAssertions;

using Xunit.Sdk;

[assembly: RegisterXunitSerializer(typeof(XUnitTestSerializer), typeof(XUnitTest), typeof(XUnitTestAsync))]

namespace Evoogle.Cloneable;

[DynamicLinqType]
public class DeepCopyTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    public abstract class SafeDeepCopyTest : XUnitTest
    {
        #region User Supplied Properties
        [JsonConverter(typeof(LambdaExpressionJsonConverter))]
        public LambdaExpression? ExpectedSafeDeepCopyAccessorExpression { get; set; }
        #endregion

        #region Constructors
        public SafeDeepCopyTest()
        {
            this.Name = this.GetType().Name;
        }
        #endregion
    }

    public class SafeDeepCopyTest<T> : SafeDeepCopyTest
        where T : class, IDeepCloneable
    {
        #region Calculated Properties
        private T? ActualSafeDeepCopy { get; set; }
        private T? ExpectedSafeDeepCopy { get; set; }
        #endregion

        #region Constructors
        public SafeDeepCopyTest()
        {
            this.Name = this.GetType().Name;
        }
        #endregion

        #region Methods
        protected override void Arrange()
        {
            var expectedSafeDeepCopyAccessorLambda = this.ExpectedSafeDeepCopyAccessorExpression?.Compile();

            this.ExpectedSafeDeepCopy = expectedSafeDeepCopyAccessorLambda != null ? (T)expectedSafeDeepCopyAccessorLambda.DynamicInvoke()! : default;

            this.WriteLine($"Source Type: {typeof(T).Name}");

            var expectedDeepCopyJson = this.ExpectedSafeDeepCopy.SafeToJson();
            this.WriteLine($"Expected Deep Copy: {expectedDeepCopyJson}");
        }

        protected override void Act()
        {
            this.ActualSafeDeepCopy = this.ExpectedSafeDeepCopy.SafeDeepCopy();

            var actualDeepCopyJson = this.ActualSafeDeepCopy.SafeToJson();
            this.WriteLine($"Actual Deep Copy:   {actualDeepCopyJson}");
        }

        protected override void Assert()
        {
            ReferenceEquals(this.ActualSafeDeepCopy, this.ExpectedSafeDeepCopy).Should().BeFalse(); // Checks expected and actual are 2 unique objects.

            if (this.ExpectedSafeDeepCopy == null)
            {
                this.ActualSafeDeepCopy.Should().BeNull();
                return;
            }

            if (this.ExpectedSafeDeepCopy.GetType() == typeof(EmptyObject))
                return;

            this.ActualSafeDeepCopy.Should().BeEquivalentTo(this.ExpectedSafeDeepCopy);
        }
        #endregion
    }

    [DynamicLinqType]
    public class EmptyObject : DeepCloneable<EmptyObject>;

    public class Person : DeepCloneable<Person>
    {
        public string? PersonId { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
    }

    public class Employee : Person
    {
        public string? EmployeeNumber { get; set; }
    }

    public class BoardOfDirectors : DeepCloneable<BoardOfDirectors>
    {
        public Person? President { get; set; }
        public Person? VicePresident { get; set; }
    }

    public class People : DeepCloneable<People>
    {
        public List<Person>? PersonCollection { get; set; }
    }

    public class Company : DeepCloneable<Company>
    {
        public string? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public BoardOfDirectors? BoardOfDirectors { get; set; }
        public People? CurrentEmployees { get; set; }
    }
    #endregion

    #region Theory Data
    public static BoardOfDirectors CreateBoardOfDirectors()
    {
        var president = new Person
        {
            PersonId = "1234",
            LastName = "Doe",
            FirstName = "John"
        };
        var vicePresident = new Person
        {
            PersonId = "5678",
            LastName = "Doe",
            FirstName = "Jane"
        };
        var boardOfDirectors = new BoardOfDirectors
        {
            President = president,
            VicePresident = vicePresident
        };

        return boardOfDirectors;
    }

    public static Employee CreateEmployee()
    {
        var employee = new Employee()
        {
            PersonId = "1234",
            LastName = "Doe",
            FirstName = "John",
            EmployeeNumber = "1234567890"
        };
        return employee;
    }

    public static Person CreatePerson()
    {
        var person = new Person
        {
            PersonId = "1234",
            LastName = "Doe",
            FirstName = "John"
        };
        return person;
    }

    public static People CreatePeople()
    {
        var person0 = new Person
        {
            PersonId = "1234",
            LastName = "Doe",
            FirstName = "John"
        };
        var person1 = new Person
        {
            PersonId = "5678",
            LastName = "Doe",
            FirstName = "Jane"
        };

        var people = new People
        {
            PersonCollection = [person0, person1]
        };
        return people;
    }

    public static Company CreateCompany()
    {
        var president = new Person
        {
            PersonId = "1111",
            FirstName = "George",
            LastName = "Washington"
        };
        var vicePresident = new Person
        {
            PersonId = "2222",
            FirstName = "John",
            LastName = "Adams"
        };
        var boardOfDirectors = new BoardOfDirectors
        {
            President = president,
            VicePresident = vicePresident
        };

        var employee0 = new Employee
        {
            PersonId = "1234",
            LastName = "Doe",
            FirstName = "John",
            EmployeeNumber = "1111111111"
        };
        var employee1 = new Employee
        {
            PersonId = "5678",
            LastName = "Doe",
            FirstName = "Jane",
            EmployeeNumber = "2222222222"
        };
        var employees = new People
        {
            PersonCollection =
            [
                employee0,
                employee1
            ]
        };

        var company = new Company
        {
            CompanyId = "Acme",
            CompanyName = "Acme, Inc.",
            BoardOfDirectors = boardOfDirectors,
            CurrentEmployees = employees
        };
        return company;
    }

    public static TheoryDataRow<IXUnitTest>[] SafeDeepCopyTheoryData =>
    [
        new SafeDeepCopyTest<EmptyObject>
        {
            Name = "Empty object",
            ExpectedSafeDeepCopyAccessorExpression = () => new EmptyObject()
        },

        new SafeDeepCopyTest<Person>
        {
            Name = "Basic object",
            ExpectedSafeDeepCopyAccessorExpression = () => CreatePerson()
        },

        new SafeDeepCopyTest<BoardOfDirectors>
        {
            Name = "Composite object",
            ExpectedSafeDeepCopyAccessorExpression = () => CreateBoardOfDirectors()
        },

        new SafeDeepCopyTest<Person>
        {
            Name = "Derived object",
            ExpectedSafeDeepCopyAccessorExpression = () => CreateEmployee()
        },

        new SafeDeepCopyTest<People>
        {
            Name = "Basic object containing a collection",
            ExpectedSafeDeepCopyAccessorExpression = () => CreatePeople()
        },

        new SafeDeepCopyTest<Company>
        {
            Name = "Complex object",
            ExpectedSafeDeepCopyAccessorExpression = () => CreateCompany()
        }
    ];
    #endregion

    #region Test Methods
    [Theory]
    [MemberData(nameof(SafeDeepCopyTheoryData))]
    public void SafeDeepCopy(IXUnitTest test)
    {
        test.Execute(this);
    }
    #endregion
}