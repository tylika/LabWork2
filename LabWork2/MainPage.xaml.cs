// LabWork2/MainPage.xaml.cs

using Microsoft.Maui.Controls;
using System;
using System.IO;
using LabWork2.XML_Manager;
using System.Xml;
using System.Reflection;
using Microsoft.Maui;

namespace LabWork2.Views
{
    public partial class MainPage : ContentPage
    {
        private string _filePath;
        private IParser _currentParser;

        public MainPage()
        {
            InitializeComponent();
            ParserPicker.SelectedIndexChanged += OnParserPickerChanged;
            UpdateAddFileButtonState(false); // Початковий стан кнопки
        }

        private async void OnAddFileClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(); // Виклик діалогу для вибору файлу

            if (result != null)
            {
                _filePath = result.FullPath; // Зберігаємо шлях до файлу
                UpdateAddFileButtonState(true);
            }
        }

        private void OnParserPickerChanged(object sender, EventArgs e)
        {
            _currentParser = ParserPicker.SelectedItem switch
            {
                "DOM" => new DomParser(),
                "LINQ to XML" => new LinqParser(),
                "SAX" => new SaxParser(),
                _ => null
            };
        }

        private async void OnFindClicked(object sender, EventArgs e)
        {
            if (_currentParser == null)
            {
                await DisplayAlert("Помилка", "Парсер не обрано.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(_filePath))
            {
                await DisplayAlert("Помилка", "Файл не обрано.", "OK");
                return;
            }

            var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore };

            using (var stream = File.OpenRead(_filePath))
            {
                if (!_currentParser.Load(stream, settings))
                {
                    await DisplayAlert("Помилка", "Не вдалося завантажити файл.", "OK");
                    return;
                }
            }

            var filters = new Filters
            {
                Name = NameCheckBox.IsChecked == true ? NameEntry.Text : "",
                Faculty = FacultyCheckBox.IsChecked == true ? FacultyEntry.Text : "",
                Course = CourseCheckBox.IsChecked == true ? CourseEntry.Text : "",
                Room = RoomCheckBox.IsChecked == true ? RoomEntry.Text : "",
                CheckInDate = CheckInCheckBox.IsChecked == true ? DateOnly.FromDateTime(CheckInEntry.Date) : (DateOnly?)null,
                CheckOutDate = CheckOutCheckBox.IsChecked == true ? DateOnly.FromDateTime(CheckOutEntry.Date) : (DateOnly?)null
            };

            var results = _currentParser.Find(filters);

            if (results.Count > 0)
            {
                await Navigation.PushAsync(new ResultPage(results));
            }
            else
            {
                await DisplayAlert("Результати пошуку", "Результатів не знайдено.", "OK");
            }
        }

        private void UpdateAddFileButtonState(bool isFileSelected)
        {
            AddFileButton.BackgroundColor = isFileSelected ? Colors.Green : Colors.Red;
            AddFileButton.Text = isFileSelected ? "Файл обрано" : "Обрати файл";
        }

        private void OnClearClicked(object sender, EventArgs e)
        {
            NameEntry.Text = FacultyEntry.Text = CourseEntry.Text = RoomEntry.Text = string.Empty;
            CheckInEntry.Date = DateTime.Now;
            CheckOutEntry.Date = DateTime.Now;
        }
    }
}
