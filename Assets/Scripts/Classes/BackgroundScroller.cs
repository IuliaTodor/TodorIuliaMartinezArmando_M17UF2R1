using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Mueve el fondo para que haga scroll
/// </summary>
public class BackgroundScroller : MonoBehaviour
{
    //Un RawImage es una forma de mostrar texturas sin modificaciones o procesos. Sirve para UI.
    [SerializeField] private RawImage image;
    [SerializeField] private float x;
    [SerializeField] private float y;
    private void Update()
    {
        //Un uvRect manipula las texturas de una RawImage. En este caso la manipulamos con los parámetros x,y
        image.uvRect = new Rect(image.uvRect.position + new Vector2(x, y) * Time.deltaTime, image.uvRect.size);
    }
}
