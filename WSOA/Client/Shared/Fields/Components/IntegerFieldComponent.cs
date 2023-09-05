using Microsoft.AspNetCore.Components;

namespace WSOA.Client.Shared.Fields.Components
{
    public class IntegerFieldComponent : ComponentBase
    {
        [Parameter]
        [EditorRequired]
        public string Label { get; set; }

        [Parameter]
        public int InputValue
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

        private int _inputValue;

        [Parameter]
        public EventCallback<int> InputValueChanged { get; set; }
    }
}
