using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    // Проверка конструктора NumberValidator.
    [TestCase(-1, TestName = "NegativePrecision")]
    [TestCase(0, TestName = "ZeroPrecision")]
    [TestCase(int.MinValue, TestName = "MinimumIntPrecision")]
    public void Constructor_ThrowException_WhenInvalidPrecision(int precision)
    {
        var act = () => new NumberValidator(precision);
        act.Should().Throw<ArgumentException>();
    }

    [TestCase(2, -1, TestName = "NegativeScale")]
    [TestCase(2, 2, TestName = "ScaleSamePrecision")]
    [TestCase(2, 3, TestName = "ScaleGreaterThenPrecision")]
    public void Constructor_ThrowException_WhenInvalidScale(int precision, int scale)
    {
        var act = () => new NumberValidator(precision, scale);
        act.Should().Throw<ArgumentException>();
    }

    // Проверка метода IsValidNumber.
    [TestCase(3, 0, true, "123", TestName = "PositiveInteger")]
    [TestCase(3, 0, false, "123", TestName = "PositiveIntegerWithoutOnlyPositive")]
    [TestCase(4, 0, true, "+123", TestName = "PositiveIntegerWithPlus")]
    [TestCase(4, 0, false, "-123", TestName = "NegativeInteger")]
    [TestCase(20, 0, true, "8580101374", TestName = "HugeIntValue")]
    [TestCase(1, 0, true, "0", TestName = "Zero")]
    [TestCase(4, 0, false, "0000", TestName = "Zeros")]
    [TestCase(2, 0, true, "+0", TestName = "ZeroWithPlus")]
    [TestCase(4, 0, true, "0123", TestName = "IntegerWithLeadingZeros")]
    [TestCase(5, 0, true, "+0123", TestName = "IntegerWithLeadingZerosAndPlus")]

    [TestCase(3, 2, true, "0.0", TestName = "ZeroWithFractionalZero")]
    [TestCase(4, 2, true, "00.00", TestName = "ZerosWithFractionalZeros")]
    [TestCase(6, 2, true, "123.98", TestName = "PositiveFractional")]
    [TestCase(6, 2, true, "+123.98", TestName = "PositiveFractionalWithPlus")]
    [TestCase(6, 2, false, "-123.98", TestName = "NegativeFractional")]
    [TestCase(15, 10, true, "+123.000", TestName = "FractionalWithOnlyInsignificantZeros")]
    [TestCase(40, 20, false, "4823942349.9812412", TestName = "HugeFractional")]
    [TestCase(15, 10, true, "+123.980000", TestName = "FractionalWithInsignificantZeros")]

    [TestCase(6, 2, false, "1,23", TestName = "PositiveFractionalWithComma")]
    [TestCase(6, 2, true, "+1,23", TestName = "PositiveFractionalWithCommaAndPlus")]
    [TestCase(6, 2, false, "-1,23", TestName = "NegativeFractionalWithComma")]
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
    [TestCase(3, 2, true, "a.sd", TestName = "LettersInFormatFractionalNumber")]
    [TestCase(3, 2, true, "num12.34", TestName = "LettersWithFractionalNumber")]
    [TestCase(3, 2, true, "+1.23", TestName = "PrecisionLessThanFractionalNumberWithPlus")]
    [TestCase(3, 2, false, "-1.23", TestName = "PrecisionLessThanFractionalNegativeNumber")]
    [TestCase(17, 2, false, null, TestName = "NullValue")]
    [TestCase(17, 2, false, "null", TestName = "NullValueAsString")]
    [TestCase(17, 2, false, "", TestName = "EmptyString")]
    [TestCase(17, 2, false, "   \t\n    \r  ", TestName = "WhitespaceOnly")]
    [TestCase(17, 2, false, " 5498", TestName = "NumberWithWhitespaceInStart")]
    [TestCase(17, 2, false, "54 98", TestName = "NumberWithWhitespaceInMiddle")]
    [TestCase(3, 1, true, "1.23", TestName = "ScaleLessThanFractionalNumber")]
    [TestCase(4, 2, true, "12..3", TestName = "TwoDotsInNumber")]
    [TestCase(4, 2, true, ".1", TestName = "FractionalNubmerWithoutIntegerPart")]
    [TestCase(4, 2, true, "1.", TestName = "FractionalNumberWithoutFractionalPart")]
    [TestCase(4, 0, true, "1.2", TestName = "FractionalNumberWithZeroScale")]
    [TestCase(5, 2, true, "-12.34", TestName = "PrecisionLessThanNegativeFractionalNumber")]

    [TestCase(5, 2, false, "XII", TestName = "RomanNumber")]
    [TestCase(5, 2, false, "10*2", TestName = "ExpressionInsteadOfNumber")]
    [TestCase(5, 2, false, "10:02", TestName = "TimeInsteadOfNumber")]
    [TestCase(20, 2, false, "0b1111011", TestName = "BinaryNumber")]
    [TestCase(5, 2, false, "0x7B", TestName = "HexNumber")]
    [TestCase(10, 0, true, "++123", TestName = "PositiveIntegerWithManyPluses")]
    [TestCase(10, 4, true, "123.,98", TestName = "PositiveFractionalWithDotAndComma")]
    public void IsValidNumber_ShouldReturnFalse_WhenInvalidNumber(int precision, int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        validator.IsValidNumber(value).Should().BeFalse();
    }
}