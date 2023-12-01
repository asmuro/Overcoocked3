using Assets.Scripts.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{    
    internal class PlayerService : MonoBehaviour, IPlayerService
    {
        #region Fields

        private List<PlayerController> players = new List<PlayerController>();

        #endregion

        private void SetColor(PlayerController player)
        {
            if (!this.players.Contains(player))
                return;

            var playerMaterial = player.transform.Find("Capsule").gameObject.GetComponent<Renderer>().material;
            switch (players.IndexOf(player))
            {
                case 0:
                    playerMaterial.color = Color.red;
                    break;
                case 1:
                    playerMaterial.color = Color.blue;
                    break;
            }
        }

        public void RegisterPlayer(PlayerController player)
        {
            this.players.Add(player);
            this.SetColor(player);
        }        

    }
}
