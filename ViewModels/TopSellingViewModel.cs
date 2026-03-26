using DrugStoreStatistics.Models;
using DrugStoreStatistics.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DrugStoreStatistics.ViewModels;

public class TopSellingViewModel : BaseViewModel
{
    private readonly MedicineService _medicineService;

    // Регульована кількість топу (за замовчуванням 3)
    private int _topCount = 3;
    public int TopCount
    {
        get => _topCount;
        set
        {
            if (Set(ref _topCount, value))
                Refresh();
        }
    }

    private ObservableCollection<Medicine> _topMedicines = new();
    public ObservableCollection<Medicine> TopMedicines
    {
        get => _topMedicines;
        set => Set(ref _topMedicines, value);
    }

    // Вибраний препарат для показу фото
    private Medicine? _selectedMedicine;
    public Medicine? SelectedMedicine
    {
        get => _selectedMedicine;
        set
        {
            if (Set(ref _selectedMedicine, value))
                _ = LoadImageAsync();
        }
    }

    // Поточне зображення для показу
    private BitmapImage? _currentImage;
    public BitmapImage? CurrentImage
    {
        get => _currentImage;
        set => Set(ref _currentImage, value);
    }

    private bool _isLoadingImage;
    public bool IsLoadingImage
    {
        get => _isLoadingImage;
        set => Set(ref _isLoadingImage, value);
    }

    private CancellationTokenSource? _imageCts;

    public TopSellingViewModel(MedicineService medicineService)
    {
        _medicineService = medicineService;
    }

    // Викликається при переході на вкладку
    public void Refresh()
    {
        var medicines = _medicineService.GetTopSelling(TopCount);
        TopMedicines = new ObservableCollection<Medicine>(medicines);

        // Автоматично обираємо перший елемент
        SelectedMedicine = TopMedicines.FirstOrDefault();
    }

    private async Task LoadImageAsync()
    {
        // Скасовуємо попереднє завантаження
        _imageCts?.Cancel();
        _imageCts = new CancellationTokenSource();
        var token = _imageCts.Token;

        CurrentImage = null;

        if (SelectedMedicine == null ||
            string.IsNullOrWhiteSpace(SelectedMedicine.ImageUrl))
        {
            IsLoadingImage = false;
            return;
        }

        var url = SelectedMedicine.ImageUrl;

        // Перевіряємо кеш — якщо є, показуємо одразу
        if (App.ImageCache.TryGetValue(url, out var cached))
        {
            CurrentImage = cached;
            return;
        }

        try
        {
            IsLoadingImage = true;

            // Затримка 150мс — якщо користувач швидко перемикається, не завантажуємо зайве
            await Task.Delay(150, token);
            if (token.IsCancellationRequested) return;

            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

            var bytes = await client.GetByteArrayAsync(url, token);
            if (token.IsCancellationRequested) return;

            using var ms = new MemoryStream(bytes);
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze(); // Дозволяє використовувати з будь-якого потоку

            if (!token.IsCancellationRequested)
            {
                // Зберігаємо в кеш — повторне відкриття буде миттєвим
                App.ImageCache[url] = bitmap;
                CurrentImage = bitmap;
            }
        }
        catch { }
        finally
        {
            if (!token.IsCancellationRequested)
                IsLoadingImage = false;
        }
    }
}