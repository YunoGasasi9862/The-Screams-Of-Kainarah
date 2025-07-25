
public interface IReceiver<T>
{
    T PerformAction(T value = default);
    T CancelAction();
}

public interface IReceiver<T, Z>
{
    Z PerformAction(T value = default);
    Z CancelAction();
}

public interface IReceiverEnhanced<IDENTIFIER, VALUE>
{
    VALUE PerformAction(VALUE value = default);

    VALUE CancelAction();
}
