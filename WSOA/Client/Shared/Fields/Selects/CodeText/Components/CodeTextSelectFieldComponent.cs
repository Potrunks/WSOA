using Microsoft.AspNetCore.Components;
using WSOA.Client.Shared.Fields.Components;

namespace WSOA.Client.Shared.Fields.Selects.CodeText.Components
{
    public class CodeTextSelectFieldComponent : FieldsComponentBase
    {
        [Parameter]
        [EditorRequired]
        public bool IsLoadingListener { get; set; }

        [Parameter]
        public IDictionary<string, string> Options { get; set; }
    }
}
