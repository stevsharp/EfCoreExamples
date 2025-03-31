
using System.ComponentModel.DataAnnotations;

namespace EfcoreConcurrency.Model;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }

    [ConcurrencyCheck]
    public int Stock { get; set; }
}

