using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace MaterialTheme {
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
        select new {FirstName = student.FirstName, LastName = student.LastName,student.RegNo,student.PhoneNo, student.Email, student.Address,student.DateOfBirth
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

      // Assigning Course to student

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

    private void BtnAdd_OnClick(object sender, RoutedEventArgs e) {
      var context = new TrainingDBContext();
      if (FirstNameTextBox.Text == "" || LastNameTextBox.Text == "" || AddressTextBox.Text == "" ||
          DateOfBirthPicker.Text == "" || EmailTextBox.Text == "" || PhoneNumberTextBox.Text == "" ||
          RegTextBox.Text == "") {
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
     
      }
    }

    private void Refresh(object sender, RoutedEventArgs e) {
      List<String> nameOfStudents = new List<String>();
      List<String> courseList = new List<String>();
      var context = new TrainingDBContext();
      // Retriving data from database
      var listOfStudents = (from student in context.Students
        select new {
          FirstName = student.FirstName,
          LastName = student.LastName,
          student.RegNo,
          student.PhoneNo,
          student.Email,
          student.Address,
          student.DateOfBirth
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
  }

}
