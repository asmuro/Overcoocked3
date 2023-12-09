using Assets.Scripts.Objects.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class Order : MonoBehaviour, IOrder
    {
        #region Properties

        [SerializeField]
        private Recipe recipe;
        public Recipe Recipe { get => this.recipe; set => recipe = value; }

        [SerializeField]
        private Sprite sprite;
        public Sprite Sprite { get => this.sprite; set => this.sprite = value; }

        #endregion

    }
}
