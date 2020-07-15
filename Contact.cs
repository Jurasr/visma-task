using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaTask
{
    class Contact
    {
        private int? _Id;
        public int? Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }

        private string _PhoneNumber;
        public string PhoneNumber
        {
            get { return _PhoneNumber; }
            set { _PhoneNumber = value; }
        }

        private string? _Address;
        public string? Address
        {
            get { return _Address; }
            set { _Address = value; }
        }


        public Contact(int id, string name, string lastName, string phoneNumber, string address)
        {
            Id = id;
            Name = name;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Address = address;
        }

        public Contact(int id, string name, string lastName, string phoneNumber)
        {
            Id = id;
            Name = name;
            LastName = lastName;
            PhoneNumber = phoneNumber;
        }

        public Contact(string name, string lastName, string phoneNumber)
        {
            Name = name;
            LastName = lastName;
            PhoneNumber = phoneNumber;
        }

        public Contact(string name, string lastName, string phoneNumber, string address)
        {
            Name = name;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Address = address;
        }
    }
}
