using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace G4mvc.TestBase.Extensions;

public static class CollectionAssertExtensions
{
    public static void AreDictionariesEquivalent<TKey, TValue>(this CollectionAssert _, object[]? expected, IReadOnlyDictionary<TKey, TValue>? actual)
    {
        var expectedIsNull = expected is null;
        var actualIsNull = actual is null;

        if (expectedIsNull != actualIsNull)
        {
            Assert.Fail($"Expected <{(expectedIsNull ? null : "not ")}null>, actually got <{(actualIsNull ? null : "not ")}null>");
        }

        if (expectedIsNull && actualIsNull)
        {
            return;
        }

        Debug.Assert(expected is not null);
        Debug.Assert(actual is not null);

        Assert.AreEqual(expected.Length / 2, actual.Count, $"Expected count <{expected.Length / 2}>, actual count is <{actual.Count}>");

        for (var i = 0; i < expected.Length; i += 2)
        {
            var expectedKey = (TKey)expected[i];
            var expectedValue = (TValue)expected[i + 1];

            Assert.IsTrue(actual.TryGetValue(expectedKey, out var actualValue), $"Expected key <{expectedKey}> does not exist in actual!");

            Assert.AreEqual(expectedValue, actualValue, $"Expected value <{expectedValue}> for key <{expectedKey}> does not match actual value <{actualValue}>");
        }
    }
}
