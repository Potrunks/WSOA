using Microsoft.AspNetCore.Components;
using WSOA.Shared.Resources;
using WSOA.Shared.ViewModel;

namespace WSOA.Client.Shared.EventHandlers
{
    public class PopupEventArgs : EventArgs
    {
        public PopupKeyResources? Key { get; set; }

        public string? Title { get; set; }

        public IEnumerable<MessageViewModel>? Messages { get; set; }

        public IEnumerable<ItemSelectableViewModel>? SelectableItems { get; set; }

        public Action? OnValid { get; set; }

        public EventCallback<IEnumerable<int>>? OnValidSelectedItemIds { get; set; }
    }
}
