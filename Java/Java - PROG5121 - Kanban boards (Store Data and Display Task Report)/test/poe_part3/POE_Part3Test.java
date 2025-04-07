/*
 * ST10393280 - Daniel Luke James
 */
package poe_part3;

import javax.swing.JOptionPane;
import org.junit.Test;
import static org.junit.Assert.*;

/**
 *
 * @author ST10393280 - Daniel Luke James
 */
public class POE_Part3Test {
    
    public POE_Part3Test() {
    }
    
    @Test
    public void displayDevelopers() {
        String[] Developers = {"Mike Smith", "Edward Harrison", "Samantha Paulson", "Glenda Oberholzer"};
        
        StringBuilder output = new StringBuilder("Developers:\n\n");

        // For loop that retrieves the information for each developer and displays the output
        for (int i = 0; i < Developers.length; i++) {
            output.append(Developers[i]).append("\n");
        }

        JOptionPane.showMessageDialog(null, output.toString());
    }    
    
    @Test
    public void displayTaskMaxDuration() {
        String[] Developers = {"Mike Smith", "Edward Harrison", "Samantha Paulson", "Glenda Oberholzer"};
        int[] TaskDurations = {5, 8, 2, 11};
        
        int maxDurationIndex = 0;
        int maxDuration = TaskDurations[0];

        // For loop indicating the longest task duration 
        for (int i = 1; i < TaskDurations.length; i++) {
            if (TaskDurations[i] > maxDuration) {
                maxDuration = TaskDurations[i];
                maxDurationIndex = i;
            }
        }

        JOptionPane.showMessageDialog(null, "Task with Longest Duration:\n" +
                "Developer: " + Developers[maxDurationIndex] + "\n" +
                "Task Duration: " + TaskDurations[maxDurationIndex]);  
    }
    
    @Test
    public void searchTaskName() {
        String[] Developers = {"Mike Smith", "Edward Harrison", "Samantha Paulson", "Glenda Oberholzer"};
        String[] TaskNames = {"Create Login", "Create Add Features", "Create Reports", "Add Arrays"};
        
        String taskName = "Create Login";

        // For loop that indicates the task name
        for (int i = 0; i < TaskNames.length; i++) {
            if (TaskNames[i].equalsIgnoreCase(taskName)) {
                JOptionPane.showMessageDialog(null, "Task Name: " + TaskNames[i] + "\n" +
                        "Developer: " + Developers[i]);
                return;
            }
        }

        JOptionPane.showMessageDialog(null, "Task was not found with that specific name.");
    
    }
    
    @Test
    public void searchTasksFromDeveloper() {
        String[] Developers = {"Mike Smith", "Edward Harrison", "Samantha Paulson", "Glenda Oberholzer"};
        String[] TaskNames = {"Create Login", "Create Add Features", "Create Reports", "Add Arrays"};
        
        String developer = "Samantha Paulson";

        StringBuilder output = new StringBuilder("Tasks assigned to developer '" + developer + "':\n");
        boolean found = false;

        // For loop that indicates the developers name 
        for (int i = 0; i < Developers.length; i++) {
            if (Developers[i].equalsIgnoreCase(developer)) {
                output.append("Task Name: ").append(TaskNames[i]).append("\n");
                found = true;
            }
        }

        if (!found) {
            output.append("No tasks were found assigned to that specific developer '").append(developer).append("'.");
        }

        JOptionPane.showMessageDialog(null, output.toString());    
    }
    
    @Test
    public void deleteTask() {
        String[] TaskNames = {"Create Login", "Create Add Features", "Create Reports", "Add Arrays"};
        
        String taskName = "Create Reports";

        // For loop that removes a task from the arrays based on the task name
        for (int i = 0; i < TaskNames.length; i++) {
            if (TaskNames[i].equalsIgnoreCase(taskName)) {
                String[] newTaskNames = new String[TaskNames.length - 1];

                System.arraycopy(TaskNames, 0, newTaskNames, 0, i);
                System.arraycopy(TaskNames, i, newTaskNames, i, TaskNames.length - i - 1);

                TaskNames = newTaskNames;

                JOptionPane.showMessageDialog(null, "Entry " + TaskNames[i] + " successfully deleted");
                return;
            }
        }

        JOptionPane.showMessageDialog(null, "Task was not found with that specific name.");   
    }
    
    @Test
    public void displayReport() {
        String[] Developers = {"Mike Smith", "Edward Harrison", "Samantha Paulson", "Glenda Oberholzer"};
        String[] TaskNames = {"Create Login", "Create Add Features", "Create Reports", "Add Arrays"};
        int[] TaskDurations = {5, 8, 2, 11};
        String[] TaskStatus = {"To Do", "Doing", "Done", "To Do"};
        
        StringBuilder output = new StringBuilder("Display Report:\n\n");

        // For loop that retrieves the information from each task and displays the output
        for (int i = 0; i < Developers.length; i++) {
            output.append("Developer: ").append(Developers[i]).append("\n");
            output.append("Task Name: ").append(TaskNames[i]).append("\n");
            output.append("Task Duration: ").append(TaskDurations[i]).append("\n");
            output.append("Task Status: ").append(TaskStatus[i]).append("\n\n");
        }

        JOptionPane.showMessageDialog(null, output.toString());     
    }
    
}
