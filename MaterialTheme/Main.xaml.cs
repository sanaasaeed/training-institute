using System;
using System.Collections.Generic;
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

namespace MaterialTheme {
  /// <summary>
  /// Interaction logic for Main.xaml
  /// </summary>
  public partial class Main : Page {
    public List<String> unassigned = new List<String>();
    public List<String> studentsWithNoCourse = new List<String>();
    public int noOfStudents { get; set; }
    public int noOfTrainers { get; set; }
    public int noOfCourses { get; set; }
    public Main() {
      InitializeComponent();
      var context = new TrainingDBContext();
      // Reports about no of things in Database
      var students = context.Students.Select(std => std);
      noOfStudents = students.Count();
      var trainers = context.Trainers.Select(tra => tra);
      noOfTrainers = trainers.Count();
      var courses = context.Courses.Select(cours => cours);
      noOfCourses = courses.Count();


      // UNASSIGNED COURSES
      var unassignedCourses = context.Courses.Where(cour => cour.TrainerId == null).Select(cour => new  {
        CourseName = cour.Title
      });
      // Unassigned Courses in combobox
      foreach (var unassignedCourse in unassignedCourses) {
        unassigned.Add(unassignedCourse.CourseName);
      }
      UnassignedCourse.ItemsSource = unassigned;
      this.DataContext = this;


      // STUDENTS TAKING NO COURSES
      var result = (from student in context.Students
        where
          !
            (from studentcourse in context.StudentCourses
              select new {
                studentcourse.StudentId
              }).Contains(new {StudentId = student.StudentId})
        select new {
          student.StudentId
        }).ToList();

      MessageBox.Show(result.Count().ToString());
      StudentCombo.ItemsSource = result;
    }





    private void OnUnassignedSelectionChange(object sender, SelectionChangedEventArgs e) {
      // Updating Data Grid 
      var context = new TrainingDBContext();
      var selectedCourse = UnassignedCourse.SelectedValue.ToString();
      var selectedQuery = context.Courses.Where(cour => cour.Title == selectedCourse).Select(cour => new {
        cour.Code, cour.Title, cour.Description, cour.TrainerId
      }).ToList();
      DataGridMain.ItemsSource = selectedQuery;
    }

    private void StudentCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
      // Updating Data Grid 
      // var context = new TrainingDBContext();
      // var selectedCourse = StudentCombo.SelectedValue.ToString();
      // var selectedQuery = context.Courses.Where(cour => cour.Title == selectedCourse).Select(cour => new {
      //   cour.Code,
      //   cour.Title,
      //   cour.Description,
      //   cour.TrainerId
      // }).ToList();
      // DataGridMain.ItemsSource = selectedQuery;
    }
  }
}
