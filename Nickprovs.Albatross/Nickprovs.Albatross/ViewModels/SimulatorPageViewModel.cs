﻿using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nickprovs.Albatross.ViewModels
{
    public class SimulatorPageViewModel : BaseViewModel
    {
        #region Fields


        #endregion

        #region Properties


        #endregion

        #region Constructors and Destructors

        public SimulatorPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.Title = "Simulator";
        }

        #endregion

        #region Private Methods


        #endregion
    }
    
}
