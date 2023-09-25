using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    private Rigidbody2D rigidbody2d;
    public float speed;

    public int maxHp = 5;//最大血量
    private int hp;
    public int Hp
    {
        get { return hp; }
    }//当前血量

    //无敌时间
    private float wuDiTime = 2f;
    private bool isWuDiTime;
    private float invincibleTimer;//计时器

    //看的方向
    private Vector2 lookDir = new Vector2(1, 0);

    private Animator animator;

    public GameObject bullePrefab;

    public AudioSource audioSource;

    public AudioSource walkAudioSource;

    //受伤音效
    public AudioClip PlayHit;

    //子弹发射
    public AudioClip audioLaunch;

    //复活的位置
    private Vector3 v3Position;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        hp = maxHp;
        animator = GetComponent<Animator>();
        //audioSource = GetComponent<AudioSource>();
        v3Position = transform.position;
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(h, v);

        //Mathf.Approximately:判断里面的两个值是否是近似相等的
        //当前玩家输入的轴向不为0
        if (!Mathf.Approximately(move.x, 0) || !Mathf.Approximately(move.y, 0))
        {
            lookDir.Set(move.x, move.y);
            lookDir.Normalize();
            if (!walkAudioSource.isPlaying)
            {
                walkAudioSource.Play();
            }
        }
        else
        {
            walkAudioSource.Stop();
        }
        animator.SetFloat("Look X", lookDir.x);
        animator.SetFloat("Look Y", lookDir.y);
        animator.SetFloat("Speed", move.magnitude);

        Vector2 position = transform.position;
        //position.x += speed * h * Time.deltaTime;
        //position.y += speed * v * Time.deltaTime;
        //位置的移动
        position = position + speed * move * Time.deltaTime;
        rigidbody2d.MovePosition(position);

        //无敌时间
        if (isWuDiTime)
        {
            invincibleTimer = invincibleTimer - Time.deltaTime;
            if (invincibleTimer <= 0)
            {
                isWuDiTime = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
        }

        //射线检测是否与NPC对话
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f,
                lookDir, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NPC npcSay = hit.collider.GetComponent<NPC>();
                if (npcSay != null)
                {
                    npcSay.Say();
                }
            }
        }
    }
    //参数：改变血量的值
    public void CountHp(int count)
    {
        //传过来的值小于0就代表受到了伤害
        if (count < 0)
        {
            if (isWuDiTime)
            {
                return;
            }
            isWuDiTime = true;
            invincibleTimer = wuDiTime;

            animator.SetTrigger("Hit");
            //播放受伤音效
            PlaySound(PlayHit);
        }
        //参数1：血量加上要改变的值（加或减）
        //参数2和3：限制参数的最大值和最小值
        hp = Mathf.Clamp(hp + count, 0, maxHp);

        if (hp <= 0)
        {
            Respawn();
        }

        UIHP.instance.SetValue(hp / (float)maxHp);
    }

    private void Launch()
    {
        if (!UIHP.instance.hasTask)
        {
            return;
        }
        //1：生成的物体
        //2：生成的位置
        //3：旋转的角度（默认）
        GameObject bulleObject = Instantiate(bullePrefab,
          rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Bulle bulle = bulleObject.GetComponent<Bulle>();
        bulle.Launch(lookDir, 400);
        animator.SetTrigger("Launch");
        PlaySound(audioLaunch);
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    //复活方法
    private void Respawn()
    {
        CountHp(maxHp);
        transform.position = v3Position;
    }
}
