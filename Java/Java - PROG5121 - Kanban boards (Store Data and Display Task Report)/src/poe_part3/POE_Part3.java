/*
 * ST10393280 - Daniel Luke James
 */
package poe_part3;
import javax.swing.JOptionPane;
/**
 *
 * @author ST10393280 - Daniel Luke James
 */
public class POE_Part3 {
    // Declaring variable features 
    private static String[] Developers;
    private static String[] TaskNames;
    private static int[] TaskID;
    private static int[] TaskDurations;
    private static String[] TaskStatus;
    
    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {

    // Switch case statement that allows the user to choose options from the menu
    int option;
        do {
            option = displayMenu();
            switch (option) {
                case 1:
                    addNewTask();
                    break;
                case 2:
                    displayTaskStatus("done");
                    break;
                case 3:
                    displayTaskMaxDuration();
                    break;
                case 4:
                    searchTaskName();
                    break;
                case 5:
                    searchTasksFromDeveloper();
                    break;
                case 6:
                    deleteTask();
                    break;
                case 7:
                    displayReport();
                    break;
                case 8:
                    JOptionPane.showMessageDialog(null, "You are exiting the application. "
                            + "Thank you and Goodbye!");
                    break;
                default:
                    JOptionPane.showMessageDialog(null, "Invalid option! Please try again.");
            }
        } while (option != 8);
    }

    // Method that displays the menu of all the options
    private static int displayMenu() {
        String menu = "Task Manager\n" +
                "1. Add a Task\n" +
                "2. Display Tasks with status 'done'\n" +
                "3. Display Task with Longest Duration\n" +
                "4. Search for Task by Name\n" +
                "5. Search Tasks by Developer\n" +
                "6. Delete a Task by Name\n" +
                "7. Display Report\n" +
                "8. Exit\n" +
                "Enter your choice:";
        
        // returns each option being entered 
        return Integer.parseInt(JOptionPane.showInputDialog(null, menu));
    }

    // Method that allows the user to add a new task
    private static void addNewTask() {
        String developer = JOptionPane.showInputDialog(" Please enter the developer's name:");
        String taskName = JOptionPane.showInputDialog(" Please enter the task name:");
        int taskID = Integer.parseInt(JOptionPane.showInputDialog("Please enter the task ID:"));
        int taskDuration = Integer.parseInt(JOptionPane.showInputDialog("Please enter the task duration:"));
        String taskStatus  = JOptionPane.showInputDialog("Please enter the task status:");

        // Initializing the array with a size based on the length of the existing array 
        int newSize = (Developers != null) ? Developers.length + 1 : 1;

        String[] newDevelopers = new String[newSize];
        String[] newTaskNames = new String[newSize];
        int[] newTaskIDs = new int[newSize];
        int[] newTaskDurations = new int[newSize];
        String[] newTaskStatus = new String[newSize];

        // Allows the variable features to be copied in one array to another array
        if (Developers != null) {
            System.arraycopy(Developers, 0, newDevelopers, 0, Developers.length);
            System.arraycopy(TaskNames, 0, newTaskNames, 0, TaskNames.length);
            System.arraycopy(TaskID, 0, newTaskIDs, 0, TaskID.length);
            System.arraycopy(TaskDurations, 0, newTaskDurations, 0, TaskDurations.length);
            System.arraycopy(POE_Part3.TaskStatus, 0, newTaskStatus, 0, POE_Part3.TaskStatus.length);
        }

        // Values that are assigned to the last elements of the new arrays which then updates the original array
        newDevelopers[newSize - 1] = developer;
        newTaskNames[newSize - 1] = taskName;
        newTaskIDs[newSize - 1] = taskID;
        newTaskDurations[newSize - 1] = taskDuration;
        newTaskStatus[newSize - 1] = taskStatus;

        Developers = newDevelopers;
        TaskNames = newTaskNames;
        TaskID = newTaskIDs;
        TaskDurations = newTaskDurations;
        POE_Part3.TaskStatus = newTaskStatus;

        JOptionPane.showMessageDialog(null, "The task has been added successfully!");
    }

    // Method that displays the task with a status of being either done or incomplete
    private static void displayTaskStatus(String status) {
        StringBuilder output = new StringBuilder("Tasks with status '" + status + "':\n");
        boolean found = false;

        // Checking if each element is equal to the specified status
        for (int i = 0; i < TaskStatus.length; i++) {
            // if a status is found, it appends the corresponding variables 
            if (TaskStatus[i].equalsIgnoreCase(status)) {
                output.append("Developer: ").append(Developers[i]).append("\n");
                output.append("Task Name: ").append(TaskNames[i]).append("\n");
                output.append("Task Duration: ").append(TaskDurations[i]).append("\n\n");
                found = true;
            }
        }

        // if no status is found, it appends a message indicating that no tasks were found in that status
        if (!found) {
            output.append("No tasks were found with that specific status '").append(status).append("'.");
        }

        JOptionPane.showMessageDialog(null, output.toString());
    }

