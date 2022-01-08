using System;
using System.Linq;
using ZWave.Channel;
using ZWave.Channel.Protocol;

namespace ZWave.CommandClasses
{
    public class MultiChannelReport : NodeReport
    {
        public readonly CommandClass CommandClass;
        public readonly byte ControllerID;
        public readonly byte EndPointID;
        public readonly string Payload;

        public readonly NodeReport Report;

        internal MultiChannelReport(Node node, byte[] payload) : base(node)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));
            if (payload.Length < 3)
                throw new ReponseFormatException($"The response was not in the expected format. {GetType().Name}: Payload: {BitConverter.ToString(payload)}");

            EndPointID = payload[0];
            ControllerID = payload[1];
            CommandClass = (CommandClass)payload[2];

            // check sub report
            if (payload.Length > 3)
            {
                switch (CommandClass)
                {
                    case CommandClass.SwitchBinary:
                        if (payload[3] == Convert.ToByte(SwitchBinary.command.Report))
                        {
                            Report = new SwitchBinaryReport(node, payload.Skip(4).ToArray<Byte>());
                        }
                        break;
                    case CommandClass.SensorMultiLevel:
                        if (payload[3] == Convert.ToByte(SensorMultiLevel.command.Report))
                        {
                            Report = new SensorMultiLevelReport(node, payload.Skip(4).ToArray<Byte>());
                        }
                        break;
                    case CommandClass.CentralScene:
                        if (payload[3] == Convert.ToByte(CentralScene.command.Notification))
                        {
                            Report = new CentralSceneReport(node, payload.Skip(4).ToArray<Byte>());
                        }
                        break;
                }
            }

            Payload = BitConverter.ToString(payload);
        }

        public override string ToString()
        {
            return $"ControllerID:{ControllerID}. EndPointID:{EndPointID}. Report:{Report}, payload:{Payload}";
        }
    }
}
