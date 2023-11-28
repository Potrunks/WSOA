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

        public IEnumerable<IdSelectableViewModel>? SelectableIds { get; set; }

        public IEnumerable<CodeSelectableViewModel>? SelectableCodes { get; set; }

        public Action? OnValid { get; set; }

        public EventCallback<IEnumerable<int>>? OnValidSelectedIds { get; set; }

        public Action<int, int, bool>? OnValidSelectedId { get; set; }

        public Action<string, int>? OnValidSelectedCode { get; set; }

        public IEnumerable<PopupButtonViewModel>? Buttons { get; set; }

        public int? ConcernedId { get; set; }

        public OptionViewModel? Option { get; set; }
    }
}
