// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Reflection;
using Evoogle.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Evoogle;

/// <summary>
///     Extension methods for the <see cref="IServiceCollection"/> that help in configuration of dependency injection.
/// </summary>
public static class ServiceCollectionExtensions
{
    #region Extension Methods
    /// <summary>
    ///     Add singleton implementations that inherit directly from respective interface type.
    /// </summary>
    /// <param name="serviceCollection">Service collection to add the singleton implementations to.</param>
    /// <param name="interfaceType">Type of interface of the singleton implementation.</param>
    /// <param name="assemblyCollection">Assemblies to reflect for possible implementations of the given interface.</param>
    /// <param name="useOpenGenericImplementation">Predicate that controls if the implementation should be an open generic or not.</param>
    public static void AddSingletonImplementations(
        this IServiceCollection serviceCollection,
        Type interfaceType,
        IEnumerable<Assembly> assemblyCollection,
        bool useOpenGenericImplementation = false)
    {
        AddImplementations(serviceCollection,
                           interfaceType,
                           assemblyCollection,
                           useOpenGenericImplementation,
                           (x, y, z) => x.AddSingleton(y, z));
    }

    /// <summary>
    ///     Add singleton implementations that inherit directly from respective interface type.
    /// </summary>
    /// <typeparam name="TInterface">Type of interface of the singleton implementation.</typeparam>
    /// <param name="serviceCollection">Service collection to add the singleton implementations to.</param>
    /// <param name="assemblyCollection">Assemblies to reflect for possible implementations of the given interface.</param>
    /// <param name="useOpenGenericImplementation">Predicate that controls if the implementation should be an open generic or not.</param>
    public static void AddSingletonImplementations<TInterface>(
        this IServiceCollection serviceCollection,
        IEnumerable<Assembly> assemblyCollection,
        bool useOpenGenericImplementation = false)
    {
        AddImplementations<TInterface>(serviceCollection,
                                       assemblyCollection,
                                       useOpenGenericImplementation,
                                       (x, y, z) => x.AddSingleton(y, z));
    }

    /// <summary>
    ///     Add scoped implementations that inherit directly from respective interface type.
    /// </summary>
    /// <param name="serviceCollection">Service collection to add the scoped implementations to.</param>
    /// <param name="interfaceType">Type of interface of the scoped implementation.</param>
    /// <param name="assemblyCollection">Assemblies to reflect for possible implementations of the given interface.</param>
    /// <param name="useOpenGenericImplementation">Predicate that controls if the implementation should be an open generic or not.</param>
    public static void AddScopedImplementations(
        this IServiceCollection serviceCollection,
        Type interfaceType,
        IEnumerable<Assembly> assemblyCollection,
        bool useOpenGenericImplementation = false)
    {
        AddImplementations(serviceCollection,
                           interfaceType,
                           assemblyCollection,
                           useOpenGenericImplementation,
                           (x, y, z) => x.AddScoped(y, z));
    }

    /// <summary>
    ///     Add scoped implementations that inherit directly from respective interface type.
    /// </summary>
    /// <typeparam name="TInterface">Type of interface of the scoped implementation.</typeparam>
    /// <param name="serviceCollection">Service collection to add the scoped implementations to.</param>
    /// <param name="assemblyCollection">Assemblies to reflect for possible implementations of the given interface.</param>
    /// <param name="useOpenGenericImplementation">Predicate that controls if the implementation should be an open generic or not.</param>
    public static void AddScopedImplementations<TInterface>(
        this IServiceCollection serviceCollection,
        IEnumerable<Assembly> assemblyCollection,
        bool useOpenGenericImplementation = false)
    {
        AddImplementations<TInterface>(serviceCollection,
                                       assemblyCollection,
                                       useOpenGenericImplementation,
                                       (x, y, z) => x.AddScoped(y, z));
    }

    /// <summary>
    ///     Add transient implementations that inherit directly from respective interface type.
    /// </summary>
    /// <param name="serviceCollection">Service collection to add the transient implementations to.</param>
    /// <param name="interfaceType">Type of interface of the transient implementation.</param>
    /// <param name="assemblyCollection">Assemblies to reflect for possible implementations of the given interface.</param>
    /// <param name="useOpenGenericImplementation">Predicate that controls if the implementation should be an open generic or not.</param>
    public static void AddTransientImplementations(
        this IServiceCollection serviceCollection,
        Type interfaceType,
        IEnumerable<Assembly> assemblyCollection,
        bool useOpenGenericImplementation = false)
    {
        AddImplementations(serviceCollection,
                           interfaceType,
                           assemblyCollection,
                           useOpenGenericImplementation,
                           (x, y, z) => x.AddTransient(y, z));
    }

    /// <summary>
    ///     Add transient implementations that inherit directly from respective interface type.
    /// </summary>
    /// <typeparam name="TInterface">Type of interface of the transient implementation.</typeparam>
    /// <param name="serviceCollection">Service collection to add the transient implementations to.</param>
    /// <param name="assemblyCollection">Assemblies to reflect for possible implementations of the given interface.</param>
    /// <param name="useOpenGenericImplementation">Predicate that controls if the implementation should be an open generic or not.</param>
    public static void AddTransientImplementations<TInterface>(
        this IServiceCollection serviceCollection,
        IEnumerable<Assembly> assemblyCollection,
        bool useOpenGenericImplementation = false)
    {
        AddImplementations<TInterface>(serviceCollection,
                                       assemblyCollection,
                                       useOpenGenericImplementation,
                                       (x, y, z) => x.AddTransient(y, z));
    }
    #endregion

    #region Implementation Methods
    private static void AddImplementations(
        IServiceCollection serviceCollection,
        Type interfaceType,
        IEnumerable<Assembly> assemblyCollection,
        bool useOpenGenericImplementation,
        Action<IServiceCollection, Type, Type> addAction)
    {
        var interfaceTypeName = interfaceType.Name;

        var implementationTypes = (assemblyCollection ?? Enumerable.Empty<Assembly>())
                                  .SelectMany(x => x.GetTypes())
                                  .ToList();
        foreach (var implementationType in implementationTypes)
        {
            var isAbstract = implementationType.IsAbstract;
            if (isAbstract)
                continue;

            var isImplementationOf = TypeReflection.IsImplementationOf(implementationType, interfaceType);
            if (!isImplementationOf)
                continue;

            if (!interfaceType.IsGenericType)
            {
                addAction(serviceCollection, interfaceType, implementationType);
                continue;
            }

            if (useOpenGenericImplementation && implementationType.IsGenericType)
            {
                addAction(serviceCollection, interfaceType, implementationType);
                continue;
            }

            var interfaceTypeClosed = implementationType.GetInterfaces()
                                                        .SingleOrDefault(x => x.Name == interfaceTypeName);
            if (interfaceTypeClosed == null)
                continue;

            addAction(serviceCollection, interfaceTypeClosed, implementationType);
        }
    }

    private static void AddImplementations<TInterface>(
        IServiceCollection serviceCollection,
        IEnumerable<Assembly> assemblyCollection,
        bool useOpenGenericImplementation,
        Action<IServiceCollection, Type, Type> addAction)
    {
        var interfaceType = typeof(TInterface);

        AddImplementations(serviceCollection,
                           interfaceType,
                           assemblyCollection,
                           useOpenGenericImplementation,
                           addAction);
    }
    #endregion
}