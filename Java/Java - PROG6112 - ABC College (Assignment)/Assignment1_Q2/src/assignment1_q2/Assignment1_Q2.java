/*
 * ST10393280 - Daniel Luke James
 */
package assignment1_q2;

import java.util.ArrayList;
import javax.swing.JOptionPane;
import java.util.Random;

/**
 *
 * Author: Daniel Luke James
 */
public class Assignment1_Q2 {

    public static void main(String[] args) {
        Inventory inventory = new Inventory();
        ArrayList<Customer> customers = new ArrayList<>();

        // Displaying the menu
        while (true) {
            String choiceString = JOptionPane.showInputDialog(
                    "Online Laptop Store\n"
                    + "1. Add a laptop\n"
                    + "2. Display all laptops\n"
                    + "3. Buy laptops\n"
                    + "4. Generate sales report\n"
                    + "5. Exit\n"
                    + "Enter your choice:"
            );

            int choice;
            try {
                choice = Integer.parseInt(choiceString);
            } catch (NumberFormatException e) {
                choice = 0;
            }

            // switch/case prompting the user to enter the laptop details
            switch (choice) {
                case 1:
                    String make = JOptionPane.showInputDialog(" Please enter laptop make:");
                    String model = JOptionPane.showInputDialog(" Please enter laptop model:");
                    String description = JOptionPane.showInputDialog("Please enter laptop description:");
                    double priceExcludingVAT;
                    try {
                        priceExcludingVAT = Double.parseDouble(JOptionPane.showInputDialog(" Please enter laptop price (in Rands):"));
                    } catch (NumberFormatException e) {
                        JOptionPane.showMessageDialog(null, "The price is invalid. Please enter a valid price.");
                        continue;
                    }
                    int quantity;
                    try {
                        quantity = Integer.parseInt(JOptionPane.showInputDialog("Please enter laptop quantity:"));
                    } catch (NumberFormatException e) {
                        JOptionPane.showMessageDialog(null, "Quantity is invalid. Please enter a valid quantity.");
                        continue;
                    }
                    if (quantity <= 0) {
                        JOptionPane.showMessageDialog(null, "The quantity must be greater than 0.");
                        continue;
                    }
                    Laptop laptop = new Laptop(make, model, description, priceExcludingVAT, quantity);
                    inventory.addProduct(laptop);
                    break;
                case 2:
                    inventory.displayProducts();
                    break;
                case 3:
                    String customerCode = generateCustomerCode();
                    Customer customer = findCustomer(customers, customerCode);
                    if (customer == null) {
                        customer = new Customer(customerCode);
                        customers.add(customer);
                    }
                    buyLaptops(inventory, customer);
                    break;
                case 4:
                    displayCustomerCodes(customers);
                    String customerCodeForReport = JOptionPane.showInputDialog("Plese enter customer account number:");
                    generateSalesReport(customers, customerCodeForReport);
                    break;

                case 5:
                    JOptionPane.showMessageDialog(null, "Exit online store.");
                    System.exit(0);
                default:
                    JOptionPane.showMessageDialog(null, "Option is invalid. Please select a valid option.");
            }
        }
    }

    // Method to allow users to buy a laptop
    static void buyLaptops(Inventory inventory, Customer customer) {
        while (true) {
            // Display all laptops with their makes, models, and codes
            StringBuilder laptopList = new StringBuilder("Available Laptops:\n");
            for (Product product : inventory.getProducts()) {
                if (product instanceof Laptop) {
                    Laptop laptop = (Laptop) product;
                    laptopList.append("Laptop Code: ").append(laptop.getCode()).append("\n");
                    laptopList.append("Make: ").append(laptop.getMake()).append("\n");
                    laptopList.append("Model: ").append(laptop.getModel()).append("\n");
                    laptopList.append("Quantity Available: ").append(laptop.getQuantity()).append("\n"); // Display quantity available
                    laptopList.append("---------------\n");
                }
            }
            JOptionPane.showMessageDialog(null, laptopList.toString());

            String laptopCode = JOptionPane.showInputDialog("Enter laptop code to purchase (or 'exit' to finish shopping):");
            if (laptopCode.equalsIgnoreCase("exit")) {
                break;
            }

            // Error handling for quantity and stock availability
            Laptop laptop = inventory.findLaptopByCode(laptopCode);
            if (laptop != null) {
                int quantityToBuy;
                try {
                    quantityToBuy = Integer.parseInt(JOptionPane.showInputDialog("Enter the quantity to buy:"));
                } catch (NumberFormatException e) {
                    JOptionPane.showMessageDialog(null, "Invalid quantity. Please enter a valid number.");
                    continue;
                }

                if (quantityToBuy <= 0) {
                    JOptionPane.showMessageDialog(null, "The quantity to buy must be greater than 0.");
                    continue;
                }

                if (quantityToBuy <= laptop.getQuantity()) {
                    if (laptop.buy(quantityToBuy)) {
                        customer.addPurchase(laptop, quantityToBuy);
                        JOptionPane.showMessageDialog(null, "Your Purchase is successful.");
                    } else {
                        JOptionPane.showMessageDialog(null, "Not enough stock available for this laptop.");
                    }
                } else {
                    JOptionPane.showMessageDialog(null, "Not enough stock available for this quantity.");
                }
            } else {
                JOptionPane.showMessageDialog(null, "Laptop with code '" + laptopCode + "' not found.");
            }
        }
    }

    // Method to generate customer codes
    private static String generateCustomerCode() {
        Random rand = new Random();
        int customerCode = rand.nextInt(9000) + 1000;
        return String.valueOf(customerCode);
    }

    // Method to find a specific customer
    static Customer findCustomer(ArrayList<Customer> customers, String code) {
        for (Customer customer : customers) {
            if (customer.getCode().equals(code)) {
                return customer;
            }
        }
        return null;
    }

    // Method to display the customer codes
    private static void displayCustomerCodes(ArrayList<Customer> customers) {
        StringBuilder codes = new StringBuilder("The available customer codes:\n");
        for (Customer customer : customers) {
            codes.append(customer.getCode()).append("\n");
        }
        JOptionPane.showMessageDialog(null, codes.toString());
    }

    // Finds the customer with the customer code
    private static void generateSalesReport(ArrayList<Customer> customers, String customerCode) {
        Customer customer = findCustomer(customers, customerCode);

        if (customer == null) {
            JOptionPane.showMessageDialog(null, "The customer was not found.");
            return;
        }

        StringBuilder report = new StringBuilder("Sales Report for Customer " + customer.getCode() + ":\n");
        report.append("Purchases:\n");

        for (Purchase purchase : customer.getPurchases()) {
            Laptop laptop = purchase.getLaptop();
            report.append("Laptop: ").append(laptop.getMake()).append(" ").append(laptop.getModel()).append("\n");
            report.append("Description: ").append(laptop.getDescription()).append("\n");
            report.append("Price per Unit (including VAT): R").append(laptop.getPriceIncludingVAT()).append("\n");
            report.append("Quantity Bought: ").append(purchase.getQuantityBought()).append("\n");
            report.append("Total Cost (including VAT): R").append(purchase.getTotalCostIncludingVAT()).append("\n");
            report.append("\n");
        }

        report.append("Total Amount Spent (including VAT): R").append(customer.getTotalAmountSpent()).append("\n");
        report.append("--------------------------------------------\n");

        JOptionPane.showMessageDialog(null, report.toString());
    }
}
