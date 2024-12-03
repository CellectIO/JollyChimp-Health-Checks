using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JollyChimp.Core.Common.Constants.Data;

namespace JollyChimp.Core.Common.Entities;

[Table(SqlTableConstants.EndPoints)]
public class EndPointEntity : BaseEntity
{

    [Required]
    [StringLength(64)]
    public string ApiPath { get; set; }

    [Required]
    [StringLength(512)]
    public string HealthChecksPredicate { get; set; }

}
