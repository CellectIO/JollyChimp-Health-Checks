using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JollyChimp.Core.Common.Constants.Data;

namespace JollyChimp.Core.Common.Entities;

[Table(SqlTableConstants.ServerSettings)]
public class ServerSettingEntity : BaseEntity
{
    [Required]
    [StringLength(512)]
    public string Value { get; set; }
}
