using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DevMode : MonoBehaviour
{
    [SerializeField] float slowness = 10;
    bool isGameSlow = false;

    bool isDrawing = false;

    [SerializeField] GameObject brush;
    LineRenderer currentLineRenderer;

    Vector2 lastPos;

    [SerializeField] GameObject artCirclePrefab;
    public enum DevType
    {
        None,
        Programming,
        Design,
        Art,
    }
    DevType devType = DevType.None;

    public bool IsGameSlow() { return isGameSlow; }

    public void EnterDevMode()
    {
        isGameSlow = true;
        foreach (Movement movement in FindObjectsOfType<Movement>())
        {
            movement.SetSlowness(slowness);
            movement.GetComponent<Attack>().canAttack = false;
        }
    }

    public void ExitDevMode(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            devType = DevType.None;
            isGameSlow = false;
            foreach (Movement movement in FindObjectsOfType<Movement>())
            {
                movement.SetSlowness(1);
                movement.GetComponent<Attack>().canAttack = true;
            }
        }
    }

    public void EnterArtDevMode(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EnterDevMode();
            devType = DevType.Art;
        }
    }

    public void Draw(InputAction.CallbackContext context)
    {
        if (context.performed && devType == DevType.Art)
        {
            CreateBrush();
            isDrawing = true;
        }

        if (context.canceled)
        {
            if (currentLineRenderer)
            {
                currentLineRenderer.GetComponent<DrawnLine>().GenerateLineCollider();
            }
            
            currentLineRenderer = null;
            isDrawing = false;
        }
    }

    public void CreateBrush()
    {
        GameObject brushInstance = Instantiate(brush);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);

    }

    public void Update()
    {
        if (devType == DevType.Art && isDrawing)
        {
            Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (mousePos != lastPos)
            {
                AddPoint(mousePos);
                lastPos = mousePos;
            }
            
            //Instantiate(artCirclePrefab, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
        }
    }

    public void AddPoint(Vector2 pointPos)
    {
        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }

}
