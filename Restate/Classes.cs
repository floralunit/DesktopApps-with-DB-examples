using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restate
{
    public class Client
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Id { get; set; }

    }
    public class Agent
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int DealShare { get; set; }
        public int Id { get; set; }


    }
    public class Object
    {
        public string Type { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Number { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        public int Id { get; set; }

    }

    public class Apartment
    {
        public double TotalArea { get; set; }
        public int Rooms { get; set; }
        public int Floor { get; set; }
        public int Id { get; set; }
    }

    public class Land
    {

        public double TotalArea { get; set; }
        public int Id { get; set; }
    }
    public class House
    {
        public double TotalArea { get; set; }
        public int TotalFloors { get; set; }
        public int Id { get; set; }
    }
    public class Supply
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Number { get; set; }
        public string Client { get; set; }
        public string Agent { get; set; }
        public long Price { get; set; }

    }
    public class Demand
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Number { get; set; }
        public string Client { get; set; }
        public string Agent { get; set; }
        public long MaxPrice { get; set; }
        public long MinPrice { get; set; }

    }
}
