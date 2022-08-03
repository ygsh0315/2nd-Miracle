using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//������� ���콺 �Է¿� ���� ��ü�� �����¿�� ȸ����Ű�� �ʹ�.

public class CamRotate : MonoBehaviour
{
    //�ʿ�Ӽ� : ȸ���ӵ�
    public float rotSpeed = 205;

    

    //�츮�� ���� ������ ��������
    float mx;
    float my;

    // Start is called before the first frame update
    void Start()
    {
        //������ �� ����ڰ� ������ ���� ������ ����
        mx = transform.eulerAngles.y;
        my = -transform.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        //������� ���콺 �Է¿� ���� ��ü�� �����¿�� ȸ����Ű�� �ʹ�.
        //1. ������� �Է¿� ����
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        mx += h * rotSpeed * Time.deltaTime;
        my += v * rotSpeed * Time.deltaTime;
        //-60~ 60 ���� ���� ���Ѱɱ�
        //x�� -> pitch, y�� -> Yaw, z�� -> Roll
        my = Mathf.Clamp(my, -60, 60);
        transform.eulerAngles = new Vector3(-my, mx, 0);
        //2. ������ �ʿ��ϴ�.
        //Vector3 dir = new Vector3(-my, mx, 0);

        //3. ȸ���ϰ� �ʹ�.
        //transform.eulerAngles += dir * rotSpeed * Time.deltaTime;
    }
}
