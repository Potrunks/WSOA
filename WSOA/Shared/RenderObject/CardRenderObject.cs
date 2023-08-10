using WSOA.Client.Pages.SignIn.Resources;
using WSOA.Shared.Utils;

namespace WSOA.Shared.RenderObject
{
    public class CardRenderObject
    {
        public CardRenderObject()
        {
            Value = CardValueResources.VALUES.GetRandomElement(new Random());
            IconFileName = CardValueResources.ICON_FILE_NAMES.GetRandomElement(new Random());
        }

        public string Value { get; set; }

        public string IconFileName { get; set; }
    }
}
