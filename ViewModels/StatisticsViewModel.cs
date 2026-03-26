using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrugStoreStatistics.Models;
using DrugStoreStatistics.Services;

namespace DrugStoreStatistics.ViewModels;

public class StatisticsViewModel : BaseViewModel
{
    private readonly MedicineService _medicineService;
    private readonly CategoryService _categoryService;

    private List<Medicine> _expiringSoon = new();
    public List<Medicine> ExpiringSoon
    {
        get => _expiringSoon;
        set => Set(ref _expiringSoon, value);
    }

    private List<Medicine> _top5Expensive = new();
    public List<Medicine> Top5Expensive
    {
        get => _top5Expensive;
        set => Set(ref _top5Expensive, value);
    }

    private List<Medicine> _top5Cheapest = new();
    public List<Medicine> Top5Cheapest
    {
        get => _top5Cheapest;
        set => Set(ref _top5Cheapest, value);
    }

    private string _topCategoryDisplay = "";
    public string TopCategoryDisplay
    {
        get => _topCategoryDisplay;
        set => Set(ref _topCategoryDisplay, value);
    }

    public StatisticsViewModel(MedicineService medicineService, CategoryService categoryService)
    {
        _medicineService = medicineService;
        _categoryService = categoryService;
    }

    // Викликається при переході на вкладку, щоб дані завжди були актуальними
    public void Refresh()
    {
        ExpiringSoon = _medicineService.GetExpiringSoon();
        Top5Expensive = _medicineService.GetTop5Expensive();
        Top5Cheapest = _medicineService.GetTop5Cheapest();

        var topCategory = _categoryService.GetCategoryWithHighestTotalValue();
        TopCategoryDisplay = topCategory != null
            ? $"{topCategory.Name}  —  " +
              $"Загальна вартість: {topCategory.Medicines.Sum(m => m.Price * m.Quantity):N2} грн  |  " +
              $"Кількість ліків: {topCategory.Medicines.Count}"
            : "Дані відсутні";
    }
}