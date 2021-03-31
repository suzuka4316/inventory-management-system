using Controllers;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for AddOrderItem.xaml
    /// </summary>
    public partial class AddOrderItemView : Page
    {
        private OrderHeader _oh;

        /// <summary>
        /// Constructor that loads the compiled page of a component, and sets an stock item collection to generate the content for the datagrid.
        /// </summary>
        /// <param name="oh">OrderHeader object</param>
        public AddOrderItemView(OrderHeader oh)
        {
            InitializeComponent();
            try
            {
                _oh = oh;
                IEnumerable<StockItem> siList = StockItemsController.Instance.GetStockItems();
                dgStockItems.ItemsSource = siList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_AddQuantity(object sender, RoutedEventArgs e)
        {
            var item = (StockItem)dgStockItems.SelectedItem;
            string quantityInput = QuantityInput.Text;
            //int convertedInput = Convert.ToInt32(quantityInput);

            if (item == null)
            {
                MessageBox.Show("Please select a stock item", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (quantityInput == "")
            {
                MessageBox.Show("Please enter quantity before clicking Add Quantity button", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (Convert.ToInt32(quantityInput) < 1)
            {
                MessageBox.Show("Quanity must be greater than zero (0)", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (Convert.ToInt32(quantityInput) > item.InStock)
            {
                MessageBox.Show($"There is currently not enough items in stock.\nRequested: {quantityInput} In stock: {item.InStock}\nThis order might be rejected if there is not enough stock when the order is being processed ", "Info", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            try
            {
                _oh = OrdersController.Instance.UpsertOrderItem(_oh.Id, item.Id, Convert.ToInt32(quantityInput));
                MessageBox.Show($"{quantityInput} {item.Name}(s) has been added for OrderHeaderId {_oh.Id}", "Item Added", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new AddOrderView(_oh));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
