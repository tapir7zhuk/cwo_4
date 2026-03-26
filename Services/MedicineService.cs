using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrugStoreStatistics.Data;
using DrugStoreStatistics.Models;
using Microsoft.EntityFrameworkCore;

namespace DrugStoreStatistics.Services;

public class MedicineService
{
    private readonly AppDbContext _db;

    public MedicineService(AppDbContext db) => _db = db;

    public List<Medicine> GetAll() =>
        _db.Medicines.Include(m => m.Category).ToList();

    public void Add(Medicine medicine)
    {
        _db.Medicines.Add(medicine);
        _db.SaveChanges();
    }

    public void Update(Medicine medicine)
    {
        _db.Medicines.Update(medicine);
        _db.SaveChanges();
    }

    public void Delete(int id)
    {
        var medicine = _db.Medicines.Find(id);
        if (medicine != null)
        {
            _db.Medicines.Remove(medicine);
            _db.SaveChanges();
        }
    }

    // Ліки, термін придатності яких менше тижня
    public List<Medicine> GetExpiringSoon()
    {
        var threshold = DateTime.Now.AddDays(7);
        return _db.Medicines
            .Include(m => m.Category)
            .Where(m => m.ExpirationDate <= threshold && m.ExpirationDate >= DateTime.Now)
            .OrderBy(m => m.ExpirationDate)
            .ToList();
    }

    // Топ N найбільш продаваних (N регулюється, за замовчуванням 3)
    public List<Medicine> GetTopSelling(int count = 3) =>
        _db.Medicines
            .Include(m => m.Category)
            .OrderByDescending(m => m.SoldQuantity)
            .Take(count)
            .ToList();

    // 5 найдорожчих
    public List<Medicine> GetTop5Expensive() =>
        _db.Medicines.Include(m => m.Category)
            .OrderByDescending(m => m.Price)
            .Take(5).ToList();

    // 5 найдешевших
    public List<Medicine> GetTop5Cheapest() =>
        _db.Medicines.Include(m => m.Category)
            .OrderBy(m => m.Price)
            .Take(5).ToList();
}
