using Labb_2___AdminBookShop.Models;
using System.Windows;

namespace Labb_2___AdminBookShop
{
    public partial class MainWindow : Window
    {
        private readonly BookshopContext _context;

        public MainWindow()
        {
            InitializeComponent();

            _context = new BookshopContext();

            LoadStores();
            LoadBooks();
        }

        private void LoadStores()
        {
            var stores = _context.Stores.OrderBy(s => s.StoreName).ToList();
            StoreSelector.ItemsSource = stores;
            StoreSelector.DisplayMemberPath = "StoreName";
            StoreSelector.SelectedValuePath = "Id";
        }

        private void LoadBooks()
        {
            var books = _context.Books.OrderBy(b => b.Title).ToList();
            BookSelector.ItemsSource = books;
        }

        private void LoadInventory()
        {
            var inventoryData = _context.Inventories.OrderBy(i => i.Store.StoreName).ThenBy(b => b.IsbnNavigation.Title)
                .Select(i => new
                {
                    StoreName = i.Store.StoreName,
                    BookTitle = i.IsbnNavigation.Title,
                    Quantity = i.Quantity
                })
                .ToList();

            InventoryGrid.ItemsSource = inventoryData;
        }

        private void FilterInventory()
        {
            int? selectedStoreId = StoreSelector.SelectedValue as int?;
            string? selectedIsbn = BookSelector.SelectedValue as string;

            var filteredInventory = _context.Inventories.OrderBy(i => i.Store.StoreName).ThenBy(b => b.IsbnNavigation.Title).AsQueryable();

            // Filter based on selected store
            if (selectedStoreId.HasValue)
            {
                filteredInventory = filteredInventory.Where(i => i.StoreId == selectedStoreId.Value);
            }

            // Filter based on selected book
            if (!string.IsNullOrWhiteSpace(selectedIsbn))
            {
                filteredInventory = filteredInventory.Where(i => i.Isbn == selectedIsbn);
            }

            if (!selectedStoreId.HasValue && selectedIsbn == null)
            {
                InventoryGrid.ItemsSource = null;
                return;
            }

            // Load and show the selected items
            var inventoryData = filteredInventory
                .Select(i => new
                {
                    StoreName = i.Store.StoreName,
                    BookTitle = i.IsbnNavigation.Title,
                    Quantity = i.Quantity
                })
                .ToList();

            InventoryGrid.ItemsSource = inventoryData;
        }

        private void AddToStore_Click(object sender, RoutedEventArgs e)
        {
            // Get selected store, book and quantity
            if (StoreSelector.SelectedValue is int storeId &&
                BookSelector.SelectedValue is string isbn &&
                int.TryParse(QuantityTextBox.Text, out int quantity))
            {
                // Check if the book exists in the selected store's inventory. If it exist, add x quantity, If not, add it with the quantity that was chosen.
                var inventoryItem = _context.Inventories.FirstOrDefault(i => i.StoreId == storeId && i.Isbn == isbn);

                if (inventoryItem != null)
                {
                    inventoryItem.Quantity += quantity;
                }
                else
                {
                    _context.Inventories.Add(new Inventory
                    {
                        StoreId = storeId,
                        Isbn = isbn,
                        Quantity = quantity
                    });
                }

                _context.SaveChanges();
                FilterInventory();
            }
            else
            {
                MessageBox.Show("Please select [both] a store and a book.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void RemoveFromStore_Click(object sender, RoutedEventArgs e)
        {
            // Get selected store and book
            if (StoreSelector.SelectedValue is int storeId && BookSelector.SelectedValue is string isbn)
            {
                var inventoryItem = _context.Inventories.FirstOrDefault(i => i.StoreId == storeId && i.Isbn == isbn);

                if (inventoryItem != null)
                {
                    // If no quantity is specified
                    if (string.IsNullOrWhiteSpace(QuantityTextBox.Text))
                    {
                        var result = MessageBox.Show("Are you sure you want to remove the book entirely from the selected store's inventory?",
                                                      "Confirm removal", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            _context.Inventories.Remove(inventoryItem);
                            _context.SaveChanges();
                            FilterInventory();
                        }
                    }
                    else
                    {
                        if (int.TryParse(QuantityTextBox.Text, out int quantity))
                        {
                            if (quantity > inventoryItem.Quantity)
                            {
                                MessageBox.Show("Quantity to remove is higher than the current quantity in store. Please enter an amount lower or equal to the current quantity", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else
                            {
                                inventoryItem.Quantity -= quantity;
                            }

                            _context.SaveChanges();
                            FilterInventory();
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid number of quantity.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("The selected book doesn't exist in the selected store.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select [both] a store and a book.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InventoryOverview_Click(object sender, RoutedEventArgs e)
        {
            StoreSelector.SelectedItem = null;
            BookSelector.SelectedItem = null;
            LoadInventory();
        }
        private void EmptySelection_Click(object sender, RoutedEventArgs e)
        {
            StoreSelector.SelectedItem = null;
            BookSelector.SelectedItem = null;
            FilterInventory();
        }

        private void StoreSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            FilterInventory();
        }

        private void BookSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            FilterInventory();
        }
    }
}