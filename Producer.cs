using System.Threading.Channels;

namespace ChannelsTest;

public class Producer(Channel<SomeDto> channel)
{
    public ValueTask<bool> WaitProducerToBeReady(CancellationToken cancellationToken) =>
        channel.Writer.WaitToWriteAsync(cancellationToken);

    public async ValueTask Send(SomeDto someDto)
    {
        await channel.Writer.WriteAsync(someDto);
        Console.WriteLine($"Sent: {someDto}");
    }
}
