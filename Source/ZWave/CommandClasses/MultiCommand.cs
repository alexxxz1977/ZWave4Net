using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZWave.Channel;

namespace ZWave.CommandClasses
{
    public class MultiCommand : CommandClassBase
    {
        enum command
        {
            Encap = 0x01
        }

        public event EventHandler<ReportEventArgs<MultiCommandReport>> Changed;

        public MultiCommand(Node node) : base(node, CommandClass.MultiCommand)
        {
        }

        protected internal override void HandleEvent(Command command)
        {
            base.HandleEvent(command);

            var report = new MultiCommandReport(Node, command.Payload);
            OnChanged(new ReportEventArgs<MultiCommandReport>(report));
        }

        protected virtual void OnChanged(ReportEventArgs<MultiCommandReport> e)
        {
            var handler = Changed;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
