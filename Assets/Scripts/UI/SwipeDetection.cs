using UnityEngine;
using UnityEngine.EventSystems;

//Без картинки с выставленным флагом Raycast Target ивенты не будут регистрироваться
[RequireComponent(typeof(UnityEngine.UI.Image))]
//Если не наследовать интерфейс IDragHandler, то остальные ивенты не будут регистрироваться. Хоть он и не нужен в самой логике, но его наличие обязательно
public class SwipeDetection : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler{

    public delegate void SwipeDown();
    public static event SwipeDown OnSwipeDown;

    [SerializeField]
    private float dragDelta = 100f;

    //Во время окончания перетаскивания вызывается этот ивент. В нем проверяем, какую дистанцию прошел курсор от начала нажатия до отпускания
    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("Press position + " + eventData.pressPosition);
        //Debug.Log("End position + " + eventData.position);
        Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
        DraggedDirection dir;
        //Чекаем, чтобы расстояние было больше погрешности, чтобы исключить случайные свайпы
        if(Mathf.Abs(eventData.position.y - eventData.pressPosition.y) > dragDelta)
        {
            dir = GetDragDirection(dragVectorDirection);
        }
        else
        {
            dir = DraggedDirection.None;
        }

        if (dir == DraggedDirection.Down)
        {
            if (OnSwipeDown != null)
                OnSwipeDown();
        }
    }

    //Направления перетаскивания/свайпа. None для отсутствия перетаскивания (если оно не больше погрешности)
    private enum DraggedDirection
    {
        Up,
        Down,
        Right,
        Left,
        None
    }
    
    //Направление перетаскивания, можно потом добавить универсальные диагональные свайпы, если понадобится
    private DraggedDirection GetDragDirection(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        DraggedDirection draggedDir;
        if (positiveX > positiveY)
        {
            draggedDir = (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
        }
        else
        {
            draggedDir = (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
        }
        return draggedDir;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag");
    }
}
