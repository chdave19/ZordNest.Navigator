namespace ZordNest.Navigator;

public interface INavigableViewModel : IDisposable
{
     
     void Created(string route);
     void Resumed(string route);
     void Paused();
     void ResolveQueryParams(Dictionary<string, object>? queryParams);
}