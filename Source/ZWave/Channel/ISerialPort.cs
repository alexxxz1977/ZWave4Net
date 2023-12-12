using System.IO;

namespace ZWave.Channel
{
    public interface ISerialPort
    {
        Stream InputStream { get; }
        Stream OutputStream { get; }

        string Name { get; }

        void Close();
        void Open();
    }
}
