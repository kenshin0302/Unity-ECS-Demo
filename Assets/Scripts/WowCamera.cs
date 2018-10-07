using UnityEngine;
using System.Collections;

/// 
/// 将此脚本附加到任意镜头上，可以使其拥有WOW镜头的控制方式
/// 
public class WowCamera : MonoBehaviour
{
    /// 
    /// 镜头目标
    /// 
    public Transform Target;

    /// 
    /// 镜头离目标的距离
    /// 
    public float Distance = 30.0f;

    /// 
    /// 最大镜头距离
    /// 
    public float MaxDistance = 30.0f;

    /// 
    /// 鼠标滚轮拉近拉远速度系数
    /// 
    public float ScrollFactor = 10.0f;

    /// 
    /// 镜头旋转速度比率
    /// 
    public float RotateFactor = 10.0f;

    /// 
    /// 镜头水平环绕角度
    /// 
    public float HorizontalAngle = 45;

    /// 
    /// 镜头竖直环绕角度
    /// 
    public float VerticalAngle = 0;

    private Transform mCameraTransform;

    void Start()
    {
        mCameraTransform = transform;
    }

    void Update()
    {
        //滚轮向前：拉近距离；滚轮向后：拉远距离
        var scrollAmount = Input.GetAxis("Mouse ScrollWheel");
        Distance -= scrollAmount * ScrollFactor;



        //保证镜头距离合法
        if (Distance < 0)
            Distance = 0;
        else if (Distance > MaxDistance)
            Distance = MaxDistance;



        //按住鼠标左右键移动，镜头随之旋转
        var isMouseRightButtonDown = Input.GetMouseButton(1);
        if (isMouseRightButtonDown)
        {
            Cursor.lockState = CursorLockMode.Locked;

            var axisX = Input.GetAxis("Mouse X");
            var axisY = Input.GetAxis("Mouse Y");

            HorizontalAngle += axisX * RotateFactor;
            VerticalAngle += axisY * RotateFactor;
            if (isMouseRightButtonDown)
            {
                //如果是鼠标右键移动，则旋转人物在水平面上与镜头方向一致
                Target.rotation = Quaternion.Euler(0, HorizontalAngle, 0);
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }



        //按镜头距离调整位置和方向
        var rotation = Quaternion.Euler(-VerticalAngle, HorizontalAngle, 0);
        var offset = rotation * Vector3.back * Distance;
        mCameraTransform.position = Target.position + offset;
        mCameraTransform.rotation = rotation;
    }
}