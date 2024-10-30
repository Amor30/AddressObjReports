using System.Xml;

namespace AddressObjReports
{
    /// <summary>
    /// Класс, который представляет сервис работы с адресами.
    /// </summary>
    public class AddressesService
    {
        //Названия Object Level по которым не нужно указывать информацию.
        private readonly string[] unusedLevels = 
            {
                "Земельный участок",
                "Здание (строение), сооружение",
                "Помещение",
                "Помещения в пределах помещения",
                "Машино-место"
            };

        /// <summary>
        /// Возвращает сгруппированные адреса в виде словаря с ключом LevelName и значением списка адресов.
        /// </summary>
        /// <param name="directoryName">Имя каталога, куда был распакован zip файл.</param>
        /// <returns></returns>
        public Dictionary<string, List<Address>> GetGroupAddresses(string directoryName)
        {
            var addresses = new List<Address>();
            var directories = Directory.GetDirectories(directoryName);
            var objectLevels = new Dictionary<int, string>();

            GetLevelNames(objectLevels, unusedLevels, directoryName);

            foreach (var directory in directories)
            {
                var file = Directory.GetFiles(directory, "AS_ADDR_OBJ_" + DateTime.Now.Year + "*");
                AddAddresses(addresses, file[0], objectLevels);
            }

            addresses.Sort((b1, b2) => b1.Name.CompareTo(b2.Name));
            var groupedAddresses = addresses
                .GroupBy(a => a.LevelName)
                .ToDictionary(x => x.Key, y => y.ToList());

            return groupedAddresses;
        }

        private void GetLevelNames(Dictionary<int, string> objectLevels, string[] unusedLevels, string directoryName)
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

        private void AddAddresses(List<Address> addresses, string path, Dictionary<int, string> objectLevels)
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
