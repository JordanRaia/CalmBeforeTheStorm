using UnityEngine;
using TMPro;

public class PointsManager : MonoBehaviour
{
    public TextMeshProUGUI pointsText; // Reference to your in-game coin display
    public int points = 0;            // Starting points
    private int maxPoints = 9999;      // Maximum point limit

    void Start()
    {
        UpdatePointsDisplay(); // Update the UI on start
    }

    // Function to add points
    public void AddPoints(int amount)
    {
        points += amount;
        if (points > maxPoints)
        {
            points = maxPoints;
        }
        UpdatePointsDisplay();
    }

    // Function to spend points
    public void SpendPoints(int amount)
    {
        points -= amount;
        if (points < 0)
        {
            points = 0;
        }
        UpdatePointsDisplay();
    }

    // Function to get current points
    public int GetPoints()
    {
        return points;
    }

    // Function to update the points display
    private void UpdatePointsDisplay()
    {
        pointsText.text = "x" + points.ToString();
    }
}
