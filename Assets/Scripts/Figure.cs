using System;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Figure : MonoBehaviour, IPointerClickHandler
{
    #pragma warning disable CS0649
    [SerializeField] private Point         _pointPosition  = Point.Zero;
    [SerializeField] private Vector2       _vectorPosition = Vector2.zero;
                     private RectTransform _rectTransform;
    
    [Header("Настройки отображения")] 
    [SerializeField] private FigureOptions _figureOptions;
    [SerializeField] private Color         _figureColor;
                     private Image         _figureSprite;
    [SerializeField] private bool          _isSelected;
    [SerializeField] private Player        _owner;
    #pragma warning restore CS0649
    

    public event Action<Figure> FigureSelected; 
    
    
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

    public Color FigureColor
    {
        get => _figureColor;
        set => _figureColor = value;
    }

    public Transform Parent
    {
        get => gameObject.transform.parent;
        set => gameObject.transform.SetParent(value);
    }

    public bool IsSelected
    {
        get => _isSelected;
        set => _isSelected = value;
    }

    public Player Owner => _owner;


    public void InitializeFields(Point pointPosition, Color figureColor, Player owner)
    {
        _rectTransform = GetComponent<RectTransform>();
        _figureSprite  = GetComponent<Image>();
        _pointPosition = pointPosition;
        _figureColor   = figureColor;
        _owner = owner;
        IsSelected = false;
    }

    public void UpdatePosition()
    {
        SetVectorPositionFromPoint(PointPosition);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        FigureSelected?.Invoke(this);
    }
    

    private void Start()
    {
        SetAppearance();
        UpdatePosition();
    }

    private void SetAppearance()
    {
        _figureSprite.color = _figureColor;
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
