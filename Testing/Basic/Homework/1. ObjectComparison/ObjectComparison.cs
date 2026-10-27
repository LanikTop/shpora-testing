using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.ObjectComparison;
public class ObjectComparison
{
    [Test]
    [Description("Проверка текущего царя")]
    [Category("ToRefactor")]
    public void CheckCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // В документации FluentAssertions нашёл метод BeEquivalentTo, который умеет сравнивать графы объектов.
        // https://fluentassertions.com/objectgraphs/
        actualTsar.Should().BeEquivalentTo(expectedTsar);
        // Он рекурсивно сравнивает все поля объектов, включая вложенные объекты.
        // Поэтому, если добавить ещё поля или изменить названия старых полей, то тест всё равно будет работать,
        // если значения полей совпадают.
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));

        // Ну, как минимум, если добавить ещё поля, то тест сломается.
        // Нужно будет вручную добавлять новые if проверки в метод AreEqual
        // Но, если лишь менять названия старых полей, то ошибок не будет, если IDE переименует их во всех местах, включая метод AreEqual.
    }

    private bool AreEqual(Person? actual, Person? expected)
    {
        if (actual == expected) return true;
        if (actual == null || expected == null) return false;
        return
            actual.Name == expected.Name
            && actual.Age == expected.Age
            && actual.Height == expected.Height
            && actual.Weight == expected.Weight
            && AreEqual(actual.Parent, expected.Parent);
    }
}
