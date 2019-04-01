using Prism.Navigation;
using Xamarin.Forms;

namespace Nickprovs.Albatross.Views
{
    public partial class MainPage : MasterDetailPage, IMasterDetailPageOptions
    {
        public bool IsPresentedAfterNavigation => Device.Idiom != TargetIdiom.Phone;

        public MainPage()
        {
            InitializeComponent();
        }
    }

}
