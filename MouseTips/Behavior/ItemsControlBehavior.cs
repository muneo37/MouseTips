using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MouseTips.Behavior
{
    internal class ItemsControlBehavior
    {
        public static DependencyProperty BlackItemProperty = DependencyProperty.RegisterAttached("BlackItem", typeof(int?), typeof(ItemsControlBehavior), new FrameworkPropertyMetadata(null, OnBlackItemChanged));

        public static int? GetBlackItem(DependencyObject target)
        {
            return (int?)target.GetValue(BlackItemProperty);
        }

        public static void SetBlackItem(DependencyObject target, int? value)
        {
            target.SetValue(BlackItemProperty, value);
        }

        private static void OnBlackItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var itemsControl = sender as ItemsControl;

            if (itemsControl == null)
                return;

            if (e.NewValue != null)
            {
                var blackItemIndex = GetBlackItem(itemsControl);

                ItemsChangedEventHandler itemsChangedHandler = null;
                itemsChangedHandler = (object _, ItemsChangedEventArgs ea) =>
                {
                    if (blackItemIndex == null)
                    {
                        itemsControl.ItemContainerGenerator.ItemsChanged -= itemsChangedHandler;
                        return;
                    }
                    int test = 1;
                    for (int index = 0; index < itemsControl.Items.Count; index++)
                    {
                        if (index == blackItemIndex)
                        {
                            itemsControl.
                        }
                    }
                };
                itemsControl.ItemContainerGenerator.ItemsChanged += itemsChangedHandler;
            }

        }
    }
}
