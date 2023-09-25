using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rigidbody;

    //轴向控制
    public bool vertical;
    //方向控制
    private int dir = 1;
    //方向改变时间间隔
    public float changeTime = 3f;
    //计时器
    private float timer;

    private Animator animator;

    //机器人是否损坏
    private bool broken;

    public ParticleSystem moke;

    private AudioSource audioSource;

    //修复音效
    public AudioClip audioClip;

    //受伤音效
    public AudioClip[] hitSound;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();

        //动画器控制
        //animator.SetFloat("MoveX", dir);
        //animator.SetBool("Vertical", vertical);
        PlayMoveAnimation();

        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (broken)
        {
            return;
        }
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            dir = -dir;
            //animator.SetFloat("MoveX", dir);
            PlayMoveAnimation();
            timer = changeTime;
        }

        Vector2 position = transform.position;
        if (vertical)
        {
            position.y += Time.deltaTime * speed * dir;
        }
        else
        {
            position.x += Time.deltaTime * speed * dir;
        }
        rigidbody.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMove player = collision.gameObject.GetComponent<PlayerMove>();
        if (player != null)
        {
            player.CountHp(-1);
        }
    }

    //播放移动动画
    private void PlayMoveAnimation()
    {
        //混合树控制
        if (vertical) //垂直轴向动画的控制
        {
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", dir);
        }
        else //水平轴向动画的控制
        {
            animator.SetFloat("MoveX", dir);
            animator.SetFloat("MoveY", 0);
        }
    }

    //修复机器人
    public void Fix()
    {
        broken = true;
        rigidbody.simulated = false;
        animator.SetTrigger("Fixed");
        moke.Stop();

        audioSource.Stop();
        int ranNum = Random.Range(0, 2);
        audioSource.PlayOneShot(hitSound[ranNum]);
        audioSource.volume = 0.5f;
        Invoke("PlayFixedSound", 0.5f);
        UIHP.instance.fixedNum++;
    }

    private void PlayFixedSound()
    {
        audioSource.PlayOneShot(audioClip);
    }
}
