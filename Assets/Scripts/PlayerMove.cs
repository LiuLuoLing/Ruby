using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    private Rigidbody2D rigidbody2d;
    public float speed;

    public int maxHp = 5;//���Ѫ��
    private int hp;
    public int Hp
    {
        get { return hp; }
    }//��ǰѪ��

    //�޵�ʱ��
    private float wuDiTime = 2f;
    private bool isWuDiTime;
    private float invincibleTimer;//��ʱ��

    //���ķ���
    private Vector2 lookDir = new Vector2(1, 0);

    private Animator animator;

    public GameObject bullePrefab;

    public AudioSource audioSource;

    public AudioSource walkAudioSource;

    //������Ч
    public AudioClip PlayHit;

    //�ӵ�����
    public AudioClip audioLaunch;

    //�����λ��
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

        //Mathf.Approximately:�ж����������ֵ�Ƿ��ǽ�����ȵ�
        //��ǰ������������Ϊ0
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
        //λ�õ��ƶ�
        position = position + speed * move * Time.deltaTime;
        rigidbody2d.MovePosition(position);

        //�޵�ʱ��
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

        //���߼���Ƿ���NPC�Ի�
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
    //�������ı�Ѫ����ֵ
    public void CountHp(int count)
    {
        //��������ֵС��0�ʹ����ܵ����˺�
        if (count < 0)
        {
            if (isWuDiTime)
            {
                return;
            }
            isWuDiTime = true;
            invincibleTimer = wuDiTime;

            animator.SetTrigger("Hit");
            //����������Ч
            PlaySound(PlayHit);
        }
        //����1��Ѫ������Ҫ�ı��ֵ���ӻ����
        //����2��3�����Ʋ��������ֵ����Сֵ
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
        //1�����ɵ�����
        //2�����ɵ�λ��
        //3����ת�ĽǶȣ�Ĭ�ϣ�
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

    //�����
    private void Respawn()
    {
        CountHp(maxHp);
        transform.position = v3Position;
    }
}
