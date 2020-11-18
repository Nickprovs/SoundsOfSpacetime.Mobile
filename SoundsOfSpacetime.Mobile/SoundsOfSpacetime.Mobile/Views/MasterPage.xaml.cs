using Prism.Navigation;
using Xamarin.Forms;

namespace SoundsOfSpacetime.Mobile.Views
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
