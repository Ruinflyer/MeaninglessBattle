using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCam : MonoBehaviour {

    //定义镜头与角色的初始距离，开始的时候定义成私有变量，我改成公有变量，为了方便在代码面板上修改  
    public float m_distanceAway = 4.5f;
    //镜头初始高度  
    public float m_distanceUp = 1.5f;
    //镜头改变时的平滑度  
    private float m_smooth = 5f;
    //玩家对象  
    public Transform m_player;
    //自己  
    private Transform m_transform;

    private float maxSuo = 100.0f;//镜头缩放范围  
    private float minSuo = 2.0f;

    private float maxSD = 20.0f;
    private float minSD = 1f;
    void Start()
    {
        m_transform = this.transform;//定义自己  
        m_player = GameObject.FindWithTag("Player").transform;
    }

    public void FindPlayer()
    {
        m_player = GameObject.FindWithTag("Player").transform;
    }

    void LateUpdate()
    {
        Zoom();//缩放  
        CameraSet();//相机设置  
        //定义一条射线  
        RaycastHit hit;
        if (Physics.Linecast(m_player.position + Vector3.up, m_transform.position, out hit))
        {
            string name = hit.collider.gameObject.tag;
            if (name != "MainCamera")
            {
                //如果射线碰撞的不是相机，那么就取得射线碰撞点到玩家的距离  
                float currentDistance = Vector3.Distance(hit.point, m_player.position);
                //如果射线碰撞点小于玩家与相机本来的距离，就说明角色身后是有东西，为了避免穿墙，就把相机拉近  
                if (currentDistance < m_distanceAway)
                {
                    m_transform.position = hit.point;
                }
            }
        }
    }
    /// <summary>  
    /// 设置相机  
    /// </summary>  
    void CameraSet()
    {
        //取得相机旋转的角度  
        float m_wangtedRotationAngel = m_player.transform.eulerAngles.y;
        //获取相机移动的高度  
        float m_wangtedHeight = m_player.transform.position.y + m_distanceUp;
        //获得相机当前角度  
        float m_currentRotationAngle = m_transform.eulerAngles.y;
        //获取相机当前的高度  
        float m_currentHeight = m_transform.position.y;
        //在一定时间内将当前角度更改为角色面对的角度  
        m_currentRotationAngle = Mathf.LerpAngle(m_currentRotationAngle, m_wangtedRotationAngel, m_smooth * Time.deltaTime);
        //更改当前高度  
        m_currentHeight = Mathf.Lerp(m_currentHeight, m_wangtedHeight, m_smooth * Time.deltaTime);
        //返回一个Y轴旋转玩家当前角度那么多的度数  
        Quaternion m_currentRotation = Quaternion.Euler(0, m_currentRotationAngle, 0);
        //玩家的位置  
        Vector3 m_position = m_player.transform.position;
        //相机位置差不多计算出来了  
        m_position -= m_currentRotation * Vector3.forward * m_distanceAway;
        //将相机应当到达的高度加进应当到达的坐标，这就是相机的新位置  
        m_position = new Vector3(m_position.x, m_currentHeight, m_position.z);
        m_transform.position = Vector3.Lerp(m_transform.position, m_position, Time.time);
        //注视玩家  
        m_transform.LookAt(m_player);
    }

    /// <summary>  
    /// 按鼠标滚轮缩放  
    /// </summary>  
    void Zoom()
    {
        //实现滑轮拖动  
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Camera.main.fieldOfView <= maxSuo)//缩放的范围  
            {
                Camera.main.fieldOfView += 2;
            }
            if (Camera.main.orthographicSize <= maxSD)
            {
                Camera.main.orthographicSize += 0.5f;
            }
        }

        //Zoom in  
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            
            if (Camera.main.fieldOfView >= minSuo)
            {
                Camera.main.fieldOfView -= 2;
            }
            if (Camera.main.orthographicSize >= minSD)
            {
                Camera.main.orthographicSize -= 0.5f;
            }
        }
    }
}