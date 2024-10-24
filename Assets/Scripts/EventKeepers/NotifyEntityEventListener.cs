using System.Threading.Tasks;
using UnityEngine.Events;
public class NotifyEntityListenerEvent : UnityEventWTAsync<NotifyEntity>
{
    private UnityEvent<NotifyEntity> m_notifyEntityEvent = new UnityEvent<NotifyEntity>();
    public override UnityEvent<NotifyEntity> GetInstance()
    {
        return m_notifyEntityEvent;
    }
    public override Task AddListener(UnityAction<NotifyEntity> action)
    {
        m_notifyEntityEvent.AddListener(action);

        return Task.CompletedTask;
    }
    public override Task Invoke(NotifyEntity value)
    {
        m_notifyEntityEvent.Invoke(value);

        return Task.CompletedTask;
    }
}