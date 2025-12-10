/*
 * ST10393280 - Daniel Luke James
 */
package assignment1_q2;

/**
 *
 * Author: Daniel Luke James
 */
public class Product {

    private static int nextCode = 1;

    // Decalring all the variables 
    private int code;
    private String make;
    private String model;
    private String description;
    private double price_ExcludingVAT;
    private int quantity;

    // Constructor
    public Product(String make, String model, String description, double priceExcludingVAT, int quantity) {
        this.code = nextCode++;
        this.make = make;
        this.model = model;
        this.description = description;
        this.price_ExcludingVAT = priceExcludingVAT;
        this.quantity = quantity;
    }

    // Getters for all the methods
    public int getCode() {
        return code;
    }

    public String getMake() {
        return make;
    }

    public String getModel() {
        return model;
    }

    public String getDescription() {
        return description;
    }

    public double getPriceExcludingVAT() {
        return price_ExcludingVAT;
    }

    public int getQuantity() {
        return quantity;
    }

    public void setQuantity(int quantity) {
        this.quantity = quantity;
    }

    public boolean buy(int quantityToBuy) {
        if (quantityToBuy <= quantity) {
            quantity -= quantityToBuy;
            return true;
        }
        return false;
    }
}
