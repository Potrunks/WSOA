using Microsoft.AspNetCore.Components;

namespace WSOA.Client.Shared.Fields.Selects.NumberText.Components
{
    public class IntegerTextSelectFieldComponent : ComponentBase
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
                if (OnInputValueChange != null)
                {
                    OnInputValueChange.Value.InvokeAsync(value);
                }
            }
        }

        private int _inputValue;

        [Parameter]
        public EventCallback<int> InputValueChanged { get; set; }

        [Parameter]
        public EventCallback<int>? OnInputValueChange { get; set; }

        [Parameter]
        [EditorRequired]
        public IDictionary<int, string> Options { get; set; }
    }
}
