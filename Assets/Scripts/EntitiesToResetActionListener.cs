using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class EntitiesToResetActionListener : MonoBehaviour, IObserver<EntitiesToReset>
{
    private void OnEnable()
    {
        PlayerObserverListenerHelper.EntitiesToReset.AddObserver(this);
    }
    private void OnDisable()
    {
        PlayerObserverListenerHelper.EntitiesToReset.RemoveOberver(this);
    }
    private Task ResetAttributes(EntitiesToReset Data)
    {
        foreach(var entity in Data.entitiesToReset)
        {
            Debug.Log((entity.entity, entity.absractEntity));
            entity.absractEntity.Health = entity.absractEntity.MaxHealth; //reset health
        }
        return Task.CompletedTask;
    }

    public async void OnNotify(EntitiesToReset Data, params object[] optional)
    {
        if (Data != null)
          await ResetAttributes(Data);
    }

}