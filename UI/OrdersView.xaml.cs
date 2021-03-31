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
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class OrdersView : Page
    {
        /// <summary>
        /// Constructor that loads the compiled page of a component, and sets an order header collection to generate the content for the datagrid.
        /// </summary>
        public OrdersView()
        {
            InitializeComponent();

            try
            {
                dgOrders.ItemsSource = OrdersController.Instance.GetOrderHeaders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoTo_OrderDetails(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderDetailsView((OrderHeader)((Button)e.Source).DataContext));
        }
        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new AddOrderView());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK);
            }
        }
    }
}
