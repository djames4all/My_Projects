/*
 *ST10393280
 */
package poe_part2;
import java.util.Scanner;
import javax.swing.JOptionPane;

/**
 *
 * @author Daniel Luke James ST10393280
 */
public class POE_Part2 {
    private static Scanner scanner;

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        
        // Declaring Scanner
        scanner = new Scanner(System.in);
        
                //Outputs welcome message to user
		String username = login();
		System.out.println("Welcome to EasyKanban");
                
                //Prompts users to choose from a numeric menu
		int totalTasks = getNumberOfTasks();
		Task[] tasks = new Task[totalTasks];
		int option = 0;
		do {
			option = displayMenuAndGetOption();
			switch (option) {
			case 1 -> createTaskID(tasks);
			case 2 -> JOptionPane.showMessageDialog(null, "Coming Soon");
			case 3 -> {
                }
			default -> System.out.println("Invalid option. Please try again.");
			}
		} while (option != 3);
                // Task option box displays behind the netbean application.
                
                //Outputs the total number of hours across all tasks
		int totalHours = returnTotalHours(tasks);
		System.out.println("Total hours: " + totalHours);
		scanner.close();
	}
    
    //Prompts user for username and password
    private static String login() {
		String username = "";
		while (true) {
			System.out.print("Please enter a username: ");
			username = scanner.nextLine();
			System.out.print("Please enter a password: ");
			String password = scanner.nextLine();
			if (loginUser(username, password)) {
				break;
			} else {
				System.out.println("Invalid username or password. Please try again.");
			}
		}
		return username;
	}
    
    //Setting conditions for username and password
    private static boolean loginUser(String username, String password) {
//		if(username.length() <= 5 && username.contains("_") && 
//                        password.contains("!") && password.contains("@") && password.length() >= 8 && 
//                        password.matches(".*[A-Z].*") && password.matches(".*[0-9].*")) {
//                  return true;
//	} else {
//
//              return false;
//           }
//
//       }
		return true;
	}
    
        //Prompts user for number of tasks
	private static int getNumberOfTasks() {
		System.out.print("Please enter the number of tasks: ");
		return Integer.parseInt(scanner.nextLine());
                // Task option box displays behind the netbean application.
	}
	
        //Outputs the menu options
        private static int displayMenuAndGetOption() {
		System.out.println("Select an option:");
		System.out.println("1) Add tasks");
		System.out.println("2) Show report");
		System.out.println("3) Quit");
		System.out.print("Choice: ");
		return Integer.parseInt(scanner.nextLine());
                // Task option box displays behind the netbean application.
	}
	
        //Prompts the user for Task details
        private static void createTaskID(Task[] tasks) {
		for (int i = 0; i < tasks.length; i++) {
			String status = JOptionPane.showInputDialog("Please enter task status: ");
			String developer = JOptionPane.showInputDialog("Please enter developer details: ");
			int taskNumber = Integer.parseInt(JOptionPane.showInputDialog("Please enter task number: "));
			String taskName = JOptionPane.showInputDialog("Please enter task name: ");
			String taskDescription = JOptionPane.showInputDialog("Please enter task description: ");
			String taskID = JOptionPane.showInputDialog("Please enter task ID: ");
			int duration = Integer.parseInt(JOptionPane.showInputDialog("Please enter duration: "));
			tasks[i] = new Task(status, developer, taskNumber, taskName, taskDescription, taskID, duration);
			printTaskDetails(tasks[i]);
                        // Task option box displays behind the netbean application.
		}
	}
        
        //Outputs the Task details
	private static void printTaskDetails(Task task) {
		String message = "Task Status: " + task.getStatus() + "\n" +
						 "Developer Details: " + task.getDeveloper() + "\n" +
						 "Task Number: " + task.getTaskNumber() + "\n" +
						 "Task Name: " + task.getTaskName() + "\n" +
						 "Task Description: " + task.getTaskDescription() + "\n" +
						 "Task ID: " + task.getTaskID() + "\n" +
						 "Duration: " + task.getDuration() + "\n";
		JOptionPane.showMessageDialog(null, message);
                // Task option box displays behind the netbean application.
	}
	
        //Calculates the total number of hours across all task 
        private static int returnTotalHours(Task[] tasks) {
		int totalHours = 0;
        for (Task task : tasks) {
            totalHours += task.getDuration();
        }
		return totalHours;
	}
}

//Created a new class called 'Task'
class Task {
	private String status;
	private String developer;
	private int taskNumber;
	private String taskName;
	private String taskDescription;
	private String taskID;
	private int duration;
	public Task(String status, String developer, int taskNumber, 
                    String taskName, String taskDescription, String taskID, int duration) {
            
		this.status = status;
		this.developer = developer;
		this.taskNumber = taskNumber;
		this.taskName = taskName;
		this.taskDescription = taskDescription;
		this.taskID = taskID;
		this.duration = duration;
	}

    Task(String statuse, String developer, String taskNumber, String taskName, String taskDescription, int duration) {
        throw new UnsupportedOperationException("Not supported yet."); // Generated from nbfs://nbhost/SystemFileSystem/Templates/Classes/Code/GeneratedMethodBody
    }
        
        // Getters and setters for each method 
	public String getStatus() {
		return status;
	}
	public void setStatus(String status) {
		this.status = status;
	}
	public String getDeveloper() {
		return developer;
	}
	public void setDeveloper(String developer) {
		this.developer = developer;
	}
	public int getTaskNumber() {
		return taskNumber;
	}
	public void setTaskNumber(int taskNumber) {
		this.taskNumber = taskNumber;
	}
	public String getTaskName() {
		return taskName;
	}
	public void setTaskName(String taskName) {
		this.taskName = taskName;
	}
	public String getTaskDescription() {
		return taskDescription;
	}
	public void setTaskDescription(String taskDescription) {
		this.taskDescription = taskDescription;
	}
	public String getTaskID() {
		return taskID;
	}
	public void setTaskID(String taskID) {
		this.taskID = taskID;
	}
	public int getDuration() {
		return duration;
	}
	public void setDuration(int duration) {
		this.duration = duration;
	}
}
