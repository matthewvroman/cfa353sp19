using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bradley.AlienArk
{
    public class ToggleSound : MonoBehaviour
    {
        private bool _isMuted = false;
        private Image muteButton;

        private void Start()
        {
            muteButton = GetComponent<Image>();    
        }

        public void ToggledSound()
        {
            _isMuted = !_isMuted; // inverts the value. false -> true. true -> false
            muteButton.sprite = Resources.Load<Sprite>(_isMuted ? "Sprites/UI/Volume_1" : "Sprites/UI/Volume_2" );
            AudioListener.volume = _isMuted ? 0f : 1f; // ? operator allows you to assing values depending on a boolean expression (expression?true:false;)
        }
    }

}


