using Microsoft.Extensions.DependencyInjection;

namespace ZordNest.Navigator;

public static class RegisterNavigatorViewModelStore
{
    public static IServiceCollection AddNavigableViewModelStore(this IServiceCollection collection)
    {
        collection.AddSingleton<NavigatorViewModelStore>();
        collection.AddTransient<INavigator, Navigator>();
        return collection;
    }
}