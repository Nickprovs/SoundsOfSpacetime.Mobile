using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Nickprovs.Albatross.ViewModels
{
    public class CreditsPageViewModel : BaseViewModel
    {

        #region Fields


        #endregion

        #region Properties

        public DelegateCommand ContactDeveloperCommand { get; }

        #endregion

        #region Constructors and Destructors

        public CreditsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.Title = "Credits";
            this.ContactDeveloperCommand = new DelegateCommand(this.OnContactDeveloper);
        }

        #endregion

        #region Private Methods

        private void OnContactDeveloper()
        {
            this.SendDeveloperEmail();
        }

        private async void SendDeveloperEmail()
        {
            string shareurl = String.Empty;
            String messageBody = "Replace this text with a comment or issue you wish to bring to the developer's attention.";
            String messageTitle = "Sounds of Spacetime App Mail";
            if (Device.RuntimePlatform == Device.iOS)
            {
                var subject = Regex.Replace(messageTitle, @"[^\u0000-\u00FF]", string.Empty);
                var body = Regex.Replace(messageBody, @"[^\u0000-\u00FF]", string.Empty);
                var email = Regex.Replace("soundsofspacetime@gmail.com", @"[^\u0000-\u00FF]", string.Empty);
                shareurl = "mailto:" + email + "?subject=" + WebUtility.UrlEncode(subject) + "&body=" + WebUtility.UrlEncode(body);
            }
            else 
            {
                //for Android it is not necessary to code nor is it necessary to assign a destination email
                shareurl = "mailto:soundsofspacetime@gmail.com?subject=" + messageTitle + "&body=" + messageBody;
            }

            await Launcher.OpenAsync(new Uri(shareurl));
        }

        #endregion

    }
}