    // Method that displays the longest duration from the menu
    private static void displayTaskMaxDuration() {
        int maxDurationIndex = 0;
        int maxDuration = TaskDurations[0];

        // For loop indicating the longest task duration 
        for (int i = 1; i < TaskDurations.length; i++) {
            if (TaskDurations[i] > maxDuration) {
                maxDuration = TaskDurations[i];
                maxDurationIndex = i;
            }
        }

        JOptionPane.showMessageDialog(null, "Task with longest duration:\n" +
                "Developer: " + Developers[maxDurationIndex] + "\n" +
                "Task Duration: " + TaskDurations[maxDurationIndex]);
    }

    // Method that allows the user to search the task by its name
    private static void searchTaskName() {
        String taskName = JOptionPane.showInputDialog(" Please enter the task name to search a specific task:");

        // For loop that indicates the task name
        for (int i = 0; i < TaskNames.length; i++) {
            if (TaskNames[i].equalsIgnoreCase(taskName)) {
                JOptionPane.showMessageDialog(null, "Task Name: " + TaskNames[i] + "\n" +
                        "Developer: " + Developers[i] + "\n" +
                        "Task Status: " + TaskStatus[i]);
                return;
            }
        }

        JOptionPane.showMessageDialog(null, "Task was not found with that specific name.");
    }

    // Method that allows the user to search the task by the developer name
    private static void searchTasksFromDeveloper() {
        String developer = JOptionPane.showInputDialog("Please enter the developer's name to search a specific name:");

        StringBuilder output = new StringBuilder("Tasks assigned to developer '" + developer + "':\n");
        boolean found = false;

        // For loop that indicates the developers name 
        for (int i = 0; i < Developers.length; i++) {
            if (Developers[i].equalsIgnoreCase(developer)) {
                output.append("Task Name: ").append(TaskNames[i]).append("\n");
                output.append("Task Status: ").append(TaskStatus[i]).append("\n\n");
                found = true;
            }
        }

        if (!found) {
            output.append("No tasks were found assigned to that specific developer '").append(developer).append("'.");
        }

        JOptionPane.showMessageDialog(null, output.toString());
    }

    // Method that allows the user to delete a specific task 
    private static void deleteTask() {
        String taskName = JOptionPane.showInputDialog("Please enter the task name that you would like to delete:");

        // For loop that removes a task from the arrays based on the task name
        for (int i = 0; i < TaskNames.length; i++) {
            if (TaskNames[i].equalsIgnoreCase(taskName)) {
                String[] newDevelopers = new String[Developers.length - 1];
                String[] newTaskNames = new String[TaskNames.length - 1];
                int[] newTaskIDs = new int[TaskID.length - 1];
                int[] newTaskDurations = new int[TaskDurations.length - 1];
                String[] newTaskStatus = new String[TaskStatus.length - 1];

                System.arraycopy(Developers, 0, newDevelopers, 0, i);
                System.arraycopy(Developers, i + 1, newDevelopers, i, Developers.length - i - 1);

                System.arraycopy(TaskNames, 0, newTaskNames, 0, i);
                System.arraycopy(TaskNames, i + 1, newTaskNames, i, TaskNames.length - i - 1);

                System.arraycopy(TaskID, 0, newTaskIDs, 0, i);
                System.arraycopy(TaskID, i + 1, newTaskIDs, i, TaskID.length - i - 1);

                System.arraycopy(TaskDurations, 0, newTaskDurations, 0, i);
                System.arraycopy(TaskDurations, i + 1, newTaskDurations, i, TaskDurations.length - i - 1);

                System.arraycopy(TaskStatus, 0, newTaskStatus, 0, i);
                System.arraycopy(TaskStatus, i + 1, newTaskStatus, i, TaskStatus.length - i - 1);

                Developers = newDevelopers;
                TaskNames = newTaskNames;
                TaskID = newTaskIDs;
                TaskDurations = newTaskDurations;
                TaskStatus = newTaskStatus;

                JOptionPane.showMessageDialog(null, "Task that has been deleted successfully!");
                return;
            }
        }

        JOptionPane.showMessageDialog(null, "Task was not found with that specific name.");
    }

    // Method that shows the user the full report from the stored data
    private static void displayReport() {
        StringBuilder output = new StringBuilder("Task Report:\n\n");

        // For loop that retrieves the information from each task and displays the output
        for (int i = 0; i < Developers.length; i++) {
            output.append("Task Name: ").append(TaskNames[i]).append("\n");
            output.append("Developer: ").append(Developers[i]).append("\n");
            output.append("Task ID: ").append(TaskID[i]).append("\n");
            output.append("Task Duration: ").append(TaskDurations[i]).append("\n");
            output.append("Task Status: ").append(TaskStatus[i]).append("\n\n");
        }

        JOptionPane.showMessageDialog(null, output.toString());
    }
    
}
