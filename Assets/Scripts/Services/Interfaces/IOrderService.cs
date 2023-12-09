using Assets.Scripts.Objects.Interfaces;

namespace Assets.Scripts.Services.Interfaces
{
    internal interface IOrderService
    {
        void ProcessReceivedRecipe(IRecipe recipeReceived);
    }
}
