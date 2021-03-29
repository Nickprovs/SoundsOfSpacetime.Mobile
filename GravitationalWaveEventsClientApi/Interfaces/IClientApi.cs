using GravitationalWaveEventsClientApi.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GravitationalWaveEventsClientApi.Interfaces
{
    public interface IClientApi
    {
        Task<IEnumerable<GravitationalWaveEvent>> GetAllEventsAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<GravitationalWaveEvent>> GetGwtc1ConfidentEventsAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<GravitationalWaveEvent>> GetGwtc1MarginalEventsAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<GravitationalWaveEvent>> GetGwtc2EventsAsync(CancellationToken cancellationToken = default);

    }
}
