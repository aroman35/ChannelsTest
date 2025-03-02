using System.Runtime.InteropServices;

namespace ChannelsTest;

[StructLayout(LayoutKind.Sequential)]
public struct SomeDto
{
    public static SomeDto CreateNew(int id)
    {
        return new SomeDto
        {
            SentTimestamp = DateTime.Now,
            Id = id
        };
    }

    public long Id;
    public DateTime SentTimestamp;

    public override string ToString()
    {
        return $"[{SentTimestamp:O}]: {Id}";
    }
}
