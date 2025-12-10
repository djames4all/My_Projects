/*
 * ST10393280 - Daniel Luke James
 */
package poe_part1;

//import java.util.ArrayList;
import java.util.Scanner;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 * @author ST10393280 - Daniel Luke James
 */
public class Login {

    public static String[] arr = new String[4];

    // Created a checkUserName method
    boolean checkUserName(String username) {

        // Username conditions
        if (username.length() <= 5 && username.contains("_")) {

            System.out.println("Username successfully captured");

            return true;

        } else {
            System.out.println("Username is not correctly formatted,"
                    + "please ensure that your username contains an "
                    + "underscore and is no more than 5 characters in length.");

            return false;
        }

    }

    // Created a checkPasswordComplexity method
    boolean checkPasswordComplexity(String password) {

        // Check if password is at least 8 characters long
        if (password.length() > 8) {
            return false;
        }

        // Password conditions
        // Check if password contains a capital letter, a number, and a special character
        Pattern pattern = Pattern.compile("^(?=.*[A-Z])(?=.*[0-9])(?=.*[@#$%^&+=])");
        Matcher matcher = pattern.matcher(password);
        //return matcher.find();

        if (matcher.find()) {
            System.out.println("Password successfully captured");
            return true;

        } else {
            System.out.println("Password is not correctly formatted,"
                    + "please ensure that the password contains at"
                    + "least 8 characters, a capital letter,"
                    + "a number and a special character.");

            return false;
        }

    }

    // Created a registerUser method
    void registerUser(String username, String password, String firstname, String lastname) {

        if (checkPasswordComplexity(password) && checkUserName(username)) {
            arr[0] = username;
            arr[1] = password;
            arr[2] = firstname;
            arr[3] = lastname;

            System.out.println("\nUser Stored.\n");

            loginUser(arr[0], arr[1]);

        }
    }

    // Created a loginUser method to verify the details
    boolean loginUser(String username, String password) {
        // Created a scanner
        Scanner scan = new Scanner(System.in);
        //Username variable
        System.out.println("Enter Username: ");
        String a = scan.next();

        //Password variable
        System.out.println("Enter Password: ");
        String b = scan.next();

        // Login conditions
        if (username == a && password == b) {
            returnLoginStatus(true);
        } else {
            returnLoginStatus(false);

        }
        return true;
    }

    // returnLoginStatus method to display login/fail message
    String returnLoginStatus(boolean a) {

        String r = "";
        // Login conditions for display messages
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
