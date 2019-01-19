using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace Assets.Scripts.MainMenu_Scripts
{
    public class UiSound : MonoBehaviour
    {

        public AudioSource HoverSound;
        public AudioSource ClickSound;

        public void OnHover()
        {
            HoverSound.Play();
        }

        public void OnClick()
        {
            ClickSound.Play();
        }
    }
}
