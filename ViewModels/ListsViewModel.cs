using DrugStoreStatistics.Models;
using DrugStoreStatistics.Services;
using System.Collections.ObjectModel;
using System.Windows;

namespace DrugStoreStatistics.ViewModels;

public class ListsViewModel : BaseViewModel
{
    private readonly MedicineService _medicineService;
    private readonly CategoryService _categoryService;

    public ObservableCollection<Medicine> Medicines { get; set; } = new();
    public ObservableCollection<Category> Categories { get; set; } = new();

    // ===== Вибрані елементи =====

    private Medicine? _selectedMedicine;
    public Medicine? SelectedMedicine
    {
        get => _selectedMedicine;
        set { Set(ref _selectedMedicine, value); LoadMedicineForm(); }
    }

    private Category? _selectedCategory;
    public Category? SelectedCategory
    {
        get => _selectedCategory;
        set { Set(ref _selectedCategory, value); LoadCategoryForm(); }
    }

    // ===== Форма ліків =====

    private string _medicineName = "";
    public string MedicineName
    {
        get => _medicineName;
        set => Set(ref _medicineName, value);
    }

    private string _medicineManufacturer = "";
    public string MedicineManufacturer
    {
        get => _medicineManufacturer;
        set => Set(ref _medicineManufacturer, value);
    }

    private string _medicinePriceText = "";
    public string MedicinePriceText
    {
        get => _medicinePriceText;
        set => Set(ref _medicinePriceText, value);
    }

    private string _medicineQuantityText = "";
    public string MedicineQuantityText
    {
        get => _medicineQuantityText;
        set => Set(ref _medicineQuantityText, value);
    }

    private string _medicineSoldQuantityText = "";
    public string MedicineSoldQuantityText
    {
        get => _medicineSoldQuantityText;
        set => Set(ref _medicineSoldQuantityText, value);
    }

    private DateTime _medicineExpirationDate = DateTime.Now.AddMonths(6);
    public DateTime MedicineExpirationDate
    {
        get => _medicineExpirationDate;
        set => Set(ref _medicineExpirationDate, value);
    }

    private string _medicineCabinetText = "";
    public string MedicineCabinetText
    {
        get => _medicineCabinetText;
        set => Set(ref _medicineCabinetText, value);
    }

    private string _medicineShelfText = "";
    public string MedicineShelfText
    {
        get => _medicineShelfText;
        set => Set(ref _medicineShelfText, value);
    }

    private string _medicineImageUrl = "";
    public string MedicineImageUrl
    {
        get => _medicineImageUrl;
        set => Set(ref _medicineImageUrl, value);
    }

    private Category? _medicineSelectedCategory;
    public Category? MedicineSelectedCategory
    {
        get => _medicineSelectedCategory;
        set => Set(ref _medicineSelectedCategory, value);
    }

    // ===== Форма категорій =====

    private string _categoryName = "";
    public string CategoryName
    {
        get => _categoryName;
        set => Set(ref _categoryName, value);
    }

    private string _categoryDescription = "";
    public string CategoryDescription
    {
        get => _categoryDescription;
        set => Set(ref _categoryDescription, value);
    }

    // ===== Команди =====

    public RelayCommand SaveMedicineCommand { get; }
    public RelayCommand DeleteMedicineCommand { get; }
    public RelayCommand ClearMedicineFormCommand { get; }

    public RelayCommand SaveCategoryCommand { get; }
    public RelayCommand DeleteCategoryCommand { get; }
    public RelayCommand ClearCategoryFormCommand { get; }

    public ListsViewModel(MedicineService medicineService, CategoryService categoryService)
    {
        _medicineService = medicineService;
        _categoryService = categoryService;

        SaveMedicineCommand = new RelayCommand(_ => SaveMedicine());
        DeleteMedicineCommand = new RelayCommand(_ => DeleteMedicine(),
            _ => SelectedMedicine != null);
        ClearMedicineFormCommand = new RelayCommand(_ => ClearMedicineForm());

        SaveCategoryCommand = new RelayCommand(_ => SaveCategory());
        DeleteCategoryCommand = new RelayCommand(_ => DeleteCategory(),
            _ => SelectedCategory != null);
        ClearCategoryFormCommand = new RelayCommand(_ => ClearCategoryForm());

        LoadData();
    }

    public void LoadData()
    {
        Medicines = new ObservableCollection<Medicine>(_medicineService.GetAll());
        Categories = new ObservableCollection<Category>(_categoryService.GetAll());
        OnPropertyChanged(nameof(Medicines));
        OnPropertyChanged(nameof(Categories));
    }

    // ===== Заповнення форм при виборі =====

