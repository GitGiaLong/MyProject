﻿using Core.Libraries.WPF.Controls.Bars.BreadcrumbBar;
using Core.Libraries.WPF.Extensions.Intputs;
using Core.Libraries.WPF.Handlers;
using System.Collections.Specialized;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Core.Libraries.WPF.Controls
{
    /// <summary>
    /// The <see cref="BreadcrumbBar"/> control provides the direct path of pages or folders to the current location.
    /// </summary>
    /// <example>
    /// <code lang="xml">
    /// &lt;ui:BreadcrumbBar x:Name="BreadcrumbBar" /&gt;
    /// </code>
    /// </example>
    [StyleTypedProperty(Property = nameof(ItemContainerStyle), StyleTargetType = typeof(BreadcrumbBarItem))]
    public class BreadcrumbBar : System.Windows.Controls.ItemsControl
    {
        /// <summary>
        /// Gets or sets custom command executed after selecting the item.
        /// </summary>
        [Localizability(LocalizationCategory.NeverLocalize)]
        public ICommand? Command
        {
            get { return (ICommand?)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand),
            typeof(BreadcrumbBar), new PropertyMetadata(null));

        /// <summary>
        /// Gets the <see cref="RelayCommand{T}"/> triggered after clicking
        /// </summary>
        public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);
        public static readonly DependencyProperty TemplateButtonCommandProperty = DependencyProperty.Register(nameof(TemplateButtonCommand), typeof(IRelayCommand),
            typeof(BreadcrumbBar), new PropertyMetadata(null));

        /// <summary>
        /// Occurs when an item is clicked in the <see cref="BreadcrumbBar"/>.
        /// </summary>
        public event TypedEventHandler<BreadcrumbBar, BreadcrumbBarItemClickedEventArgs> ItemClicked
        {
            add => AddHandler(ItemClickedEvent, value);
            remove => RemoveHandler(ItemClickedEvent, value);
        }
        public static readonly RoutedEvent ItemClickedEvent = EventManager.RegisterRoutedEvent(nameof(ItemClicked), RoutingStrategy.Bubble,
            typeof(TypedEventHandler<BreadcrumbBar, BreadcrumbBarItemClickedEventArgs>), typeof(BreadcrumbBar));

        /// <summary>
        /// Initializes a new instance of the <see cref="BreadcrumbBar"/> class.
        /// </summary>
        public BreadcrumbBar()
        {
            SetValue(TemplateButtonCommandProperty, new RelayCommand<object>(OnTemplateButtonClick));

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        protected virtual void OnItemClicked(object item, int index)
        {
            var args = new BreadcrumbBarItemClickedEventArgs(ItemClickedEvent, this, item, index);
            RaiseEvent(args);

            if (Command?.CanExecute(item) ?? false) { Command.Execute(item); }

            if (Command?.CanExecute(null) ?? false) { Command.Execute(null); }
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is BreadcrumbBarItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new BreadcrumbBarItem();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ItemContainerGenerator.ItemsChanged += ItemContainerGeneratorOnItemsChanged;
            ItemContainerGenerator.StatusChanged += ItemContainerGeneratorOnStatusChanged;

            UpdateLastContainer();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            Unloaded -= OnUnloaded;

            ItemContainerGenerator.ItemsChanged -= ItemContainerGeneratorOnItemsChanged;
            ItemContainerGenerator.StatusChanged -= ItemContainerGeneratorOnStatusChanged;
        }

        private void ItemContainerGeneratorOnStatusChanged(object? sender, EventArgs e)
        {
            if (ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
            {
                return;
            }

            if (ItemContainerGenerator.Items.Count <= 1)
            {
                UpdateLastContainer();

                return;
            }

            InteractWithItemContainer(2, static item => item.SetCurrentValue(BreadcrumbBarItem.IsLastProperty, false));
            UpdateLastContainer();
        }

        private void ItemContainerGeneratorOnItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Remove)
            {
                return;
            }

            UpdateLastContainer();
        }

        private void OnTemplateButtonClick(object? obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException("Item content is null");
            }

            DependencyObject container = ItemContainerGenerator.ContainerFromItem(obj);
            int index = ItemContainerGenerator.IndexFromContainer(container);

            OnItemClicked(obj, index);
        }

        private void InteractWithItemContainer(int offsetFromEnd, Action<BreadcrumbBarItem> action)
        {
            if (ItemContainerGenerator.Items.Count <= 0) { return; }

            var item = ItemContainerGenerator.Items[^offsetFromEnd];
            var container = (BreadcrumbBarItem)ItemContainerGenerator.ContainerFromItem(item);

            action.Invoke(container);
        }

        private void UpdateLastContainer() =>
            InteractWithItemContainer(1, static item => item.SetCurrentValue(BreadcrumbBarItem.IsLastProperty, true));
    }
}