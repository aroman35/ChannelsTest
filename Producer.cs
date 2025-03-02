using System.Threading.Channels;

namespace ChannelsTest;

public class Producer(Channel<SomeDto> channel, ManualResetEventSlim manualResetEventSlim)
{
    public ValueTask<bool> WaitProducerToBeReady(CancellationToken cancellationToken) =>
        channel.Writer.WaitToWriteAsync(cancellationToken);

    public async ValueTask Send(SomeDto someDto)
    {
        manualResetEventSlim.Wait();
        await channel.Writer.WriteAsync(someDto);
        Console.WriteLine($"Sent: {someDto}");
        manualResetEventSlim.Reset();
    }
}
