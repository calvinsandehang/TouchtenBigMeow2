using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CurvedGridLayoutGroup : MonoBehaviour
{
    public float curveRadius = 200f;
    public Vector2 cellSize = new Vector2(100f, 100f);
    public int cellsPerRow = 3;
    public float rotationFactor = 10f; // The angle difference between adjacent cards.

    private RectTransform rectTransform;

    void OnValidate()
    {
        UpdateLayout();
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        UpdateLayout();
    }

    private void UpdateLayout()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        int rowCount = 0;
        int colCount = 0;
        int totalActiveChildren = 0;

        // First, find out how many active children we have.
        for (int i = 0; i < rectTransform.childCount; i++)
        {
            RectTransform child = rectTransform.GetChild(i) as RectTransform;
            if (child.gameObject.activeSelf)
            {
                totalActiveChildren++;
            }
        }

        for (int i = 0; i < rectTransform.childCount; i++)
        {
            RectTransform child = rectTransform.GetChild(i) as RectTransform;
            LayoutElement layoutElement = child.GetComponent<LayoutElement>();

            if (layoutElement != null && layoutElement.ignoreLayout)
            {
                continue;
            }

            if (child == null || !child.gameObject.activeSelf)
            {
                continue;
            }

            float theta = (colCount + rowCount * cellsPerRow) * 360f / (cellsPerRow * Mathf.CeilToInt((float)totalActiveChildren / cellsPerRow));
            float xPos = Mathf.Sin(theta * Mathf.Deg2Rad) * curveRadius;
            float yPos = Mathf.Cos(theta * Mathf.Deg2Rad) * curveRadius - cellSize.y / 2f;  // Adjusted to consider bottom pivot

            child.anchoredPosition = new Vector2(xPos, yPos);
            child.sizeDelta = cellSize;

            // Set the pivot to the bottom
            child.pivot = new Vector2(0.5f, 0f);

            // Calculate rotation to make it look like it's being held
            float rotationOffset = (totalActiveChildren - 1) * rotationFactor / 2f;
            float rotation = colCount * rotationFactor - rotationOffset;
            child.localRotation = Quaternion.Euler(0, 0, rotation);

            if (++colCount >= cellsPerRow)
            {
                colCount = 0;
                rowCount++;
            }
        }
    }

}
