using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MaterialTheme {
  class TrainerRecord {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Designation { get; set; }
    public string Department { get; set; }
    public string Salary { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string DateOfBirth { get; set; }
    public string PhotoSrc { get; set; }
  }
  /// <summary>
  /// Interaction logic for Students.xaml
  /// </summary>
  public partial class Trainers : Page {
    public Trainers() {

      InitializeComponent();
      List<String> nameOfTrainers = new List<String>();
      List<String> courseList = new List<String>();
      var context = new TrainingDBContext();
      // Retriving data from database
      var listOfTrainers = (from trainer in context.Trainers
                            select new TrainerRecord() {
                              FirstName = trainer.FirstName,
                              LastName = trainer.LastName,
                              Designation = trainer.Designation,
                              Department = trainer.Department,
                              Salary = trainer.Salary,
                              Email = trainer.Email,
                              Address = trainer.Address,
                              DateOfBirth = trainer.DateOfBirth,
                              PhotoSrc = trainer.PhotoSrc
                            }).ToList();
      // Binding data
      foreach (var student in listOfTrainers) {
        nameOfTrainers.Add(student.FirstName);
      }

      var coursesList = (from course in context.Courses
                         where course.TrainerId == null
                         select new {
                           CourseTitle = course.Title
                         }).ToList();

      foreach (var courses in coursesList) {
        courseList.Add(courses.CourseTitle);
      }
      ListTrainers.ItemsSource = listOfTrainers;
      // AssignTrainerCombo.ItemsSource = nameOfTrainers;
      DeleteTrainerCombo.ItemsSource = nameOfTrainers;
      // AssignCourseCombo.ItemsSource = courseList;
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
          DateOfBirthPicker.Text == "" || EmailTextBox.Text == "" || DesignationTextBox.Text == "" ||
          DepartmentTextBox.Text == "" || DepartmentTextBox.Text == "" || Photo.Source == null) {
        MessageBox.Show("All fields are required");
      }
      else {
        var trainerObj = new Trainer(){
          FirstName = FirstNameTextBox.Text,
          LastName = LastNameTextBox.Text,
          Address = AddressTextBox.Text,
          DateOfBirth = DateOfBirthPicker.Text,
          Email = EmailTextBox.Text,
          Designation = DesignationTextBox.Text,
          Department = DepartmentTextBox.Text,
          Salary = SalaryTextBox.Text,
          PhotoSrc = Photo.Source.ToString()
        };

        context.Trainers.Add(trainerObj);
        context.SaveChanges();
        MessageBox.Show("Added: Refresh to see changes");
        FirstNameTextBox.Text = "";
        LastNameTextBox.Text = "";
        AddressTextBox.Text = "";
        DateOfBirthPicker.Text = "";
        EmailTextBox.Text = "";
        DesignationTextBox.Text = "";
        DepartmentTextBox.Text = "";
        SalaryTextBox.Text = "";
        Photo.Source = null;
      }
    }

    private void Refresh(object sender, RoutedEventArgs e) {
      List<String> nameOfTrainers = new List<String>();
      List<String> courseList = new List<String>();
      var context = new TrainingDBContext();
      // Retriving data from database
      var listOfTrainers = (from trainer in context.Trainers
                            select new TrainerRecord() {
                              FirstName = trainer.FirstName,
                              LastName = trainer.LastName,
                              Designation = trainer.Designation,
                              Department = trainer.Department,
                              Email = trainer.Email,
                              Salary = trainer.Salary,
                              Address = trainer.Address,
                              DateOfBirth = trainer.DateOfBirth,
                              PhotoSrc = trainer.PhotoSrc
                            }).ToList();
      // Binding data
      foreach (var trainer in listOfTrainers) {
        nameOfTrainers.Add(trainer.FirstName);
      }

     
      ListTrainers.ItemsSource = listOfTrainers;
      // AssignTrainerCombo.ItemsSource = nameOfTrainers;
      DeleteTrainerCombo.ItemsSource = nameOfTrainers;
      // AssignCourseCombo.ItemsSource = courseList;
    }
 
    

    private void DeleteBtn_OnClick(object sender, RoutedEventArgs e) {
      var db = new TrainingDBContext();
      var trainerIdDel = 0;
      // Getting the studentId of selected object
      var confirmation =
        MessageBox.Show("Do You Really want to delete? ", "Deletion Confirmation", MessageBoxButton.YesNo);
      if (confirmation == MessageBoxResult.Yes) {
        if (DeleteTrainerCombo.SelectedItem != null) {
          var firstNameFromCombo = DeleteTrainerCombo.SelectedValue.ToString();
          var getTrainerId = from trainer in db.Trainers
                             where
                               trainer.FirstName == firstNameFromCombo
                             select new {
                               trainer.TrainerId
                             };
          foreach (var trId in getTrainerId) {
            trainerIdDel = trId.TrainerId;
          }

          var queryCourses =
            from cour in db.Courses
            where
              cour.TrainerId == trainerIdDel
            select cour;
          foreach (var cour in queryCourses) {
            cour.TrainerId = null;
          }
          db.SaveChanges();

          var queryTrainers = 
            from trainer in db.Trainers
            where
              trainer.FirstName == firstNameFromCombo
            select trainer;
          foreach (var del in queryTrainers){
            db.Trainers.Remove(del);
          }

          db.SaveChanges();
          MessageBox.Show("Deleted! Refresh list");
          DeleteTrainerCombo.Text = "Select Record to delete";
        }
      }
    }

    private void ListTrainers_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
      var selectedData = (TrainerRecord)ListTrainers.SelectedValue;
      if (selectedData != null) {
        if (selectedData.LastName == null) {
          UpLastName.Text = "";
        }
         if (selectedData.Department == null) {
          UpDepartment.Text = "";
        } if (selectedData.Designation == null) {
          UpDesignation.Text = "";
        }  if (selectedData.DateOfBirth == null) {
          UpDateOfBirth.Text = "";
        }  if (selectedData.Address == null) {
          UpAddress.Text = "";
        }  if (selectedData.Email == null) {
          UpEmail.Text = "";
        }  if (selectedData.Salary == null) {
          UpSalary.Text = "";
        } if (selectedData.PhotoSrc == null) {
          if (PhotoUpdate != null) {
            PhotoUpdate.Source = null;
          }
        }
         UpFirstName.Text = selectedData.FirstName;
          UpLastName.Text = selectedData.LastName;
          UpDepartment.Text = selectedData.Department;
          UpAddress.Text = selectedData.Address;
          UpDesignation.Text = selectedData.Designation;
          UpSalary.Text = selectedData.Salary;
          UpDateOfBirth.Text = selectedData.DateOfBirth;
          UpEmail.Text = selectedData.Email;
          if (PhotoUpdate != null) {
            if (selectedData.PhotoSrc != null) {
              var uriSource = new Uri(selectedData.PhotoSrc);
              PhotoUpdate.Source = new BitmapImage(uriSource);
          }
            
          }


      }


    }

    private void Update_OnClick(object sender, RoutedEventArgs e) {
      var db = new TrainingDBContext();
      var selectedData = (TrainerRecord)ListTrainers.SelectedValue;
      if (selectedData != null) {
        var trainerName = selectedData.FirstName;
        var tranId = (from trainer in db.Trainers
                     where trainer.FirstName == trainerName
                     select trainer.TrainerId).ToList();
        int trainerID = tranId[0];
        var queryTrainers =
          from trainer in db.Trainers
          where
            trainer.TrainerId == trainerID
          select trainer;
        foreach (var tran in queryTrainers) {
          tran.FirstName = UpFirstName.Text;
          tran.LastName = UpLastName.Text;
          tran.Designation = UpDesignation.Text;
          tran.Email = UpEmail.Text;
          tran.Address = UpAddress.Text;
          tran.Department = UpDepartment.Text;
          tran.Salary = UpSalary.Text;
          tran.DateOfBirth = UpDateOfBirth.Text;
          tran.PhotoSrc = PhotoUpdate.Source.ToString();
        }

        MessageBox.Show(queryTrainers.ToString());
        db.SaveChanges();
        MessageBox.Show("Updated! Refresh to see changes");

      }
      UpFirstName.Text = "";
      UpLastName.Text = "";
      UpAddress.Text = "";
      UpDateOfBirth.Text = "";
      UpEmail.Text = "";
      UpDepartment.Text = "";
      UpDesignation.Text = "";
      UpSalary.Text = "";
      PhotoUpdate.Source = null;
    }
  }

}
