using System.Threading;
using UnityEngine;

public class PlayerAttributesNotifier: MonoBehaviour, ISubject<IObserver<Transform>>
{
    private Transform PlayerTransform { get; set; }

    private PlayerAttributesDelegator PlayerAttributesDelegator { get; set; }

    private void OnEnable()
    {
        PlayerTransform = GetComponent<Transform>();

        PlayerAttributesDelegator = Helper.GetDelegator<PlayerAttributesDelegator>();
    }

    private void Start()
    {
        PlayerAttributesDelegator.AddToSubjectsDict(typeof(PlayerAttributesNotifier).ToString(), gameObject.name, new Subject<IObserver<Transform>>());

        PlayerAttributesDelegator.GetSubsetSubjectsDictionary(typeof(PlayerAttributesNotifier).ToString())[gameObject.name].SetSubject(this);
    }

    public void OnNotifySubject(IObserver<Transform> data, NotificationContext notificationContext, CancellationToken cancellationToken, SemaphoreSlim semaphoreSlim, params object[] optional)
    {
        StartCoroutine(PlayerAttributesDelegator.NotifyObserver(data, PlayerTransform, notificationContext, cancellationToken, semaphoreSlim));
    }
}