using System.ComponentModel.DataAnnotations;

namespace EfcoreConcurrency.Model;

public class Product1
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Stock { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }
}