    private void LoadMedicineForm()
    {
        if (SelectedMedicine == null) return;
        MedicineName = SelectedMedicine.Name;
        MedicineManufacturer = SelectedMedicine.Manufacturer;
        MedicinePriceText = SelectedMedicine.Price.ToString();
        MedicineQuantityText = SelectedMedicine.Quantity.ToString();
        MedicineSoldQuantityText = SelectedMedicine.SoldQuantity.ToString();
        MedicineExpirationDate = SelectedMedicine.ExpirationDate;
        MedicineCabinetText = SelectedMedicine.CabinetNumber.ToString();
        MedicineShelfText = SelectedMedicine.ShelfNumber.ToString();
        MedicineImageUrl = SelectedMedicine.ImageUrl ?? "";
        MedicineSelectedCategory = Categories
            .FirstOrDefault(c => c.Id == SelectedMedicine.CategoryId);
    }

    private void LoadCategoryForm()
    {
        if (SelectedCategory == null) return;
        CategoryName = SelectedCategory.Name;
        CategoryDescription = SelectedCategory.Description;
    }

    // ===== CRUD ліків =====

    private void SaveMedicine()
    {
        if (!decimal.TryParse(MedicinePriceText, out var price) ||
            !int.TryParse(MedicineQuantityText, out var qty) ||
            !int.TryParse(MedicineSoldQuantityText, out var sold) ||
            !int.TryParse(MedicineCabinetText, out var cabinet) ||
            !int.TryParse(MedicineShelfText, out var shelf) ||
            MedicineSelectedCategory == null ||
            string.IsNullOrWhiteSpace(MedicineName))
        {
            MessageBox.Show("Будь ласка, заповніть усі обов'язкові поля коректно.",
                "Помилка валідації");
            return;
        }

        if (SelectedMedicine == null)
        {
            _medicineService.Add(new Medicine
            {
                Name = MedicineName,
                Manufacturer = MedicineManufacturer,
                Price = price,
                Quantity = qty,
                SoldQuantity = sold,
                ExpirationDate = MedicineExpirationDate,
                CabinetNumber = cabinet,
                ShelfNumber = shelf,
                ImageUrl = string.IsNullOrWhiteSpace(MedicineImageUrl)
                    ? null : MedicineImageUrl,
                CategoryId = MedicineSelectedCategory.Id
            });
        }
        else
        {
            SelectedMedicine.Name = MedicineName;
            SelectedMedicine.Manufacturer = MedicineManufacturer;
            SelectedMedicine.Price = price;
            SelectedMedicine.Quantity = qty;
            SelectedMedicine.SoldQuantity = sold;
            SelectedMedicine.ExpirationDate = MedicineExpirationDate;
            SelectedMedicine.CabinetNumber = cabinet;
            SelectedMedicine.ShelfNumber = shelf;
            SelectedMedicine.ImageUrl = string.IsNullOrWhiteSpace(MedicineImageUrl)
                ? null : MedicineImageUrl;
            SelectedMedicine.CategoryId = MedicineSelectedCategory.Id;
            _medicineService.Update(SelectedMedicine);
        }

        ClearMedicineForm();
        LoadData();
    }

    private void DeleteMedicine()
    {
        if (SelectedMedicine == null) return;
        if (MessageBox.Show($"Видалити '{SelectedMedicine.Name}'?", "Підтвердження",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            _medicineService.Delete(SelectedMedicine.Id);
            ClearMedicineForm();
            LoadData();
        }
    }

    private void ClearMedicineForm()
    {
        SelectedMedicine = null;
        MedicineName = "";
        MedicineManufacturer = "";
        MedicinePriceText = "";
        MedicineQuantityText = "";
        MedicineSoldQuantityText = "";
        MedicineExpirationDate = DateTime.Now.AddMonths(6);
        MedicineCabinetText = "";
        MedicineShelfText = "";
        MedicineImageUrl = "";
        MedicineSelectedCategory = null;
    }

    // ===== CRUD категорій =====

    private void SaveCategory()
    {
        if (string.IsNullOrWhiteSpace(CategoryName))
        {
            MessageBox.Show("Введіть назву категорії.", "Помилка валідації");
            return;
        }

        if (SelectedCategory == null)
        {
            _categoryService.Add(new Category
            {
                Name = CategoryName,
                Description = CategoryDescription
            });
        }
        else
        {
            SelectedCategory.Name = CategoryName;
            SelectedCategory.Description = CategoryDescription;
            _categoryService.Update(SelectedCategory);
        }

        ClearCategoryForm();
        LoadData();
    }

    private void DeleteCategory()
    {
        if (SelectedCategory == null) return;
        if (MessageBox.Show($"Видалити категорію '{SelectedCategory.Name}'?",
                "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            _categoryService.Delete(SelectedCategory.Id);
            ClearCategoryForm();
            LoadData();
        }
    }

    private void ClearCategoryForm()
    {
        SelectedCategory = null;
        CategoryName = "";
        CategoryDescription = "";
    }
}