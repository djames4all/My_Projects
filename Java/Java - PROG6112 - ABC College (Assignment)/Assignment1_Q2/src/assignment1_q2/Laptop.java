/*
 * ST10393280 - Daniel Luke James
 */
package assignment1_q2;

/**
 *
 * Author: Daniel Luke James
 */

// Inheritance from the laptop class and product class
public class Laptop extends Product {

    private static final double VAT_RATE = 0.15;

    public Laptop(String make, String model, String description, double priceExcludingVAT, int quantity) {
        super(make, model, description, priceExcludingVAT, quantity);
    }

    public double getPriceIncludingVAT() {
        double priceIncludingVAT = getPriceExcludingVAT() * (1 + VAT_RATE);

        // Calculation for VAT included
        return Math.round(priceIncludingVAT * 100.0) / 100.0;
    }
}
