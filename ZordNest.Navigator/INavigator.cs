namespace ZordNest.Navigator;

public interface INavigator
{
    bool NavigateTo(string key);
    bool AddToStack(string key, Dictionary<string, object>? queryParams);
    bool CanGoBack { get; }
    event EventHandler<object> CurrentScreenEvent;
    void ClearStack();
    bool PopStack();
}