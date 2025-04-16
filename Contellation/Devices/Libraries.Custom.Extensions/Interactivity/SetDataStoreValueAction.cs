using Libraries.Custom.Interactivities;

namespace Libraries.Custom.Extensions.Interactivity
{
    [DefaultTrigger(typeof(UIElement), typeof(Events.EventTrigger), "Loaded")]
    public class SetDataStoreValueAction : ChangePropertyAction { }
}
