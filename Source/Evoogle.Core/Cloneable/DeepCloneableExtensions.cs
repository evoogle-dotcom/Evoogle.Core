// Copyright (c) 2024 Evoogle.com
// Licensed under the MIT License. See License.txt in the project root for license information.
namespace Evoogle.Cloneable;

/// <summary>
///     Extension methods for any object that implements the <see cref="IDeepCloneable"/> interface.
/// </summary>
public static class DeepCloneableExtensions
{
    #region Extension Methods
    /// <summary>
    ///     Create a deep copy from the source deep cloneable object.
    /// </summary>
    /// <typeparam name="T">Type of object to create a deep copy of.</typeparam>
    /// <param name="source">Source object to create a deep copy of.</param>
    /// <returns>Deep copy of the source object.</returns>
    /// <exception cref="InvalidOperationException">The created deep copy of the source object is null.</exception>
    /// <exception cref="InvalidCastException">The created deep clone could not be cast to the generic type parameter.</exception>
    public static T DeepCopy<T>(this T source)
        where T : IDeepCloneable
    {
        var clone = (source.DeepClone()) ?? throw new InvalidOperationException($"{nameof(source.DeepClone)} returned a null object.");
        return (T)clone;
    }

    /// <summary>
    ///     Create a collection of deep copy objects from a source collection of deep cloneable objects.
    /// </summary>
    /// <typeparam name="T">Type of object in the collection to create a deep copy of.</typeparam>
    /// <param name="sourceCollection">Source collection to create a deep copy of.</param>
    /// <returns>Collection of deep copy objects of the source collection.</returns>
    /// <exception cref="InvalidOperationException">Any created deep copy collection item of the source collection item is null.</exception>
    /// <exception cref="InvalidCastException">The created deep clone collection item could not be cast to the generic type parameter.</exception>
    public static IEnumerable<T> DeepCopyRange<T>(this IEnumerable<T> sourceCollection)
        where T : IDeepCloneable
    {
        return DeepCopyRangeInner();

        IEnumerable<T> DeepCopyRangeInner()
        {
            foreach (var source in sourceCollection)
            {
                var clone = (source.DeepClone()) ?? throw new InvalidOperationException($"{nameof(source.DeepClone)} returned a null object.");
                yield return (T)clone;
            }
        }
    }

    /// <summary>
    ///     Create a deep copy from a deep cloneable object.
    /// </summary>
    /// <typeparam name="T">Type of object to create a deep copy of.</typeparam>
    /// <param name="source">Source object to create a deep copy of.</param>
    /// <returns>Null if the source object is null or the created deep clone is null, otherwise a deep copy of the source object.</returns>
    public static T? SafeDeepCopy<T>(this T? source)
        where T : IDeepCloneable
    {
        var clone = source?.DeepClone();
        if (clone == null)
            return default;

        return (T)clone;
    }

    /// <summary>
    ///     Create a collection of deep copy objects from a source collection of deep cloneable objects.
    /// </summary>
    /// <typeparam name="T">Type of object in the collection to create a deep copy of.</typeparam>
    /// <param name="sourceCollection">Source collection to create a deep copy of.</param>
    /// <returns>Collection of deep copy objects of the source collection.</returns>
    /// <returns>Null if the source collection is null, otherwise a deep copy collection of the source collection.</returns>
    public static IEnumerable<T?> SafeDeepCopyRange<T>(this IEnumerable<T?> sourceCollection)
        where T : IDeepCloneable
    {
        if (sourceCollection != null)
        {
            foreach (var source in sourceCollection)
            {
                // Check for possible item null reference.
                if (source != null)
                {
                    yield return source.DeepCopy();
                }
                else
                {
                    yield return default;
                }
            }
        }
        else
        {
            yield return default;
        }
    }
    #endregion
}