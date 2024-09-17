using System.Windows;
using ContactBookTest.ViewModels;

namespace ContactBookTest;

public partial class EditContactWindow : Window
{
    public EditContactWindow(EditContactViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}