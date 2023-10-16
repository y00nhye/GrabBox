using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectMovement : MonoBehaviour
{
    private MachineMove machineMove;
    private GameObject grabPoint;

    private bool isGrab = false;
    private Vector3 poolPos = new Vector3(10, 3, 0);

    [SerializeField] Text countTxt;

    private void Awake()
    {
        machineMove = FindObjectOfType<MachineMove>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //오브젝트가 잡혔을 때
        if (other.CompareTag("Grab"))
        {
            grabPoint = other.gameObject;

            if (!other.GetComponent<Picker>().isPick)
            {
                other.GetComponent<Picker>().isPick = true;
                isGrab = true;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //오브젝트가 출구에 닿았을 때
        if (other.CompareTag("Exit"))
        {
            if (!machineMove.isMoveUp)
            {
                transform.position = poolPos;
                GameManager.Instance.objectCount--;

                countTxt.text = "남은 수 : " + GameManager.Instance.objectCount;

                isGrab = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //시작 시 오브젝트가 떨어지면서 닿았을 때 잡기 방지
        if (other.CompareTag("Grab"))
        {
            isGrab = false;
            other.GetComponent<Picker>().isPick = false;
        }
    }

    private void Update()
    {
        if (machineMove.isMoveUp && isGrab)
        {
            transform.position = grabPoint.transform.position;
        }
    }
}
