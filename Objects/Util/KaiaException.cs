using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Util
{
    public class KaiaException : Exception
    {
        public KaiaException(string Message, string? ParamName = null)
        {
            this.Message = Message;
            this.ParamName = ParamName;
        }

        public override string Message { get; }

        public string? ParamName { get; }
    }
}
