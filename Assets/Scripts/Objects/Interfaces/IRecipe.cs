using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Objects.Interfaces
{
    public interface IRecipe
    {
        List<GameObject> Ingredients { get; set; }        
    }
}
