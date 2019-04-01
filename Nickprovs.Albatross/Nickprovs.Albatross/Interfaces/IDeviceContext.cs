using System;
using System.Threading.Tasks;

namespace Nickprovs.Albatross.Interfaces
{
    public interface IDeviceContext
    {
        Task<T> BeginInvokeOnMainThreadAsync<T>(Func<T> a);
    }
}
