using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MaterialTheme {
    public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
      Home.Content = new Main();
      List<MenuBox> menus = new List<MenuBox>() {
        new MenuBox() {
          Name = "Students",
          IconPath = "Images/student.png"
        },
        new MenuBox() {
          Name = "Trainers",
          IconPath = "Images/trainer.png"
        },
        new MenuBox() {
          Name = "Courses",
          IconPath = "Images/course.png"
        }
      };
      MenuItemBox.ItemsSource = menus;

      var context = new TrainingDBContext();
    }




    private void OnSelectionChangeMenuBox(object sender, SelectionChangedEventArgs e) {
      var name = ((MenuBox) MenuItemBox.SelectedItem).Name;
      if (name == "Students") {
        Home.Content = new Students();
      } else if (name == "Trainers") {
        Home.Content = new Trainers();
      } else if (name == "Courses") {
        Home.Content = new Courses();
      }
    }

    private void OnHomeBtnCLick(object sender, MouseButtonEventArgs e) {
      Home.Content = new Main();
    }
  }
  public class MenuBox {
    public string Name { get; set; }
    public string IconPath { get; set; }
  }
}
