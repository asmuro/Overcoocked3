using Assets.Scripts.Objects.Interfaces;

namespace Assets.Scripts.Objects.Dummy
{
    internal class CookableDummy : ICookable
    {
        public bool IsCookable => false;

        public float CookPercentage => 0f;

        public float CookTime => 0f;

        public float CookActionTimeConsumed => 0f;

        public float FromCookTimeToBurn => 0f;

        public float StartAlertTimeBeforeBurn => 0f;

        public void SetCookActionTimeConsumed(float newChopActionTimeConsumed)
        {
            return;
        }
    }
}
