using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VismaTask
{
    class ConsoleUI
    {
        private ContactManager _ContactManager;

        private readonly string[] OPERATIONS = { "create", "read", "update", "delete" };

        public ConsoleUI()
        {
            _ContactManager = new ContactManager();
            Console.WriteLine("Contact Manager");
            PrintAll();

            GetUserInput();
        }

        public void PrintAll()
        {
            Console.WriteLine();
            Console.WriteLine("All Contacts:");
            _ContactManager.GetAll().ForEach(x => PrintOne(x));
            Console.WriteLine();
        }

        public void PrintOne(Contact contact)
        {
            Console.WriteLine($"ID: {contact.Id}, Name: {contact.Name}, Last Name: {contact.LastName}, Phone Number: {contact.PhoneNumber}{(contact.Address != null ? $", Address: {contact.Address}" : null)}.");
        }

        public void GetUserInput()
        {
            Console.WriteLine("Please type one of the following to perform an operation:");
            Console.WriteLine("create - To create a new contact");
            Console.WriteLine("read - To get data of all contacts");
            Console.WriteLine("read n - To get data of the contact with ID of n");
            Console.WriteLine("update n - To update a contact with ID of n");
            Console.WriteLine("delete n - To delete a contact with ID of n");
            Console.WriteLine("exit - To exit the program");
            Console.WriteLine("Please note that n should be a positive integer - for example: read 6");

            ParseUserInput(Console.ReadLine());
        }

        public void ParseUserInput(string input)
        {
            switch (input.ToLower())
            {
                case "exit":
                    return;
                case "read":
                    PrintAll();
                    GetUserInput();
                    break;
                case "create":
                    _ContactManager.Add(Create());
                    break;
                default:
                    MapUserInput(ValidateUserInput(input));
                    break;
            }

            PrintAll();
            GetUserInput();
        }

        public (string operation, int index) ValidateUserInput(string input)
        {
            string[] inputArr = input.Split(' ');

            if (inputArr.Length != 2)
            {
                Console.WriteLine("Invalid operation");
                GetUserInput();
            }
            
            string operation = inputArr[0];

            if (! OPERATIONS.Contains(operation))
            {
                Console.WriteLine($"Invalid operation name - operation name can only be {string.Join(", ", OPERATIONS)}");
                GetUserInput();
            }

            int id = int.Parse(inputArr[1]);

            if (id < 1)
            {
                Console.WriteLine($"Invalid operation ID - operation ID can only be a positive integer value.");
                GetUserInput();
            }

            return (operation, id);
        }

        public void MapUserInput((string name, int id) operation)
        {
            switch (operation.name)
            {
                case "read":
                    Read(operation.id);
                    break;
                case "update":
                    Update(operation.id);
                    break;
                case "delete":
                    Delete(operation.id);
                    break;
            }
        }

        public void Delete(int id)
        {
            try
            {
                _ContactManager.Delete(id);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not delete a contact with ID of {id} - {e.Message}");
            }
        }

        public void Read(int id)
        {
            try
            {
                PrintOne(_ContactManager.Get(id));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not get a contact with ID of {id} - {e.Message}");
            }
        }

        public Contact Create()
        {
            string name = GetUserInputText("name", 5);

            string lastName = GetUserInputText("last name", 5);

            Console.WriteLine("Please enter contact phone number:");
            string phoneNumber = Console.ReadLine().Trim();
            string phonePattern = @"^\+3706\d{7}$";
            Match m = Regex.Match(phoneNumber, phonePattern);
            while (!m.Success)
            {
                Console.WriteLine("Contact phone number should be in lithuanian format - start with +3706 and have 7 more digits after. Exmaple: +37061234567");
                Console.WriteLine("Please enter contact phone number:");
                phoneNumber = Console.ReadLine().Trim();
                m = Regex.Match(phoneNumber, phonePattern);
            }

            string address = GetUserInputText("address", 5, true);

            Contact contact = string.IsNullOrEmpty(address) ? new Contact(name, lastName, phoneNumber) : new Contact(name, lastName, phoneNumber, address);

            return contact;

        }

        public void Update(int id)
        {
            try
            {
                Contact contact = _ContactManager.Get(id);

                Contact updatedContact = Create();

                updatedContact.Id = id;

                _ContactManager.Update(updatedContact);

            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not get a contact with ID of {id} - {e.Message}");
            }
        }

        string GetUserInputText(string propName, int propLength, bool canBeEmpty = false)
        {
            Console.WriteLine($"Please enter contact {propName}:");

            if (canBeEmpty)
            {
                Console.WriteLine($"Enter nothing if you want to leave {propName} empty.");
            }

            string input = Console.ReadLine().Trim();

            if (canBeEmpty == true && input.Length == 0)
            {
                return input;
            }

            while (input.Length < propLength)
            {
                Console.WriteLine($"Contact {propName} should be atleast {propLength} characters length{(canBeEmpty ? " or empty" : null)}.");
                Console.WriteLine($"Please enter contact {propName}:");
                input = Console.ReadLine().Trim();

                if (canBeEmpty == true && input.Length == 0)
                {
                    return input;
                }
            }

            return input;
        }
    }
}
