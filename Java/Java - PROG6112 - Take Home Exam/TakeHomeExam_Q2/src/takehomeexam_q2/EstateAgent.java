/*
 *ST10393280
 */
package takehomeexam_q2;

/**
 *
 * @author Daniel Luke James ST10393280
 */
public class EstateAgent implements IEstateAgent {

    // Method that calculates the commission
    @Override
    public double CalculateCommission(double propertyPrice, double commissionPercentage) {
        return propertyPrice * (commissionPercentage / 100.0);
    }

    // Validates the input data
    @Override
    public boolean ValidateData(String agentLocation, String agentName, double propertyPrice, double commissionPercentage) {
        if (agentLocation.isEmpty()) {
            throw new IllegalArgumentException("Agent Location cannot be empty.");
        }

        if (agentName.isEmpty()) {
            throw new IllegalArgumentException("Agent Name cannot be empty.");
        }

        if (propertyPrice <= 0) {
            throw new IllegalArgumentException("Property Price must be a valid number greater than zero.");
        }

        if (commissionPercentage <= 0) {
            throw new IllegalArgumentException("Commission percentage must be a valid number greater than zero.");
        }

        return true;
    }
}
