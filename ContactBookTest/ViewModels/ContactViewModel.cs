using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ContactBookTest.Commands;
using ContactBookTest.Data;
using ContactBookTest.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactBookTest.ViewModels;

public class ContactViewModel : INotifyPropertyChanged
{
    private Contact _selectedContact;
    private readonly ContactContext _context;
    private string _searchText;
    private string _selectedContactType;
    private Task _searchTask;
    private readonly object _searchLock = new object();

    public ObservableCollection<Contact> Contacts { get; set; }
    public ObservableCollection<string> ContactTypes { get; set; }

    public Contact SelectedContact
    {
        get => _selectedContact;
        set
        {
            _selectedContact = value;
            OnPropertyChanged(nameof(SelectedContact));
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged(nameof(SearchText));
            StartSearchTask();
        }
    }

    public string SelectedContactType
    {
        get => _selectedContactType;
        set
        {
            _selectedContactType = value;
            OnPropertyChanged(nameof(SelectedContactType));
            StartSearchTask();
        }
    }

    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand SortCommand { get; }

    public ContactViewModel()
    {
        _context = new ContactContext();
        Contacts = new ObservableCollection<Contact>();
        ContactTypes = new ObservableCollection<string> { "All", "Friend", "Coworker", "Unknowned" };

        AddCommand = new AsyncRelayCommand(AddContactAsync);
        EditCommand = new AsyncRelayCommand(EditContactAsync, CanEditDelete);
        DeleteCommand = new AsyncRelayCommand(DeleteContactAsync, CanEditDelete);
        SortCommand = new AsyncRelayCommand(SortContactsAsync);

        LoadContactsAsync();
    }

    private async Task LoadContactsAsync()
    {
        var contacts = await _context.Contacts.ToListAsync();
        Contacts.Clear();
        foreach (var contact in contacts)
        {
            Contacts.Add(contact);
        }
    }

    private async Task AddContactAsync()
    {
        var addWindow = new AddContactWindow(null);
        var addViewModel = new AddContactViewModel(this, addWindow, ContactTypes);
        addWindow.DataContext = addViewModel;
        addWindow.ShowDialog();
    }

    public async Task SaveContactAsync(Contact contact)
    {
        Contacts.Add(contact);
        await _context.Contacts.AddAsync(contact);
        await _context.SaveChangesAsync();
    }

    private async Task EditContactAsync()
    {
        if (SelectedContact != null)
        {
            var editWindow = new EditContactWindow(null);
            var editViewModel = new EditContactViewModel(SelectedContact, this, editWindow, ContactTypes);
            editWindow.DataContext = editViewModel;
            editWindow.ShowDialog();
        }
    }

    public async Task ApplyChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    private async Task DeleteContactAsync()
    {
        if (SelectedContact != null)
        {
            _context.Contacts.Remove(SelectedContact);
            Contacts.Remove(SelectedContact);
            await _context.SaveChangesAsync();
        }
    }

    private bool CanEditDelete()
    {
        return SelectedContact != null;
    }

    private void StartSearchTask()
    {
        lock (_searchLock)
        {
            if (_searchTask != null)
            {
                _searchTask.ContinueWith(_ => FindContactsAsync(SelectedContactType));
            }
            else
            {
                _searchTask = Task.Delay(500).ContinueWith(_ => FindContactsAsync(SelectedContactType));
            }
        }
    }

    private async Task FindContactsAsync(string contactType = null)
    {
        var searchText = _searchText?.ToLower() ?? string.Empty;
        var query = _context.Contacts.AsQueryable();

        if (!string.IsNullOrEmpty(contactType) && contactType != "All")
        {
            query = query.Where(c => c.ContactType == contactType);
        }

        if (!string.IsNullOrEmpty(searchText))
        {
            query = query.Where(c => c.FirstName.ToLower().Contains(searchText) ||
                                    c.LastName.ToLower().Contains(searchText) ||
                                    c.PhoneNumber.ToLower().Contains(searchText) ||
                                    c.Email.ToLower().Contains(searchText));
        }

        var foundContacts = await query.ToListAsync();

        Application.Current.Dispatcher.Invoke(() =>
        {
            Contacts.Clear();
            foreach (var contact in foundContacts)
            {
                Contacts.Add(contact);
            }
        });
    }

    private async Task SortContactsAsync()
    {
        var sortedContacts = Contacts.OrderBy(c => c.FirstName).ToList();
        Contacts.Clear();
        foreach (var contact in sortedContacts)
        {
            Contacts.Add(contact);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}