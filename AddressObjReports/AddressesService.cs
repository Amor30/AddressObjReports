using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace AddressObjReports
{
    public class AddressesService
    {
        // Имена Level которые не надо указывать
        private readonly string[] unusedLevels = 
            {
                "Земельный участок",
            };
        public Dictionary<string, List<Address>> GetGroupAddresses(string directoryName)
        {
            var addresses = new List<Address>();
            var directories = Directory.GetDirectories(directoryName);
            var objectLevels = new Dictionary<int, string>();

            //Названия Object Level по которым не нужно указывать информацию
            GetLevelNames(objectLevels, unusedLevels, directoryName);
            foreach (var directory in directories)
            {
                var file = Directory.GetFiles(directory, "AS_ADDR_OBJ_" + DateTime.Now.Year + "*");
                AddAddresses(addresses, file[0], objectLevels);
            }
            addresses.Sort((b1, b2) => b1.Name.CompareTo(b2.Name));
            var groupedAddresses = addresses.GroupBy(a => a.LevelName)
                .ToDictionary(x => x.Key, y => y.ToList());
            return groupedAddresses;
        }
        private static void GetLevelNames(Dictionary<int, string> objectLevels, string[] unusedLevels, string directoryName)
        {
            using var xmlReader = XmlReader.Create(Directory.GetFiles(directoryName, "AS_OBJECT_LEVELS*")[0]);
            while (xmlReader.Read())
            {
                if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "OBJECTLEVEL"))
                {
                    if (xmlReader.HasAttributes &&
                        (!unusedLevels.Contains<string>(xmlReader.GetAttribute("NAME") ?? throw new InvalidOperationException())))
                    {

                        objectLevels.Add(Int32.Parse(xmlReader.GetAttribute("LEVEL") ?? throw new InvalidOperationException()),
                            xmlReader.GetAttribute("NAME") ?? throw new InvalidOperationException());
                    }
                }
            }
        }
        private static void AddAddresses(List<Address> addresses, string path, Dictionary<int, string> objectLevels)
        {
            using var xmlReader = XmlReader.Create(path);
            while (xmlReader.Read())
            {
                if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "OBJECT"))
                {
                    if (xmlReader.HasAttributes &&
                        (xmlReader.GetAttribute("ISACTIVE") == "1") &&
                        (objectLevels.ContainsKey(Int32.Parse(xmlReader.GetAttribute("LEVEL") ?? throw new InvalidOperationException()))))
                    {
                        var address = new Address
                        {
                            LevelName = objectLevels[Int32.Parse(xmlReader.GetAttribute("LEVEL") ?? throw new InvalidOperationException())],
                            TypeName = xmlReader.GetAttribute("TYPENAME") ?? throw new InvalidOperationException(),
                            Name = xmlReader.GetAttribute("NAME") ?? throw new InvalidOperationException(),
                            Level = Int32.Parse(xmlReader.GetAttribute("LEVEL") ?? throw new InvalidOperationException())
                        };
                        addresses.Add(address);
                    }
                }
            }
        }
    }
}
