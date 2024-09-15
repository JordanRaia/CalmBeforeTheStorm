using UnityEngine;
using TMPro; // Make sure to include this if using TextMeshPro

public class PointsManager : MonoBehaviour
{
    public TextMeshProUGUI pointsText; // Drag and drop your text object in the Inspector
    private int points = 0;            // Starting points
    private int maxPoints = 9999;      // Maximum point limit

    void Start()
    {
        UpdatePointsDisplay(); // Update the UI on start
    }

    // Function to add points
    public void AddPoints(int amount)
    {
        points += amount;

        // Cap the points at the maximum value
        if (points > maxPoints)
        {
            points = maxPoints;
        }

        UpdatePointsDisplay(); // Update the UI whenever points change
    }

    // Function to update the points display
    private void UpdatePointsDisplay()
    {
        pointsText.text = "x" + points.ToString();
    }
}
