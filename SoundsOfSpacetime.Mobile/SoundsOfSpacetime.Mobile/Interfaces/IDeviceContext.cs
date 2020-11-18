using System;
using System.Threading.Tasks;

namespace SoundsOfSpacetime.Mobile.Interfaces
{
    public interface IDeviceContext
    {
        Task<T> BeginInvokeOnMainThreadAsync<T>(Func<T> a);
    }
}
