﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Jammit.Forms
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class SettingsPage : ContentPage
  {
    public SettingsPage()
    {
      InitializeComponent();
    }

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
      //Hack: Manually flushing settings.
      //TODO: Replace with tow-way binding.
      Settings.ServiceUri = ServiceUriEntry.Text;

      Navigation.PopModalAsync();
    }

    private void AuthorizeButton_Clicked(object sender, EventArgs e)
    {
      App.Client.RequestAuthorization().Wait();
    }
  }
}