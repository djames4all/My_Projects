/*
 * ST10393280 - Daniel Luke James
 */
package takehometest_q2;

/**
 *
 * @author ST10393280 - Daniel Luke James
 */
// Subclass that extends the estateAgent class
class EstateAgentSales extends EstateAgent {

    // Implementing a constructor
    public EstateAgentSales(String agentName, double propertyPrice) {
        super(agentName, propertyPrice);
    }

    // Property Report Method to display the report
    public void printPropertyReport() {
        System.out.println("Estate Agent Report");
        System.out.println("********************");
        System.out.println("Estate Agent: " + getAgentName());
        System.out.println("Property Sale Price: R" + getPropertyPrice());
        System.out.println("Agent Commission: R" + getAgentCommission());
    }
}
