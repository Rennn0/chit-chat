using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Source.Db.Models;

public class TenantConfiguration
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public decimal Price { get; set; }

    public TENANT_TYPE Type { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTimeOffset CreatedTime { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTimeOffset UpdatedTime { get; set; }

    public enum TENANT_TYPE
    {
        Free,
        Basic,
        Premium,
    }
}
