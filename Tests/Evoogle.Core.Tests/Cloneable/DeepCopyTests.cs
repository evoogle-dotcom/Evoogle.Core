// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using Evoogle.XUnit;

using FluentAssertions;
using Xunit.v3;

namespace Evoogle.Cloneable;

public class DeepCopyTests(ITestOutputHelper output) : XUnitTests(output)
{
    #region Test Classes
    public class SafeDeepCopyTest<T> : XUnitTest
        where T : class, IDeepCloneable
    {
        #region Calculated Properties
        private T? ActualSafeDeepCopy { get; set; }
        private T? ExpectedSafeDeepCopy { get; set; }
        #endregion

        #region User Supplied Properties
        public Func<T?>? ExpectedSafeDeepCopyAccessor { get; set; }
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
            this.ExpectedSafeDeepCopy = this.ExpectedSafeDeepCopyAccessor != null ? this.ExpectedSafeDeepCopyAccessor() : default;

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

    private class EmptyObject : DeepCloneable<EmptyObject>;

    private class Person : DeepCloneable<Person>
    {
        public string? PersonId { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
    }

    private class Employee : Person
    {
        public string? EmployeeNumber { get; set; }
    }

    private class BoardOfDirectors : DeepCloneable<BoardOfDirectors>
    {
        public Person? President { get; set; }
        public Person? VicePresident { get; set; }
    }

    private class People : DeepCloneable<People>
    {
        public List<Person>? PersonCollection { get; set; }
    }

    private class Company : DeepCloneable<Company>
    {
        public string? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public BoardOfDirectors? BoardOfDirectors { get; set; }
        public People? CurrentEmployees { get; set; }
    }
    #endregion

    #region Theory Data
    public static TheoryDataRow<IXUnitTest>[] SafeDeepCopyTheoryData =>
    [
        new SafeDeepCopyTest<EmptyObject>
        {
            Name = "Empty object",
            ExpectedSafeDeepCopyAccessor = () => new EmptyObject()
        },

        new SafeDeepCopyTest<Person>
        {
            Name = "Basic object",
            ExpectedSafeDeepCopyAccessor = () => new Person
            {
                PersonId  = "1234",
                LastName  = "Doe",
                FirstName = "John"
            }
        },

        new SafeDeepCopyTest<BoardOfDirectors>
        {
            Name = "Composite object",
            ExpectedSafeDeepCopyAccessor = () =>
            {
                var president = new Person
                {
                    PersonId  = "1234",
                    LastName  = "Doe",
                    FirstName = "John"
                };
                var vicePresident = new Person
                {
                    PersonId  = "5678",
                    LastName  = "Doe",
                    FirstName = "Jane"
                };
                var boardOfDirectors = new BoardOfDirectors
                {
                    President     = president,
                    VicePresident = vicePresident
                };
                return boardOfDirectors;
            }
        },

        new SafeDeepCopyTest<Person>
        {
            Name = "Derived object",
            ExpectedSafeDeepCopyAccessor = () => new Employee
            {
                PersonId       = "1234",
                LastName       = "Doe",
                FirstName      = "John",
                EmployeeNumber = "1234567890"
            }
        },

        new SafeDeepCopyTest<People>
        {
            Name = "Basic object containing a collection",
            ExpectedSafeDeepCopyAccessor = () =>
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
                    PersonCollection =
                    [
                        person0,
                                person1
                    ]
                };
                return people;
            }
        },

        new SafeDeepCopyTest<Company>
        {
            Name = "Complex object",
            ExpectedSafeDeepCopyAccessor = () =>
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