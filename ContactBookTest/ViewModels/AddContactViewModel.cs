using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using ContactBookTest.Commands;
using ContactBookTest.Models;

namespace ContactBookTest.ViewModels;

public class AddContactViewModel : INotifyPropertyChanged
{
    private Contact _contact;
    private readonly ContactViewModel _parentViewModel;
    private readonly Window _addWindow;
    public ObservableCollection<string> ContactTypes { get; }

    public Contact Contact
    {
        get => _contact;
        set
        {
            _contact = value;
            OnPropertyChanged();
        }
    }

    public AsyncRelayCommand SaveCommand { get; }

    public AddContactViewModel(ContactViewModel parentViewModel, Window addWindow, ObservableCollection<string> contactTypes)
    {
        _contact = new Contact();
        _parentViewModel = parentViewModel;
        _addWindow = addWindow;
        ContactTypes = contactTypes;
        SaveCommand = new AsyncRelayCommand(SaveContactAsync);
    }

    private async Task SaveContactAsync()
    {
        if (!ValidateContact())
        {
            MessageBox.Show("Contact data is invalid. Please correct the errors.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        await _parentViewModel.SaveContactAsync(Contact);
        _addWindow.Close();
    }

    private bool ValidateContact()
    {
        if (string.IsNullOrWhiteSpace(Contact.FirstName)) return false;
        if (string.IsNullOrWhiteSpace(Contact.LastName)) return false;
        if (string.IsNullOrWhiteSpace(Contact.PhoneNumber) || !Regex.IsMatch(Contact.PhoneNumber, @"^\+?\d{10,15}$")) return false;
        if (string.IsNullOrWhiteSpace(Contact.Email) || !Regex.IsMatch(Contact.Email, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")) return false;
        if (string.IsNullOrWhiteSpace(Contact.ContactType)) return false;

        return true;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}