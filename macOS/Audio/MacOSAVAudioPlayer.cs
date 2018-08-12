using System;

using Foundation;
using AVFoundation;

namespace Jammit.Audio
{
  public class MacOSAVAudioPlayer : IAVAudioPlayer
  {
    #region private members

    AVAudioPlayer player;
    AVPlayer avp;

    #endregion // private members

    public event EventHandler CurrentTimeChanged;

    public MacOSAVAudioPlayer(Model.PlayableTrackInfo track, System.IO.Stream stream)
    {
      NSError error;
      player = AVAudioPlayer.FromData(NSData.FromStream(stream), out error);

      player.FinishedPlaying += delegate
      {
        player.Dispose();
        player = null;
      };
      player.NumberOfLoops = 0;
      player.AddObserver("currentTime", NSKeyValueObservingOptions.New, (NSObservedChange obj) =>
      {
        bool observerWorks = true;
      });

      player.PrepareToPlay();
    }

    public MacOSAVAudioPlayer(Model.JcfMedia media, Model.PlayableTrackInfo track)
    {
      var url = new NSUrl($"file://{media.Path}/{track.Identifier.ToString().ToUpper()}_jcfx.aifc");
      avp = new AVPlayer(url);
      avp.AddPeriodicTimeObserver(CoreMedia.CMTime.FromSeconds(1, 1), CoreFoundation.DispatchQueue.MainQueue, (CoreMedia.CMTime time) =>
      {
        //handler?.Invoke(this, new EventArgs());
        CurrentTimeChanged?.Invoke(this, new EventArgs());
      });
    }

    #region IAvAudioPlayer members

    public void Play()
    {
      //player.Play();
      avp.Play();
    }

    public void PlayAtTime(double time)
    {
      //player.PlayAtTime(time);
    }

    public void Stop()
    {
      //player.Stop();
      avp.Pause();
    }

    public void Dispose()
    {
      //player.Dispose();
      avp.Dispose();
    }

    public double Duration => avp.CurrentItem.Duration.Seconds;//player.Duration;

    public double CurrentTime
    {
      get
      {
        //return player.CurrentTime;
        return avp.CurrentTime.Seconds;
      }

      set
      {
        //player.CurrentTime = value;
        //avp.Seek(CoreMedia.CMTime.FromSeconds(value));
        avp.Seek(CoreMedia.CMTime.FromSeconds(value, 1));
      }
    }

    public float Volume
    {
      get
      {
        //return player.Volume;
        return avp.Volume;
      }

      set
      {
        //player.Volume = value;
        avp.Volume = value;
      }
    }

    public int NumberOfLoops
    {
      get
      {
        //return (int)player.NumberOfLoops;
        return 0;
      }

      set
      {
        //player.NumberOfLoops = (nint)value;
      }
    }

    #endregion // IAvAudioPlayer members
  }
}
