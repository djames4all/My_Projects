/*
 * ST10393280 - Daniel Luke James
 */
package takehometest_q1;

import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.awt.*;
import java.text.DecimalFormat;
import java.text.DecimalFormatSymbols;

/**
 *
 * @author ST10393280 - Daniel Luke James
 */
public class TakeHomeTest_Q1 extends JFrame {

    private static final String CURRENCY_SYMBOL = "R";

    // Single and 2D Arrays to store the manufacturer names and costs
    private String[] manufacturers;
    private double[][] cameraCosts;

    private JTable table;

    // Constructors 
    public TakeHomeTest_Q1() {
        initializeUI();
        gatherCameraData();
        createTable();
        setVisible(true);
    }

    // Initializing the the user interface
    private void initializeUI() {
        int manufacturerCount = getValidInput("Please enter the number of camera manufacturers: ");
        manufacturers = new String[manufacturerCount];
        cameraCosts = new double[manufacturerCount][2];

        setTitle("Camera Technology Report");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setSize(600, 400);
    }

    // Gathering the camera information from the user
    private void gatherCameraData() {
        for (int i = 0; i < manufacturers.length; i++) {
            JOptionPane.showMessageDialog(null, "Please enter the details for the manufacturer" + (i + 1));

            // Getting the manufacturer name, mirrorless costs and DSLR costs
            manufacturers[i] = JOptionPane.showInputDialog("Manufacturer: ");
            cameraCosts[i][0] = getValidCostInput("Cost of Mirrorless: ");
            cameraCosts[i][1] = getValidCostInput("Cost of DSLR: ");
        }
    }

    // Helper method to get the integer input from the user
    private int getValidInput(String message) {
        int value = 0;
        boolean validInput = false;

        while (!validInput) {
            try {
                value = Integer.parseInt(JOptionPane.showInputDialog(message));
                validInput = true;
            } catch (NumberFormatException e) {
                JOptionPane.showMessageDialog(null, "Invalid input. Please enter a valid number.");
            }
        }

        return value;
    }

    // Helper method to get the double input for camera costs from the user
    private double getValidCostInput(String message) {
        double value = 0;
        boolean validInput = false;

        while (!validInput) {
            try {
                value = Double.parseDouble(JOptionPane.showInputDialog(message + CURRENCY_SYMBOL));
                validInput = true;
            } catch (NumberFormatException e) {
                JOptionPane.showMessageDialog(null, "Invalid input. Please enter a number.");
            }
        }

        return value;
    }

    // Creating a table to display camera information
    private void createTable() {
        String[] columnNames = {"Manufacturer", "Mirrorless Cost", "DSLR Cost"};
        DefaultTableModel model = new DefaultTableModel(columnNames, 0);

        // Implementing a for loop
        for (int i = 0; i < manufacturers.length; i++) {
            model.addRow(new Object[]{manufacturers[i], formatCurrency(cameraCosts[i][0]), formatCurrency(cameraCosts[i][1])});
        }

        table = new JTable(model);
        JScrollPane scrollPane = new JScrollPane(table);

        JPanel panel = createTablePanel(scrollPane);
        add(panel);
    }

    // Create a table to calculate the cost difference
    private JPanel createTablePanel(JScrollPane scrollPane) {
        JPanel panel = new JPanel();
        panel.setLayout(new BorderLayout());
        panel.add(scrollPane, BorderLayout.CENTER);

        JButton calculateButton = new JButton("Calculate");
        panel.add(calculateButton, BorderLayout.SOUTH);

        calculateButton.addActionListener(e -> calculateResults());

        return panel;
    }

    // Calculating and displaying the cost differences to print stars
    private void calculateResults() {
        double[] costDifferences = new double[manufacturers.length];
        String[] stars = new String[manufacturers.length];

        for (int i = 0; i < manufacturers.length; i++) {
            costDifferences[i] = cameraCosts[i][0] - cameraCosts[i][1];
            stars[i] = costDifferences[i] >= 2500 ? "***" : "";
        }

        DefaultTableModel model = new DefaultTableModel();
        model.addColumn("Manufacturer");
        model.addColumn("Cost Difference");
        model.addColumn("Stars");

        for (int i = 0; i < manufacturers.length; i++) {
            model.addRow(new Object[]{manufacturers[i], formatCurrency(costDifferences[i]), stars[i]});
        }

        table.setModel(model);

        int maxIndex = 0;
        double maxDifference = costDifferences[0];
        for (int i = 1; i < manufacturers.length; i++) {
            if (costDifferences[i] > maxDifference) {
                maxDifference = costDifferences[i];
                maxIndex = i;
            }
        }

        // Displaying the cost difference 
        JOptionPane.showMessageDialog(this, "Camera With The Most Cost Difference\nManufacturer: " + manufacturers[maxIndex]);
    }

    // Currency value with the correct format
    private String formatCurrency(double value) {
        DecimalFormatSymbols symbols = new DecimalFormatSymbols();
        symbols.setCurrencySymbol(CURRENCY_SYMBOL);
        DecimalFormat df = new DecimalFormat("#,##0.00", symbols);
        String formattedValue = df.format(Math.abs(value));
        return (value < 0 ? "-" : "") + CURRENCY_SYMBOL + " " + formattedValue;
    }

    // Main method
    public static void main(String[] args) {
        SwingUtilities.invokeLater(TakeHomeTest_Q1::new);
    }
}
