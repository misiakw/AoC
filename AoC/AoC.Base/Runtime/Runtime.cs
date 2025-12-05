using System;
using System.Linq;
using System.Reflection;
using AoC.Base.Abstraction;
using Microsoft.Extensions.DependencyInjection;
namespace AoC.Base.Runtime;

public class Runtime
{
    IServiceProvider _serviceProvider;
    public Runtime(Action<ServiceCollection> registerServices = null)
    {
        ServiceCollection collection = new ServiceCollection();
        RegisterDays(collection);
        if(registerServices != null)
            registerServices.Invoke(collection);

        _serviceProvider = collection.BuildServiceProvider();
    }

    public Runtime RunDay(int dayNum)
    {
        var day = _serviceProvider.GetRequiredKeyedService<IDay>(dayNum);
        day.RunAoC();
        return this;
    }

    private void RegisterDays(ServiceCollection services)
    {
        var assembly = Assembly.GetEntryAssembly();
        foreach (var dayType in assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IDay)) && !t.IsAbstract))
        {
            var dayNumAttr = dayType.GetCustomAttribute<DayNumAttribute>();
            int dayNum = dayNumAttr?.DayNum ?? int.Parse(dayType.Name.Substring(3));
            
            services.AddKeyedScoped(typeof(IDay), dayNum, dayType);
        }
    }
}