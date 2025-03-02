using System.Threading.Channels;
using ChannelsTest;

using var cancellationTokenSource = new CancellationTokenSource();
var manualResetEventSlim = new ManualResetEventSlim(true, 1);
var channel = Channel.CreateBounded<SomeDto>(new BoundedChannelOptions(1)
{
    FullMode = BoundedChannelFullMode.Wait,
    SingleReader = true,
    SingleWriter = true,
    AllowSynchronousContinuations = true
});

var consumer = new Consumer(channel, manualResetEventSlim);
var producer = new Producer(channel, manualResetEventSlim);
var cancellationToken = cancellationTokenSource.Token;
_ = Task.Run(() => consumer.ConsumeChannel(cancellationToken), cancellationToken);

for (var i = 0; i < 5; i++)
{
    if (await producer.WaitProducerToBeReady(cancellationToken))
    {
        var trade = SomeDto.CreateNew(i);
        await producer.Send(trade);
    }
}

manualResetEventSlim.Wait(cancellationToken);
await cancellationTokenSource.CancelAsync();
