using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{
    public enum ControlType
    {
        ConstantWidth,       //Camera will keep constant width
        ConstantHeight      //Camera will keep constant height
    }

    //camera control type
    public ControlType controlType;
    //camera will autoscale to fit the object
    public SpriteRenderer focusObjectRenderer;
    // Horizontal offset from focus object edges
    public float offsetX = 0f;
    // Vertical offset from focus object edges
    public float offsetY = 0f;
    //camera speed when moving(draging)
    public float dragSpeed = 2f;
    //restrictive points for camera moving
    private float maxX, minX, maxY, minY;
    //camera dragging at now camrea
    private float moveX, moveY;
    //camera component from this gameobject
    private Camera cam;
    //origin camera aspect ratio
    private float originaAspect;

     void Start()
    {
        cam = GetComponent<Camera>();
        Debug.Assert(focusObjectRenderer && cam, "Wrong initial settings");
        originaAspect = cam.aspect;
        //get restrictive points from focus object's corners
        maxX = focusObjectRenderer.bounds.max.x;
        minX = focusObjectRenderer.bounds.min.x;
        maxY = focusObjectRenderer.bounds.max.y;
        minY = focusObjectRenderer.bounds.min.y;
        UpdateCameraSize();
    }

    //:LateUpdate是在所有Update函数调用后被调用,多用于摄像机，控制Update速度
    void LateUpdate()
    {
        //Camera aspect ratio is changed
        if (originaAspect!=cam.aspect)
        {
            UpdateCameraSize();
            originaAspect = cam.aspect;
        }

        //need to move camera horizontally
        if (moveX != 0f)
        {
            //Allowed to move horizontally
            if (controlType==ControlType.ConstantHeight)
            {
                bool permit =false;
                //move to right
                if (moveX>0f)
                {   //if restrictive point does not reached
                    //am.aspect 获取相机的宽高比 cam.orthograhicSize获取相的默认值
                    if (cam.transform.position.x + (cam.orthographicSize * cam.aspect) < maxX - offsetX)
                    {
                        permit= true;
                    }
                }
                else
                {
                    //if restrictive point does not reached
                    if (cam.transform.position.x - (cam.orthographicSize * cam.aspect) > minX + offsetX)
                    {
                        permit = true;
                    }
                }
                if (permit==true)
                {
                    //move camera
                    transform.Translate(Vector3.right * moveX * dragSpeed, Space.World);
                }
            }
            moveX = 0f;
        }
    }
    public void MoveX(float distance)
    {
        moveX = distance;
    }

    public void  MoveY(float distance)
    {
        moveY = distance;
    }
    private void UpdateCameraSize()
    {
        switch (controlType)
        {   //orthographicSize:摄像机镜头值大小
            case ControlType.ConstantWidth:
                cam.orthographicSize = (maxX - minX - 2 * offsetX) / (2f * cam.aspect);
                break;
            case ControlType.ConstantHeight:
                cam.orthographicSize = (maxY - minY - 2 * offsetY) / 2f;
                break;
        }
    }
}
