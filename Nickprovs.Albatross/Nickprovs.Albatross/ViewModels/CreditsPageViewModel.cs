using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nickprovs.Albatross.ViewModels
{
    public class CreditsPageViewModel : BaseViewModel
    {

        #region Fields


        #endregion

        #region Properties


        #endregion

        #region Constructors and Destructors

        public CreditsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.Title = "Credits";
        }

        #endregion

        #region Private Methods


        #endregion

    }
}
