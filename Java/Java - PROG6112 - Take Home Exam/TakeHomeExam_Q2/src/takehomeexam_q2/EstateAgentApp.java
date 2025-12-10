/*
 * ST10393280
 */
package takehomeexam_q2;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.FileWriter;
import java.io.IOException;

/**
 *
 * @author Daniel Luke James ST10393280
 */
public class EstateAgentApp extends JFrame implements ActionListener {

    // Declaring variables 
    private JComboBox<String> locationComboBox;
    private JTextField agentNameField;
    private JTextField propertyPriceField;
    private JTextField commissionPercentageField;
    private JTextArea reportTextArea;

    private EstateAgent estateAgent;

    public EstateAgentApp() {
        estateAgent = new EstateAgent();

        // Implementation of the java form
        setTitle("Estate Agent Commission Calculator");
        setSize(400, 400);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

        // Implementation of input fields
        locationComboBox = new JComboBox<>(new String[]{"", "Cape Town", "Durban", "Pretoria"});
        agentNameField = new JTextField();
        propertyPriceField = new JTextField();
        commissionPercentageField = new JTextField();
        reportTextArea = new JTextArea();

        // Menu bar 
        JMenuBar menuBar = new JMenuBar();
        setJMenuBar(menuBar);

        JMenu fileMenu = new JMenu("File");
        JMenu toolsMenu = new JMenu("Tools");

        // Implementation of menu bar options
        JMenuItem exitMenuItem = new JMenuItem("Exit");
        JMenuItem processReportMenuItem = new JMenuItem("Process Report");
        JMenuItem saveReportMenuItem = new JMenuItem("Save Report");
        JMenuItem clearMenuItem = new JMenuItem("Clear");

        exitMenuItem.addActionListener(this);
        processReportMenuItem.addActionListener(this);
        saveReportMenuItem.addActionListener(this);
        clearMenuItem.addActionListener(this);

        fileMenu.add(exitMenuItem);
        toolsMenu.add(processReportMenuItem);
        toolsMenu.add(saveReportMenuItem);
        toolsMenu.add(clearMenuItem);

        menuBar.add(fileMenu);
        menuBar.add(toolsMenu);

        // Layout of java form panel
        JPanel formPanel = new JPanel(new GridLayout(5, 2));

        // Implementation of input fields in the form panel
        formPanel.add(new JLabel("Agent Location:"));
        formPanel.add(locationComboBox);
        formPanel.add(new JLabel("Agent Name:"));
        formPanel.add(agentNameField);
        formPanel.add(new JLabel("Property Price:"));
        formPanel.add(propertyPriceField);
        formPanel.add(new JLabel("Commission Percentage:"));
        formPanel.add(commissionPercentageField);

        JPanel reportPanel = new JPanel();
        reportPanel.setLayout(new BoxLayout(reportPanel, BoxLayout.Y_AXIS));
        reportPanel.add(new JLabel("Report:"));
        reportPanel.add(new JScrollPane(reportTextArea));

        add(formPanel, BorderLayout.NORTH);
        add(reportPanel, BorderLayout.CENTER);

        // Implementation of properties
        locationComboBox.setSelectedIndex(0);
        reportTextArea.setEditable(false);
    }

    // Switch-case implemented
    @Override
    public void actionPerformed(ActionEvent e) {
        String actionCommand = e.getActionCommand();

        switch (actionCommand) {
            case "Exit":
                System.exit(0);
                break;
            case "Process Report":
                processReport();
                break;
            case "Save Report":
                saveReport();
                break;
            case "Clear":
                clearForm();
                break;
        }
    }

    // Report and validation implementation 
    private void processReport() {
        String agentLocation = (String) locationComboBox.getSelectedItem();
        String agentName = agentNameField.getText();
        String propertyPriceText = propertyPriceField.getText();
        String commissionPercentageText = commissionPercentageField.getText();

        double propertyPrice;
        double commissionPercentage;

        try {
            propertyPrice = Double.parseDouble(propertyPriceText);
        } catch (NumberFormatException e) {
            propertyPrice = 0;
        }

        try {
            commissionPercentage = Double.parseDouble(commissionPercentageText);
        } catch (NumberFormatException e) {
            commissionPercentage = 0;
        }

        try {

            estateAgent.ValidateData(agentLocation, agentName, propertyPrice, commissionPercentage);
        } catch (IllegalArgumentException ex) {

            JOptionPane.showMessageDialog(this, ex.getMessage(), "Error", JOptionPane.ERROR_MESSAGE);
            return;
        }

        double commission = estateAgent.CalculateCommission(propertyPrice, commissionPercentage);
        String report = String.format("Agent Location: %s\nAgent Name: %s\nProperty Price: R%.2f\nCommission Percentage: %.2f%%\nCommission Earned: R%.2f",
                agentLocation, agentName, propertyPrice, commissionPercentage, commission);
        reportTextArea.setText("\n" + report);
    }

    // Allows the user to save the report
    private void saveReport() {
        String report = reportTextArea.getText();
        String formattedReport = "ESTATE AGENT REPORT \n\n*******************************\n" + report + "\n\n*******************************\n";

        try (FileWriter writer = new FileWriter("report.txt")) {
            writer.write(formattedReport);
            JOptionPane.showMessageDialog(this, "Report saved to report.txt", "Success", JOptionPane.INFORMATION_MESSAGE);
        } catch (IOException e) {
            JOptionPane.showMessageDialog(this, "Error saving report to file.", "Error", JOptionPane.ERROR_MESSAGE);
        }
    }

    // Allows the user to clear the form 
    private void clearForm() {
        agentNameField.setText("");
        propertyPriceField.setText("");
        commissionPercentageField.setText("");
        reportTextArea.setText("");
    }

    // Main method
    public static void main(String[] args) {
        SwingUtilities.invokeLater(() -> {
            EstateAgentApp app = new EstateAgentApp();
            app.setVisible(true);
        });
    }
}
