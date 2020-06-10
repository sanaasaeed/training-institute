using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MaterialTheme {
  public partial class Courses : Page {
    class CoursesRecord {
      public int Code { get; set; }
      public string Title { get; set; }
      public string Description { get; set; }
      public string TrainerName { get; set; }
    }
    public Courses() {
      InitializeComponent();
      List<String> nameOfCourses = new List<String>();
      List<String> assignCourses = new List<String>();
      List<String> trainerNames = new List<String>();
      var context = new TrainingDBContext();
      // Retriving data from database
      var listOfCourses = (from course in context.Courses
        select new CoursesRecord() {
          Code = course.Code,
          Title = course.Title,
          Description = course.Description,
          TrainerName = course.Trainer.FirstName
        }).ToList();
      var trainerList = (from tran in context.Trainers
        select new {
          tran.FirstName
        });
      foreach (var trainer in trainerList) {
        trainerNames.Add(trainer.FirstName);
      }
      var assignedCourses = (from cour in context.Courses
        where
          cour.TrainerId == null
        select new {
          cour.Title
        });
      foreach (var assignedCourse in assignedCourses) {
        assignCourses.Add(assignedCourse.Title);
      }
      // Binding data

      foreach (var course in listOfCourses) {
        nameOfCourses.Add(course.Title);
      }
      ListCourses.ItemsSource = listOfCourses;
      DeleteCourseCombo.ItemsSource = nameOfCourses;
      AssignCourseCombo.ItemsSource = assignCourses;
      AssignTrainerCombo.ItemsSource = trainerNames;
    }

    private void Assignment_CourseToTrainer(object sender, RoutedEventArgs e) {
      // Assigning Course to Trainer
      var db = new TrainingDBContext();
      string assignedName = AssignTrainerCombo.SelectedValue.ToString();
      string assignedCourse = AssignCourseCombo.SelectedValue.ToString();
      int trainerId = 0;
      var queryTrainer = from trainer in db.Trainers
        where
          trainer.FirstName == assignedName
        select new {
          TrainerID = trainer.TrainerId
        };
      foreach (var query in queryTrainer) {
        trainerId = query.TrainerID;
      }
      // // UPDATION
      var queryCourses =
        from cour in db.Courses
        where
          cour.Title == assignedCourse
        select cour;
      foreach (var course in queryCourses) {
        course.TrainerId = trainerId;
      }
      db.SaveChanges();

      MessageBox.Show("Assigned! Please Refresh");

    }

    private void listCoursesOnSelection(object sender, SelectionChangedEventArgs e) {
      var selectedData = (CoursesRecord)ListCourses.SelectedValue;
      if (selectedData != null) {
        UpCode.Text = selectedData.Code.ToString();
        UpDescription.Text = selectedData.Description;
        UpTitle.Text = selectedData.Title;
      }
    }

    private void Refresh(object sender, RoutedEventArgs e) {
      List<String> nameOfCourses = new List<String>();
      List<String> trainerNames = new List<String>();
      List<String> assignCourses = new List<String>();
      var context = new TrainingDBContext();
      // Retriving data from database
      var listOfCourses = (from course in context.Courses
        select new CoursesRecord() {
          Code = course.Code,
          Title = course.Title,
          Description = course.Description,
          TrainerName = course.Trainer.FirstName
        }).ToList();


      var trainerList = (from tran in context.Trainers
        select new {
          tran.FirstName
        });
      foreach (var trainer in trainerList) {
        trainerNames.Add(trainer.FirstName);
      }
      var assignedCourses = (from cour in context.Courses
        where
          cour.TrainerId == null
        select new {
          cour.Title
        });
      foreach (var assignedCourse in assignedCourses) {
        assignCourses.Add(assignedCourse.Title);
      }
      // Binding data
      foreach (var course in listOfCourses) {
        nameOfCourses.Add(course.Title);
      }
      ListCourses.ItemsSource = listOfCourses;
      DeleteCourseCombo.ItemsSource = nameOfCourses;
      AssignCourseCombo.ItemsSource = assignCourses;
      AssignTrainerCombo.ItemsSource = trainerNames;
    }

    private void AddBtnCourse(object sender, RoutedEventArgs e) {
      var context = new TrainingDBContext();
      if (CodeTextBox.Text == "" || TitleTextBox.Text == "" || DescriptionTextBox.Text == "") {
        MessageBox.Show("All fields are required");
      }
      else {
        var courseObj = new Course() {
          Code = int.Parse(CodeTextBox.Text),
          Title = TitleTextBox.Text,
          Description = DescriptionTextBox.Text
        };

        context.Courses.Add(courseObj);
        context.SaveChanges();
        MessageBox.Show("Added: Refresh to see changes");
        CodeTextBox.Text = "";
        TitleTextBox.Text = "";
        DescriptionTextBox.Text = "";
      }
    }

    private void DeleteBtnCLick(object sender, RoutedEventArgs e) {
      var db = new TrainingDBContext();
      var CourseIdDel = 0;
      // Getting the studentId of selected object
      var confirmation =
        MessageBox.Show("Do You Really want to delete? ", "Deletion Confirmation", MessageBoxButton.YesNo);
      if (confirmation == MessageBoxResult.Yes) {
        if (DeleteCourseCombo.SelectedItem != null) {
          var titleFromCombo = DeleteCourseCombo.SelectedValue.ToString();
          var getCourseId = from cour in db.Courses
            where
              cour.Title == titleFromCombo
            select new {
              cour.Code
            };
          foreach (var crId in getCourseId) {
            CourseIdDel = crId.Code;
          }

          var queryStudentCourses =
            from stdCour in db.StudentCourses
            where
              stdCour.CourseId == CourseIdDel
            select stdCour;
          foreach (var del in queryStudentCourses) {
            db.StudentCourses.Remove(del);
          }

          db.SaveChanges();

          var queryCourses =
            from course in db.Courses
            where
              course.Title== titleFromCombo
            select course;
          foreach (var del in queryCourses) {
            db.Courses.Remove(del);
          }

          db.SaveChanges();
          MessageBox.Show("Deleted! Refresh list");
          DeleteCourseCombo.Text = "Select Record to delete";
        }
      }
    }

    private void UpdateOnClick(object sender, RoutedEventArgs e) {
      var db = new TrainingDBContext();
      var selectedData = (CoursesRecord)ListCourses.SelectedValue;
      if (selectedData != null) {
        var courseTitle = selectedData.Title;
        var courseId = (from cour in db.Courses
          where cour.Title == courseTitle
          select cour.Code).ToList();
        int courseID = courseId[0];
        var queryCourses =
          from cour in db.Courses
          where
            cour.Code == courseID
          select cour;
        foreach (var cour in queryCourses) {
          cour.Title = UpTitle.Text;
          cour.Description = UpDescription.Text;
        }
        db.SaveChanges();
        MessageBox.Show("Updated! Refresh to see changes");

      }
      UpTitle.Text = "";
      UpCode.Text = "";
      UpDescription.Text = "";
    }
  }
}
