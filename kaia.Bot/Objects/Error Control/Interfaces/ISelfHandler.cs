using Kaia.Bot.Objects.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.CCB_Structures.Derivations
{
    public interface ISelfHandler
    {
        Task OnErrorAsync(Exception Exception);
    }
}
