/*
 * ST10393280 - Daniel Luke James
 */
package takehometest_q2;

/**
 *
 * @author ST10393280 - Daniel Luke James
 */
// Abstract class 
abstract class EstateAgent implements iEstateAgent {

    //Variables for agent name and property price
    private String agentName;
    private double propertyPrice;

    //Implementing a constructor
    public EstateAgent(String agentName, double propertyPrice) {
        this.agentName = agentName;
        this.propertyPrice = propertyPrice;
    }

    //Method to override the getters for all the variables 
    @Override
    public String getAgentName() {
        return agentName;
    }

    @Override
    public double getPropertyPrice() {
        return propertyPrice;
    }

    @Override
    public double getAgentCommission() {
        return 0.2 * propertyPrice; // 20% commission
    }
}
