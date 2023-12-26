using Assets.Scripts.Objects.Interfaces;

namespace Assets.Scripts.Objects.Dummy
{
    internal class ChoppableDummy : IChoppable
    {
        bool IChoppable.IsChoppable => false;

        float IChoppable.ChopTotalActionTime => 9f;

        float IChoppable.ChopActionTimeConsumed => 0f;

        void IChoppable.SetChopActionTimeConsumed(float newChopActionTimeConsumed)
        {
            return;
        }
    }
}
