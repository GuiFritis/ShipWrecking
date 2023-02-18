using System;
using System.Collections.Generic;
using UnityEngine;

namespace Screens
{
    public enum GameplayScreenType
    {
        PLAYER_HUD,
        MENU,
        GAME_OVER,
        CONFIG,
        INSTRUCTIONS
    }

    public class ScreenController : MonoBehaviour
    {    
        public static ScreenController Instance;
        public List<ScreenSetup> screens = new List<ScreenSetup>();

        void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            } 
            else
            {
                Destroy(gameObject);
            }
        }

        public void ShowScreen(string screenType)
        {
            try
            {
                ShowScreen(Enum.Parse<GameplayScreenType>(screenType, true));
            }
            catch
            {
                ShowScreen(GameplayScreenType.MENU);
            }
        }

        public void ShowScreen(GameplayScreenType screenType, bool active = true)
        {
            var setup = screens.Find(i => i.screenType == screenType);
            if(setup != null)
            {
                setup.screen.SetActive(active);
            }
        }

        public void HideAllScreens()
        {
            foreach (var item in screens)
            {
                item.screen.SetActive(false);
            }
        }
    }

    [System.Serializable]
    public class ScreenSetup
    {
        public GameObject screen;
        public GameplayScreenType screenType;
    }
}