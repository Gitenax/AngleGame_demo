using System;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;


public class SelectableTile : MonoBehaviour, IPointerClickHandler
{
    #pragma warning disable CS0649
    [SerializeField] private Point         _pointPosition  = Point.Zero;
    [SerializeField] private Vector2       _vectorPosition = Vector2.zero;
    [SerializeField] private FigureOptions _figureOptions;
                     private RectTransform _rectTransform;
                     private bool          _isMultijumpRoot;
    #pragma warning restore CS0649

    
    public event Action<SelectableTile> TileSelected;

    
    public Point PointPosition
    {
        get => _pointPosition;
        set => _pointPosition = value;
    }
    
    public Vector2 VectorPosition
    {
        get => _vectorPosition;
        set => _vectorPosition = value;
    }

    public bool IsMultijumpRoot
    {
        get => _isMultijumpRoot;
        set => _isMultijumpRoot = value;
    }


    public void InitializeFields(Point pointPosition)
    {
        _rectTransform = GetComponent<RectTransform>();
        _pointPosition = pointPosition;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        TileSelected?.Invoke(this);
    }
    
    
    private void Start()
    {
        SetVectorPositionFromPoint(PointPosition);
    }
    
    private void SetVectorPositionFromPoint(Point point)
    {
        int offsetX = _figureOptions.FigureWidth;
        int offsetY = _figureOptions.FigureHeight;

        VectorPosition = new Vector2(
            point.X * offsetX,
            (point.Y * offsetY * -1) - offsetY);
       
        // -1 чтобы расположение было сверху->вниз
        // -offsetY чтобы сместить фигуру вниз по ее размеру
        
        _rectTransform.anchoredPosition = VectorPosition;
    }
}
