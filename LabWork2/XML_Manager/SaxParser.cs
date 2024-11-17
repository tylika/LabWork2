// LabWork2/XML_Manager/SaxParser.cs

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace LabWork2.XML_Manager
{
    public class SaxParser : IParser
    {
        private readonly List<Person> _people;
        private Person _currentPerson;
        private string _currentElement;

        public SaxParser()
        {
            _people = new List<Person>();
        }

        public bool Load(Stream inputStream, XmlReaderSettings settings)
        {
            _people.Clear();

            try
            {
                using var reader = XmlReader.Create(inputStream, settings);

                while (reader.Read())
                {
                    ProcessXmlNode(reader);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IList<Person> Find(Filters filters) =>
            _people.FindAll(filters.ValidatePerson);

        private void ProcessXmlNode(XmlReader reader)
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    HandleStartElement(reader);
                    break;

                case XmlNodeType.Text:
                    HandleTextNode(reader);
                    break;

                case XmlNodeType.EndElement:
                    HandleEndElement(reader);
                    break;
            }
        }

        private void HandleStartElement(XmlReader reader)
        {
            if (reader.Name == "Person")
            {
                _currentPerson = new Person();
            }
            else if (_currentPerson != null)
            {
                _currentElement = reader.Name;
            }
        }

        private void HandleTextNode(XmlReader reader)
        {
            if (_currentPerson != null && !string.IsNullOrEmpty(_currentElement))
            {
                SetPersonData(_currentElement, reader.Value);
            }
        }

        private void HandleEndElement(XmlReader reader)
        {
            if (reader.Name == "Person" && _currentPerson != null)
            {
                _people.Add(_currentPerson);
                _currentPerson = null;
            }

            _currentElement = null;
        }

        private void SetPersonData(string elementName, string value)
        {
            if (_currentPerson == null) return;

            switch (elementName)
            {
                case "FirstName":
                    _currentPerson.Name.FirstName = value;
                    break;
                case "LastName":
                    _currentPerson.Name.LastName = value;
                    break;
                case "Faculty":
                    _currentPerson.Faculty = value;
                    break;
                case "Course":
                    _currentPerson.Course = value;
                    break;
                case "Room":
                    _currentPerson.Room = value;
                    break;
                case "CheckInDate":
                    _currentPerson.CheckInDate = ParseDate(value);
                    break;
                case "CheckOutDate":
                    _currentPerson.CheckOutDate = ParseDate(value);
                    break;
            }
        }

        private static DateOnly? ParseDate(string value) =>
            DateOnly.TryParse(value, out var date) ? date : (DateOnly?)null;
    }
}
