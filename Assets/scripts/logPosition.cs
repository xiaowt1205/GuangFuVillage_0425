using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class logPosition : MonoBehaviour

{
    [SerializeField]
    private ARTrackedImageManager m_TrackedImageManager;

    [SerializeField]
    private ARPlaneManager m_PlaneManager;
    public GameObject model;

    [SerializeField]
    private Vector3 planePos;

    private GameObject inst;

    public Text anno;

    public Animator stepAni;

    private bool getTarget;

    private bool aniPlayOnce;

    private GameObject camera;
    void Awake(){
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
        m_PlaneManager = GetComponent<ARPlaneManager>();
        m_TrackedImageManager.enabled = false;
        m_PlaneManager.enabled = false;
        aniPlayOnce = false;
        camera = GameObject.Find("AR Camera");
    }
    
    void Start(){
        anno.text = "請翻轉螢幕";
        stepAni.SetInteger("step",1);
        StartCoroutine(Timer1());

    }
    void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void Update(){
        foreach (var plane in m_PlaneManager.trackables)
        {   
            if(!getTarget){
                planePos.y = plane.transform.position.y;
                anno.text = "以取得地面高度";
                StartCoroutine(Timer2());
            }else{
                plane.gameObject.SetActive(false);
            }
        }
    }

    
    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            Vector3 savePosition = trackedImage.gameObject.transform.localPosition;
            Quaternion saveRotate = trackedImage.gameObject.transform.rotation;
            inst = Instantiate(model,Vector3.zero,Quaternion.identity);
            inst.gameObject.transform.parent = trackedImage.transform;
            inst.transform.localPosition = savePosition;
            Vector3 w_instPos = transform.TransformPoint(inst.transform.localPosition);
            inst.gameObject.transform.parent = null;
            inst.transform.position = new Vector3(w_instPos.x,planePos.y,w_instPos.z);
            m_PlaneManager.enabled = false;
            PlayTextAni(true);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            
        }
    }

    void PlayTextAni(bool active){
        if(active && !aniPlayOnce){
            anno.text = "歡迎來到光復興村～";
            stepAni.SetInteger("step",4);
            anno.gameObject.GetComponent<Animator>().SetBool("textFadeOut",true);
            aniPlayOnce = true;
        }
    }

    IEnumerator Timer1(){
        yield return new WaitForSecondsRealtime(4);
        anno.text = "偵測地面高度 請將鏡頭朝下並且隨機移動";
        stepAni.SetInteger("step",2);
        m_PlaneManager.enabled = true;
    }

    IEnumerator Timer2(){
        yield return new WaitForSecondsRealtime(3);
        getTarget = true;
        anno.text = "請掃描圖卡";
        stepAni.SetInteger("step",3);
        m_TrackedImageManager.enabled = true;
    }
}
