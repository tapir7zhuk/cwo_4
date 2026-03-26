using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrugStoreStatistics.Services;

namespace DrugStoreStatistics.ViewModels;

public class MainViewModel : BaseViewModel
{
    public ListsViewModel Lists { get; }
    public StatisticsViewModel Statistics { get; }
    public TopSellingViewModel TopSelling { get; }

    private int _selectedTabIndex;
    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set
        {
            if (Set(ref _selectedTabIndex, value))
                OnTabChanged(value);
        }
    }

    public MainViewModel(MedicineService medicineService, CategoryService categoryService)
    {
        Lists = new ListsViewModel(medicineService, categoryService);
        Statistics = new StatisticsViewModel(medicineService, categoryService);
        TopSelling = new TopSellingViewModel(medicineService);

        // Третя вкладка відкривається разом із програмою
        TopSelling.Refresh();
    }

    private void OnTabChanged(int index)
    {
        switch (index)
        {
            case 1:
                Statistics.Refresh();
                break;
            case 2:
                TopSelling.Refresh();
                break;
        }
    }
}