using System.Threading.Tasks;

public interface INotifcation
{
    public Task NotifyEntity();

    public Task UpdateNotifyList();
}