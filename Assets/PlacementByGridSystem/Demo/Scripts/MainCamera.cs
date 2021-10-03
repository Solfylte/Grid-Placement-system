using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private Vector3 startPosition;
    private Camera cam;
    public Camera interfaceCam;
    private Vector3 targetPos;  

    public TouchEventSystem touchEventSystem;

    [SerializeField, Range(1f, 20f)] private float minZumDistance; 
    [SerializeField, Range(1f, 20f)] private float maxZumDistance;

    [SerializeField, Range(0, 20f)] private int additionalCamMuvLimit;
    private Vector2 minCamDistance;
    private Vector2 maxCamDistance;

    [SerializeField] private Grid grid;

    private void Start()
    {
        cam = GetComponent<Camera>();
        minCamDistance = new Vector2(grid.Origin.x + grid.CellSize * additionalCamMuvLimit, grid.Origin.y + grid.CellSize);
        maxCamDistance = new Vector2((grid.Width - additionalCamMuvLimit) * grid.CellSize, (grid.Height - additionalCamMuvLimit) * grid.CellSize);

        touchEventSystem.zumSwypeMessage += ZoomCam;
    }

    void Update()
    {
        if (!GlobalSetting.mainScreenControlAvailable)
            return;

        Move();
    }

    private void Move()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 curPosition = new Vector3(cam.ScreenToViewportPoint(Input.mousePosition).x - startPosition.x,
                                              0, cam.ScreenToViewportPoint(Input.mousePosition).y - startPosition.y);

            targetPos = new Vector3(Mathf.Clamp(transform.position.x - curPosition.x, minCamDistance.x, maxCamDistance.x), 0,
                                    Mathf.Clamp(transform.position.z - curPosition.z, minCamDistance.y, maxCamDistance.y));

            transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            return;
        }
    }

    private void ZoomCam(float velocity) => transform.position = new Vector3(transform.position.x,
                                                                            Mathf.Clamp(transform.position.y + Time.deltaTime * velocity, minZumDistance, maxZumDistance),
                                                                            transform.position.z);
    public void changeCam()
    {
        interfaceCam.gameObject.SetActive(cam.isActiveAndEnabled);
        cam.gameObject.SetActive(!cam.isActiveAndEnabled);
    }
}