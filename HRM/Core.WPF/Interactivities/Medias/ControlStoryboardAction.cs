﻿using Core.WPF.Interactivities.Enums;
using System.Windows;
using System.Windows.Media.Animation;

namespace Core.WPF.Interactivities.Medias
{

    [CLSCompliant(false)]
    public class ControlStoryboardAction : StoryboardAction
    {
        public static readonly DependencyProperty ControlStoryboardProperty = DependencyProperty.Register("ControlStoryboardOption", typeof(ControlStoryboardOption), typeof(ControlStoryboardAction));

        public ControlStoryboardAction() { }

        public ControlStoryboardOption ControlStoryboardOption
        {
            get { return (ControlStoryboardOption)this.GetValue(ControlStoryboardProperty); }
            set { this.SetValue(ControlStoryboardProperty, value); }
        }

        protected override void Invoke(object parameter)
        {
            if (this.AssociatedObject != null && this.Storyboard != null)
            {
                switch (this.ControlStoryboardOption)
                {
                    case ControlStoryboardOption.Play:
                        this.Storyboard.Begin();
                        break;
                    case ControlStoryboardOption.Stop:
                        this.Storyboard.Stop();
                        break;
                    case ControlStoryboardOption.TogglePlayPause:
                        ClockState clockState = ClockState.Stopped;
                        bool isPaused = false;
                        try
                        {
                            clockState = this.Storyboard.GetCurrentState();
                            isPaused = this.Storyboard.GetIsPaused();
                        }
                        catch (InvalidOperationException)
                        {
                        }
                        if (clockState == ClockState.Stopped)
                        {
                            this.Storyboard.Begin();
                        }
                        else if (isPaused)
                        {
                            this.Storyboard.Resume();
                        }
                        else
                        {
                            this.Storyboard.Pause();
                        }
                        break;
                    case ControlStoryboardOption.Pause:
                        this.Storyboard.Pause();
                        break;
                    case ControlStoryboardOption.Resume:
                        this.Storyboard.Resume();
                        break;
                    case ControlStoryboardOption.SkipToFill:
                        this.Storyboard.SkipToFill();
                        break;
                }
            }
        }
    }
}