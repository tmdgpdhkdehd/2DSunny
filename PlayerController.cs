using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class playerController : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator animator;

    public float jumpPower = 3f;
    public float movePower = 1f;

    float actWait = 0;

    enum _state_
    {
        _NONE_,
        _IDLE_,
        _RUN_,
        _WAIT_JUMP_,
        _JUMP_,
    }

    _state_ st = _state_._IDLE_;

    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputHoriz = Input.GetAxisRaw("Horizontal");
        bool isJumpPressed = Input.GetButtonDown("Jump");
        bool isMovPressed = (inputHoriz != 0);
        bool isMovNotPressed = (inputHoriz == 0);

        //Debug.Log(inputHoriz);
        //Debug.Log(st);

        switch (st)
        {
            case _state_._IDLE_:
                {
                    if (isJumpPressed)
                    {
                        Jump();
                        st = _state_._WAIT_JUMP_;
                    }
                    else if (isMovPressed)
                    {
                        animator.SetTrigger("run");
                        st = _state_._RUN_;
                    }
                }
                break;
            case _state_._RUN_:
                {
                    if (isMovNotPressed)
                    {
                        st = _state_._IDLE_;
                        animator.SetTrigger("idle");
                        break;
                    }
                    else if (isJumpPressed)
                    {
                        Jump();
                        animator.SetTrigger("jump");
                        st = _state_._WAIT_JUMP_;
                        break;
                    }

                    Mov(inputHoriz);
                }
                break;
            case _state_._JUMP_:
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f);
                    Debug.DrawRay(transform.position, Vector2.down*0.5f, Color.black);

                    if(hit)
                    {
                        animator.SetTrigger("idle");
                        st = _state_._IDLE_;
                    }

                    if (isMovPressed)
                        Mov(inputHoriz);

                    if (isJumpPressed)
                        Jump();
                }
                break;
            case _state_._WAIT_JUMP_:
                {
                    actWait += Time.deltaTime;
                    if(actWait>1.0f)
                    {
                        actWait = 0;
                        st = _state_._JUMP_;
                    }

                    if(isMovPressed)
                        Mov(inputHoriz);

                    if (isJumpPressed)
                        Jump();
                }
                break;
        }
    }

    void Jump()
    {   
        animator.SetTrigger("jump"); //점프 애니메이션

        rigid.velocity = Vector2.zero;

        Vector2 jumpVelocity = new Vector2(0, jumpPower);
        rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);
    }

    private void Mov(float inputHoriz)
    {
        Vector3 moveVelocity = Vector3.zero;

        if (inputHoriz < 0)
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(-1, 1, 1); //왼쪽 바라봄
        }
        else if (inputHoriz > 0)
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(1, 1, 1); //오른쪽 바라봄
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.tag == "cherry")
        {
            Debug.Log("cherry hit ...");
            Destroy(collision.gameObject);
        }
        else if (collision.transform.gameObject.tag == "diamond")
        {
            Debug.Log("diamond hit ...");
            Destroy(collision.gameObject);
        }
        else if (collision.transform.gameObject.tag == "GameOver")
        {
            Debug.Log("GameOver !");
            GameObject.Find("Canvas").transform.Find("Text").gameObject.SetActive(true);
            this.gameObject.SetActive(false);

        }

    }

}
