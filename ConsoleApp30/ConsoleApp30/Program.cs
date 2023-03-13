using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;

namespace ConsoleApp30
{
    [Serializable]
    [DataContract]
    class Program
    {
        [DataMember]
        public string name { set; get; }
        [DataMember]
        public string company { set; get; }
        [DataMember]
        public int price { set; get; }
        [DataMember]
        public string begin { set; get; }

        public Program(string NP, string NC, int PP, string DoP)
        {
            name = NP;
            company = NC;
            price = PP;
            begin = DoP;
        }

        public Program() { }
        public void Add()
        {
            XDocument xdoc = XDocument.Load("New.xml");
            XElement root = xdoc.Element("Phone");

            if (root != null)
            {
                root.Add(new XElement("Phone 2",
                            new XAttribute("model", "IPhone 14"),
                            new XElement("manufacturer", "Steve Jobs"),
                            new XElement("Price", 30000),
                            new XElement("Begin", "2022 year")));

                xdoc.Save("Phone.xml");
            }
            Console.WriteLine(xdoc);
        }

        public void Search()
        {
            XDocument xdoc = XDocument.Load("Phone.xml");

            var tom = xdoc.Element("Phone")?   
               .Elements("Phone")             
               .FirstOrDefault(p => p.Attribute("model")?.Value == "Iphone 13 pro");

            var name = tom?.Attribute("model")?.Value;
            var pr = tom?.Element("Price")?.Value;
            var company = tom?.Element("manufacturer")?.Value;

            Console.WriteLine($"Model: {name}  Price: {pr}  Manufacturer: {company}");
        }

        public void Del()
        {
            XDocument xdoc = XDocument.Load("Phone.xml");
            XElement root = xdoc.Element("Phone");

            if (root != null)
            {
                var bob = root.Elements("Phone")
                    .FirstOrDefault(p => p.Attribute("model")?.Value == "IPhone 13 pro");
                if (bob != null)
               {
                    bob.Remove();
                    xdoc.Save("New.xml");
               }
            }
              Console.WriteLine(xdoc);
        }

        public void Edit()
        {
            XDocument xdoc = XDocument.Load("Phone.xml");

            var tom = xdoc.Element("Phone")?
                .Elements("Phone")
                .FirstOrDefault(p => p.Attribute("model")?.Value == "IPhone 13 pro");

            if (tom != null)
            {
                var name = tom.Attribute("model");
                if (name != null) name.Value = "IPhone 12";


                var age = tom.Element("Begin");
                if (age != null) age.Value = "2020 year";

                xdoc.Save("Phone.xml");
            }

            Console.WriteLine(xdoc);
        }

        public void Output()
        {
             XDocument xdoc = XDocument.Load("Phone.xml");
             XElement people = xdoc.Element("Phone");
                  foreach (XElement person in people.Elements("Phone"))
                 {
                     XAttribute name = person.Attribute("model");
                     XElement company = person.Element("manufacturer");
                     XElement pr = person.Element("Price");
                     XElement beg = person.Element("Begin");

                     Console.WriteLine($"Model: {name?.Value}");
                     Console.WriteLine($"manufacturer: {company?.Value}");
                     Console.WriteLine($"Price: {pr?.Value}");
                     Console.WriteLine($"Begin: {beg?.Value}");
                     Console.WriteLine(); 
                 }
        }

        static void Main(string[] args)
        {
            FileStream file = new FileStream("New.xml", FileMode.Create);
            XDocument xdoc = XDocument.Load("New.xml");
            XElement r = xdoc.Root;

            r.Add(new XElement("Phone",
                new XAttribute("model", "IPhone 13 pro"),
                new XElement("manufacturer", "Steve Jobs"),
                new XElement("Price", 20000),
                new XElement("Begin", "2021 year")));
            xdoc.Save("New.xml");

            Program p = new Program();
            p.Add();
            p.Search();
            p.Edit();
            p.Del();
            p.Output();

            XmlSerializer xmlSerializer = null;
            FileStream fileStream = null;
            DataContractJsonSerializer dataContractJson = null;

            Program phone = new Program("IPhone 12", "Apple", 30000, "2020");
            Console.WriteLine("1)XML - десериализация объекта");
            Console.WriteLine("2)XML - сериализация объекта");
            Console.WriteLine("3)JSON - сериализация объекта");
            Console.WriteLine("4)JSON - сериализация объекта");
            Console.WriteLine("Ваш выбор: ");
            int a = Convert.ToInt32(Console.ReadLine());
            switch (a)
            {
                case 1:
                    fileStream = new FileStream("../../New.xml", FileMode.Open);
                    xmlSerializer = new XmlSerializer(typeof(Program));
                    phone = (Program)xmlSerializer.Deserialize(fileStream);
                    Console.WriteLine(phone.name + " " + phone.company + "" + phone.price + " " + phone.begin + " ");
                    fileStream.Close();
                    break;
                case 2:
                    fileStream = new FileStream("../../Phone.xml", FileMode.Open);
                    xmlSerializer = new XmlSerializer(typeof(Program));
                    xmlSerializer.Serialize(fileStream, phone);
                    fileStream.Close();
                    break;
                case 3:
                    fileStream = new FileStream("../../Phone.json", FileMode.Open);
                    dataContractJson = new DataContractJsonSerializer(typeof(Program));
                    dataContractJson.WriteObject(fileStream, phone);
                    fileStream.Close();
                    break;
                case 4:
                    fileStream = new FileStream("../../Phone1.json", FileMode.Open);
                    dataContractJson = new DataContractJsonSerializer(typeof(Program));
                    phone = (Program)dataContractJson.ReadObject(fileStream);
                    Console.WriteLine(phone.name + " " + phone.company + " " + phone.price + " " + phone.begin + " ");
                    fileStream.Close();
                    break;

            }

        }
    }
}
