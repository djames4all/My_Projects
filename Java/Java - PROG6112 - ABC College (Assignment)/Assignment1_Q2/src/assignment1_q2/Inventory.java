/*
 * ST10393280 - Daniel Luke James
 */
package assignment1_q2;

import java.util.ArrayList;
import javax.swing.JOptionPane;

/**
 *
 * Author: Daniel Luke James
 */
public class Inventory {

    // Array list created for products and used item numbers
    private ArrayList<Product> products = new ArrayList<>();
    private ArrayList<Integer> usedItemNumbers = new ArrayList<>();

// Method to allow the user to add a product
    public void addProduct(Product product) {
        if (usedItemNumbers.contains(product.getCode())) {
            JOptionPane.showMessageDialog(null, "The item number must be unique.");
        } else {
            usedItemNumbers.add(product.getCode());
            products.add(product);
        }
    }

    // Method to display all the products
    public void displayProducts() {
        StringBuilder output = new StringBuilder("Inventory:\n");
        for (Product product : products) {
            output.append("Code: ").append(product.getCode()).append("\n");
            output.append("Make: ").append(product.getMake()).append("\n");
            output.append("Model: ").append(product.getModel()).append("\n");
            output.append("Description: ").append(product.getDescription()).append("\n");
            output.append("Price (excluding VAT): R").append(product.getPriceExcludingVAT()).append("\n");
            output.append("Price (including VAT): R").append(((Laptop) product).getPriceIncludingVAT()).append("\n");
            output.append("Quantity: ").append(product.getQuantity()).append("\n");
            output.append("---------------\n");
        }
        JOptionPane.showMessageDialog(null, output.toString());
    }

    // // Method to find a spscific laptop code
    public Laptop findLaptopByCode(String code) {
        for (Product product : products) {
            if (product instanceof Laptop && String.valueOf(product.getCode()).equals(code)) {
                return (Laptop) product;
            }
        }
        return null;
    }

    // Adding this method to get the list of all the products
    public ArrayList<Product> getProducts() {
        return products;
    }
}
