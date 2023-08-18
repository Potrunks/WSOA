using Microsoft.AspNetCore.Components;

namespace WSOA.Client.Shared.Fields.Selects.CodeText.Components
{
    public class CodeTextSelectFieldComponent : ComponentBase
    {
        [Parameter]
        [EditorRequired]
        public string Label { get; set; }

        [Parameter]
        public string InputValue
        {
            get => _inputValue;
            set
            {
                if (_inputValue == value)
                {
                    return;
                }
                _inputValue = value;
                InputValueChanged.InvokeAsync(value);
            }
        }

        private string _inputValue;

        [Parameter]
        public EventCallback<string> InputValueChanged { get; set; }

        [Parameter]
        [EditorRequired]
        public bool IsLoadingListener { get; set; }

        [Parameter]
        public IDictionary<string, string> Options { get; set; }
    }
}
