using System.Threading.Tasks;
public interface ISubjectAsync<T>
{
    public Task OnNotifySubject(T data, params object[] optional);
}

public interface ISubject<T>
{
    public void OnNotifySubject(T data, NotificationContext notificationContext, params object[] optional);
}

public interface ISubjectAsync
{
    public Task OnNotifySubject(params object[] optional);
}