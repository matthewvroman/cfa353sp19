using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bradley.AlienArk
{
    public class MainMenu : MonoBehaviour
    {
        private void Awake()
        {
            AudioManager.Instance.PlaySound("Background Music");
        }
        public void PlayGame ()
        {
            SceneManager.LoadScene("Level Select");
        }
        
        public void QuitGame ()
        {
            Debug.Log("Quit!");
            Application.Quit(); //*Uncomment this if you actually want the game to quit*
        }
    }
}
