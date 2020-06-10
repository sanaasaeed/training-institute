using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MaterialTheme {
  class StudentRecords {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string RegNo { get; set; }
    public string PhoneNo { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string DateOfBirth { get; set; }
    public string PhotoSrc { get; set; }
  }
  /// <summary>
  /// Interaction logic for Students.xaml
  /// </summary>
  public partial class Students : Page {
    public Students() {
      
      InitializeComponent();
      List<String> nameOfStudents = new List<String>();
       List<String> courseList = new List<String>();
      var context = new TrainingDBContext();
      // Retriving data from database
      var listOfStudents = (from student in context.Students
        select new StudentRecords(){FirstName = student.FirstName, LastName = student.LastName, RegNo = student.RegNo, PhoneNo = student.PhoneNo, Email = student.Email, Address = student.Address, DateOfBirth = student.DateOfBirth, PhotoSrc = student.PhotoSrc
        }).ToList();
      // Binding data
      foreach (var student in listOfStudents) {
        nameOfStudents.Add(student.FirstName);
      }

      var coursesList = (from course in context.Courses
        select new {
          CourseTitle = course.Title
        }).ToList();

      foreach (var courses in coursesList) {
        courseList.Add(courses.CourseTitle);
      }
      ListStudents.ItemsSource = listOfStudents;
      AssignStdCombo.ItemsSource = nameOfStudents;
      DeleteStdCombo.ItemsSource = nameOfStudents;
      AssignCourseCombo.ItemsSource = courseList;

      

    }

    private void BrowseBtnClicked(object sender, RoutedEventArgs e) {
        OpenFileDialog op = new OpenFileDialog();
        op.Title = "Select a picture";
        op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                    "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                    "Portable Network Graphic (*.png)|*.png";
        if (op.ShowDialog() == true) {
          Uri fileUri = new Uri(op.FileName);
          Photo.Source = new BitmapImage(fileUri);
        }
    }
    private void UpBrowseBtnClicked(object sender, RoutedEventArgs e) {
      OpenFileDialog op = new OpenFileDialog();
      op.Title = "Select a picture";
      op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                  "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                  "Portable Network Graphic (*.png)|*.png";
      if (op.ShowDialog() == true) {
        Uri fileUri = new Uri(op.FileName);
        PhotoUpdate.Source = new BitmapImage(fileUri);
      }
    }

    private void BtnAdd_OnClick(object sender, RoutedEventArgs e) {
      var context = new TrainingDBContext();
      if (FirstNameTextBox.Text == "" || LastNameTextBox.Text == "" || AddressTextBox.Text == "" ||
          DateOfBirthPicker.Text == "" || EmailTextBox.Text == "" || PhoneNumberTextBox.Text == "" ||
          RegTextBox.Text == "" || Photo.Source == null) {
        MessageBox.Show("All fields are required");
      }
      else {
        var studentObj = new Student() {
          FirstName = FirstNameTextBox.Text,
          LastName = LastNameTextBox.Text,
          Address = AddressTextBox.Text,
          DateOfBirth = DateOfBirthPicker.Text,
          Email = EmailTextBox.Text,
          PhoneNo = PhoneNumberTextBox.Text,
          RegNo = RegTextBox.Text,
          PhotoSrc = Photo.Source.ToString()
        };

        context.Students.Add(studentObj);
        context.SaveChanges();
        MessageBox.Show("Added: Refresh to see changes");
        FirstNameTextBox.Text = "";
        LastNameTextBox.Text = "";
        AddressTextBox.Text = "";
        DateOfBirthPicker.Text = "";
        EmailTextBox.Text = "";
        PhoneNumberTextBox.Text = "";
        RegTextBox.Text = "";
        Photo.Source = null;
      }
    }

    private void Refresh(object sender, RoutedEventArgs e) {
      List<String> nameOfStudents = new List<String>();
      List<String> courseList = new List<String>();
      var context = new TrainingDBContext();
      // Retriving data from database
      var listOfStudents = (from student in context.Students
        select new StudentRecords(){
          FirstName = student.FirstName,
          LastName = student.LastName,
          RegNo = student.RegNo,
          PhoneNo = student.PhoneNo,
          Email = student.Email,
          Address = student.Address,
          DateOfBirth = student.DateOfBirth,
          PhotoSrc = student.PhotoSrc
        }).ToList();
      // Binding data
      foreach (var student in listOfStudents) {
        nameOfStudents.Add(student.FirstName);
      }

      var coursesList = (from course in context.Courses
        select new {
          CourseTitle = course.Title
        }).ToList();

      foreach (var courses in coursesList) {
        courseList.Add(courses.CourseTitle);
      }
      ListStudents.ItemsSource = listOfStudents;
      AssignStdCombo.ItemsSource = nameOfStudents;
      DeleteStdCombo.ItemsSource = nameOfStudents;
      AssignCourseCombo.ItemsSource = courseList;
    }

    private void Assignment_CourseToStd(object sender, RoutedEventArgs e) {
      // Assigning Course to student
      var context = new TrainingDBContext();
      string assignedName = AssignStdCombo.SelectedValue.ToString();
      string assignCourseTitle = AssignCourseCombo.SelectedValue.ToString();
      int studentId = 0;
      int courseCode = 0;
      var queryStd = from std in context.Students
        where
          std.FirstName == assignedName
        select new {
          StudentID = std.StudentId
        };
      var queryCourse = from cour in context.Courses
        where
          cour.Title == assignCourseTitle
        select new {
          CourseCode = cour.Code
        };
      foreach (var query in queryStd) {
        studentId = query.StudentID;
      }
      foreach (var query in queryCourse) {
        courseCode = query.CourseCode;
      }
      StudentCourse iStudentCourses = new StudentCourse {
        StudentId = studentId,
        CourseId = courseCode
      };
      context.StudentCourses.Add(iStudentCourses);
      context.SaveChanges();
      MessageBox.Show("Assigned");
      AssignStdCombo.Text = "";
      AssignCourseCombo.Text = "";
    }

    private void DeleteBtn_OnClick(object sender, RoutedEventArgs e) {
      var db = new TrainingDBContext();
      var studentIdDel = 0;
      // Getting the studentId of selected object
      var confirmation =
        MessageBox.Show("Do You Really want to delete? ", "Deletion Confirmation", MessageBoxButton.YesNo);
      if (confirmation == MessageBoxResult.Yes) {
        if (DeleteStdCombo.SelectedItem != null) {
          var firstNameFromCombo = DeleteStdCombo.SelectedValue.ToString();
          var getStudentId = from std in db.Students
            where
              std.FirstName == firstNameFromCombo
            select new {
              std.StudentId
            };
          foreach (var stdId in getStudentId) {
            studentIdDel = stdId.StudentId;
          }

          var queryStudentCourses =
            from stdCour in db.StudentCourses
            where
              stdCour.StudentId == studentIdDel
            select stdCour;
          foreach (var del in queryStudentCourses) {
            db.StudentCourses.Remove(del);
          }

          db.SaveChanges();
          var queryStudents =
            from std in db.Students
            where
              std.StudentId == studentIdDel
            select std;
          foreach (var del in queryStudents) {
            db.Students.Remove(del);
          }

          db.SaveChanges();
          MessageBox.Show("Deleted! Refresh list");
          DeleteStdCombo.Text = "Select Record to delete";
        }
      }
    }

    private void ListStudents_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
      var selectedData = (StudentRecords)ListStudents.SelectedValue;
      if (selectedData != null) {
        UpFirstName.Text = selectedData.FirstName;
        UpLastName.Text = selectedData.LastName;
        UpRegNo.Text = selectedData.RegNo;
        UpAddress.Text = selectedData.Address;
        UpPhoneNo.Text = selectedData.PhoneNo;
        UpDateOfBirth.Text = selectedData.DateOfBirth;
        UpEmail.Text = selectedData.Email;
        if (PhotoUpdate != null) {
          var uriSource = new Uri(selectedData.PhotoSrc);
          PhotoUpdate.Source = new BitmapImage(uriSource);
        }
      }
      
      
    }

    private void Update_OnClick(object sender, RoutedEventArgs e) {
      var db = new TrainingDBContext();
      var selectedData = (StudentRecords)ListStudents.SelectedValue;
      if (selectedData != null) {
        var stdName = selectedData.FirstName;
        var stdId = (from std in db.Students
          where std.FirstName == stdName
          select std.StudentId).ToList();
        int studentId = stdId[0];
        var queryStudents =
          from std in db.Students
          where
            std.StudentId == studentId
          select std;
        foreach (var stds in queryStudents) {
          stds.FirstName = UpFirstName.Text;
          stds.LastName = UpLastName.Text;
          stds.RegNo = UpRegNo.Text;
          stds.Email = UpEmail.Text;
          stds.Address = UpAddress.Text;
          stds.PhoneNo = UpPhoneNo.Text;
          stds.DateOfBirth = UpDateOfBirth.Text;
          stds.PhotoSrc = PhotoUpdate.Source.ToString();
        }

        MessageBox.Show(queryStudents.ToString());
        db.SaveChanges();
        MessageBox.Show("Updated! Refresh to see changes");

      }
      UpFirstName.Text = "";
      UpLastName.Text = "";
      UpAddress.Text = "";
      UpDateOfBirth.Text = "";
      UpEmail.Text = "";
      UpPhoneNo.Text = "";
      UpRegNo.Text = "";
      PhotoUpdate.Source = null;
    }
  }

}
