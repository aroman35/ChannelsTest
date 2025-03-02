using System.Threading.Channels;
using ChannelsTest;

using var cancellationTokenSource = new CancellationTokenSource();
var channel = Channel.CreateBounded<SomeDto>(new BoundedChannelOptions(1)
{
    FullMode = BoundedChannelFullMode.Wait,
    SingleReader = true,
    SingleWriter = true,
    AllowSynchronousContinuations = true
});

var consumer = new Consumer(channel);
var producer = new Producer(channel);
var cancellationToken = cancellationTokenSource.Token;
var consumerTask = Task.Run(() => consumer.ConsumeChannel(cancellationToken), cancellationToken);

for (var i = 0; i < 5; i++)
{
    var trade = SomeDto.CreateNew(i);
    await producer.Send(trade);
}

await producer.Complete(cancellationToken);
await consumerTask;
