using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrugStoreStatistics.Data;
using DrugStoreStatistics.Models;
using Microsoft.EntityFrameworkCore;

namespace DrugStoreStatistics.Services;

public class CategoryService
{
    private readonly AppDbContext _db;

    public CategoryService(AppDbContext db) => _db = db;

    public List<Category> GetAll() =>
        _db.Categories.Include(c => c.Medicines).ToList();

    public void Add(Category category)
    {
        _db.Categories.Add(category);
        _db.SaveChanges();
    }

    public void Update(Category category)
    {
        _db.Categories.Update(category);
        _db.SaveChanges();
    }

    public void Delete(int id)
    {
        var category = _db.Categories.Find(id);
        if (category != null)
        {
            _db.Categories.Remove(category);
            _db.SaveChanges();
        }
    }

    // Категорія з найбільшою загальною вартістю (ціна * кількість)
    public Category? GetCategoryWithHighestTotalValue() =>
        _db.Categories
            .Include(c => c.Medicines)
            .OrderByDescending(c => c.Medicines.Sum(m => m.Price * m.Quantity))
            .FirstOrDefault();
}