using System;
using System.Threading;
using System.Threading.Tasks;
public interface ISubjectAsync<T>
{
    public Task OnNotifySubject(T data, params object[] optional);
}

//in - contravariance allows you to pass generic types where a derived type is expected - (only for input arguments)
//out - covariance allows you to pass dervied types where base type is expected (for return types)
public interface ISubject<in T>
{
    public void OnNotifySubject(T observer, NotificationContext notificationContext, CancellationToken cancellationToken, SemaphoreSlim semaphoreSlim, params object[] optional);
}

public interface ISubjectAsync
{
    public Task OnNotifySubject(params object[] optional);
}