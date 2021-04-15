using System.Collections;
using UnityEngine;

public class CameraMomement : MonoBehaviour
{
    public LayerMask clippingIngnoreMask;
    public GameObject player; //everything that needs a reference to the player uses this one
    PlayerMovement playerMovement;

    public float startXRotation;
    public float Xrot { get; private set; }
    public float Yrot { get; private set; }
    public float sensitivity;

    public float cameraSmoothing;
    public float maxCameraLength;
    public float minCameraLength;
    float currCameraLength;
    Transform cameraTransform;

    void Start()
    {
        currCameraLength = maxCameraLength;
        playerMovement = player.GetComponent<PlayerMovement>();
        cameraTransform = transform.GetChild(0).transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Xrot = startXRotation;
    }

    bool moveCam;
    void FixedUpdate()
    {
        if (moveCam) //fade to cameras max length
        {
            float smoothPosZ = Mathf.Lerp(cameraTransform.localPosition.z, -currCameraLength, cameraSmoothing * Time.deltaTime);
            cameraTransform.localPosition = new Vector3(0, 0, smoothPosZ);
        }
    }

    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z); //camera follows player

        CameraSnapping();

        Look();

        Zoom();

        CheckCameraClipping();
    }

    public bool canLook = true;
    void Look()
    {
        if (canLook)
        {
            //rotates camera to where player is looking
            if (!snapping)
            {
                Yrot += Input.GetAxisRaw("Mouse X") * sensitivity; //get mouse input
                Xrot += Input.GetAxisRaw("Mouse Y") * sensitivity;
            }

            Xrot = Mathf.Clamp(Xrot, -90, 90); //can't look past 90 up or down

            transform.rotation = Quaternion.Euler(Xrot, Yrot, 0);
        }
    }

    void CheckCameraClipping()
    {
        //makes sure camera dosn't clip through walls

        if (Physics.Raycast(transform.position, cameraTransform.position - transform.position, out RaycastHit hit, currCameraLength, ~clippingIngnoreMask)) //if a wall is between the player and the camera
        {
            float clipLength = hit.distance - .1f; //set the distance to slightly closer to the player to not go through walls
            cameraTransform.localPosition = new Vector3(0, 0, -clipLength);
        }
        else
        {
            moveCam = cameraTransform.localPosition.z != -currCameraLength; //if camera is not at its max lenth
        }
    }

    float zoomSpeed = .5f;
    void Zoom()
    {
        //camera zooming

        float scrollAxis = Input.GetAxis("Mouse ScrollWheel");

        if (scrollAxis > 0)
        {
            if (currCameraLength - zoomSpeed <= minCameraLength) //if reached min length
            {
                currCameraLength = minCameraLength;
            }
            else
            {
                currCameraLength -= zoomSpeed;
            }
        }
        else if (scrollAxis < 0)
        {
            if (currCameraLength + zoomSpeed >= maxCameraLength) //if reached max length
            {
                currCameraLength = maxCameraLength;
            }
            else
            {
                currCameraLength += zoomSpeed;
            }
        }
    }

    void CameraSnapping()
    {
        if (Input.GetButtonDown("CameraSnap") && !snapping) //snaps the camera to the players forward direction, if not already snapping
        {
            float newY = playerMovement.targetRotation.eulerAngles.y; //gets new y rotation
            Yrot %= 360; //remove for beyblade action
            if (Yrot < 0) Yrot += 360; //makes sure y rotation isn't negative, because new y will never be negative

            if (Mathf.Abs(newY - Yrot) > 180) //can never rotate more than 180 degrees, if it does use nagative values instead
            {
                if (newY > 180)
                {
                    newY -= 360;
                }

                if (Yrot > 180)
                {
                    Yrot -= 360;
                }
            }

            StartCoroutine(RotateCamera(startXRotation, newY));
        }
    }

    public float snapSpeed;
    bool snapping;
    IEnumerator RotateCamera(float x, float y)
    {
        snapping = true; //can't move camera with mouse during coroutine

        //which axis will take longer
        float xDiff = Mathf.Abs(x - Xrot);
        float yDiff = Mathf.Abs(y - Yrot);

        if (yDiff > xDiff)
        {
            float xSpeed = xDiff / (yDiff / snapSpeed); //make sure they finish at the same time

            while (y != Yrot)
            {
                Xrot = FixedInterpolation(Xrot, x, xSpeed);
                Yrot = FixedInterpolation(Yrot, y, snapSpeed);

                yield return new WaitForSeconds(.02f);
            }
        }
        else
        {
            float ySpeed = yDiff / (xDiff / snapSpeed);

            while (x != Xrot)
            {
                Xrot = FixedInterpolation(Xrot, x, snapSpeed);
                Yrot = FixedInterpolation(Yrot, y, ySpeed);

                yield return new WaitForSeconds(.02f);
            }
        }
        snapping = false;
    }

    float FixedInterpolation(float a, float b, float distance)
    {
        //get a plus distance towards b

        //a gets bigger
        if (a < b)
        {
            a += distance;

            if (a > b) a = b;
        }
        //a gets smaller
        else if (a > b)
        {
            a -= distance;

            if (a < b) a = b;
        }

        return a;
    }
}
