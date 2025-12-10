/*
 * ST10393280 - Daniel Luke James
 */
package assignment1_q1;

import javax.swing.JOptionPane;

/**
 *
 * Author: Daniel Luke James
 */
public class Assignment1_Q1 {

    public void run() {
        while (true) {
            int option = displayMenu();
            if (option == 1) {

                // User can continue to the main menu
                Student mainMenu = new Student();
                mainMenu.run();
            } else {

                // User can exit the application
                exitStudentApplication();
                break;
            }
        }
    }

    // Display Menu 
    private int displayMenu() {
        String menu = "Student Management Application\n"
                + "1) Enter (1) to launch main menu or any other key to exit\n";
        return Integer.parseInt(JOptionPane.showInputDialog(null, menu));
    }

    // Method to exit application
    private void exitStudentApplication() {
        JOptionPane.showMessageDialog(null, "Thank you for using Student Management Application, Goodbye!");
        System.exit(0);
    }

    public static void main(String[] args) {
        Assignment1_Q1 app = new Assignment1_Q1();
        app.run();
    }
}
