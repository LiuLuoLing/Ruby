using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rigidbody;

    //�������
    public bool vertical;
    //�������
    private int dir = 1;
    //����ı�ʱ����
    public float changeTime = 3f;
    //��ʱ��
    private float timer;

    private Animator animator;

    //�������Ƿ���
    private bool broken;

    public ParticleSystem moke;

    private AudioSource audioSource;

    //�޸���Ч
    public AudioClip audioClip;

    //������Ч
    public AudioClip[] hitSound;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();

        //����������
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

    //�����ƶ�����
    private void PlayMoveAnimation()
    {
        //���������
        if (vertical) //��ֱ���򶯻��Ŀ���
        {
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", dir);
        }
        else //ˮƽ���򶯻��Ŀ���
        {
            animator.SetFloat("MoveX", dir);
            animator.SetFloat("MoveY", 0);
        }
    }

    //�޸�������
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
