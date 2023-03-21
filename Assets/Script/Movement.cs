using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{

    public float speedCur = 0;
    public float DecrementSpeed;
    public float AddSpeed;

    public float MaxSpeed = 10;
    public float JumpForce = 10;
    public bool okJump = false;

    public float speedRotation = 15;
    public float gravityPush;
    public float distRay;

    public LayerMask GroundLayer;

    public Transform camY;
    public Animator anim;

    public GameObject SonicBasic;
    public GameObject SonicAnims;

    public Rigidbody rb;
    public DetectGround coll;
    [SerializeField]
    private Vector3 groundDir;
    // Start is called before the first frame update
    void Start()
    {
        groundDir = transform.up;
        SonicAnims.SetActive(false);
        SonicBasic.SetActive(true);
    }

    private void Update()
    {
        speedCur = Mathf.Clamp(speedCur, 0, MaxSpeed);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        bool ground = coll.COOLL(-groundDir);

        SonicAnims.SetActive(true);
        SonicBasic.SetActive(false);
        if (ground)
        {
            gravityPush = 5;
            if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            {
                gravityPush = 0;
                if (speedCur > 0)
                {
                    speedCur -= DecrementSpeed * Time.deltaTime;
                }
                if (speedCur == 0)
                {
                    SonicAnims.SetActive(false);
                    SonicBasic.SetActive(true);
                }
            }else
            {
                speedCur += AddSpeed * Time.deltaTime;
            }
            //move
            if (speedCur >= 8)
            {
                Debug.Log("Advanced");
                movePlayerSloop(speedCur);
            }
            else
            {
                Debug.Log("basic");
                movePlayerBasic(speedCur);
            }
            //jump
            if (Input.GetButtonDown("Jump"))
            {
                StartCoroutine(jumpUp(JumpForce));
                StopCoroutine(jumpUp(JumpForce));
            }
            anim.SetBool("inAir", false);
            anim.SetFloat("Speed", speedCur);
        }

        if (!ground)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation, 0.02f);
            groundDir = transform.up;
            anim.SetBool("inAir", true);
        }
    }
    void movePlayerBasic(float SpeedCur)
    {
        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        JumpForce = 15;
        float spd = SpeedCur;

        var MoveRotation = Quaternion.LookRotation(new Vector3(_xMov, 0, _zMov));
        var camRotation = Quaternion.Euler(0, camY.transform.root.eulerAngles.y, 0);

        if (_xMov == 0 && _zMov == 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation, speedRotation * Time.deltaTime);
        } else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, camRotation * MoveRotation, speedRotation * Time.deltaTime);
        }

        Vector3 move = transform.forward * spd;

        rb.velocity = move;
    }
    void movePlayerSloop(float SpeedCur)
    {
        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        float spd = SpeedCur;

        JumpForce = 25;
        groundDir = RayCastGround();

        var MoveRotation = Quaternion.LookRotation(new Vector3(_xMov,0,_zMov));
        var camRotation = Quaternion.Euler(0, camY.transform.root.eulerAngles.y, 0);

        var slopeRotation = Quaternion.FromToRotation(transform.up, groundDir);
        if (_xMov == 0 && _zMov == 0)
        {
            //parado
            transform.rotation = Quaternion.Lerp(transform.rotation, slopeRotation * transform.rotation, speedRotation * Time.deltaTime);
        }else
        {
            //move
            var slopeRotationMoving = Quaternion.FromToRotation(Vector3.up, groundDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, slopeRotationMoving * camRotation * MoveRotation, speedRotation * Time.deltaTime);
        }

        Vector3 move = transform.forward * spd;
        move -= groundDir * gravityPush;
        rb.velocity = move;
    }

    Vector3 RayCastGround()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position,-transform.up, out hit, distRay, GroundLayer);

        Vector3 My = transform.up;

        if(hit.transform != null)
          My = hit.normal;

        Debug.DrawLine(transform.position, transform.position + (My.normalized * distRay), Color.red);
        return My.normalized;
    }

    IEnumerator jumpUp(float ForceJump)
    {
        okJump = false;

        rb.AddForce(transform.up * ForceJump, ForceMode.Impulse);

        yield return new WaitForSecondsRealtime(0.3f);
        okJump = true;
    }
}
