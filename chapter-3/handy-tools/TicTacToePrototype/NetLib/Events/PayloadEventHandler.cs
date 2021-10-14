namespace NetLib
{
    public delegate void PayloadEventHandler<T>(object sender, PayloadEventArgs<T> e);

    public class PayloadEventArgs<T>
    {
        public T Payload { get; }

        public PayloadEventArgs(T payload)
        {
            Payload = payload;
        }
    }
}