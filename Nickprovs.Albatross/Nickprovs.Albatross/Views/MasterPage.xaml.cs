using Prism.Navigation;
using Xamarin.Forms;

namespace Nickprovs.Albatross.Views
{
    public partial class MasterPage : MasterDetailPage, IMasterDetailPageOptions
    {
        public bool IsPresentedAfterNavigation => Device.Idiom != TargetIdiom.Phone;

        public MasterPage()
        {
            InitializeComponent();
        }
    }

}
