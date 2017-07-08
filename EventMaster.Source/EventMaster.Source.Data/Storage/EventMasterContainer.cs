using EventMaster.Source.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EventMaster.Source.Data.Storage
{
    public class EventMasterFileContainer
    {
        public static EventMasterFileContainer CurrentInstance { get; private set; }
        public string FilePath { get; private set; }

        public List<Employee> Employees { get; set; }
        public List<Event> Events { get; set; }
        public List<EventRegistration> EventRegistrations { get; set; }
        public List<Participant> Participants { get; set; }

        public static void SaveToFile(string filePathToSaveTo)
        {
            using (FileStream stream = new FileStream(filePathToSaveTo, FileMode.OpenOrCreate))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(EventMasterFileContainer));
                serializer.Serialize(stream, CurrentInstance);
            }
        }
        public static void LoadFromFile(string filePathToReadFrom)
        {
            using (FileStream stream = new FileStream(filePathToReadFrom, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(EventMasterFileContainer));
                CurrentInstance = (EventMasterFileContainer)serializer.Deserialize(stream);
            }
        }
    }
}
