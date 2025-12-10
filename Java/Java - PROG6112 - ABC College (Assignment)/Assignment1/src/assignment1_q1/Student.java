/*
 * ST10393280 - Daniel Luke James
 */
package assignment1_q1;

import javax.swing.JOptionPane;
import java.util.ArrayList;

/**
 *
 * Author: Daniel Luke James
 */
public class Student {

    // Creating an array list to store the students details
    private ArrayList<StudentInfo> student_List;

    public Student() {
        student_List = new ArrayList<>();
    }

    // Menu options using their methods as required
    public void run() {
        while (true) {
            int option = displayMenu();
            switch (option) {
                case 1:
                    saveStudent();
                    break;
                case 2:
                    searchStudent();
                    break;
                case 3:
                    deleteStudent();
                    break;
                case 4:
                    studentReport();
                    break;
                case 5:
                    exitStudentApplication();
                    break;
                default:
                    JOptionPane.showMessageDialog(null, "Oops, invalid option. Please try again.");
            }
        }
    }

    // Display menu options
    private int displayMenu() {
        String menulayout = "Student Management Application\n"
                + "1) Capture a new student\n"
                + "2) Search for a student\n"
                + "3) Delete a student\n"
                + "4) Print student report\n"
                + "5) Exit application\n"
                + "Enter your choice:";
        return Integer.parseInt(JOptionPane.showInputDialog(null, menulayout));
    }

    private void saveStudent() {
        String name = JOptionPane.showInputDialog("Please enter student name:");
        int age;

        // Error handling for student age
        while (true) {
            try {
                age = Integer.parseInt(JOptionPane.showInputDialog("Please enter student age:"));
                if (age >= 16) {
                    break;
                } else {
                    JOptionPane.showMessageDialog(null, " Age is invalid. The age must be greater than or equal to 16.");
                }
            } catch (NumberFormatException e) {
                JOptionPane.showMessageDialog(null, "Input is invalid. Please enter a valid number.");
            }
        }

        String id;
        String email;
        String course;

        while (true) {
            id = JOptionPane.showInputDialog(" Please enter student ID:");

            if (id.matches("\\d+")) {

                boolean idExists = false;
                for (StudentInfo studentInfo : student_List) {
                    if (studentInfo.getId().equals(id)) {
                        idExists = true;
                        break;
                    }
                }

                if (!idExists) {
                    email = JOptionPane.showInputDialog("Please enter student email:");
                    while (true) {
                        course = JOptionPane.showInputDialog("Please enter student course, only 4 characters are allowed:");
                        if (course.length() == 4) {
                            break;
                        } else {
                            JOptionPane.showMessageDialog(null, "Course format is invalid. The course name must be 4 characters.");
                        }
                    }

                    // Creating an object that adds it to the student list
                    StudentInfo studentInfo = new StudentInfo(id, name, age, email, course);
                    student_List.add(studentInfo);
                    JOptionPane.showMessageDialog(null, "The student details have been saved successfully.");
                    break;
                } else {

                    JOptionPane.showMessageDialog(null, "The student ID already exists. Please enter a different ID.");
                }
            } else {

                JOptionPane.showMessageDialog(null, "ID format is invalid. Please enter a numeric ID.");
            }
        }
    }

// Method to search a specific student
    private void searchStudent() {
        String idToSearch = JOptionPane.showInputDialog("Please enter a student ID to search:");
        boolean found = false;
        for (StudentInfo studentInfo : student_List) {
            if (studentInfo.getId().equals(idToSearch)) {
                JOptionPane.showMessageDialog(null, "Student has been found:\n" + studentInfo.toString());
                found = true;
                break;
            }
        }
        if (!found) {
            JOptionPane.showMessageDialog(null, "Student is not found.");
        }
    }

    // Method to delete a specific student
    private void deleteStudent() {
        String idToDelete = JOptionPane.showInputDialog("Please enter a student ID to delete:");
        for (StudentInfo studentInfo : student_List) {
            if (studentInfo.getId().equals(idToDelete)) {
                int confirmation = JOptionPane.showConfirmDialog(null, "Are you sure you want to delete this student?");
                if (confirmation == JOptionPane.YES_OPTION) {
                    student_List.remove(studentInfo);
                    JOptionPane.showMessageDialog(null, "The student has been deleted successfully.");
                } else {
                    JOptionPane.showMessageDialog(null, "The deletion of the student is canceled.");
                }
                return;
            }
        }
        JOptionPane.showMessageDialog(null, "The student was not found.");
    }

    // Method to display the student report
    private void studentReport() {
        StringBuilder report = new StringBuilder("Student Report:\n");
        for (StudentInfo studentInfo : student_List) {
            report.append("\n").append(studentInfo.toString()).append("\n");
        }
        JOptionPane.showMessageDialog(null, report.toString());
    }

    // Method to exit the application
    private void exitStudentApplication() {
        JOptionPane.showMessageDialog(null, "Thank you for using Student Management Application, Goodbye!");
        System.exit(0);
    }

    public static void main(String[] args) {
        Student app = new Student();
        app.run();
    }
}

// Declaring the variables 
class StudentInfo {

    private String id;
    private String name;
    private int age;
    private String email;
    private String course;

    public StudentInfo(String id, String name, int age, String email, String course) {
        this.id = id;
        this.name = name;
        this.age = age;
        this.email = email;
        this.course = course;
    }

    // Getters method to get the information from each method
    public String getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public int getAge() {
        return age;
    }

    public String getEmail() {
        return email;
    }

    public String getCourse() {
        return course;
    }

    @Override
    public String toString() {
        return "ID: " + id + "\nName: " + name + "\nAge: " + age + "\nEmail: " + email + "\nCourse: " + course;
    }
}
