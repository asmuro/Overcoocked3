using UnityEngine;

namespace Assets.Scripts.Objects.Interfaces
{
    public interface IOrder
    {
        Recipe Recipe { get; set; }
        
        Sprite Sprite { get; set; }
        
    }
}
