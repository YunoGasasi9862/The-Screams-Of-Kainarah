using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[Asset(AssetType = Asset.MONOBEHAVIOR, AddressLabel = "AsyncCoroutine")]
public class AsyncCoroutine : MonoBehaviour, IAsyncCoroutine<WaitForSeconds>, IAsyncCoroutine<WaitUntil>, ISubject<IObserver<AsyncCoroutine>>
{
    private AsyncCoroutineDelegator m_asyncCoroutineDelegator;
    private void Start()
    {
        m_asyncCoroutineDelegator = Helper.GetDelegator<AsyncCoroutineDelegator>();

        m_asyncCoroutineDelegator.Subject.SetSubject(this);
    }

    public async Task ExecuteAsyncCoroutine(IAsyncEnumerator<WaitForSeconds> asyncCoroutine)
    {
        while (await asyncCoroutine.MoveNextAsync())
        {
            await Task.Yield();
        }
    }

    public async Task ExecuteAsyncCoroutine(IAsyncEnumerator<WaitUntil> asyncCoroutine)
    {
        while (await asyncCoroutine.MoveNextAsync()) //checks if the coroutine i have passed has an element/next item to process, if so it yields it (with await)
        {
            await Task.Yield();
        }
    }

    public void OnNotifySubject(IObserver<AsyncCoroutine> data, NotificationContext notificationContext, CancellationToken cancellationToken, SemaphoreSlim semaphoreSlim, params object[] optional)
    {
        StartCoroutine(m_asyncCoroutineDelegator.NotifyObserver(data, this, notificationContext, cancellationToken: cancellationToken));
    }

}