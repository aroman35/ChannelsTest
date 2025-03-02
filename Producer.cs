using System.Threading.Channels;

namespace ChannelsTest;

public class Producer(Channel<SomeDto> channel)
{
    public ValueTask<bool> WaitProducerToBeReady(CancellationToken cancellationToken) =>
        channel.Writer.WaitToWriteAsync(cancellationToken);

    public async ValueTask Send(SomeDto someDto)
    {
        await channel.Writer.WriteAsync(someDto);
        await Console.Out.WriteLineAsync($"Sent: {someDto}");
    }

    public async ValueTask Complete(CancellationToken cancellationToken)
    {
        if (await channel.Writer.WaitToWriteAsync(cancellationToken) ^ channel.Writer.TryComplete())
        {
            await Console.Error.WriteLineAsync("Channel cannot be completed");
            return;
        }
        await Console.Out.WriteLineAsync("Channel was closed");
    }
}
