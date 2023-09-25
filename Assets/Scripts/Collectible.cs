using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public AudioClip audioClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMove player = collision.GetComponent<PlayerMove>();
        //判断是否获得了组件
        if (player != null)
        {
            //判断是否是满血
            if (player.Hp < player.maxHp)
            {
                player.CountHp(1);
                player.PlaySound(audioClip);
                Destroy(gameObject);
            }
        }
    }
}
