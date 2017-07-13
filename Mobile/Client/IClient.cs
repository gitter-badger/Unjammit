﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jammit.Mobile.Client
{
  public interface IClient
  {
    int Foo();

    Task<List<Model.SongInfo>> LoadCatalog();

    Task<System.IO.Stream> DownloadSong(Guid id);
  }
}