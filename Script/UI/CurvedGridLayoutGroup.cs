using UnityEngine;
using UnityEngine.UI;

namespace Big2Meow.UI 
{
    /// <summary>
    /// A custom GridLayoutGroup that arranges its children in a curved layout.
    /// </summary>
    [ExecuteInEditMode]
    public class CurvedGridLayoutGroup : MonoBehaviour
    {
        /// <summary>
        /// The radius of the curvature.
        /// </summary>
        public float curveRadius = 200f;

        /// <summary>
        /// The size of each cell.
        /// </summary>
        public Vector2 cellSize = new Vector2(100f, 100f);

        /// <summary>
        /// The number of cells per row.
        /// </summary>
        public int cellsPerRow = 3;

        /// <summary>
        /// The angle difference between adjacent cards.
        /// </summary>
        public float rotationFactor = 10f;

        private RectTransform rectTransform;

        /// <summary>
        /// Called when the inspector's values change.
        /// </summary>
        void OnValidate()
        {
            UpdateLayout();
        }

        /// <summary>
        /// Called when the script is started.
        /// </summary>
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        /// <summary>
        /// Called every frame to update the layout.
        /// </summary>
        void Update()
        {
            UpdateLayout();
        }

        /// <summary>
        /// Updates the layout of the children in a curved pattern.
        /// </summary>
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

}

