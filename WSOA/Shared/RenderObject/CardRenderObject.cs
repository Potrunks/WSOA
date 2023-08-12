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

        public CardRenderObject(CardRenderObject cardAlreadyGiven)
        {
            Value = CardValueResources.VALUES.GetRandomElement(new Random());
            IconFileName = CardValueResources.ICON_FILE_NAMES.GetRandomElement(new Random());

            if (cardAlreadyGiven != null)
            {
                while (cardAlreadyGiven.Value == Value && cardAlreadyGiven.IconFileName == IconFileName)
                {
                    Value = CardValueResources.VALUES.GetRandomElement(new Random());
                    IconFileName = CardValueResources.ICON_FILE_NAMES.GetRandomElement(new Random());
                }
            }
        }

        public string Value { get; set; }

        public string IconFileName { get; set; }
    }
}
