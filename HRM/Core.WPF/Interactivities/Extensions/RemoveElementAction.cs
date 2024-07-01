﻿using System.Windows;
using System.Windows.Controls;

namespace Core.WPF.Interactivities.Extensions
{

    public class RemoveElementAction : TargetedTriggerAction<FrameworkElement>
    {
        protected override void Invoke(object parameter)
        {
            if (this.AssociatedObject != null && this.Target != null)
            {
                DependencyObject parent = this.Target.Parent;

                Panel panel = parent as Panel;
                if (panel != null)
                {
                    panel.Children.Remove(this.Target);
                    return;
                }

                ContentControl contentControl = parent as ContentControl;
                if (contentControl != null)
                {
                    if (contentControl.Content == this.Target)
                    {
                        contentControl.Content = null;
                    }
                    return;
                }

                ItemsControl itemsControl = parent as ItemsControl;
                if (itemsControl != null)
                {
                    itemsControl.Items.Remove(this.Target);
                    return;
                }

                Page page = parent as Page;
                if (page != null)
                {
                    if (page.Content == this.Target)
                    {
                        page.Content = null;
                    }
                    return;
                }

                Decorator decorator = parent as Decorator;
                if (decorator != null)
                {
                    if (decorator.Child == this.Target)
                    {
                        decorator.Child = null;
                    }
                    return;
                }

                if (parent != null)
                {
                    throw new InvalidOperationException(ExceptionStringTable.UnsupportedRemoveTargetExceptionMessage);
                }
            }
        }
    }
}
