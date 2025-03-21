using System.Threading;
using UnityEngine;

public class CelestialBodiesLightPackageGenerator : MonoBehaviour, IObserver<MonoBehaviour, ILightPreprocess>, IObserver<LightPackage>
{
    [SerializeField]
    LightPackageDelegator lightPackageDelegator;
    LightPreprocessDelegatorManager lightPreprocessDelegatorManager;

    private ILightPreprocess celestialBodyLightningPreprocess;

    private string CelestialBodyLightningUniqueKey { get; set; }

    private void Start()
    {
        StartCoroutine(lightPreprocessDelegatorManager.LightPreprocessDelegator.NotifyWhenActive(this, new NotificationContext()
        {
            GameObject = gameObject,
            GameObjectName = gameObject.name,
            GameObjectTag = gameObject.tag,    
        }));
    }

    public void OnNotify(LightPackage data, NotificationContext notificationContext, SemaphoreSlim semaphoreSlim, params object[] optional)
    {
        throw new System.NotImplementedException();
    }

    public void OnKeyNotify(string key, NotificationContext context, SemaphoreSlim semaphoreSlim, params object[] optional)
    {
        CelestialBodyLightningUniqueKey = key;

        //CelestialBodiesLightPackageGenerator can be casted to Monobehavior since it inherits from it
        //just be aware that the observer gets it properly
        StartCoroutine(lightPreprocessDelegatorManager.LightPreprocessDelegator.NotifySubject(CelestialBodyLightningUniqueKey, this));
    }

    public void OnNotify(ILightPreprocess data, NotificationContext context, SemaphoreSlim semaphoreSlim, params object[] optional)
    {
        celestialBodyLightningPreprocess = data;
    }
}