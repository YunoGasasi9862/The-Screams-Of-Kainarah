using UnityEngine.Events;
public class LedgeGrabAnimationEvent : UnityEventWT<bool>
{
    private static UnityEvent<bool> _instance = new UnityEvent<bool>();
    public override UnityEvent<bool> GetInstance()
    {
        return _instance;
    }
    public static void AddEventListener(UnityAction<bool> value) //takes a bool unity event
    {
        _instance.AddListener(value);
    }
}