using PlayerAnimationHandler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectsToBeNotified : MonoBehaviour
{

    private List<IObserver> _potentialObservers = new();

    public void AddObserver(IObserver observer)
    {
        _potentialObservers.Add(observer);
    }

    public void RemoveOberver(IObserver observer)
    {
        _potentialObservers.Remove(observer);
    }

    protected void NotifyObservers(string objectPickedup, Vector2 position)
    {
        foreach (var observer in _potentialObservers)
        {
            observer.OnNotify(objectPickedup, position);
        }
    }


}