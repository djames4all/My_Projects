/*
 * ST10393280 - Daniel Luke James
 */
package poe_part1;

import java.util.Scanner;
import org.junit.Test;
import static org.junit.Assert.*;

/**
 * @author ST10393280 - Daniel Luke James
 */
public class POE_Part1Test {

    String returnPOE_Part1Test;

    public POE_Part1Test() {
    }

    @Test
    public void username() {
        InputOutput inputOutput = new InputOutput();

        assertEquals("kyl_1”", inputOutput.getInput());
    }

    public void password() {
        InputOutput inputOutput = new InputOutput();

        assertEquals("Ch&&sec@ke99!", inputOutput.getInput());
    }

    public void firstname() {
        InputOutput inputOutput = new InputOutput();

        assertEquals("Kyle", inputOutput.getInput());
    }

    public void lastname() {
        InputOutput inputOutput = new InputOutput();

        assertEquals("Chsecke", inputOutput.getInput());

    }

    private static class InputOutput {

        public InputOutput() {
        }

        private Object getInput() {
            throw new UnsupportedOperationException("Not supported yet."); // Generated from nbfs://nbhost/SystemFileSystem/Templates/Classes/Code/GeneratedMethodBody
        }
    }
}
