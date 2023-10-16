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
        //������Ʈ�� ������ ��
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
        //������Ʈ�� �ⱸ�� ����� ��
        if (other.CompareTag("Exit"))
        {
            if (!machineMove.isMoveUp)
            {
                transform.position = poolPos;
                GameManager.Instance.objectCount--;

                countTxt.text = "���� �� : " + GameManager.Instance.objectCount;

                isGrab = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //���� �� ������Ʈ�� �������鼭 ����� �� ��� ����
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
