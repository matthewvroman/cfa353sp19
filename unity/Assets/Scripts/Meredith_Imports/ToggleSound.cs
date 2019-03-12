using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSound : MonoBehaviour
{
    private bool _isMuted = false;    

    public void ToggledSound()
    {
        _isMuted = !_isMuted; // inverts the value. false -> true. true -> false
        AudioListener.volume = _isMuted ? 1f : 0f; // ? operator allows you to assing values depending on a boolean expression (expression?true:false;)
    }

}


