using System.Threading.Channels;

namespace ChannelsTest;

public class Consumer(Channel<SomeDto> channel, ManualResetEventSlim manualResetEventSlim)
{
    public async Task ConsumeChannel(CancellationToken cancellationToken)
    {
        var queue = channel.Reader.ReadAllAsync(cancellationToken);
        await foreach (var dto in queue)
        {
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            Console.WriteLine($"Received: {dto}");
            manualResetEventSlim.Set();
        }
    }
}
