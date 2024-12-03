using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JollyChimp.Core.Common.Constants.Data;
using JollyChimp.Core.Common.Encryption;
using Microsoft.EntityFrameworkCore;

namespace JollyChimp.Core.Common.Entities;

[Table(SqlTableConstants.WebHookParameters)]
public class WebHookParameterEntity : BaseEntity
{

    [Required]
    [StringLength(4096)]
    [EncryptProperty]
    public string Value { get; set; }

    [Required]
    public int WebHookId { get; set; }

    [ForeignKey(nameof(WebHookId))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public virtual WebHookEntity WebHook { get; set; }

}
