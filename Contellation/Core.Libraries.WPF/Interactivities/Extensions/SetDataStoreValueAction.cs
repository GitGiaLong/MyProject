﻿namespace Core.Libraries.WPF.Interactivities.Extensions
{
    [DefaultTrigger(typeof(UIElement), typeof(EventTrigger), "Loaded")]
    public class SetDataStoreValueAction : ChangePropertyAction { }
}
