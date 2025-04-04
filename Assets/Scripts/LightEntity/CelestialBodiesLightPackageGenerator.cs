using System;
using System.Threading;
using UnityEngine;

[ObserverSystem(SubjectType = typeof(CelestialBodyLightning), ObserverType = typeof(CelestialBodiesLightPackageGenerator))]
[ObserverSystem(SubjectType = typeof(CelestialBodiesLightPackageGenerator), ObserverType = typeof(CustomLightProcessing))]
public class CelestialBodiesLightPackageGenerator : MonoBehaviour, IObserver<ILightPreprocess>, ISubject<IObserver<LightPackage>>
{
    [SerializeField]
    LightPackageDelegator lightPackageDelegator;
    [SerializeField]
    LightPreprocessDelegator lightPreprocessDelegator;

    private ILightPreprocess celestialBodyLightningPreprocess;

    private void Start()
    {

        StartCoroutine(lightPreprocessDelegator.NotifySubject(this, new NotificationContext()
        {
            ObserverName = gameObject.name,
            ObserverTag = gameObject.tag,
            SubjectType = typeof(CelestialBodyLightning).ToString()
        }));

        //subject for custom lightning
        lightPackageDelegator.AddToSubjectsDict(gameObject.tag, new Subject<IObserver<LightPackage>>());

        lightPackageDelegator.GetSubject(gameObject.tag).SetSubject(this);
    }

    public void OnNotify(ILightPreprocess data, NotificationContext context, SemaphoreSlim semaphoreSlim, params object[] optional)
    {
        celestialBodyLightningPreprocess = data;

        Debug.Log($"Got the instance! {data}");
    }

    public void OnNotifySubject(IObserver<LightPackage> data, NotificationContext notificationContext, params object[] optional)
    {
        Debug.Log("From Custom Lightning");
    }
}