using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject sayText;

    private float disTime = 3f;

    private float timerDisPlay;

    public Text text;

    private AudioSource audioSource;
    public AudioClip audioClip;

    private bool hasPlay;
    void Start()
    {
        sayText.SetActive(false);
        timerDisPlay = -1;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (timerDisPlay >= 0)
        {
            timerDisPlay -= Time.deltaTime;
            if (timerDisPlay < 0)
            {
                {
                    sayText.SetActive(false);
                }
            }
        }
    }

    public void Say()
    {
        timerDisPlay = disTime;
        sayText.SetActive(true);
        UIHP.instance.hasTask = true;
        if (UIHP.instance.fixedNum >= 4)
        {
            //任务完成，修改对话内容
            text.text = "任务完成";
            if (!hasPlay)
            {
                audioSource.PlayOneShot(audioClip);
                hasPlay = true;
            }
        }
    }
}
