using Controllers;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI
{
    /// <summary>
    /// Interaction logic for AddOrder.xaml
    /// </summary>
    public partial class AddOrderView : Page
    {
        /// <summary>
        /// Constructor that loads the compiled page of a component, and sets the data context for an OrderHeader object that's used for data binding.
        /// </summary>
        /// <param name="oh">OrderHeader object</param>
        public AddOrderView(OrderHeader oh = null)
        {
            try
            {
                if (oh == null)
                {
                    DataContext = OrdersController.Instance.CreateNewOrderHeader();
                }
                else
                {
                    DataContext = oh;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            InitializeComponent();
        }

        private void GoTo_AddOrderItems(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddOrderItemView((OrderHeader)DataContext));
        }

        private void btn_DeleteOrderItem(object sender, RoutedEventArgs e)
        {
            try
            {
                var oi = (OrderItem)((Button)e.Source).DataContext;
                var oh = (OrderHeader)DataContext;
                DataContext = OrdersController.Instance.DeleteOrderItem(oh.Id, oi.StockItemId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_CancelOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                OrdersController.Instance.DeleteOrderHeaderAndOrderItems(((OrderHeader)DataContext).Id);
                MessageBox.Show("Order has been canceled and removed from database.", "Order Canceled", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new OrdersView());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_SubmitOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                OrdersController.Instance.SubmitOrder(((OrderHeader)DataContext).Id);
                NavigationService.Navigate(new OrdersView());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
