using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private int sceneToLoad = -1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController pc = collision.GetComponent<PlayerController>();
        if(pc != null)
        {
            StartCoroutine(Transition());
        }

    }

    private IEnumerator Transition()
    {
        SceneManager.LoadScene(sceneToLoad);
        yield return null;
    }
    public void SetTargetLevel(int targetLevel)
    {
        sceneToLoad = targetLevel;
    }
    public int GetTargetLevel()
    {
        return sceneToLoad;
    }

}
