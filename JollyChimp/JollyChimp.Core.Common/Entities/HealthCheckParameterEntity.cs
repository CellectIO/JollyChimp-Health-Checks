using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JollyChimp.Core.Common.Constants.Data;
using JollyChimp.Core.Common.Encryption;
using Microsoft.EntityFrameworkCore;

namespace JollyChimp.Core.Common.Entities;

[Table(SqlTableConstants.HealthCheckParameters)]
public class HealthCheckParameterEntity : BaseEntity
{

    [Required]
    [StringLength(4096)]
    [EncryptProperty]
    public string Value { get; set; }

    [Required]
    public int HealthCheckId { get; set; }

    [ForeignKey(nameof(HealthCheckId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual HealthCheckEntity HealthCheck { get; set; }

}
