using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JollyChimp.Core.Common.Entities;

public abstract class BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [Required]
    [StringLength(128)]
    public string Name { get; set; }

    /// <summary>
    /// Determines if the target entity is enabled for use by the system.
    /// </summary>
    /// <remarks>
    /// This is important because health check entities only take effect if the server restarts.
    /// </remarks>
    [Required]
    [DefaultValue(true)]
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Determines if the target entity has been deleted by the user.
    /// </summary>
    [Required]
    [DefaultValue(false)]
    public bool IsDeleted { get; set; }
}
