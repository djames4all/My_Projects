/*
 * ST10393280 - Daniel Luke James
 */
package takehomeexam_q1;

import javax.swing.JOptionPane;

/**
 *
 * @author ST10393280 - Daniel Luke James
 */
public class EstateAgentApp {

    public static void main(String[] args) {
        String[] agentNames = new String[2];
        double[][] propertySales = new double[2][3];

        // For loop implemented to get the estate agents names
        for (int i = 0; i < 2; i++) {
            boolean validAgentName = false;
            while (!validAgentName) {
                agentNames[i] = JOptionPane.showInputDialog("Enter name for Estate Agent " + (i + 1) + ":");
                if (agentNames[i] != null && !agentNames[i].trim().isEmpty() && agentNames[i].matches("[a-zA-Z]+")) {
                    validAgentName = true;
                } else if (agentNames[i] == null) {

                    System.exit(0);
                } else {
                    JOptionPane.showMessageDialog(null, "Invalid agent name. Please enter a valid name.");
                }
            }
        }

        // For loop implemented to get the poperty sales
        for (int i = 0; i < 2; i++) {
            JOptionPane.showMessageDialog(null, "Enter property sales for " + agentNames[i] + ":");

            for (int j = 0; j < 3; j++) {
                boolean validSale = false;
                while (!validSale) {
                    String input = JOptionPane.showInputDialog("Enter sales for month " + (j + 1) + " for " + agentNames[i] + ": ZAR");
                    if (input != null && !input.trim().isEmpty()) {
                        try {
                            propertySales[i][j] = Double.parseDouble(input);
                            validSale = true;
                        } catch (NumberFormatException e) {
                            JOptionPane.showMessageDialog(null, "Invalid sale value. Please enter a valid number.");
                        }
                    } else if (input == null) {

                        System.exit(0);
                    } else {
                        JOptionPane.showMessageDialog(null, "Sale value cannot be blank. Please enter a valid number.");
                    }
                }
            }
        }

        IEstateAgent estateAgent1 = new EstateAgent();

        // Declaring array
        double[] totalSales = new double[2];
        double[] totalCommissions = new double[2];

        // For loop that calculates the total sales and commissions for the estate agents
        for (int i = 0; i < propertySales.length; i++) {
            totalSales[i] = estateAgent1.estateAgentSales(propertySales[i]);
            totalCommissions[i] = estateAgent1.estateAgentCommission(totalSales[i]);
        }

        int topAgentIndex = estateAgent1.topEstateAgent(totalSales);

        // Displays the sales report for the agent
        System.out.println("ESTATE AGENT SALES REPORT");
        System.out.println();
        System.out.printf("%-20s %-20s %-20s %-20s\n", "", "January", "February", "March");
        System.out.println("-------------------------------------------------------------------------");

        for (int i = 0; i < propertySales.length; i++) {
            System.out.printf("%-20s %-20s %-20s %-20s\n", agentNames[i], "R" + propertySales[i][0], "R" + propertySales[i][1], "R" + propertySales[i][2]);
        }
        System.out.println();
        System.out.printf("%-20s %-15s\n", "Total Property Sales For " + agentNames[0], "= R" + (propertySales[0][0] + propertySales[0][1] + propertySales[0][2]), "R" + totalSales[0], "R" + totalCommissions[0]);
        System.out.printf("%-20s %-15s\n", "Total Property Sales For " + agentNames[1], "= R" + (propertySales[1][0] + propertySales[1][1] + propertySales[1][2]), "R" + totalSales[1], "R" + totalCommissions[1]);
        System.out.println();
        System.out.printf("%-20s %-15s\n", "Sales Commission For " + agentNames[0], "= R" + estateAgent1.estateAgentCommission(totalSales[0]), 0.0, "R" + estateAgent1.estateAgentCommission(totalSales[0]));
        System.out.printf("%-20s %-15s\n", "Sales Commission For " + agentNames[1], "= R" + estateAgent1.estateAgentCommission(totalSales[1]), 0.0, "R" + estateAgent1.estateAgentCommission(totalSales[1]));
        System.out.println();
        System.out.println("Top Performing Estate Agent: " + agentNames[topAgentIndex]);
    }
}
