/*
 * ST10393280 - Daniel Luke James
 */
package assignment1_q2;

/**
 *
 * Author: Daniel Luke James
 */
public class Purchase {

    // Declaring the variables
    private Laptop laptop;
    private int quantityBought;

    public Purchase(Laptop laptop, int quantityBought) {
        this.laptop = laptop;
        this.quantityBought = quantityBought;
    }

    // Getters for all the methods
    public Laptop getLaptop() {
        return laptop;
    }

    public int getQuantityBought() {
        return quantityBought;
    }

    public double getTotalCostIncludingVAT() {
        return laptop.getPriceIncludingVAT() * quantityBought;
    }
}
