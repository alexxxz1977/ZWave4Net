using System;
using System.Collections.Generic;
using ZWave.Channel.Protocol;

namespace ZWave.CommandClasses
{
    public class MultiCommandReport : NodeReport
    {
        public readonly byte NumCommands;
        public readonly List<NodeReport> Reports = new List<NodeReport>();

        enum multiCommandReports
        {
            Battery = 0x8003,
            Alarm = 0x7105,
            SensorMultiLevel = 0x3105
        }

        internal MultiCommandReport(Node node, byte[] payload) : base(node)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }
            if (payload.Length < 5)
            {
                throw new ReponseFormatException($"The response was not in the expected format. {GetType().Name}: Payload: {BitConverter.ToString(payload)}");
            }

            NumCommands = payload[0];
            int i = 1;
            byte k = 0;
            while (i < payload.Length)
            {
                byte cmdLength = payload[i];
                var cmdClass = (multiCommandReports)((payload[i + 1] << 8) | payload[i + 2]);
                var cmdData = new byte[cmdLength - 2];
                Array.Copy(payload, i + 3, cmdData, 0, cmdData.Length);
                switch (cmdClass)
                {
                    case multiCommandReports.Battery:
                        Reports.Add(new BatteryReport(Node, cmdData));
                        break;
                    case multiCommandReports.Alarm:
                        Reports.Add(new AlarmReport(Node, cmdData));
                        break;
                    case multiCommandReports.SensorMultiLevel:
                        Reports.Add(new SensorMultiLevelReport(Node, cmdData));
                        break;
                }
                i += cmdLength + 1;
                k++;
            }
            if (k != NumCommands)
            {
                throw new ReponseFormatException($"The response was not in the expected format. {GetType().Name}: Expected {NumCommands} commands, actual value is {k}");
            }
        }

        public override string ToString()
        {
            return $"Commands: {NumCommands}";
        }
    }
}
