using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeMove : MonoBehaviour
{
   private Controls control;
   [SerializeField,Range(0,10)] private float _speed = 2;
   [SerializeField, Range(0, 10)] private float _stepSize = 5;
   private List<GameObject> _Shape = new();
   public enum Shape { Cube , Truangle }
   [SerializeField] private Shape _shape;

    private void Awake() => control = new();
    
    private void OnEnable()
    {
      control.Enable();
      control.ShapeMontion.StartMon.started += callbackContext => StartMon(_shape);
      control.ShapeMontion.StopMon.started += callbackContext => StopMon();
    }

    private void StartMon(Shape shape)
    {
        switch (shape)
        {
            case Shape.Cube:
                StartCoroutine(ShapeCube());
            break;
            case Shape.Truangle:
                StartCoroutine(ShapeTruangle());
            break;
        }
    }

    private void StopMon()
    {
        StopAllCoroutines();
        transform.position = new Vector3(10,0,0);
        transform.rotation = Quaternion.identity;
        removeAllPoint();
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

    private IEnumerator ShapeCube()
    {
            do
            {
                yield return CharacterMove();
                yield return new WaitForSeconds(0.5f);
                CreatePoint();
                transform.Rotate(Vector3.up, 90);
            } while (true);
    }

    private IEnumerator ShapeTruangle()
    {
        do
        {
            yield return CharacterMove();
            yield return new WaitForSeconds(0.5f);
            CreatePoint();
            transform.Rotate(Vector3.up, 120);
        } while (true);
    }

    private void CreatePoint()
    {
        GameObject createPoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
        createPoint.transform.localScale = Vector3.one * 0.3f;
        createPoint.transform.position = transform.position;
        _Shape.Add(createPoint);
    }

    private void removeAllPoint()
    {
        _Shape.ForEach(shape => Destroy(shape));
        _Shape.Clear(); 
    }

}
