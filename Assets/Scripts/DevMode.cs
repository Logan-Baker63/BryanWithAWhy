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
    
    public float lineDist = 0;

    [SerializeField] float lengthBeforeUseConsumed = 10;

    AbilityMeter designMeter;
    AbilityMeter programmingMeter;
    AbilityMeter artMeter;

    float pointCost = 0;

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

    private void Awake()
    {
        foreach (AbilityMeter meter in FindObjectsOfType<AbilityMeter>())
        {
            if (meter.abilityType == AbilityMeter.AbilityType.Design)
            {
                designMeter = meter;
            }
            else if (meter.abilityType == AbilityMeter.AbilityType.Programming)
            {
                programmingMeter = meter;
            }
            else if (meter.abilityType == AbilityMeter.AbilityType.Art)
            {
                artMeter = meter;
            }
        }
    }

    public void EnterDevMode()
    {
        isGameSlow = true;
        foreach (Movement movement in FindObjectsOfType<Movement>())
        {
            movement.SetSlowness(slowness);
            movement.GetComponent<Attack>().canAttack = false;
        }
    }

    public void OnExitDevMode(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ExitDevMode();
        }
    }

    public void ExitDevMode()
    {
        devType = DevType.None;
        isGameSlow = false;
        foreach (Movement movement in FindObjectsOfType<Movement>())
        {
            movement.SetSlowness(1);
            movement.GetComponent<Attack>().canAttack = true;
        }
        lineDist = 0;
    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);

        ExitDevMode();
    }

    public void EnterArtDevMode(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EnterDevMode();
            lineDist = 0;
            devType = DevType.Art;
        }
    }

    public void Draw(InputAction.CallbackContext context)
    {
        Debug.Log(devType);
        if (context.performed && devType == DevType.Art)
        {
            CreateBrush();
            isDrawing = true;
        }

        if (context.canceled && devType == DevType.Art)
        {
            CompleteLine();
        }
    }

    public void CompleteLine()
    {
        if (currentLineRenderer)
        {
            artMeter.SpendAbilityPoints((int)pointCost);
            currentLineRenderer.GetComponent<DrawnLine>().GenerateLineCollider();
        }

        currentLineRenderer = null;
        isDrawing = false;
        lineDist = 0;
        lastPos = Vector2.zero;
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
                pointCost = (lineDist / lengthBeforeUseConsumed) + 1;
                Debug.Log("Attempting Drawing... cost equals: " + pointCost + " and line dist equals: " + lineDist);
                if (pointCost < artMeter.GetAbilityPoints() + 1)
                {
                    Debug.Log("Allowed to draw with cost of: " + pointCost);
                    artMeter.SetHighlightPoints((int)pointCost);
                    if (lastPos != Vector2.zero)
                    {
                        lineDist += (mousePos - lastPos).magnitude;
                        Debug.Log("Updated line dist, success");
                    }

                    AddPoint(mousePos);
                    lastPos = mousePos;
                }
                else
                {
                    //StartCoroutine(Delay(1f));
                    CompleteLine();
                    ExitDevMode();
                }
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
