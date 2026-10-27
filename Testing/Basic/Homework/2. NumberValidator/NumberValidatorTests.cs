using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    // Проверка конструктора NumberValidator.
    [Test]
    public void Constructor_ThrowException_AfterNegativePrecision()
    {
        var act = () => new NumberValidator(-1, 1);
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Constructor_ThrowException_AfterZeroPrecision()
    {
        var act = () => new NumberValidator(0, 2);
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Constructor_ThrowException_AfterNegativeScale()
    {
        var act = () => new NumberValidator(2, -2);
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Constructor_ThrowException_AfterScaleSamePrecision()
    {
        var act = () => new NumberValidator(2, 2);
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Constructor_ThrowException_AfterScaleGreaterThenPrecision()
    {
        var act = () => new NumberValidator(2, 3);
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Constructor_OnlyOneParameter()
    {
        var act = () => new NumberValidator(2);
        act.Should().NotThrow();
    }

    [Test]
    public void Constructor_OnlyTwoParameters()
    {
        var act = () => new NumberValidator(2, 1);
        act.Should().NotThrow();
    }

    [TestCase(1, 0, true)]
    [TestCase(10, 5, false)]
    [TestCase(9999999, 0, true)]
    [TestCase(199999, 9999, false)]
    public void Constructor_NotThrow(int precision, int scale, bool onlyPositive)
    {
        var act = () => new NumberValidator(precision, scale, onlyPositive);
        act.Should().NotThrow();
    }

    // Хотел ещё проверить, что поля в экземпляре класса инициализируются корректно,
    // но они private, поэтому не могу.



    // Проверка метода IsValidNumber.
    [TestCase(3, 0, true, "123")]
    [TestCase(4, 0, true, "+123")]
    [TestCase(4, 0, false, "-123")]
    [TestCase(1, 0, true, "0")]
    [TestCase(3, 2, true, "0.0")]
    [TestCase(4, 2, true, "00.00")]
    [TestCase(6, 2, true, "+123.98")]
    [TestCase(6, 2, false, "-123.98")]
    [TestCase(6, 2, true, "1,23")]
    [TestCase(6, 2, false, "-1,23")]
    public void IsValidNumber_ValidNumber(int precision, int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        validator.IsValidNumber(value).Should().BeTrue();
    }

    [TestCase(2, 0, true, "123")]
    [TestCase(3, 0, true, "+123")]
    [TestCase(4, 0, true, "-123")]
    [TestCase(3, 0, false, "-123")]
    [TestCase(2, 0, true, "+")]
    [TestCase(2, 0, true, "-")]
    [TestCase(2, 0, true, ".")]
    [TestCase(2, 0, true, ",")]
    [TestCase(2, 0, true, "a")]
    [TestCase(3, 2, true, "a.sd")]
    [TestCase(3, 2, true, "+1.23")]
    [TestCase(3, 2, false, "-1.23")]
    [TestCase(17, 2, false, null)]
    [TestCase(17, 2, false, "")]
    [TestCase(17, 2, false, "   \t\n    \r  ")]
    [TestCase(3, 1, true, "1.23")]
    [TestCase(4, 2, true, "12..3")]
    [TestCase(4, 2, true, ".1")]
    [TestCase(4, 2, true, "1.")]
    [TestCase(4, 0, true, "1.2")]
    [TestCase(5, 2, true, "-12.34")]
    public void IsValidNumber_InvalidNumber(int precision, int scale, bool onlyPositive, string? value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        validator.IsValidNumber(value).Should().BeFalse();
    }
}