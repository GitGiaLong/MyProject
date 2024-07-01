using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace Core.WPF.Interactivities
{
    internal static class DataBindingHelper
    {
        private static Dictionary<Type, IList<DependencyProperty>> DependenciesPropertyCache = new Dictionary<Type, IList<DependencyProperty>>();

        public static void EnsureDataBindingUpToDateOnMembers(DependencyObject dpObject)
        {
            IList<DependencyProperty> dpList = null;

            if (!DependenciesPropertyCache.TryGetValue(dpObject.GetType(), out dpList))
            {
                dpList = new List<DependencyProperty>();
                Type type = dpObject.GetType();

                while (type != null)
                {
                    FieldInfo[] fieldInfos = type.GetFields();

                    foreach (FieldInfo fieldInfo in fieldInfos)
                    {
                        if (fieldInfo.IsPublic &&
                            fieldInfo.FieldType == typeof(DependencyProperty))
                        {
                            DependencyProperty property = fieldInfo.GetValue(null) as DependencyProperty;
                            if (property != null)
                            {
                                dpList.Add(property);
                            }
                        }
                    }

                    type = type.BaseType;
                }
                // Cache the list of DP for performance gain
                DependenciesPropertyCache[dpObject.GetType()] = dpList;
            }

            if (dpList == null)
            {
                return;
            }

            foreach (DependencyProperty property in dpList)
            {
                EnsureBindingUpToDate(dpObject, property);
            }

        }

        public static void EnsureDataBindingOnActionsUpToDate(TriggerBase<DependencyObject> trigger)
        {
            foreach (TriggerAction action in trigger.Actions)
            {
                EnsureDataBindingUpToDateOnMembers(action);
            }
        }

        public static void EnsureBindingUpToDate(DependencyObject target, DependencyProperty dp)
        {
            BindingExpression binding = BindingOperations.GetBindingExpression(target, dp);
            if (binding != null)
            {
                binding.UpdateTarget();
            }
        }
    }
}
