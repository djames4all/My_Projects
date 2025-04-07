/*
 *ST10393280
 */
package takehomeexam_q2;

import static org.junit.Assert.*;
import org.junit.Test;

/**
 *
 * @author Daniel Luke James ST10393280
 */
public class EstateAgentTest {

    // Determines the commision value returned successfully
    @Test
    public void CalculateCommission_CalculatedSuccessfully() {
        EstateAgent estateAgent = new EstateAgent();
        double propertyPrice = 50000.0;
        double commissionPercentage = 10.0;

        double expectedCommission = propertyPrice * (commissionPercentage / 100.0);
        double actualCommission = estateAgent.CalculateCommission(propertyPrice, commissionPercentage);

        assertEquals(expectedCommission, actualCommission, 0.001);
    }

    // Determines the commision value returned unsuccessfully
    // Test must fail on purpose
    @Test
    public void CalculateCommission_CalculatedUnsuccessfully() {
        EstateAgent estateAgent = new EstateAgent();
        double propertyPrice = -10000.0;
        double commissionPercentage = 10.0;

        try {
            estateAgent.CalculateCommission(propertyPrice, commissionPercentage);
            fail("Expected IllegalArgumentException was not thrown");
        } catch (IllegalArgumentException e) {

        }
    }

    // The validation performs well
    @Test
    public void ValidateData_ValidData() {
        EstateAgent estateAgent = new EstateAgent();
        String agentLocation = "Cape Town";
        String agentName = "Daniel James";
        double propertyPrice = 500000.0;
        double commissionPercentage = 5.5;

        boolean result = estateAgent.ValidateData(agentLocation, agentName, propertyPrice, commissionPercentage);

        assertTrue(result);

    }
}
