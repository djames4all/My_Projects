/*
 * ST10393280 - Daniel Luke James
 */
package takehomeexam_q1;

import static org.junit.Assert.assertEquals;
import org.junit.Test;

/**
 *
 * @author ST10393280 - Daniel Luke James
 */
public class EstateAgentTest {

    // Determines the correct total of sales value returned
    @Test
    public void CalculateTotalSales_ReturnsTotalSales() {
        EstateAgent estateAgent = new EstateAgent();
        double[] propertySales = {20000.0, 30000.0, 40000.0};

        double result = estateAgent.estateAgentSales(propertySales);

        assertEquals(90000.0, result, 0.01);
    }

    // Determines the correct agents commision 
    @Test
    public void CalculateTotalCommission_ReturnsCommission() {
        EstateAgent estateAgent = new EstateAgent();
        double propertySales = 40000.0;

        double result = estateAgent.estateAgentCommission(propertySales);

        assertEquals(800.0, result, 0.01);
    }

    // Determines the top selling property sales of the agent
    @Test
    public void TopAgent_ReturnsTopPosition() {
        EstateAgent estateAgent = new EstateAgent();
        double[] totalSales = {20000.0, 40000.0, 30000.0};

        int result = estateAgent.topEstateAgent(totalSales);

        assertEquals(1, result);
    }
}
