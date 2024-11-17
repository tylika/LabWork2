using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using LabWork2.XML_Manager;

namespace LabWork2.Views
{
    public partial class ResultPage : ContentPage
    {
        public ObservableCollection<Person> SearchResults { get; set; }

        public ResultPage(IList<Person> searchResults)
        {
            InitializeComponent();

            // ����������� �����
            SearchResults = new ObservableCollection<Person>(searchResults);
            ResultsCollectionView.ItemsSource = SearchResults;
            BindingContext = this;
        }

        private async void OnTransformToHtmlClicked(object sender, EventArgs e)
        {
            if (SearchResults == null || !SearchResults.Any())
            {
                await DisplayAlert("�������", "���� ���������� ��� �������������.", "OK");
                return;
            }

            try
            {
                // ���������� XML-����
                string tempXmlPath = Path.Combine(Path.GetTempPath(), "searchResults.xml");
                SaveResultsToXml(tempXmlPath, SearchResults);

                var xslt = new XslCompiledTransform();

                // ������������ XSL-�������
                using (Stream xslStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LabWork2.Resources.template.xsl"))
                {
                    if (xslStream == null)
                    {
                        await DisplayAlert("�������", "������ template.xsl �� �������� �� ������.", "OK");
                        return;
                    }

                    using (XmlReader reader = XmlReader.Create(xslStream))
                    {
                        xslt.Load(reader);
                    }
                }

                // HTML-����
                string outputHtmlPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "searchResults.html");

                // ��������� �������������
                using (var writer = new StreamWriter(outputHtmlPath))
                {
                    xslt.Transform(tempXmlPath, null, writer);
                }

                await DisplayAlert("����", $"HTML ���� ��������� �� �������� ����: {outputHtmlPath}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("�������", $"�� ������� �������� �������������: {ex.Message}", "OK");
            }
        }

        private void SaveResultsToXml(string filePath, IEnumerable<Person> results)
        {
            var document = new XDocument(new XElement("Results"));

            foreach (var person in results)
            {
                var personElement = new XElement("Person",
                    new XElement("Name",
                        new XElement("FirstName", person.Name.FirstName ?? "N/A"),
                        new XElement("LastName", person.Name.LastName ?? "N/A")
                    ),
                    new XElement("Faculty", person.Faculty ?? "N/A"),
                    new XElement("Course", person.Course ?? "N/A"),
                    new XElement("Room", person.Room ?? "N/A"),
                    new XElement("Dates",
                        new XElement("CheckInDate", person.CheckInDate?.ToString("dd.MM.yyyy") ?? "N/A"),
                        new XElement("CheckOutDate", person.CheckOutDate?.ToString("dd.MM.yyyy") ?? "N/A")
                    )
                );

                document.Root.Add(personElement);
            }

            document.Save(filePath);
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); // ���������� �� ��������� �������
        }
    }
}
