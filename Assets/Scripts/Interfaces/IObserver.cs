

using System.Threading;

public interface IObserver<in T>
{
    public abstract void OnNotify(T data, NotificationContext notificationContext, SemaphoreSlim semaphoreSlim, params object[] optional);

}
public interface IExtendedObserver<T, Y, Z>
{
    public abstract void OnNotify(T Data, Z value1, Y value2);
}

/// <summary>
/// Represents a subject for asynchronous observer pattern
/// Allows us to use ping the same type of subject with different values type
/// And helps us differentiate what type of subject we want, and what value we are seeking from that subject
/// <typeparam name="T">The type of Implementation for the subject/typeparam>
/// <typeparam name="Z">The data we want from the subject</typeparam>
/// </summary>
public interface IObserver<in T, in Z> 
{
    public abstract void OnNotify(Z data, NotificationContext context, SemaphoreSlim semaphoreSlim, params object[] optional);

    /// <summary>
    /// Gets called when a subject broadcasts its unique key.
    /// </summary>
    /// <param name="key">The key associated with a subject</param>
    public abstract void OnKeyNotify(string key, NotificationContext context, SemaphoreSlim semaphoreSlim, params object[] optional);
}

/// <summary>
/// Represents a subject for asynchronous observer pattern
/// Allows us to use ping the same type of subject with different values type
/// And helps us differentiate what type of subject we want, and what value we are seeking from that subject
/// <typeparam name="T">The type of Implementation for the subject/typeparam>
/// <typeparam name="Z">The data we want from the subject</typeparam>
/// <typeparam name="X">For casting to X</typeparam>
/// </summary>
public interface IObserver<in T, in Z, X>
{
    public abstract void OnNotify(Z data, NotificationContext context, SemaphoreSlim semaphoreSlim, params object[] optional);

    /// <summary>
    /// Gets called when a subject broadcasts its unique key.
    /// </summary>
    /// <param name="key">The key associated with a subject</param>
    public abstract void OnKeyNotify(string key, NotificationContext context, SemaphoreSlim semaphoreSlim, params object[] optional);

    public X castTo(T data);
}