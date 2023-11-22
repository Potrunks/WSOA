using Microsoft.AspNetCore.Components;

namespace WSOA.Client.Shared.Fields.Checkboxes.Components
{
    public class IconCheckboxComponent : ComponentBase
    {
        [Parameter]
        public bool Value
        {
            get => _value;
            set
            {
                if (_value == value)
                {
                    return;
                }
                _value = value;
                ValueChanged.InvokeAsync(value);
            }
        }
        private bool _value;

        [EditorRequired]
        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public EventCallback<bool> ValueChanged { get; set; }

        public EventCallback SwitchValue => EventCallback.Factory.Create(this, () =>
        {
            Value = !Value;
        });
    }
}
