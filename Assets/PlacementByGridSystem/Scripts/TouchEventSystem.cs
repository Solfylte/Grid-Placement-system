using UnityEngine;
//using UnityEngine.EventSystems;

public class TouchEventSystem : MonoBehaviour
{
    public delegate void ZumSwype(float velocity);
    public event ZumSwype zumSwypeMessage;

    public delegate void DoubleTouch();
    public event DoubleTouch doubleTouchMessage;

    //Зум свайп
    [SerializeField, Range(1f, 50f)]
    private float zumSpeed;                         // швидкість (для зуму)
    [SerializeField, Range(0, 5f)]
    private float treshold;                         // мінімальна чутливість свайпу
    private Vector2 touch0start;                    // тач0, початкові координати
    private Vector2 touch1start;                    // тач1, початкові координати

    //подвійний клік
    private int TapCount=0;
    [SerializeField, Range(0.1f, 5f)]
    private float MaxDubbleTapTime;
    private float NewTime;

    void Update()
    {
        CheckZoomSwype();
        CheckDoubleTouch();
#if UNITY_EDITOR
        //Перевіряю мишу тільки для редактора!
        CheckDoubleClick();
#endif
    }

    private void CheckZoomSwype()
    {
        //Перевіряю мишу тільки для редактора!
#if UNITY_EDITOR
        //Скролл колесом миші для PC
        float mouseWhellCount = Input.GetAxis("Mouse ScrollWheel");
        if (mouseWhellCount != 0)
            zumSwypeMessage?.Invoke(-mouseWhellCount * 20f);
        //transform.position += transform.forward * Time.deltaTime * mouseWhellCount * 100f;
#endif
        //Скролл свайпом двома пальцями
        if (Input.touchCount == 2)
        {
            //Обновити початкові позиції лише в момент дотику до екрану
            if (touch0start == Vector2.zero && touch1start == Vector2.zero)
            {
                touch0start = Input.GetTouch(0).position;
                touch1start = Input.GetTouch(1).position;
            }

            //Обновити поточні позиції
            Vector2 f0position = Input.GetTouch(0).position;
            Vector2 f1position = Input.GetTouch(1).position;

            //Дистанція між початковими координатами тача, та поточними
            float deltaTouch = Vector2.Distance(touch0start, touch1start) - Vector2.Distance(f0position, f1position);

            //Напрямок зуму
            float dir = Mathf.Sign(deltaTouch);

            //Рух камери в напрямку її осі Z, якщо є мінімально встановлений рух двома пальцями
            if (deltaTouch > treshold || deltaTouch < -treshold)
                zumSwypeMessage?.Invoke(zumSpeed * dir);

            //присвоєння початковим координатам поточних координат, що б переконатись в наступному кадрі, що необхідно продовжувати зум
            touch0start = f0position;
            touch1start = f1position;

            return;
        }
        else //Після закінчення свайпа обнулити координати
        {
            touch0start = Vector2.zero;
            touch1start = Vector2.zero;
        }
        return;
    }

    private void CheckDoubleTouch()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
                TapCount += 1;

            if (TapCount == 1)
            {
                NewTime = Time.time + MaxDubbleTapTime;
            }
            else if (TapCount == 2 && Time.time <= NewTime)
            { 
                doubleTouchMessage?.Invoke();
                TapCount = 0;
            }
        }

        if (Time.time > NewTime)
            TapCount = 0;
    }

#if UNITY_EDITOR
    private void CheckDoubleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TapCount += 1;

            if (TapCount == 1)
            {
                NewTime = Time.time + MaxDubbleTapTime;
            }
            else if (TapCount == 2 && Time.time <= NewTime)
            {
                doubleTouchMessage?.Invoke();
                TapCount = 0;
            }

            if (Time.time > NewTime)
                TapCount = 0;
        }
    }
#endif
}
