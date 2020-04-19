namespace CleanFunc.Application.Common.Interfaces
{
    public interface IBusFactory
    {
        IBus Create(string queueOrTopicName);
        IBus Create<TPayload>() where TPayload: class;
    }
}