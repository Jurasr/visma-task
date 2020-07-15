using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaTask
{
    class ContactManager
    {
        private List<Contact> _Contacts { get; set; }

        private string _FileName { get; set; } 

        public ContactManager()
        {
            _FileName = this.GetType().Name + ".csv";
            _Contacts = new List<Contact>();
            ReadFromFile();
        }

        public List<Contact> GetAll()
        {
            return _Contacts;
        }

        public Contact Get(int id)
        {
            Contact contact = _Contacts.FirstOrDefault(x => x.Id == id);

            if (contact == null)
            {
                throw new ArgumentException($"Contact with ID of {id} not found.");
            }

            return contact;
        }

        public void Add(Contact contact)
        {
            if (contact.Id == null)
            {
                contact.Id = GenerateID();
            }

            ValidateContact(contact);

            _Contacts.Add(contact);

            WriteToFile();
        }

        public void Delete(int id)
        {
            Get(id);

            _Contacts = _Contacts.Where(x => x.Id != id).ToList();

            WriteToFile();
        }

        public void Update(Contact contact)
        {
            if (contact.Id == null)
            {
                throw new ArgumentNullException("Please provide a contact with ID for updating");
            }
            
            Get((int)contact.Id);

            for (int i = 0; i < _Contacts.Count; i++)
            {
                if (_Contacts[i].Id == contact.Id)
                {
                    _Contacts[i] = contact;
                    break;
                }
            }

            WriteToFile();
        }

        void ValidateContact(Contact contact)
        {
            if (_Contacts.Select(x => x.Id).Contains(contact.Id))
            {
                throw new DuplicateNameException($"Could not add a contact with ID of {contact.Id} - a contact with this ID already exists.");
            }
            else if (_Contacts.Select(x => x.PhoneNumber).Contains(contact.PhoneNumber))
            {
                throw new DuplicateNameException($"Could not add a contact with Phone Number of {contact.PhoneNumber} - a contact with this Phone Number already exists.");
            }
        }

        public int GenerateID()
        {
            using (StreamReader reader = new StreamReader(_FileName))
            {
                string contactLine;

                int ID = 0;

                while ((contactLine = reader.ReadLine()) != null)
                {
                    int currentID = int.Parse(contactLine.Split(',')[0]);

                    if (ID < currentID)
                    {
                        ID = currentID;
                    }
                }

                return ID + 1;
            }
        }

        void ReadFromFile()
        {
            if (File.Exists(_FileName))
            {
                using (StreamReader reader = new StreamReader(_FileName))
                {
                    string contactLine;

                    int lineNumber = 1;

                    while ((contactLine = reader.ReadLine()) != null)
                    {
                        string[] lineElements = contactLine.Split(',');

                        ValidateLine(lineElements, lineNumber);

                        lineNumber++;

                        int contactId = int.Parse(lineElements[0]);

                        Contact contact = lineElements.Length == 5 ?
                            new Contact(contactId, lineElements[1], lineElements[2], lineElements[3], lineElements[4]) :
                            new Contact(contactId, lineElements[1], lineElements[2], lineElements[3]);

                        ValidateContact(contact);

                        _Contacts.Add(contact);
                    }
                }
            }
            else
            {
                File.Create(_FileName);
            }
        }

        void WriteToFile()
        {
            using (StreamWriter writer = new StreamWriter(_FileName, false))
            {
                foreach (Contact contact in _Contacts)
                {
                    writer.WriteLine(FormatToFile(contact));
                }
            }
        }
       
        public string FormatToFile(Contact contact)
        {
            return string.Join(",", contact.GetType().GetProperties().Where(x => x.GetValue(contact) != null).Select(y => y.GetValue(contact)).ToList());
        }

        void ValidateLine(string[] lineElements, int lineNumber)
        {
            if (lineElements.Length != 4 && lineElements.Length != 5)
            {
                throw new InvalidDataException($"Could not parse contact from {_FileName}. Line {lineNumber} has incorrect format.");
            }
        }

        public void PrintAll()
        {
            foreach (Contact contact in _Contacts)
            {
                Console.WriteLine($"ID: {contact.Id}, Name: {contact.Name}, Last Name: {contact.LastName}, Phone: {contact.PhoneNumber}, Address: {contact.Address}");
            }    
        }
    }
}
