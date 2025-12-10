/*
 * ST10393280 - Daniel Luke James
 */
package takehometest_q2;

import java.util.Scanner;

/**
 *
 * @author ST10393280 - Daniel Luke James
 */
// est class to run the application
public class RunApplication {

    // Main method that implements the scanner 
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);

        // User input for estate agent name 
        System.out.print(" Please enter estate agent name: ");
        String agentName = scanner.nextLine();

        double propertyPrice = 0.0;
        boolean validInput = false;

        // While loop for valid input
        while (!validInput) {

            // User input for property sales price 
            System.out.print("Please enter the property sale psrice (R): ");
            String input = scanner.nextLine();
            try {
                propertyPrice = Double.parseDouble(input);
                validInput = true;
            } catch (NumberFormatException e) {

                // Validation error of invalid input
                System.out.println("Invalid input. Please enter a valid numeric price.");
            }
        }

        // Closing of scanner 
        scanner.close();

        // Implementing a constructor
        EstateAgentSales agent = new EstateAgentSales(agentName, propertyPrice);

        // Displaying the entire report
        System.out.println();
        agent.printPropertyReport();
    }
}
