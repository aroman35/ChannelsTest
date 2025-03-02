using System.Threading.Channels;

namespace ChannelsTest;

public class Consumer(Channel<SomeDto> channel)
{
    public async Task ConsumeChannel(CancellationToken cancellationToken)
    {
        var queue = channel.Reader.ReadAllAsync(cancellationToken);
        await foreach (var dto in queue)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1000), cancellationToken);
            await Console.Out.WriteLineAsync($"Received: {dto}");
        }
        await channel.Reader.Completion;
        await Console.Out.WriteLineAsync("Consumption completed");
    }
}
