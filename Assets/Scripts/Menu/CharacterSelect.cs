using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    public void playGame(AnimatorController animator)
    {
        PlayerInteraction.sApearance = animator;
        SceneManager.LoadScene(2);
    }
}
