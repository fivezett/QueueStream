
public class QueueStream : Stream
{
    private Queue<byte> _queue;
    private long size;
    public QueueStream()
    {
        _queue = new Queue<byte>();
    }

    public override bool CanRead => true;
    public override bool CanSeek => false;
    public override bool CanWrite => true;

    public override long Length => size;

    public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

    public override void Flush()
    {
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        int bytesRead = 0;
        for (int i = 0; i < count; ++i)
        {
            if (_queue.Count == 0)
            {
                break;
            }
            --size;
            buffer[i] = _queue.Dequeue();
            ++bytesRead;
        }
        return bytesRead;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        for (int i = 0; i < count; i++)
        {
            _queue.Enqueue(buffer[i]);
            ++size;
        }
    }

    public void Dispose()
    {
        _queue.Clear();
        _queue = null;
        GC.Collect();
        _queue = new Queue<byte>();
        size = 0;
    }
    public override void CopyTo(Stream destination, int bufferSize = 1024 * 1024)
    {
        byte[] buffer = new byte[bufferSize];
        int bytesRead;
        while ((bytesRead = Read(buffer, 0, bufferSize)) > 0)
        {
            destination.Write(buffer, 0, bytesRead);
        }
        buffer = null;
    }
}