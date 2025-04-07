/*
 *ST10393280
 */
package poe_part2;

import javax.swing.JOptionPane;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/**
 *
 * @author Daniel Luke James ST10393280
 */
public class POE_Part2Test {
    
    public POE_Part2Test() {
    }
    
    @Test
    public void createTaskID() {
    Task[] tasks = new Task[1];
           String[] statuses = {"To Do"};
           String[] developers = {"Robyn Harrison"};
           String[] taskNumbers = {"1", "2", "3"};
           String[] taskNames = {"Login Feature"};
           String[] taskDescriptions = {"Create Login to authenticate users"};
           int[] durations = {8};
           for(int i=0; i<3; i++) {
               tasks[i] = new Task(statuses[i], developers[i], taskNumbers[i], taskNames[i], taskDescriptions[i], durations[i]);
           }
           for(Task task: tasks) {
               printTaskDetails(task);
           }
       }

    private void printTaskDetails(Task task) {
        throw new UnsupportedOperationException("Not supported yet."); // Generated from nbfs://nbhost/SystemFileSystem/Templates/Classes/Code/GeneratedMethodBody
    }
    }
    
    @Test
    public void testTotalHours() {
            List<Task> tasks = new ArrayList<>();
            tasks.add(new Task("Task 1", 5));
            tasks.add(new Task("Task 2", 3));
            tasks.add(new Task("Task 3", 8));
            assertEquals(16, calculateTotalHours(tasks));
    }
    public int calculateTotalHours(List<Task> tasks) {
            int totalHours = 0;
            for (Task task : tasks) {
                    totalHours += task.getDuration();
            }
            return totalHours;
    }

    @Test
    public void testTaskDescrip() {
        String taskDescription = "This is a task description that is too long and should fail the test due to having more than fifty characters.";
        Assert.assertTrue("Task Description is too long.", taskDescription.length() <= 50);
    }
    
}
