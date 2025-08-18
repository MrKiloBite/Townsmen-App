using EsnafYonetim.BLL.Managers;
using EsnafYonetim.Core.Models;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EsnafYonetim.UI.ViewModels
{
    public partial class AccountingViewModel : ViewModelBase
    {
        private readonly AccountingManager _accountingManager;

        public ObservableCollection<Muhasebe> Transactions { get; } = new();

        public AccountingViewModel()
        {
            _accountingManager = new AccountingManager();
            _ = LoadTransactionsAsync();
        }

        [RelayCommand]
        public async Task LoadTransactionsAsync()
        {
            Transactions.Clear();
            var transactionsFromDb = await _accountingManager.GetAllTransactionsAsync();
            foreach (var transaction in transactionsFromDb)
            {
                Transactions.Add(transaction);
            }
        }
    }
}
