using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugStoreStatistics.Models;

public class Medicine
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }          // Загальна кількість
    public int SoldQuantity { get; set; }      // Кількість проданих
    public DateTime ExpirationDate { get; set; }
    public int CabinetNumber { get; set; }
    public int ShelfNumber { get; set; }
    public string? ImageUrl { get; set; }      // Посилання на фото

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}
