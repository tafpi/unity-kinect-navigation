using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPlayer : MonoBehaviour
{
    public GameObject logger;
    [HideInInspector] public LogSystem logSystem;
    public int interactionTimeNeeded = 2;
    [Range(0f, 1f)] public float viewportFactor = 0.35f;
    [HideInInspector] public GameObject player;
    [HideInInspector] public Camera cam;
    public bool drawTarget;
    [HideInInspector] public Color targetColor;
    [HideInInspector] public int picked;
    [HideInInspector] public float totalTimeSearching;
    [HideInInspector] public PickupObject activePickupObject;

    private GUIStyle targetStyle = new GUIStyle();
    Camera c;
    public Canvas canvas;
    

    private void Awake()
    {
        logSystem = logger.GetComponent<LogSystem>();
        cam = GetComponentInParent<Camera>();
        player = cam.transform.parent.gameObject;
        //foreach (var pickupGroup in logSystem.pickupGroup)
        //{
        foreach (Transform pickup in logSystem.pickupGroup.transform)
        {
            pickup.GetComponent<PickupObject>().pickupPlayer = this;
            logSystem.runLogger.totalPickups++;
        }
        //}

        c = transform.GetComponentInParent<Camera>();
        canvas = transform.GetChild(0).GetComponent<Canvas>();
        canvas.transform.GetChild(0).localScale = Vector3.one * viewportFactor;
        canvas.gameObject.SetActive(false);
    }

    void Start()
    {

    }

    private void Update()
    {

    }

    public bool PickupInTarget(Vector3 pickupPos, Renderer renderer)
    {
        // check if pickup object's position relative to camera viewport is in bounds of camera's target
        if (renderer.isVisible)
        {
            Vector3 viewPos = cam.WorldToViewportPoint(pickupPos);
            return (viewPos.x > (1 - viewportFactor) / 2 && viewPos.x < (1 + viewportFactor) / 2 && viewPos.y > (1 - viewportFactor) / 2 && viewPos.y < (1 + viewportFactor) / 2);
        }
        else
        {
            return false;
        }
    }

    public void TotalSearchTime()
    {
        float total = 0;
        //foreach (var pickupGroup in logSystem.pickupGroup.transform)
        //{
        foreach (Transform pickup in logSystem.pickupGroup.transform)
        {
            total += pickup.GetComponent<PickupObject>().searchDuration;
        }
        //}
        logSystem.runLogger.pickupSearch = total;
    }

    //private void OnDrawGizmos()
    //{
    //    Camera c = GetComponentInParent<Camera>();
    //    if (drawTarget)
    //    {
    //        // draw a target: part of the viewport with same aspect ratio and dimensions defined by factoring. ie: factor 1 means same dimensions as viewport
    //        Gizmos.color = targetColor;
    //        Gizmos.matrix = Matrix4x4.TRS(c.transform.position, transform.rotation, Vector3.one);
    //        Gizmos.DrawFrustum(Vector3.zero, c.fieldOfView * viewportFactor, 10f, 10.5f, c.aspect);
    //    }        
    //}

    //private void OnGUI()
    //{
    //    Camera c = GetComponentInParent<Camera>();
    //    if (drawTarget)
    //    {
    //        //GUI.Box(new Rect(0, Screen.height / 4, Screen.width, Screen.height), "sdf", style);
    //        Texture2D texture = new Texture2D(1, 1);
    //        texture.SetPixel(0, 0, new Color(targetColor.r, targetColor.g, targetColor.b, 0.15f));
    //        texture.Apply();
    //        GUI.skin.box.normal.background = texture;
    //        GUI.Box(new Rect(Screen.width * ((1 - viewportFactor) / 2), Screen.height * ((1 - viewportFactor) / 2), Screen.width * viewportFactor, Screen.height * viewportFactor), GUIContent.none);
    //    }
    //}
}