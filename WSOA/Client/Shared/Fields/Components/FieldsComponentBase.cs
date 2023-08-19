using Microsoft.AspNetCore.Components;

namespace WSOA.Client.Shared.Fields.Components
{
    public class FieldsComponentBase : ComponentBase
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
    }
}
