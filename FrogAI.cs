using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frogAI : MonoBehaviour
{
    enum _state_
    {
        _IDLE_,
        _JUMP_,
    }

    _state_ st = _state_._IDLE_;

    float act = 0;

    Animator anim;
    Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (st)
        {
            case _state_._IDLE_:
                {
                    act += Time.deltaTime;
                    if(act>1.0f)
                    {
                        act = 0;
                        anim.SetTrigger("jump");

                        int rv = Random.Range(0, 2);
                        Vector2 v1;

                        // limit x position, within 2.0f ~ 8.0f
                        if (transform.position.x > 8.0f)
                            rv = 0;
                        if (transform.position.x < 2.0f)
                            rv = 1;

                        if (rv == 0)
                        {
                            v1 = new Vector2(-1, 5);
                            transform.localScale = new Vector3(1, 1, 1); //왼쪽 바라봄
                        }
                        else
                        {
                            v1 = new Vector2(1, 5);
                            transform.localScale = new Vector3(-1, 1, 1); //오른쪽 바라봄
                        }

                        rigid.AddForce(0.5f*v1, ForceMode2D.Impulse);
                        st = _state_._JUMP_;
                    }

                }
                break;
            case _state_._JUMP_:
                {
                    act += Time.deltaTime;
                    if (act > 1.0f)
                    {
                        act = 0;
                        anim.SetTrigger("idle");
                        st = _state_._IDLE_;
                    }
                    break;
                }
        }
        
    }
}
