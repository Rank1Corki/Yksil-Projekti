using UnityEngine;

public class InventoryToggle1 : MonoBehaviour
{
    public CanvasGroup inventoryCanvasGroup;  // Reference to the CanvasGroup of the inventory panel
    public KeyCode toggleKey = KeyCode.Tab;   // The key to hold for toggling the inventory

    void Update()
    {
        // Check if the toggle key is held down
        if (Input.GetKey(toggleKey))
        {
            ShowInventory();
        }
        else
        {
            HideInventory();
        }
    }

    // Method to show the inventory
    void ShowInventory()
    {
        inventoryCanvasGroup.alpha = 1;  // Make the inventory visible
        inventoryCanvasGroup.interactable = true;  // Enable interaction with the inventory
        inventoryCanvasGroup.blocksRaycasts = true;  // Enable clicking on the inventory
    }

    // Method to hide the inventory
    void HideInventory()
    {
        inventoryCanvasGroup.alpha = 0;  // Make the inventory invisible
        inventoryCanvasGroup.interactable = false;  // Disable interaction with the inventory
        inventoryCanvasGroup.blocksRaycasts = false;  // Disable clicking on the inventory
    }
}
