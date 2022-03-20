using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    private Text txt;
    private float timer = 10f;

    private void Start()
    {
        txt = transform.GetChild(1).GetComponent<Text>();
    }

    private void Update()
    {
        txt.text = timer.ToString("F2");

        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            ReloadApp();
        }
    }

    private void ReloadApp()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
