using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace ZordNest.Navigator;

public class NavigatorViewModelStore
{
    private static ConcurrentDictionary<string,(object? instance, Type type, IServiceScope? scope)>? _cachedViewModels;
    private static IServiceProvider? _servicesProvider;
    public NavigatorViewModelStore(IServiceProvider provider)
    {
        _servicesProvider = provider;
    }
    public static void RegisterViewModel<T>(string key) where T : INavigableViewModel
    {
        _cachedViewModels ??= new();
        _cachedViewModels.TryAdd(key, (null, typeof(T), null));
    }
    public object? ResolveViewModel(string key)
    {
        if (_cachedViewModels is not null && _cachedViewModels.TryGetValue(key, out var viewModel))
        {
            var type = viewModel.type;
            if (viewModel.instance is null)
            {
                var scope = _servicesProvider?.CreateScope();
                viewModel.instance ??= scope?.ServiceProvider.GetRequiredService(type);
                (viewModel.instance as INavigableViewModel)?.Created(key);
                _cachedViewModels[key] = (viewModel.instance, type, scope);
            }
            (viewModel.instance as INavigableViewModel)?.Resumed(key);
            return viewModel.instance;   
        }
        return null;
    }
    public bool EjectViewModel(string key)
    {
        if (_cachedViewModels?.TryGetValue(key, out var viewModel) ?? false)
        {
            var viewModelInstance = viewModel.instance as INavigableViewModel;
            viewModel.scope?.Dispose();
            _cachedViewModels[key] = (null, viewModel.type, null);
            var lengthOfParentRoute = key.Length;
            var subRoutes = _cachedViewModels
                .Where(viewModelKeyPairs => viewModelKeyPairs.Key.Length >
                    lengthOfParentRoute && viewModelKeyPairs.Key.StartsWith(key));
            foreach (var viewModelsKeyPairs in subRoutes)
            {
                viewModelsKeyPairs.Value.scope?.Dispose();
                _cachedViewModels[viewModelsKeyPairs.Key] = (null, viewModelsKeyPairs.Value.type, null);
            }
            return true;
        }
        return false;
    }
}