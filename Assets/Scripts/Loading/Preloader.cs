using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Preloader: MonoBehaviour, IPreloadWithAction, IPreloadWithGenericAction
{
    private GameLoad PooledGameLoad { get; set; }
    private EntityPool<GameObject> EntityPool { get; set; }
    private EntityPoolManager EntityPoolManagerReference { get; set; }

    private async void Awake()
    {
        await InitializePoolObjects();
    }

    public Task ExecuteAction<TAction>(Action<TAction> action, TAction value)
    {
        action.Invoke(value);

        return Task.CompletedTask;
    }

    public Task ExecuteGenericAction(Action action)
    {
        action.Invoke();

        return Task.CompletedTask;
    }

    public async Task<UnityEngine.Object> PreloadAsset<T>(AssetReference assetReference, EntityType entityType)
    {
        Debug.Log($"Pooled Game Object: {PooledGameLoad}");

         return await PooledGameLoad.PreloadAsset<T>(assetReference, entityType);
    }

    private async Task InitializePoolObjects()
    {
        EntityPoolManagerReference = SceneSingleton.EntityPoolManager;

        EntityPool = await EntityPoolManagerReference.GetPooledEntity(Constants.GAME_PRELOAD) as EntityPool<GameObject>;

        Debug.Log($"Entity Pool {EntityPool}");

        if (EntityPool.Entity.GetComponent<GameLoad>() == null)
        {
            throw new ApplicationException("Game Load Not Found!");
        }

        PooledGameLoad = EntityPool.Entity.GetComponent<GameLoad>();
    }
}