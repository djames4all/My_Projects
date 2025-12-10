/*
 * ST10393280 - Daniel Luke James
 */
package assignment1_q2;

import java.util.ArrayList;

/**
 *
 * Author: Daniel Luke James
 */
public class Customer {

    private String code;
    // Creating an array list to store all the purchases
    private ArrayList<Purchase> purchases = new ArrayList<>();

    public Customer(String code) {
        this.code = code;
    }

    public String getCode() {
        return code;
    }

    public void addPurchase(Laptop laptop, int quantityBought) {
        purchases.add(new Purchase(laptop, quantityBought));
    }

    // Method to get the purchases
    public ArrayList<Purchase> getPurchases() {
        return purchases;
    }

    // Method to get the total amount spent on a product
    public double getTotalAmountSpent() {
        double totalAmount = 0;
        for (Purchase purchase : purchases) {
            totalAmount += purchase.getTotalCostIncludingVAT();
        }
        return totalAmount;
    }
}
