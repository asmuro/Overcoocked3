using Assets.Scripts.Enumerables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Objects.Interfaces
{
    internal interface IIngredient
    {
        string Name { get; }

        IngredientState State { get; }
    }
}
