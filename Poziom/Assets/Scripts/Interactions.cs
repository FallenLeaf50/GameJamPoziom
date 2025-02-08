using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class Interactions : MonoBehaviour
{
    public GameObject Player;
    public Camera mainCam;
    public Transform Floor;

    public Vector3 Offset;

    public Material DefaultMaterial;
    public Material OutlinedMaterial;
    public GameObject LatestRaycastObj;


    InputSystem_Actions input;
    bool _IsLeftButtonPressed;
    bool _CanInteract = true;
    
    bool _IsObjectLifted;
    GameObject _ObjectLifted;
    Ray _Ray;
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Raycast();

    }

    private void Raycast()
    {
        _Ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (!_IsObjectLifted)
        {
            if (Physics.Raycast(_Ray, out hit, Mathf.Infinity))

            {
                if (hit.collider.tag == "Interactable")
                {
                    if (_IsLeftButtonPressed)
                    {

                    }
                    Debug.Log("Did Hit");
                }
                else if (hit.collider.tag == "CanBeLifted")
                {
                    if (LatestRaycastObj != hit.collider.gameObject)
                    {
                        if (LatestRaycastObj != null)
                        {
                            LatestRaycastObj.GetComponent<MeshRenderer>().material = DefaultMaterial;
                        }
                        hit.collider.gameObject.GetComponent<MeshRenderer>().material = OutlinedMaterial;
                        LatestRaycastObj = hit.collider.gameObject;
                    }

                    if (_IsLeftButtonPressed && !_IsObjectLifted)
                    {
                        _IsObjectLifted = true;
                        _ObjectLifted = hit.collider.gameObject;

                        hit.collider.transform.parent.gameObject.transform.SetParent(Player.transform);
                        hit.collider.transform.parent.gameObject.transform.localRotation = Quaternion.identity;
                        hit.collider.transform.parent.gameObject.transform.localPosition = Vector3.forward + Offset;
                    }
                    Debug.Log("Did Hit");
                }
                else
                {
                    if (LatestRaycastObj != null)
                    {
                        LatestRaycastObj.GetComponent<MeshRenderer>().material = DefaultMaterial;
                        LatestRaycastObj = null;
                    }
                }
                
            }
            else
            {
                if(LatestRaycastObj != null)
                {
                    LatestRaycastObj.GetComponent<MeshRenderer>().material = DefaultMaterial;
                    LatestRaycastObj = null;
                }

                Debug.Log("Did not Hit");
            }
        }
        
       
        Debug.DrawRay(mainCam.transform.position, mainCam.transform.TransformDirection(Vector3.forward) * 100, Color.yellow);
    }
    private void OnEnable()
    {
        input = new InputSystem_Actions();
        input.Player.Enable();
        input.Player.Interact.started += (ctx) => 
        { 
            if(_CanInteract == true)
            {
                _IsLeftButtonPressed = true;
                if (_IsObjectLifted)
                {
                    _ObjectLifted.transform.parent.gameObject.transform.position = new Vector3(_ObjectLifted.transform.position.x, Floor.position.y, _ObjectLifted.transform.position.z);
                    _ObjectLifted.transform.parent.gameObject.transform.SetParent(Floor);
                    _IsObjectLifted = false;
                    _ObjectLifted = null;
                }
                //_CanInteract = false;
                //StartCoroutine(CanInteractTimer());
            }
           

        };

        input.Player.Interact.canceled += (ctx) => { _IsLeftButtonPressed = false; };
    }
    IEnumerator CanInteractTimer()
    {
        yield return new WaitForSeconds(1.0f);
        _CanInteract = true;
    }
    private void OnDisable()
    {
        input.Player.Disable();
    }

}
