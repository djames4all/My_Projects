/*
 * ST10393280 - Daniel Luke James
 */
package takehomeexam_q1;

/**
 *
 * @author ST10393280 - Daniel Luke James
 */
// Implementation of interface class
interface IEstateAgent {

    double estateAgentSales(double[] propertySales);

    double estateAgentCommission(double propertySales);

    int topEstateAgent(double[] totalSales);
}
