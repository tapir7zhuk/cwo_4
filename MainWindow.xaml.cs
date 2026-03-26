using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DrugStoreStatistics.Services;
using DrugStoreStatistics.ViewModels;
using System.Windows;

namespace DrugStoreStatistics;

public partial class MainWindow : Window
{
    public MainWindow(MedicineService medicineService, CategoryService categoryService)
    {
        InitializeComponent();
        DataContext = new MainViewModel(medicineService, categoryService);
    }
}