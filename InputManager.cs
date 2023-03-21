using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Drawing;
using Mirror;

public class InputManager : NetworkBehaviour
{
    #region Singletone
    private static InputManager _instance;

    public static InputManager Instance
    {


        get
        {
            return _instance;
        }
    }
    #endregion
    public Image circle;
    public Vector3 offsetCircle;

    private Vector3 movementVector = new Vector3();
    [SerializeField]
    private Player player;
    public float movementSpeed = 5f;
    public Camera cam;

    [SyncVar]
    [SerializeField] float TimeToChop = 3f;
    private float time;
    Vector3 cursorPos;
    private bool isMove;

    void Awake()
    {
        _instance = this;
    }


    public void SetPlayer(Player pl)
    {
        player = pl;
    }
    #region Move
    private void Move()
    {
        Vector3 mov = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        mov = Vector3.ClampMagnitude(mov, movementSpeed);

        if (mov != Vector3.zero)
        {
            if(player != null)
           player.anim.SetBool("isRun", true);

            player.CmdMovePlayer( mov * Time.fixedDeltaTime, mov);
        }
        else
        {
            if(player != null)
            player.anim.SetBool("isRun", false);
        }
    }
    #endregion
    private void FixedUpdate()
    {
        if(isMove)
        Move();

    }
    private void Update()
    {
        if(cam == null)
        {
            cam = Camera.main;
        }
        else 
        ChopTree();
    }

    #region ChopTree
    private void ChopTree()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) && hit.transform.GetComponent<TreeBehaviour>())
        {
            if (Input.GetMouseButton(0))
            {
                CircleImage.timeLeft = TimeToChop;
                player.transform.LookAt(hit.transform.position);
                isMove = false;
                player.anim.SetBool("isChop", true);
                time += Time.deltaTime;
                ChopCursorCircle();
                if (time >= TimeToChop)
                {
                    player.CmdDestroy(hit.transform.gameObject);
                    //  Destroy(hit.transform.gameObject);
                    time = 0;
                    //  player.CmdChopTree(hit.transform.gameObject);
                }
            }
            else
            {
                isMove = true;
                player.anim.SetBool("isChop", false);
                time = 0;
            }

        }
        else
        {
            isMove = true;
            time = 0;
            player.anim.SetBool("isChop", false);
            circle.gameObject.SetActive(false);
        }
    }
    void ChopCursorCircle()
    {
        circle.gameObject.SetActive(true);
        circle.transform.position = Input.mousePosition + offsetCircle;
        circle.GetComponent<CircleImage>().Chop(time);
    }
    #endregion
}

