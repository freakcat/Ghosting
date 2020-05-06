using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    private GameObject myShadow;
    private Transform theMain2DWord;
    private Transform playerControl;

    [Range(1, 100)]
    public float speed;


    public float curX;
    public float curY;
    public bool moving;

    [Range(0f, 1f)]
    public float disappearTime = 0.05f;

    //存放影子
    public Queue<GameObject> shadows = new Queue<GameObject>();
    private void Awake()
    {
        theMain2DWord = GameObject.Find("Canvas").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        myShadow = Resources.Load<GameObject>("shadow");
        playerControl = transform;

        for (int i = 0; i < 10; i++)
        {
            var queueShadow = Instantiate(myShadow, theMain2DWord);
            SetShadow(queueShadow);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("a"))
        {
            playerControl.Translate(Vector3.left * speed);

        }
        if (Input.GetKey("d"))
        {
            playerControl.Translate(Vector3.right * speed);
        }
        if (Input.GetKey("w"))
        {
            playerControl.Translate(Vector3.up * speed);

        }
        if (Input.GetKey("s"))
        {
            playerControl.Translate(Vector3.down * speed);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            playerControl.GetComponent<RectTransform>().sizeDelta *= 0.9f; 
        }

        if (curX != playerControl.position.x|| curY != playerControl.position.y)
        {
            moving = true;
            curX = playerControl.position.x;
            curY = playerControl.position.y;
        }
        else
        {
            moving = false;
        }
        if (moving)
        {
            //var theShadow = Instantiate(myShadow, theMain2DWord);
            //theShadow.transform.position = playerControl.position;
            //StartCoroutine(TimeToDestroyShadow(theShadow));
            //Time.frameCount % 5 == 0 控制获取影子的频率
            if (Time.frameCount % 5 == 0)
            {
                var theShadow = GetShadow(1f, () => { });
                if (theShadow != null)
                {
                    theShadow.transform.SetParent(theMain2DWord);
                    theShadow.transform.position = playerControl.position;
                    theShadow.GetComponent<RectTransform>().sizeDelta = playerControl.GetComponent<RectTransform>().sizeDelta;
                   // theShadow.GetComponent<RectTransform>().anchorMax = playerControl.GetComponent<RectTransform>().anchorMax;
                  //  theShadow.GetComponent<RectTransform>().anchorMin = playerControl.GetComponent<RectTransform>().anchorMin;
                }
            }
        }

    }



    /// <summary>
    /// 获取影子
    /// </summary>
    /// <returns></returns>
    private GameObject GetShadow()
    {
        //如果没有了，新建
        if (shadows.Count <= 0) CerateShadow();
        //取得影子并初始化
        var theShadow = shadows.Dequeue();
        theShadow.transform.localScale = Vector3.one;
        theShadow.GetComponent<UnityEngine.UI.Image>().color = new Color(1f, 1f, 1f, 0.7f);
        return theShadow;
    }

    /// <summary>
    /// 获取影子
    /// </summary>
    /// <param name="time">回收时间</param>
    /// <param name="action">回收事件</param>
    /// <returns></returns>
    private GameObject GetShadow(float time, System.Action action = null)
    {
        //如果没有了，新建
        if (shadows.Count <= 0) CerateShadow();
        //取得影子并初始化
        var theShadow = shadows.Dequeue();
        theShadow.transform.localScale = Vector3.one;
        theShadow.GetComponent<UnityEngine.UI.Image>().color = new Color(1f, 1f, 1f, 0.7f);
        //设置自动回收
        StartCoroutine(TimeToDestroyShadow(theShadow));
        return theShadow;
    }

    /// <summary>
    /// 回收影子
    /// </summary>
    /// <param name="obj">要回收的影子</param>
    private void SetShadow(GameObject obj)
    {
        //初始化
        obj.transform.localScale = Vector3.zero;
        obj.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        shadows.Enqueue(obj);
    }

    /// <summary>
    /// 创建影子
    /// </summary>
    private void CerateShadow()
    {
        var queueShadow = Instantiate(myShadow, theMain2DWord);
        SetShadow(queueShadow);
    }


    /// <summary>
    /// 等待回收影子
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    IEnumerator TimeToDestroyShadow(GameObject obj)
    {
        var a = obj.GetComponent<UnityEngine.UI.Image>().color.a;

        while (a > 0)
        {
            yield return new WaitForSeconds(disappearTime);
            a -= 0.1f;
            obj.GetComponent<UnityEngine.UI.Image>().color = new Color(1f, 1f, 1f, a);
        }

        yield return new WaitForSeconds(0.1f);
        SetShadow(obj);
    }
}
