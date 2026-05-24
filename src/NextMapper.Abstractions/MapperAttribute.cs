namespace NextMapper.Abstractions;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class MapperAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public class MapMemberAttribute : Attribute
{
    public string Source { get; }
    public string Target { get; }

    public MapMemberAttribute(string source, string target)
    {
        Source = source;
        Target = target;
    }
}
