
namespace Assets.Scripts.Objects.Interfaces
{
    internal interface ICookable
    {
        bool IsCookable { get; }

        float CookTime { get; }

        float CookActionTimeConsumed { get; }

        float FromCookTimeToBurn { get; }

        float StartAlertTimeBeforeBurn { get; }

        void SetCookActionTimeConsumed(float newChopActionTimeConsumed);        
    }
}
