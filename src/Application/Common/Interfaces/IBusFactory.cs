namespace CleanFunc.Application.Common.Interfaces
{
    public interface IBusFactory
    {
        IBusMessageSender Create(string queueOrTopicName);
        IBusMessageSender Create<TPayload>() where TPayload: class;
    }
}