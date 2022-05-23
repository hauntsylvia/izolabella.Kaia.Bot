using izolabella.CompetitiveCounting.Bot.Objects.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izolabella.CompetitiveCounting.Bot.Objects.CCB_Structures.Derivations
{
    public interface ISelfHandler
    {
        Task OnErrorAsync(Exception Exception);
    }
}
