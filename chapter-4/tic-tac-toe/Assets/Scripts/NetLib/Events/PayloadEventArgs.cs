public class PayloadEventArgs<T>
{
    public T Payload { get; }

    public PayloadEventArgs(T payload)
    {
        Payload = payload;
    }
}