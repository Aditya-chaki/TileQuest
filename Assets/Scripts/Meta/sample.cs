using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sample : MonoBehaviour
{
    public void MenuEnabler()
    {
         SceneManager.LoadScene("Menu");
    }
    public void KingStoryEnabler()
    {
         SceneManager.LoadScene("King");
         Config.Gold = Config.Gold -3;
    }
    public void ManagerStoryEnabler()
    {
         SceneManager.LoadScene("Manager");
         Config.Gold = Config.Gold - 2;
    }

}
