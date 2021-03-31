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
    /// Interaction logic for OrderDetails.xaml
    /// </summary>
    /// 
    public partial class OrderDetailsView : Page
    {
        /// <summary>
        /// Constructor that loads the compiled page of a component, and sets the data context for an OrderHeader object that's used for data binding.
        /// </summary>
        /// <param name="order">OrderHeader object</param>
        public OrderDetailsView(OrderHeader order)
        {
            DataContext = order;
            InitializeComponent();
        }

        private void btn_SubmitOrProcess(object sender, RoutedEventArgs e)
        {
            try
            {
                var order = (OrderHeader)DataContext;
                if (order.State is OrderNew)
                {
                    DataContext = OrdersController.Instance.SubmitOrder(order.Id);
                    MessageBox.Show("Order has been submitted", "Order Submitted", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (order.State is OrderPending)
                {
                    DataContext = OrdersController.Instance.ProcessOrder(order.Id);
                    if (((OrderHeader)DataContext).State is OrderComplete) //If OrderState has changed to OrderComplete after executing ProcessOrder()
                    {
                        MessageBox.Show("Order has been completed", "Order Processed", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Order has been rejected", "Order Processed", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoTo_OrdersView(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrdersView());
        }
    }
}
