using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JollyChimp.Core.Common.Constants.Data;

namespace JollyChimp.Core.Common.Entities;

[Table(SqlTableConstants.DeleteQueue)]
public class DeleteQueueEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [Required]
    [MaxLength(64)]
    public string XabarilTableName { get; set; }
    
    [Required]
    public int XabarilId { get; set; }
    
    [Required]
    public bool IsDeleted { get; set; }

    public static DeleteQueueEntity CreateQueueEntity(string tableName, int id)
        => new()
        {
            XabarilTableName = tableName,
            XabarilId = id,
            IsDeleted = false
        };
}