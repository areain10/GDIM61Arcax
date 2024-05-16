using UnityEngine;
using UnityEngine.UI;

public class TaskBase : MonoBehaviour
{
    // Reference to the task UI canvas
    public GameObject taskCanvas;

    // Whether the task is currently active
    private bool taskActive = false;

    // Reference to the button component on the task canvas
    public Button taskButton;

    // String representing the key
    private string keys = "Keys: ";

    private int keyNum = 0;

    // Whether the task has been completed
    private bool taskCompleted = false;

    public GameObject taskReward;

    // Start is called before the first frame update
    void Start()
    {
        // Deactivate the task canvas at the start
        taskCanvas.SetActive(false);

        // Add listener to the task button
        taskButton.onClick.AddListener(CompleteTask);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is near the object and presses the interaction key (e.g., 'E')
        if (Input.GetKeyDown(KeyCode.E) && Vector3.Distance(transform.position, mainplayer.instance.transform.position) < 2f && !taskActive && !taskCompleted)
        {
            StartTask();
        }

        // Check if the escape key is pressed and a task is active
        if (Input.GetKeyDown(KeyCode.Escape) && taskActive && !taskCompleted)
        {
            CloseTask();
        }
    }

    // Method to start the task
    void StartTask()
    {
        // Check if the task is not completed
        if (!taskCompleted)
        {
            // Show the task canvas
            taskCanvas.SetActive(true);

            // Lock player movement or perform other necessary actions
            mainplayer.instance.UpdatePlayerState(PlayerStates.Interacting); // Example function, adjust according to your player controller

            // Set task as active
            taskActive = true;
        }
    }

    // Method to close the task
    void CloseTask()
    {
        // Hide the task canvas
        taskCanvas.SetActive(false);

        // Unlock player movement or perform other necessary actions
        mainplayer.instance.UpdatePlayerState(PlayerStates.Walking); // Example function, adjust according to your player controller

        // Set task as inactive
        taskActive = false;
    }

    // Method to complete the task
    public void CompleteTask()
    {
        if (!taskCompleted)
        {
            // Close the task
            CloseTask();

            // Mark the task as completed
            taskCompleted = true;

            // Increment the key by one
            keyNum += 1;

            GameObject reward = Instantiate(taskReward, transform.position, transform.rotation);
            mainplayer.instance.keyItems.Add(reward);
        }
    }
}
