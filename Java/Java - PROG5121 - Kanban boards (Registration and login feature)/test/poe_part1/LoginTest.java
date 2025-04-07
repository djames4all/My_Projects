/*
 * ST10393280 - Daniel Luke James
 */
package poe_part1;

import java.util.Scanner;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import org.junit.Test;
import static org.junit.Assert.*;

/**
 * @author ST10393280 - Daniel Luke James
 */
public class LoginTest {

    public LoginTest() {

    }

    @Test
    public void checkUserName() {

        if (username.length() <= 5 && username.contains("_")
       {
            assertTrue("Username successfully captured", checkUserName.method());
        }
        assertFalse(("Username is not correctly formatted,"
                + "please ensure that your username contains an "
                + "underscore and is no more than 5 characters in length."), checkUserName.method());
    }

    private static class username {

        private static boolean contains(String a_) {
            throw new UnsupportedOperationException("Not supported yet."); // Generated from nbfs://nbhost/SystemFileSystem/Templates/Classes/Code/GeneratedMethodBody
        }

        private static int length() {
            throw new UnsupportedOperationException("Not supported yet."); // Generated from nbfs://nbhost/SystemFileSystem/Templates/Classes/Code/GeneratedMethodBody
        }

        public username() {
        }
    }

    private static class checkUserName {

        private static boolean method() {
            throw new UnsupportedOperationException("Not supported yet."); // Generated from nbfs://nbhost/SystemFileSystem/Templates/Classes/Code/GeneratedMethodBody
        }

        public checkUserName() {
        }
    }
}

public void checkPasswordComplexit() {

      if (password.length() > 8)
      Pattern pattern = Pattern.compile("^(?=.*[A-Z])(?=.*[0-9])(?=.*[@#$%^&+=])");
      Matcher matcher = pattern.matcher(password);

      assertTrue("Password successfully captured", checkPasswordComplexit.method());
      assertFalse(("Password is not correctly formatted,"
                    + "please ensure that the password contains at"
                    + "least 8 characters, a capital letter,"
                    + "a number and a special character."), checkPasswordComplexit.method());
    }
  }

    public void registerUser() {

        if (checkPasswordComplexity(password) && checkUserName(username)) {
            arr[0] = username;
            arr[1] = password;
            arr[2] = firstname;
            arr[3] = lastname;

            System.out.println("\nUser Stored.\n");

            loginUser(arr[0], arr[1]);
    }
  }
    
    public void username() {
        POE_Part1Test.InputOutput inputOutput = new POE_Part1Test.InputOutput();

        assertEquals("kyl_1”", inputOutput.getInput());
    }

    
    public void returnLoginStatus() {
        Boolean a,
        String r = "";
        if (a) {
            r = "Welcome " + arr[2] + arr[3]
                    + " it is great to see you again.";
        }

        if (!a) {
            r = "Username or password incorrect, please try again.";
        }
        return r;
    }
}
}
