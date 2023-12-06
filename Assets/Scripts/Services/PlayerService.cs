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

        #region Register Players

        public void RegisterPlayer(PlayerController player)
        {
            this.players.Add(player);
            
        }

        #endregion
    }
}
