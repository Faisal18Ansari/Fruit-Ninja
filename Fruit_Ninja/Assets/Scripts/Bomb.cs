using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bomb: the dramatic plot twist. Touch one and the game angrily restarts the scene.
// Also a great example of 'why you should pay attention'.
public class Bomb : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // If the player (i.e. blade) touches this, inform the GameManager and brace for
        // consequences. We use FindObjectOfType because this is a tiny game and we like
        // doing things the lazy-but-effective way.
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().Explode();
        }
    }
}
