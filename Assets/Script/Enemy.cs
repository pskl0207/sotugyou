using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player p;

    public Vector2 speed;               //敵が動くスピード
    int hp;                             //体力
    int attack;                         //攻撃力
    float p_e_distans;                  //プレイヤーと敵の距離
    bool counterFlg;                    //カウンターの受付時間かどうか
    bool slowFlg;                       //スロー状態かどうか
    GameObject player;                  //プレイヤーのゲームオブジェクト
    Vector3 p_pos;                      //プレイヤーの位置
    Vector3 e_pos;                      //自分(エネミー)の位置
    Animator anim;                      //武器のアニメーション
    private float timer;                //計測用変数
    private bool timerflg;              //計っていいかどうか
    float m_Counter = 0;	            // カウンター
    int branch = 0;

    // Start is called before the first frame update
    void Start()
    {
        speed = new Vector2(0.1f, 0f);
        hp = 1;
        attack = 1;
        p_e_distans = 0f;
        counterFlg = false;
        slowFlg = false;
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        p_e_distans = e_findPlayer();
        Debug.Log("距離:" + counterFlg);
        //get_animName();
        
        if (hp <= 0)
        {
            e_destoroy();
        }

    }

    void FixedUpdate()
    {
        e_chasePlayer();
        e_slowMode();
    }

    public float e_findPlayer()
    {
        p_pos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        Debug.Log("ppos" + p_pos);
        e_pos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        p_e_distans = e_pos.y - p_pos.y;
        return p_e_distans;
    }

    void e_chasePlayer()
    {
        Debug.Log("Mathf.Abs(e_pos.x) - Mathf.Abs(p_pos.x):" + (Mathf.Abs(e_pos.x) - Mathf.Abs(p_pos.x)));
        float chaseDistance = e_pos.x - p_pos.x;
        if (chaseDistance >= 0.3f)
        {
            e_pos.x = e_pos.x - speed.x;
            this.transform.position = e_pos;
        }
        if (chaseDistance <= -0.3f)
        {
            e_pos.x = e_pos.x + speed.x;
            this.transform.position = e_pos;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("???????????");
        if(collision.gameObject.tag == "Player")
        {
            this.tag = "inCombat";
        }
        //e_attackPlayer();
    }

    void OntriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            this.tag = "enemy";
        }
    }

    void e_attackPlayer()
    {
        if (branch == 0)
        {
            Debug.Log("1らしい");
            anim.SetTrigger("New Trigger ");
            branch = 1;
        }
        if (branch == 1)
        {
            Debug.Log("counter:" + m_Counter);
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("doing"))           // ここに到達直後はnormalizedTimeが"Default"の経過時間を拾ってしまうので、Resultに遷移完了するまではreturnする。
                return;
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)      // 待機時間を作りたいならば、ここの値を大きくする。
                return;
            if (m_Counter < 1.0f)                                               // この値を大きくすると、次の処理開始までに待機時間を設けることができる
            {
                m_Counter += Time.deltaTime;
                Debug.Log("2らしい");
            }
            else if (m_Counter >= 1.0f)
            {
                Debug.Log("3らしい");
                m_Counter = 0f;
                anim.SetTrigger("New Trigger2");
                branch = 2;
            }
        }
        if (branch == 2)
        {
            Debug.Log("counter2:" + m_Counter);
            Debug.Log("4らしい");
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("testattack"))      // ここに到達直後はnormalizedTimeが"Default"の経過時間を拾ってしまうので、Resultに遷移完了するまではreturnする。
                return;
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)      // 待機時間を作りたいならば、ここの値を大きくする。
                return;
            if (m_Counter < 1.0f)                                               // この値を大きくすると、次の処理開始までに待機時間を設けることができる
            {
                m_Counter += Time.deltaTime;
            }
            else
            {
                m_Counter = 0f;
                branch = 0;
            }
        }

    }

    void e_destoroy()
    {
        //アニメーション
        //SE
        Destroy(this.gameObject);
    }

    bool e_damage(int damage)
    {
        hp--;
        return counterFlg;
    }

    public void e_slowMode()
    {
        slowFlg = p.p_slowStartFlg();
        slowFlg = p.p_slowEndFlg();

        if (slowFlg == false)
        {
            slowFlg = true;
        }
        else if (slowFlg == true)
        {
            float slowNum = 0.1f;
            speed.x *= slowNum;
            speed.y *= slowNum;
            anim.speed *= slowNum;
        }
    }

    void get_animName()
    {

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("doing"))
        {
            counterFlg = true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("testattack"))
        {
            counterFlg = false;
        }
    }

    public int returnAttack()
    {
        return attack;
    }
}
