namespace ZordNest.Navigator;

public class Navigator(NavigatorViewModelStore store) : INavigator
{
    private string _currentRoute = string.Empty;
    public event EventHandler<object>? CurrentScreenEvent;
    private Stack<(string key, object viewModel)> BackStack { get; } = new();
    private int StackCount => BackStack.Count;
    public bool CanGoBack => StackCount > 1;
    private const int MaxStackSize = 10;
    
    ///<summary>
    ///Use this only for top level routes because it will clear the stack and keep only one screen in the stack.
    ///</summary>
    public bool NavigateTo(string key)
    {
        if (_currentRoute == key) return false;
        var viewModel = store.ResolveViewModel(key);
        if (viewModel is not null)
        {
            _currentRoute = key;
            CurrentScreenEvent?.Invoke(this, viewModel);
            if(StackCount > 0) 
                (BackStack.FirstOrDefault().viewModel as INavigableViewModel)?.Paused();
            BackStack.Clear();
            BackStack.Push((key, viewModel));
            return true;
        }
        return false;
    }

    ///<summary>
    ///Will add a new screen untop the stack.
    ///</summary>
    public bool AddToStack(string key, Dictionary<string, object>? queryParams)
    {
        if (StackCount >= MaxStackSize) return false;
        var viewModel = store.ResolveViewModel(key);
        if (viewModel is null) return false;
        (BackStack.Last().viewModel as INavigableViewModel)?.Paused();
        CurrentScreenEvent?.Invoke(this, viewModel);
        (viewModel as INavigableViewModel)?.ResolveQueryParams(queryParams);
        BackStack.Push((key, viewModel));
        _currentRoute = key;
        return false;
    }
    
    ///<summary>
    ///Will remove the screen ontop the stack
    ///</summary>
    public bool PopStack()
    {
        if (!CanGoBack) return false;
        var clearedScreen = BackStack.Pop();
        store.EjectViewModel(clearedScreen.key);
        var currentScreen = BackStack.Last();
        CurrentScreenEvent?.Invoke(this, currentScreen.viewModel);
        _currentRoute = currentScreen.key;
        return true;
    }

    public void ClearStack()
    {
        foreach (var screen in BackStack)
        {
            store.EjectViewModel(screen.key);
        }
        BackStack.Clear();
    }
}