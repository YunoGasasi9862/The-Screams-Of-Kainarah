using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

public class EntityPoolManager: MonoBehaviour, IEntityPool
{
    [SerializeField]
    EntityPoolEvent entityPoolEvent;
    [SerializeField]
    EntityPoolManagerActiveEvent entityPoolManagerActiveEvent;

    private Dictionary<string, AbstractEntityPool> entityPoolDict = new Dictionary<string, AbstractEntityPool>();

    private void OnEnable()
    {
        entityPoolManagerActiveEvent.Invoke(this);

        entityPoolEvent.AddListener(InvokeEntityPool);
    }

    public Task Pool(AbstractEntityPool entityPool)
    {
        entityPoolDict.Add(entityPool.Tag, entityPool);

        Debug.Log($"Pool county {entityPoolDict.Count}");

        return Task.CompletedTask;
    }
    public Task UnPool(string tag)
    {
        if (entityPoolDict.TryGetValue(tag, out AbstractEntityPool entityPool))
        {
            entityPoolDict.Remove(tag);
        }

        return Task.CompletedTask;
    }
    public async Task<AbstractEntityPool> GetEntityPool(string tag)
    {
        TaskCompletionSource<AbstractEntityPool> tcs = new TaskCompletionSource<AbstractEntityPool>();

        if (entityPoolDict.TryGetValue(tag, out AbstractEntityPool entityPool))
        {
           bool resultSet = tcs.TrySetResult(entityPool);
        }
        return await tcs.Task;
    }

    public Task Activate(string tag)
    {
        TaskCompletionSource<AbstractEntityPool> tcs = new TaskCompletionSource<AbstractEntityPool>();

        if (entityPoolDict.TryGetValue(tag, out AbstractEntityPool entityPool))
        {
            entityPool.Entity.SetActive(true);

            tcs.SetResult(entityPool);
        }

        return tcs.Task;
    }
    public Task Deactivate(string tag)
    {
        TaskCompletionSource<AbstractEntityPool> tcs = new TaskCompletionSource<AbstractEntityPool>();

        if (entityPoolDict.TryGetValue(tag, out AbstractEntityPool entityPool))
        {
            entityPool.Entity.SetActive(false);

            tcs.SetResult(entityPool);
        }

        return tcs.Task;
    }

    public async void InvokeEntityPool(AbstractEntityPool entityPool)
    {
        await Pool(entityPool);
    }
}