using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Player : MonoBehaviour
{
    public Enemy e;                                     //エネミーのスクリプト

    private int hp = 5;                                     //プレイヤーのHP

    private int attack = 1;                                 //プレイヤーの攻撃力

    private Vector2 speed;                                  //プレイヤーのスピード

    private GameObject enemy;                               //敵のオブジェクトを収納する変数

    [SerializeField]
    private float plusSpeed = 0f;                           //プレイヤーの移動変化量

    private int slowGauge = 100;                            //スローゲージ

    private int weaponNumber = 0;                           //プレイヤーの武器の種類                        

    private bool attackFlg = false;                         //プレイヤーが攻撃していいかのフラグ

    private bool comboFlg = false;                          //攻撃時に行われるコンボ用のフラグ

    private bool counterFlg = false;                        //敵の攻撃に対してのカウンターフラグ

    private bool slowFlg = false;                           //プレイヤーをスローに切り替えるためのフラグ

    private bool damageFlg = false;

    // ゲームスタート時に初期化する
    void Start()
    {
        speed = transform.position;

    }

    // 常時起動する
    void Update()
    {
        //プレイヤーの移動関数
        p_xMove();
        p_yMove();

        //アタックできる場合関数を呼び出す
        if (attackFlg == true)
        {
            p_attackStartFlg();
        }

        //ダメージを受ける際に関数を呼び出す
        if (damageFlg == true)
        {
            p_damage();
        }

        //スローモードを起動する際に敵の関数を呼び出す
        if (slowFlg == true)
        {
            e.e_slowMode();
            //Debug.Log("t="+dummy);
        }

        //スローモードを終了する際に敵の関数を呼び出す
        if (slowFlg == false)
        {

            //Debug.Log("x="+dummy);
        }

    }

    //プレイヤーの移動関数
    public void p_xMove()
    {
        //プレイヤーの現在座標を代入
        speed = transform.position;

        //Aキーに該当するボタンが押されている際に行う処理
        if (Keyboard.current.aKey.isPressed)
        {
            speed.x -= plusSpeed;
            //Debug.Log(transform.position);
        }

        //Dキーに該当するボタンが押されている際に行う処理
        if (Keyboard.current.dKey.isPressed)
        {
            speed.x += plusSpeed;
            //Debug.Log(transform.position);
        }

        //変更した値をプレイヤーの座標に代入
        transform.position = speed;
    }
    //プレイヤーを上昇させる関数
    private void p_yMove()
    {
        speed.y += 0.05f;
        transform.position = speed;
    }

    //プレイヤーが攻撃する際の関数
    private void p_attackStartFlg()
    {
        //マウス左クリックを押された1フレームだけ動く
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("aaaa");
            enemy = GameObject.FindWithTag("inCombat");
            comboFlg = true;
            Destroy(enemy, 2.0f);
        }
        else
        {
            comboFlg = false;
        }
    }

    //slowにする際に敵に変数を渡す関数
    public bool p_slowStartFlg()
    {
        //マウス右クリックを押し続けている時場合
        if (Mouse.current.rightButton.isPressed)
        {
            slowFlg = true;
        }
        return slowFlg;
    }

    //slowをやめる際に敵に変数を渡す関数
    public bool p_slowEndFlg()
    {
        //マウス右クリックを離した時か、スローゲージがゼロになった場合
        if (Input.GetMouseButtonUp(1) || slowGauge == 0)
        {
            slowFlg = false;
        }
        return slowFlg;
    }

    //敵を倒してコンボが発生する際に行う関数
    public bool p_comboAttack()
    {
        if (comboFlg == true)
        {
            slowFlg = true;
        }
        return slowFlg;
    }
    //プレイヤーと敵の距離が一定の数値になった時にダメージを与える関数
    public void p_damage()
    {
        //敵の攻撃力
        int e_attack = e.returnAttack();
        //敵とプレイヤーの座標の差
        float distans = e.e_findPlayer();

        //座標の差が数値より小さかった場合
        if (Mathf.Abs(distans) <= 5f)
        {
            damageFlg = false;
            Debug.Log("damageFlg=" + damageFlg);
            hp = hp - e_attack;
        }
        Debug.Log("p_hp = " + hp);
    }


    //当たり判定（中にとどまった時）
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "inCombat")
        {
            attackFlg = true;

            //Debug.Log("a");
        }
    }
    //当たり判定(判定に入るとき)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy")
        {
            damageFlg = true;
            Debug.Log("Enter");
        }
    }
    //当たり判定(判定から出るとき)
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "inCombat")
        {
            damageFlg = false;
            Debug.Log("Exit");

        }
    }
}


