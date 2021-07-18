using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Scripts.Lobby
{
    public class LobbyUIController : MonoBehaviour
    {
        #region Private Serialized Field

        [SerializeField] private GameObject controlPanel;
        [SerializeField] private GameObject progressLabel;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            ShowControlPanel();
            HideProgressLabel();
        }

        #endregion
        
        #region Public Methods

        public void ShowControlPanel()
        {
            controlPanel.SetActive(true);
        }

        public void HideControlPanel()
        {
            controlPanel.SetActive(false);
        }

        public void ShowProgressLabel()
        {
            progressLabel.SetActive(true);
        }

        public void HideProgressLabel()
        {
            progressLabel.SetActive(false);
        }

        #endregion
    }
}

