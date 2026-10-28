using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    // Проверка конструктора NumberValidator.
    [TestCase(-1, TestName = "NegativePrecision")]
    [TestCase(0, TestName = "ZeroPrecision")]
    public void Constructor_ThrowException_AfterInvalidPrecision(int precision)
    {
        var act = () => new NumberValidator(precision);
        act.Should().Throw<ArgumentException>();
    }

    [TestCase(2, -1, TestName = "NegativeScale")]
    [TestCase(2, 2, TestName = "ScaleSamePrecision")]
    [TestCase(2, 3, TestName = "ScaleGreaterThenPrecision")]
    public void Constructor_ThrowException_AfterInvalidScale(int precision, int scale)
    {
        var act = () => new NumberValidator(precision, scale);
        act.Should().Throw<ArgumentException>();
    }


    // Проверка метода IsValidNumber.
    [TestCase(3, 0, true, "123", TestName = "ValidPositiveInteger")]
    [TestCase(4, 0, true, "+123", TestName = "ValidPositiveIntegerWithPlus")]
    [TestCase(4, 0, false, "-123", TestName = "ValidNegativeInteger")]
    [TestCase(20, 0, true, "58032764011056", TestName = "ValidBigInteger")]
    [TestCase(1, 0, true, "0", TestName = "ValidZero")]
    [TestCase(4, 0, false, "0000", TestName = "ValidZeros")]
    [TestCase(2, 0, true, "+0", TestName = "ValidZeroWithPlus")]
    [TestCase(4, 0, true, "0123", TestName = "ValidIntegerWithLeadingZeros")]
    [TestCase(5, 0, true, "+0123", TestName = "ValidIntegerWithLeadingZerosAndPlus")]

    [TestCase(3, 2, true, "0.0", TestName = "ValidZeroWithDecimalZero")]
    [TestCase(4, 2, true, "00.00", TestName = "ValidZerosWithDecimalZeros")]
    [TestCase(6, 2, true, "123.98", TestName = "ValidPositiveDecimal")]
    [TestCase(6, 2, true, "+123.98", TestName = "ValidPositiveDecimalWithPlus")]
    [TestCase(6, 2, false, "-123.98", TestName = "ValidNegativeDecimal")]
    [TestCase(40, 20, false, "4823942349.9812412", TestName = "ValidBigDecimal")]
    [TestCase(15, 10, true, "+123.980000", TestName = "ValidDecimalWithUselessZeros")]

    [TestCase(6, 2, true, "+1,23", TestName = "ValidPositiveDecimalWithComma")]
    [TestCase(6, 2, false, "-1,23", TestName = "ValidNegativeDecimalWithComma")]
    public void IsValidNumber_ShouldReturnTrue_WhenValidNumber(int precision, int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        validator.IsValidNumber(value).Should().BeTrue();
    }

    [TestCase(2, 0, true, "123", TestName = "PrecisionLessThanNumber")]
    [TestCase(3, 0, true, "+123", TestName = "PrecisionLessThanNumberWithPlus")]
    [TestCase(4, 0, true, "-123", TestName = "PrecisionLessThanNegativeNumber")]
    [TestCase(3, 0, false, "-123", TestName = "PrecisionLessThanNegativeNumberWithMinus")]
    [TestCase(2, 0, true, "+", TestName = "OnlyPlus")]
    [TestCase(2, 0, true, "-", TestName = "OnlyMinus")]
    [TestCase(2, 0, true, ".", TestName = "OnlyDot")]
    [TestCase(2, 0, true, ",", TestName = "OnlyComma")]
    [TestCase(2, 0, true, "abs", TestName = "LettersInNumber")]
    [TestCase(3, 2, true, "a.sd", TestName = "LettersInFormatDecimalNumber")]
    [TestCase(3, 2, true, "+1.23", TestName = "PrecisionLessThanDemicalNumberWithPlus")]
    [TestCase(3, 2, false, "-1.23", TestName = "PrecisionLessThanDemicalNegativeNumber")]
    [TestCase(17, 2, false, null, TestName = "NullValue")]
    [TestCase(17, 2, false, "", TestName = "EmptyString")]
    [TestCase(17, 2, false, "   \t\n    \r  ", TestName = "WhitespaceOnly")]
    [TestCase(17, 2, false, "\t5498", TestName = "NumberWithWhitespaceInStart")]
    [TestCase(17, 2, false, "54\t98", TestName = "NumberWithWhitespaceInMiddle")]
    [TestCase(3, 1, true, "1.23", TestName = "ScaleLessThanDecimalNumber")]
    [TestCase(4, 2, true, "12..3", TestName = "TwoDotsInNumber")]
    [TestCase(4, 2, true, ".1", TestName = "DemicalNubmerWithoutIntegerPart")]
    [TestCase(4, 2, true, "1.", TestName = "DecimalNumberWithoutDecimalPart")]
    [TestCase(4, 0, true, "1.2", TestName = "DemicalNumberWithZeroScale")]
    [TestCase(5, 2, true, "-12.34", TestName = "PrecisionLessThanNegativeDecimalNumber")]
    public void IsValidNumber_ShouldReturnFalse_WhenInvalidNumber(int precision, int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        validator.IsValidNumber(value).Should().BeFalse();
    }
}