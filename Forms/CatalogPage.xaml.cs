﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Jammit.Model;

namespace Jammit.Forms
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class CatalogPage : ContentPage
  {
    public static List<SongInfo> Catalog { get; private set; }

    public CatalogPage()
    {
      InitializeComponent();

      Task.Run(async () => await LoadCatalog()).Wait();

      //TODO: Move back into XAML bindings.
      this.CatalogView.ItemsSource = Catalog;
    }

    private async Task LoadCatalog()
    {
      Catalog = await App.Client.LoadCatalog();
    }

    private async void LoadButton_Clicked(object sender, EventArgs e)
    {
      await LoadCatalog();
    }

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
      Navigation.PopModalAsync();
    }

    private async void DownloadButton_Clicked(object sender, EventArgs e)
    {
      if (null == CatalogView.SelectedItem)
        return;

      // Download song
      var selectedSong = CatalogView.SelectedItem as SongInfo;
      try
      {
        // Make sure Downloads directory exists.
        var downloadsDir = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(App.DataDirectory, "Downloads"));
        var zipPath = System.IO.Path.Combine(downloadsDir.FullName, selectedSong.Id.ToString().ToUpper() + ".zip");

        await App.Client.DownloadSong(selectedSong, zipPath);
        var downloadedStream = System.IO.File.OpenRead(zipPath);
        var song = App.Library.AddSong(downloadedStream);
        System.IO.File.Delete(zipPath); // Delete downloaded file.

        //TODO: Assert selected item and downloaded content metadata are equal.

        await DisplayAlert("Downloaded Song", song.ToString(), "OK");
      }
      catch (Exception ex)
      {
        await DisplayAlert("Error", $"Could not download or install song with ID [{selectedSong.Id}].", "OK");
      }
    }
  }
}