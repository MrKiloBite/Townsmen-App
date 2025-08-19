using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EsnafYonetim.BLL.Managers;
using EsnafYonetim.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EsnafYonetim.UI.ViewModels
{
    public partial class BildirimAddEditViewModel : ViewModelBase
    {
        [ObservableProperty]
        private Bildirim _bildirim;

        private readonly BildirimManager _bildirimManager;
        private readonly bool _isNew;

        public ObservableCollection<BildirimTipi> BildirimTipleri { get; }
        public ObservableCollection<KarsilastirmaOperatoru> Operatorler { get; }

        [ObservableProperty]
        private bool _isZamanlanmisVisible;

        [ObservableProperty]
        private bool _isStokSeviyesiVisible;

        public event Action? OnRequestClose;

        public BildirimTipi Tip
        {
            get => Bildirim.Tip;
            set
            {
                if (SetProperty(Bildirim.Tip, value, Bildirim, (b, v) => b.Tip = v))
                {
                    UpdateVisibility();
                }
            }
        }

        public BildirimAddEditViewModel(Bildirim? bildirimToEdit)
        {
            _bildirimManager = new BildirimManager();
            _isNew = (bildirimToEdit == null);

            BildirimTipleri = new ObservableCollection<BildirimTipi>(Enum.GetValues<BildirimTipi>());
            Operatorler = new ObservableCollection<KarsilastirmaOperatoru>(Enum.GetValues<KarsilastirmaOperatoru>());

            if (_isNew)
            {
                _bildirim = new Bildirim { OlusturmaTarihi = DateTime.Now, Aktif = true, Tip = BildirimTipi.Zamanlanmis };
            }
            else
            {
                _bildirim = bildirimToEdit!;
            }
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            IsZamanlanmisVisible = Tip == BildirimTipi.Zamanlanmis;
            IsStokSeviyesiVisible = Tip == BildirimTipi.StokSeviyesi;
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (_isNew)
            {
                await _bildirimManager.AddAsync(Bildirim);
            }
            else
            {
                await _bildirimManager.UpdateAsync(Bildirim);
            }
            OnRequestClose?.Invoke();
        }

        [RelayCommand]
        private void Cancel()
        {
            OnRequestClose?.Invoke();
        }

        [RelayCommand]
        private void BrowseSoundFile()
        {
            // TODO: Avalonia's OpenFileDialog'u kullanarak ses dosyası seçme mantığı eklenecek.
        }
    }
}
