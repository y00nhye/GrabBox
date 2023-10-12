using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineMove : MonoBehaviour
{
    [Header("Machine Moving Parts")]
    [SerializeField] GameObject machineXaxis;
    [SerializeField] GameObject machineYaxis;
    [SerializeField] GameObject machineZaxis;

    [SerializeField] GameObject blockBox;

    [Header("Moving Speed")]
    [SerializeField] float speed = 0f;
    [SerializeField] private float x = 0f;
    [SerializeField] private float z = 0f;
    [SerializeField] float waitTime = 0f;

    private bool isClick = false; //����Ű ���� ����
    private bool isPush = false; //push ��ư ���� ����
    public bool isMoveUp = false; //��谡 �ö󰡴� ����

    //������Ʈ ���� ���� �� �ⱸ ����
    private void Start()
    {
        Invoke("ExitBlockingOff", 5);
    }
    private void ExitBlockingOff()
    {
        blockBox.SetActive(false);
    }

    //����Ű ������ ����
    private void Movement()
    {
        if (x != 0)
        {
            //x�� ������ ���� ����
            if (machineXaxis.transform.localPosition.x < 1.3f && x < 0)
            {
                x = 0;
            }
            if (machineXaxis.transform.localPosition.x > 2.7f && x > 0)
            {
                x = 0;
            }

            machineXaxis.transform.localPosition += new Vector3(x, 0, 0) * Time.deltaTime;
            machineYaxis.transform.localPosition += new Vector3(x, 0, 0) * Time.deltaTime;
        }
        if (z != 0)
        {
            //z�� ������ ���� ����
            if (machineZaxis.transform.localPosition.z < -1.7f && z < 0)
            {
                z = 0;
            }
            if (machineZaxis.transform.localPosition.z > -0.3f && z > 0)
            {
                z = 0;
            }

            machineZaxis.transform.localPosition += new Vector3(0, 0, z) * Time.deltaTime;
            machineYaxis.transform.localPosition += new Vector3(0, 0, z) * Time.deltaTime;
        }
    }

    private void Update()
    {
        if (isClick && !isPush)
        {
            Movement();
        }
    }

    //��ư �̺�Ʈ
    public void PushBtn()
    {
        if (!isPush)
        {
            StartCoroutine(PushBtn_co());
        }
    }
    public void RightForce()
    {
        x = speed;
        z = 0;
    }
    public void LeftForce()
    {
        x = -speed;
        z = 0;
    }
    public void UPForce()
    {
        x = 0;
        z = speed;
    }
    public void DownForce()
    {
        x = 0;
        z = -speed;
    }
    public void PointUp()
    {
        isClick = false;
    }
    public void PointDown()
    {
        isClick = true;
    }

    //push btn ���� �� ������ ����
    IEnumerator PushBtn_co()
    {
        isPush = true;

        while (machineYaxis.transform.localPosition.y > -0.18f)
        {
            machineYaxis.transform.localPosition += new Vector3(0, -speed, 0) * Time.deltaTime;

            yield return null;
        }

        machineYaxis.transform.localPosition = new Vector3(machineYaxis.transform.localPosition.x, -0.18f, machineYaxis.transform.localPosition.z);

        yield return new WaitForSeconds(waitTime);

        isMoveUp = true;

        while (machineYaxis.transform.localPosition.y < 0.8f)
        {
            machineYaxis.transform.localPosition += new Vector3(0, speed, 0) * Time.deltaTime;

            yield return null;
        }

        machineYaxis.transform.localPosition = new Vector3(machineYaxis.transform.localPosition.x, 0.8f, machineYaxis.transform.localPosition.z);

        while (machineYaxis.transform.localPosition.x > 1.3f || machineYaxis.transform.localPosition.z > -1.7f)
        {
            if (machineYaxis.transform.localPosition.x > 1.3f)
            {
                machineXaxis.transform.localPosition += new Vector3(-speed, 0, 0) * Time.deltaTime;
                machineYaxis.transform.localPosition += new Vector3(-speed, 0, 0) * Time.deltaTime;
            }
            if (machineYaxis.transform.localPosition.z > -1.7f)
            {
                machineZaxis.transform.localPosition += new Vector3(0, 0, -speed) * Time.deltaTime;
                machineYaxis.transform.localPosition += new Vector3(0, 0, -speed) * Time.deltaTime;
            }

            yield return null;
        }

        machineXaxis.transform.localPosition = new Vector3(1.3f, 1, -1);
        machineYaxis.transform.localPosition = new Vector3(1.3f, 0.8f, -1.7f);
        machineZaxis.transform.localPosition = new Vector3(2, 1, -1.7f);

        isMoveUp = false;
        isPush = false;
    }
}
