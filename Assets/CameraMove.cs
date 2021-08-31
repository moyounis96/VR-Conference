using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour
{
    private Vector3 firstpoint; //change type on Vector3
    private Vector3 secondpoint;
    private float xAngle = 0.0f; //angle for axes x for rotation
    private float yAngle = 0.0f;
    private float xAngTemp = 0.0f; //temp variable for angle
    private float yAngTemp = 0.0f;
    private bool forward = false, backward;
    public static Vector3 initPos;
    public static Quaternion initRot;

    private void Awake()
    {
        initPos = transform.position;
        initRot = transform.rotation;
    }
    void Start()
    {
        //Initialization our angles of camera
        xAngle = transform.eulerAngles.y;
        yAngle = transform.eulerAngles.x;
        transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
    }
    void Update()
    {
        Vector3 translation = new Vector3(0, 0, forward ? 1 : backward ? -1 : 0);
        transform.Translate(translation * Time.deltaTime);
        if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.IsPointerOverGameObject(0) || EventSystem.current.IsPointerOverGameObject(1)) return;
#if UNITY_EDITOR
        forward = Input.GetAxis("Vertical") > 0.5f;
        backward = Input.GetAxis("Vertical") < -0.5f;
        //Touch began, save position
        if (Input.GetMouseButtonDown(0))
        {
            firstpoint = Input.mousePosition;
            xAngTemp = xAngle;
            yAngTemp = yAngle;
        }
        //Move finger by screen
        if (Input.GetMouseButton(0))
        {
            secondpoint = Input.mousePosition;
            //Mainly, about rotate camera. For example, for Screen.width rotate on 180 degree
            xAngle = xAngTemp - (secondpoint.x - firstpoint.x) * 180.0f / Screen.width;
            yAngle = yAngTemp + (secondpoint.y - firstpoint.y) * 90.0f / Screen.height;
            //Rotate camera
            this.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
        }
#else
        //Check count touches
        if (Input.touchCount > 0)
        {
            //Touch began, save position
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                firstpoint = Input.GetTouch(0).position;
                xAngTemp = xAngle;
                yAngTemp = yAngle;
            }
            //Move finger by screen
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                secondpoint = Input.GetTouch(0).position;
                //Mainly, about rotate camera. For example, for Screen.width rotate on 180 degree
                xAngle = xAngTemp - (secondpoint.x - firstpoint.x) * 180.0f / Screen.width;
                yAngle = yAngTemp + (secondpoint.y - firstpoint.y) * 90.0f / Screen.height;
                //Rotate camera
                this.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
            }
        }
#endif
    }
    public void SetForward(bool value)
    {
        forward = value;
    }
    public void SetBackward(bool value)
    {
        backward = value;
    }
}
