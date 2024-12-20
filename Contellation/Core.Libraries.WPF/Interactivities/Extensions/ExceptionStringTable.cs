﻿using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace Core.Libraries.WPF.Interactivities
{
    internal class ExceptionStringTable
    {
        private static ResourceManager resourceMan;

        private static CultureInfo resourceCulture;

        internal ExceptionStringTable() { }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager
        {
            get
            {
                if (ReferenceEquals(resourceMan, null))
                {
                    resourceMan ??= new ResourceManager(nameof(ExceptionStringTable), typeof(ExceptionStringTable).Assembly);
                }
                return resourceMan;
            }
        }
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get { return resourceCulture; }
            set { resourceCulture = value; }
        }

        /// <summary>
        ///   Looks up a localized string similar to Could not find method named &apos;{0}&apos; on object of type &apos;{1}&apos; that matches the expected signature..
        /// </summary>
        internal static string CallMethodActionValidMethodNotFoundExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(CallMethodActionValidMethodNotFoundExceptionMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cannot set the same BehaviorCollection on multiple objects..
        /// </summary>
        internal static string CannotHostBehaviorCollectionMultipleTimesExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(CannotHostBehaviorCollectionMultipleTimesExceptionMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to An instance of a Behavior cannot be attached to more than one object at a time..
        /// </summary>
        internal static string CannotHostBehaviorMultipleTimesExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(CannotHostBehaviorMultipleTimesExceptionMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cannot host an instance of a TriggerAction in multiple TriggerCollections simultaneously. Remove it from one TriggerCollection before adding it to another..
        /// </summary>
        internal static string CannotHostTriggerActionMultipleTimesExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(CannotHostTriggerActionMultipleTimesExceptionMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cannot set the same TriggerCollection on multiple objects..
        /// </summary>
        internal static string CannotHostTriggerCollectionMultipleTimesExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(CannotHostTriggerCollectionMultipleTimesExceptionMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to An instance of a trigger cannot be attached to more than one object at a time..
        /// </summary>
        internal static string CannotHostTriggerMultipleTimesExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(CannotHostTriggerMultipleTimesExceptionMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to More than one potential addition operator was found on type &apos;{0}&apos;..
        /// </summary>
        internal static string ChangePropertyActionAmbiguousAdditionOperationExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(ChangePropertyActionAmbiguousAdditionOperationExceptionMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cannot animate a property change on a type &apos;{0}&apos; Target. Property changes can only be animated on types derived from DependencyObject..
        /// </summary>
        internal static string ChangePropertyActionCannotAnimateTargetTypeExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(ChangePropertyActionCannotAnimateTargetTypeExceptionMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cannot find a property named &quot;{0}&quot; on type &quot;{1}&quot;..
        /// </summary>
        internal static string ChangePropertyActionCannotFindPropertyNameExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(ChangePropertyActionCannotFindPropertyNameExceptionMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The Increment property cannot be set to True if the Duration property is set..
        /// </summary>
        internal static string ChangePropertyActionCannotIncrementAnimatedPropertyChangeExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(ChangePropertyActionCannotIncrementAnimatedPropertyChangeExceptionMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The &apos;{0}&apos; property cannot be incremented because its value cannot be read..
        /// </summary>
        internal static string ChangePropertyActionCannotIncrementWriteOnlyPropertyExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(ChangePropertyActionCannotIncrementWriteOnlyPropertyExceptionMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cannot assign value of type &quot;{0}&quot; to property &quot;{1}&quot; of type &quot;{2}&quot;. The &quot;{1}&quot; property can be assigned only values of type &quot;{2}&quot;..
        /// </summary>
        internal static string ChangePropertyActionCannotSetValueExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(ChangePropertyActionCannotSetValueExceptionMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Property &quot;{0}&quot; defined by type &quot;{1}&quot; does not expose a set method and therefore cannot be modified..
        /// </summary>
        internal static string ChangePropertyActionPropertyIsReadOnlyExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(ChangePropertyActionPropertyIsReadOnlyExceptionMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The command &quot;{0}&quot; does not exist or is not publicly exposed on {1}..
        /// </summary>
        internal static string CommandDoesNotExistOnBehaviorWarningMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(CommandDoesNotExistOnBehaviorWarningMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cannot find state named &apos;{0}&apos; on type &apos;{1}&apos;. Ensure that the state exists and that it can be accessed from this context..
        /// </summary>
        internal static string DataStateBehaviorStateNameNotFoundExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(DataStateBehaviorStateNameNotFoundExceptionMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to &quot;{0}&quot; is not a valid type for the TriggerType parameter. Make sure &quot;{0}&quot; derives from TriggerBase..
        /// </summary>
        internal static string DefaultTriggerAttributeInvalidTriggerTypeSpecifiedExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(DefaultTriggerAttributeInvalidTriggerTypeSpecifiedExceptionMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cannot add the same instance of &quot;{0}&quot; to a &quot;{1}&quot; more than once..
        /// </summary>
        internal static string DuplicateItemInCollectionExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(DuplicateItemInCollectionExceptionMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The event &quot;{0}&quot; on type &quot;{1}&quot; has an incompatible signature. Make sure the event is public and satisfies the EventHandler delegate..
        /// </summary>
        internal static string EventTriggerBaseInvalidEventExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(EventTriggerBaseInvalidEventExceptionMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cannot find an event named &quot;{0}&quot; on type &quot;{1}&quot;..
        /// </summary>
        internal static string EventTriggerCannotFindEventNameExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(EventTriggerCannotFindEventNameExceptionMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Target {0} does not define any VisualStateGroups. .
        /// </summary>
        internal static string GoToStateActionTargetHasNoStateGroups
        {
            get
            {
                return ResourceManager.GetString(nameof(GoToStateActionTargetHasNoStateGroups), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to LeftOperand of type &quot;{0}&quot; cannot be used with operator &quot;{1}&quot;..
        /// </summary>
        internal static string InvalidLeftOperand
        {
            get
            {
                return ResourceManager.GetString(nameof(InvalidLeftOperand), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to LeftOperand of type &quot;{1}&quot; and RightOperand of type &quot;{0}&quot; cannot be used with operator &quot;{2}&quot;..
        /// </summary>
        internal static string InvalidOperands
        {
            get
            {
                return ResourceManager.GetString(nameof(InvalidOperands), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to RightOperand of type &quot;{0}&quot; cannot be used with operator &quot;{1}&quot;..
        /// </summary>
        internal static string InvalidRightOperand
        {
            get
            {
                return ResourceManager.GetString(nameof(InvalidRightOperand), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to An object of type &quot;{0}&quot; cannot have a {3} property of type &quot;{1}&quot;. Instances of type &quot;{0}&quot; can have only a {3} property of type &quot;{2}&quot;..
        /// </summary>
        internal static string RetargetedTypeConstraintViolatedExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(RetargetedTypeConstraintViolatedExceptionMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cannot attach type &quot;{0}&quot; to type &quot;{1}&quot;. Instances of type &quot;{0}&quot; can only be attached to objects of type &quot;{2}&quot;..
        /// </summary>
        internal static string TypeConstraintViolatedExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(TypeConstraintViolatedExceptionMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Unable to resolve TargetName &quot;{0}&quot;..
        /// </summary>
        internal static string UnableToResolveTargetNameWarningMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(UnableToResolveTargetNameWarningMessage), resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The target of the RemoveElementAction is not supported..
        /// </summary>
        internal static string UnsupportedRemoveTargetExceptionMessage
        {
            get
            {
                return ResourceManager.GetString(nameof(UnsupportedRemoveTargetExceptionMessage), resourceCulture);
            }
        }
    }
}
