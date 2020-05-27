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
      List<Me> listme = new List<Me>() {
        new Me() {
          Name = "Sana Saeed",
          Age = 23
        },
        new Me() {
          Name = "Musfirah Mansoor",
          Age = 22
        }
      };
      InitializeComponent();
      ListStudents.ItemsSource = listme;
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
  }

  public class Me {
    public string Name { get; set; }
    public int Age { get; set; }
  }
}
