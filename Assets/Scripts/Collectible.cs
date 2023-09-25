using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public AudioClip audioClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMove player = collision.GetComponent<PlayerMove>();
        //�ж��Ƿ��������
        if (player != null)
        {
            //�ж��Ƿ�����Ѫ
            if (player.Hp < player.maxHp)
            {
                player.CountHp(1);
                player.PlaySound(audioClip);
                Destroy(gameObject);
            }
        }
    }
}
