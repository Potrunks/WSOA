using Microsoft.AspNetCore.Components;

namespace WSOA.Client.Shared.Fields.Components
{
    public class DateFieldsComponent : ComponentBase
    {
        [Parameter]
        [EditorRequired]
        public string Label { get; set; }

        [Parameter]
        public DateTime InputValue
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

        private DateTime _inputValue;

        [Parameter]
        public EventCallback<DateTime> InputValueChanged { get; set; }
    }
}
