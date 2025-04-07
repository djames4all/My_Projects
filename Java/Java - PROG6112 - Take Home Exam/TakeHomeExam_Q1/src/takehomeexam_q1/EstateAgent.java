/*
 * ST10393280 - Daniel Luke James
 */
package takehomeexam_q1;

/**
 *
 * @author ST10393280 - Daniel Luke James
 */
class EstateAgent implements IEstateAgent {

    // Calculates total sales for sales agents
    @Override
    public double estateAgentSales(double[] propertySales) {
        double totalSales = 0;
        for (double sale : propertySales) {
            totalSales += sale;
        }
        return totalSales;
    }

    // Calculates the 2% commsion for sales agents
    @Override
    public double estateAgentCommission(double propertySales) {
        return propertySales * 0.02;
    }

    // Displays the top selling estate agent
    @Override
    public int topEstateAgent(double[] totalSales) {
        int topAgentIndex = 0;
        double maxSales = totalSales[0];

        for (int i = 1; i < totalSales.length; i++) {
            if (totalSales[i] > maxSales) {
                maxSales = totalSales[i];
                topAgentIndex = i;
            }
        }

        return topAgentIndex;
    }
}
