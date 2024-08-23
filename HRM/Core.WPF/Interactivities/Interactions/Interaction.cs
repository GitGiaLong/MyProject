namespace Core.WPF.Interactivities
{
    public static class Interaction
    {
        internal static bool ShouldRunInDesignMode { get; set; }

        private static readonly DependencyProperty TriggersProperty = DependencyProperty.RegisterAttached("ShadowTriggers",
                                                                                                                typeof(TriggerCollection),
                                                                                                                typeof(Interaction),
                                                                                                                new FrameworkPropertyMetadata(
                                                                                                                    new PropertyChangedCallback(OnTriggersChanged)));

        private static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached("ShadowBehaviors",
                                                                                                                        typeof(BehaviorCollection),
                                                                                                                        typeof(Interaction),
                                                                                                                        new FrameworkPropertyMetadata(
                                                                                                                            new PropertyChangedCallback(OnBehaviorsChanged)));


        public static TriggerCollection GetTriggers(DependencyObject obj)
        {
            TriggerCollection triggerCollection = (TriggerCollection)obj.GetValue(TriggersProperty);
            if (triggerCollection == null)
            {
                triggerCollection = new TriggerCollection();
                obj.SetValue(TriggersProperty, triggerCollection);
            }
            return triggerCollection;
        }

        public static BehaviorCollection GetBehaviors(DependencyObject obj)
        {
            BehaviorCollection behaviorCollection = (BehaviorCollection)obj.GetValue(BehaviorsProperty);
            if (behaviorCollection == null)
            {
                behaviorCollection = new BehaviorCollection();
                obj.SetValue(BehaviorsProperty, behaviorCollection);
            }
            return behaviorCollection;
        }

        private static void OnBehaviorsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            BehaviorCollection oldCollection = (BehaviorCollection)args.OldValue;
            BehaviorCollection newCollection = (BehaviorCollection)args.NewValue;

            if (oldCollection != newCollection)
            {
                if (oldCollection != null && ((IBehavior)oldCollection).AssociatedObject != null)
                {
                    oldCollection.Detach();
                }

                if (newCollection != null && obj != null)
                {
                    if (((IBehavior)newCollection).AssociatedObject != null)
                    {
                        throw new InvalidOperationException(ExceptionStringTable.CannotHostBehaviorCollectionMultipleTimesExceptionMessage);
                    }

                    newCollection.Attach(obj);
                }
            }
        }

        private static void OnTriggersChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            TriggerCollection oldCollection = args.OldValue as TriggerCollection;
            TriggerCollection newCollection = args.NewValue as TriggerCollection;

            if (oldCollection != newCollection)
            {
                if (oldCollection != null && ((IBehavior)oldCollection).AssociatedObject != null)
                {
                    oldCollection.Detach();
                }

                if (newCollection != null && obj != null)
                {
                    if (((IBehavior)newCollection).AssociatedObject != null)
                    {
                        throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerCollectionMultipleTimesExceptionMessage);
                    }

                    newCollection.Attach(obj);
                }
            }
        }

        internal static bool IsElementLoaded(FrameworkElement element)
        {
            return element.IsLoaded;
        }
    }
}
