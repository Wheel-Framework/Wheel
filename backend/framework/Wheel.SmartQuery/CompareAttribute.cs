namespace Wheel.SmartQuery;

[AttributeUsage(AttributeTargets.Property)]
public class CompareAttribute : Attribute
{
    public CompareAttribute(CompareType compareType)
    {
        CompareType = compareType;
    }

    public CompareAttribute(CompareType compareType, string compareProperty) : this(compareType)
    {
        CompareProperty = compareProperty;
    }

    public CompareType CompareType { get; set; }

    public CompareSite CompareSite { get; set; } = CompareSite.LEFT;

    public string? CompareProperty { get; set; }
}
