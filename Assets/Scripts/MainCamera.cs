using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private Vector3 startPosition;                  // стартова позиція дотику
    private Camera cam;                             // скрипт повинен бути компонентом камери
    public Camera interfaceCam;                     // Камера інтерфейсу
    private Vector3 targetPos;                      // точка, в яку буде відбуватись зміщення                 

    public TouchEventSystem touchEventSystem;

    [SerializeField, Range(1f, 20f)]
    private float minZumDistance;                   // мінімально допустиме відхилення камери по осі Y
    [SerializeField, Range(1f, 20f)]
    private float maxZumDistance;                   // максимально допустиме відхилення камери по осі Y

    [SerializeField, Range(0, 20f)]
    private int additionalCamMuvLimit;              // Додаткове обмеження руху камери, в клітинках сітки
    private Vector2 minCamDistance;                 // Мінімальне обмеження руху камери 
    private Vector2 maxCamDistance;                 // Максимальне обмеження руху камери

    public Map map;                                 // поточна мапа(в данному випадку, встановлена з редактора)

    private void Start()
    {
        cam = GetComponent<Camera>();
        minCamDistance = new Vector2(map.xStartPoint + map.CellSize * additionalCamMuvLimit, map.zStartPoint + map.CellSize);
        maxCamDistance = new Vector2((map.XLength - additionalCamMuvLimit) *map.CellSize, (map.ZLength - additionalCamMuvLimit) * map.CellSize);

        touchEventSystem.zumSwypeMessage += ZoomCam;
    }

    void Update()
    {
        if (!GlobalSetting.mainScreenControlAvailable)      //перервати обробку, якщо управління заблоковано
            return;
        
        Move();         // Рух камери
    }

    private void Move()
    {
        if (Input.GetMouseButtonDown(0))        // натиснення лівої кнопки
        {
            startPosition = cam.ScreenToViewportPoint(Input.mousePosition); //Координати зажатого курсора на екрані
            return;
        }

        if (Input.GetMouseButton(0))            // зажатий курсор
        {
            // Різниця між зажатим до цього курсором та його позицією в наступних кадрах
            // (вісь Y на екрані -> вісь Z в ігровому просторі)
            Vector3 curPosition = new Vector3(cam.ScreenToViewportPoint(Input.mousePosition).x - startPosition.x,
                                              0, cam.ScreenToViewportPoint(Input.mousePosition).y - startPosition.y);

            //на цю різницю зміщую камеру в кожному кадрі
            targetPos = new Vector3(Mathf.Clamp(transform.position.x - curPosition.x, minCamDistance.x, maxCamDistance.x), 0,
                                    Mathf.Clamp(transform.position.z - curPosition.z, minCamDistance.y, maxCamDistance.y));

            transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            return;
        }
    }

    //Наближення/віддалення (рух камери по осі Y з певними обмеженнями)
    private void ZoomCam(float velocity) => transform.position = new Vector3(transform.position.x,
                                                                            Mathf.Clamp(transform.position.y + Time.deltaTime * velocity, minZumDistance, maxZumDistance),
                                                                            transform.position.z);
    // Зміна камер (при умові що одна з них вимкнена в редакторі. Інакше необхідні додаткові перевірки)
    public void changeCam()
    {
        interfaceCam.gameObject.SetActive(cam.isActiveAndEnabled);
        cam.gameObject.SetActive(!cam.isActiveAndEnabled);
    }
}