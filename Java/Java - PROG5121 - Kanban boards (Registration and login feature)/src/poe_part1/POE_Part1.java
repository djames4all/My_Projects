/*
 * ST10393280 - Daniel Luke James
 */
package poe_part1;

import java.util.Scanner;

/**
 * @author ST10393280 - Daniel Luke James
 */
public class POE_Part1 {

    public static void main(String[] args) {

        Login user = new Login();

        // Created a scanner
        Scanner scan = new Scanner(System.in);

        //Username variable
        System.out.println("Enter Username: ");
        String username = scan.next();
        user.checkUserName(username);

        //Password variable
        System.out.println("Enter Password: ");
        String password = scan.next();
        user.checkPasswordComplexity(password);

        //First Name variable
        System.out.println("Enter First Name: ");
        String firstname = scan.next();

        //Last Names variable
        System.out.println("Enter Last Name: ");
        String lastname = scan.next();

        user.registerUser(username, password, firstname, lastname);
        // user.loginUser(username, password);
    }

}
