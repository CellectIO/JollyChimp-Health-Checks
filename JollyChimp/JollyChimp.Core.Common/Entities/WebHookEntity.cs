using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JollyChimp.Core.Common.Constants.Data;
using JollyChimp.Core.Common.Enums;

namespace JollyChimp.Core.Common.Entities;

[Table(SqlTableConstants.WebHooks)]
public class WebHookEntity : BaseEntity
{
    [Required]
    public WebHookTypes Type { get; set; }

    public virtual IList<WebHookParameterEntity> WebHookParameters { get; set; }
}
