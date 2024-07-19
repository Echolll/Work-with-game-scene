using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ShapeMove;
using static ShapeMove2;

public class ShapeMove2 : MonoBehaviour
{
    private Controls control;
    [SerializeField, Range(0, 10)] private float _speed = 2;
    [SerializeField, Range(0, 10)] private float _stepSize = 5;
    private List<GameObject> _Shape2 = new();
    public enum Shape2 { Hexagon, Star }
    [SerializeField] private Shape2 _ComplexShapes;

    private void Awake() => control = new();

    private void OnEnable()
    {
        control.Enable();
        control.ShapeMontion.StartMon.started += callbackContext => StartMon(_ComplexShapes);
        control.ShapeMontion.StopMon.started += callbackContext => StopMon();
    }

    private void StopMon()
    {
        StopAllCoroutines();
        transform.position = new Vector3(-10, 0, 0);
        transform.rotation = Quaternion.identity;
        removeAllPoint();
    }

    private void StartMon(Shape2 shape)
    {
        switch (shape)
        {
            case Shape2.Hexagon:
                StartCoroutine(ShapeHexagon());
                break;
            case Shape2.Star:
                StartCoroutine(ShapeStar());
                break;
        }
    }

    private IEnumerator CharacterMove()
    {
        Vector3 MoveToPoint = transform.position + transform.forward * _stepSize;
        while (transform.position != MoveToPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, MoveToPoint, _speed * Time.deltaTime);
            yield return null;
        }
        yield break;
    }

    private IEnumerator ShapeHexagon()
    {
        do
        {
            yield return CharacterMove();
            yield return new WaitForSeconds(0.5f);
            CreatePoint();
            transform.Rotate(Vector3.up, 60);
        } while (true);
    }

    private IEnumerator ShapeStar()
    {
        do
        {
            yield return CharacterMove();
            CreatePoint();
            transform.Rotate(Vector3.up, -72);
            yield return CharacterMove();
            CreatePoint();
            transform.Rotate(Vector3.up, 144);
        }while (true);
    }

    private void CreatePoint()
    {
        GameObject createPoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
        createPoint.transform.localScale = Vector3.one * 0.3f;
        createPoint.transform.position = transform.position;
        _Shape2.Add(createPoint);
    }

    private void removeAllPoint()
    {
        _Shape2.ForEach(shape => Destroy(shape));
        _Shape2.Clear();
    }
}
