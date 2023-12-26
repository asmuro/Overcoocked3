using UnityEngine;

namespace Assets.Scripts
{
    internal class SpriteBillboard : MonoBehaviour
    {
        #region MonoBehaviour

        private void Update()
        {
            this.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        }

        #endregion
    }
}
