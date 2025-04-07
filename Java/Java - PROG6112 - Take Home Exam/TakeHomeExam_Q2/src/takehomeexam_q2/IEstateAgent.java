/*
 *ST10393280
 */
package takehomeexam_q2;

/**
 *
 * @author Daniel Luke James ST10393280
 */
// Implementation of the interface class
public interface IEstateAgent {

    double CalculateCommission(double propertyPrice, double agentCommission);

    boolean ValidateData(String agentLocation, String agentName, double propertyPrice, double commissionPercentage);
}
