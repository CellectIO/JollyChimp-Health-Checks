using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JollyChimp.Core.Common.Constants.Data;
using JollyChimp.Core.Common.Enums;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace JollyChimp.Core.Common.Entities;

[Table(SqlTableConstants.HealthChecks)]
public class HealthCheckEntity : BaseEntity
{

    [Required]
    public HealthCheckType Type { get; set; }

    [Required]
    public HealthStatus HealthStatus { get; set; }

    [Required]
    [StringLength(512)]
    public string Tags { get; set; }

    public virtual IList<HealthCheckParameterEntity> HealthCheckParameters { get; set; }

}
