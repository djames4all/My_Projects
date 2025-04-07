/*
 * ST10393280 - Daniel Luke James
 */
package assignment1_q2;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
import java.util.ArrayList;

/**
 *
 * Author: Daniel Luke James
 */
public class Assignment1_Q2Test {

    private Inventory inventory;
    private ArrayList<Customer> customers;

    @BeforeEach
    public void setUp() {
        inventory = new Inventory();
        customers = new ArrayList<>();
    }

    @Test
    public void testAddLaptopToInventory() {
        Laptop laptop = new Laptop("Lenovo", "T540p", "Intel i7", 3500.0, 8);

        inventory.addProduct(laptop);

        assertEquals(1, inventory.getProducts().size());
    }

    @Test
    public void testBuyLaptops() {
        Laptop laptop = new Laptop("Lenovo", "T540p", "Intel i7", 3500.0, 8);
        inventory.addProduct(laptop);

        Customer customer = new Customer("1000");
        customers.add(customer);

        Assignment1_Q2.buyLaptops(inventory, customer);

        assertEquals(1, customer.getPurchases().size());
    }

    @Test
    public void testGenerateCustomerCode() {
        String customerCode = Assignment1_Q2.generateCustomerCode();

        assertNotNull(customerCode);
        assertTrue(customerCode.matches("\\d{4}"));
    }

    @Test
    public void testFindCustomer() {
        Customer customer = new Customer("1000");
        customers.add(customer);

        Customer foundCustomer = Assignment1_Q2.findCustomer(customers, "1000");

        assertNotNull(foundCustomer);
        assertEquals("1000", foundCustomer.getCode());
    }

    @Test
    public void testFindCustomer_NotFound() {
        Customer customer = new Customer("1010");
        customers.add(customer);

        Customer foundCustomer = Assignment1_Q2.findCustomer(customers, "1011"); // Change the searched code to "1011" to test not found case

        assertNull(foundCustomer);
    }
}
