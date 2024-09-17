using System.Configuration;
using System.Data;
using System.Windows;
using ContactBookTest.Data;

namespace ContactBookTest;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        using (var context = new ContactContext())
        {
            context.Database.EnsureCreated();
        }
    }
}