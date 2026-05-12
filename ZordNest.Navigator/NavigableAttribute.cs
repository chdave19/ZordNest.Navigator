namespace ZordNest.Navigator;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class NavigableAttribute : Attribute
{
    public string Route { get; init; }
    public NavigableAttribute(string route)
    {
        Route = route;
    }
    
}