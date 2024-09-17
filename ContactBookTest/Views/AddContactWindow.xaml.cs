using System.Windows;
using ContactBookTest.ViewModels;

namespace ContactBookTest;

public partial class AddContactWindow : Window
{
    public AddContactWindow(AddContactViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}