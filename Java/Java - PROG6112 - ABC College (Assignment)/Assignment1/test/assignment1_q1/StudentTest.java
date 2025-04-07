/*
 * ST10393280 - Daniel Luke James
 */
package assignment1_q1;

import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

/**
 *
 * Author: Daniel Luke James
 */
public class StudentTest {

    private Student student;

    @BeforeEach
    public void setUp() {
        student = new Student();
    }

    @Test
    public void testSaveStudent() {
        // Provide student details
        String studentId = "100";
        String studentName = "Daniel";
        int studentAge = 25;
        String studentEmail = "daniel@gmail.com";
        String studentCourse = "PROG";

        // Save the student
        student.saveStudent(studentId, studentName, studentAge, studentEmail, studentCourse);

        // Retrieve the saved student
        StudentInfo savedStudent = student.searchStudent(studentId);

        assertNotNull(savedStudent);
        assertEquals(studentId, savedStudent.getId());
        assertEquals(studentName, savedStudent.getName());
        assertEquals(studentAge, savedStudent.getAge());
        assertEquals(studentEmail, savedStudent.getEmail());
        assertEquals(studentCourse, savedStudent.getCourse());
    }

    @Test
    public void testSearchStudent() {
        // Provide student details
        String studentId = "100";
        String studentName = "Daniel";
        int studentAge = 25;
        String studentEmail = "daniel@gmail.com";
        String studentCourse = "PROG";

        // Save the student
        student.saveStudent(studentId, studentName, studentAge, studentEmail, studentCourse);

        // Search for the student
        StudentInfo foundStudent = student.searchStudent(studentId);

        assertNotNull(foundStudent);
        assertEquals(studentId, foundStudent.getId());
        assertEquals(studentName, foundStudent.getName());
        assertEquals(studentAge, foundStudent.getAge());
        assertEquals(studentEmail, foundStudent.getEmail());
        assertEquals(studentCourse, foundStudent.getCourse());
    }

    @Test
    public void testSearchStudent_StudentNotFound() {
        // Search for a non-existent student
        StudentInfo foundStudent = student.searchStudent("108");

        assertNull(foundStudent);
    }

    @Test
    public void testDeleteStudent() {
        // Provide student details
        String studentId = "100";
        String studentName = "Daniel";
        int studentAge = 25;
        String studentEmail = "daniel@gmail.com";
        String studentCourse = "PROG";

        // Save the student
        student.saveStudent(studentId, studentName, studentAge, studentEmail, studentCourse);

        // Delete the student
        boolean isDeleted = student.deleteStudent(studentId);

        assertTrue(isDeleted);

        // Attempt to find the deleted student
        StudentInfo deletedStudent = student.searchStudent(studentId);

        assertNull(deletedStudent);
    }

    @Test
    public void testDeleteStudent_StudentNotFound() {
        // Attempt to delete a non-existent student
        boolean isDeleted = student.deleteStudent("108");

        assertFalse(isDeleted);
    }

    @Test
    public void testStudentAge_StudentAgeValid() {
        // Provide a valid student age
        int validAge = 25;

        assertTrue(student.isStudentAgeValid(validAge));
    }

    @Test
    public void testStudentAge_StudentAgeInvalid() {
        // Provide an invalid student age (less than 16)
        int invalidAge = 10;

        assertFalse(student.isStudentAgeValid(invalidAge));
    }

    @Test
    public void testStudentAge_StudentAgeInvalidCharacter() {
        // Provide an invalid character as age
        int invalidAge = Integer.parseInt("qwerty");

        assertFalse(student.isStudentAgeValid(invalidAge));
    }
}
