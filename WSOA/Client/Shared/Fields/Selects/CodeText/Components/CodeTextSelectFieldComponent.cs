using Microsoft.AspNetCore.Components;
using WSOA.Client.Shared.Fields.Components;

namespace WSOA.Client.Shared.Fields.Selects.CodeText.Components
{
    public class CodeTextSelectFieldComponent : FieldsComponentBase
    {
        [Parameter]
        [EditorRequired]
        public IDictionary<string, string> Options { get; set; }
    }
}
