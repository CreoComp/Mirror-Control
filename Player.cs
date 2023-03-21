using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    [SyncVar]
    [SerializeField]
    private float speed;
    public Rigidbody rb;
    public Animator anim;

    [SerializeField] GameObject Camera;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        if (isClient && isLocalPlayer)
        {
            SetInputManagerPlayer();


        }
        if(hasAuthority)
        {
            var cam = Instantiate(Camera);
            cam.GetComponent<CameraManager>().player = transform;
        }

        if (isServer)
        {

            speed = 15;

        }
    }
    private void SetInputManagerPlayer()
    {
        InputManager.Instance.SetPlayer(this);

    }


    [Command]
    public void CmdMovePlayer(Vector3 movement, Vector3 rot)
    {
        rb.MovePosition(movement * speed + transform.position);
        rb.MoveRotation(Quaternion.LookRotation(rot));
    }
    [Command]
    public void CmdDestroy(GameObject obj)
    {
        RpcDestroyChop(obj);
    }
    [ClientRpc]
    public void RpcDestroyChop(GameObject obj)
    {
        if (obj.transform.GetComponent<TreeBehaviour>())
        {
            Destroy(obj);
        }
    }
}
