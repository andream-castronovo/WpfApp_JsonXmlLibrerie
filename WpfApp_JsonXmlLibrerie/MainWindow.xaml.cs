using System;
using System.Windows;
using System.IO;
using System.Xml.Serialization;

using Newtonsoft.Json;
using System.Security.RightsManagement;
using Newtonsoft.Json.Bson;

namespace WpfApp_JsonXmlLibrerie
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class MySerialazableClass
        {
            public string nome;
            public int eta;
        }

        public MySerialazableClass myObject;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnScriviXML_Click(object sender, RoutedEventArgs e)
        {
            myObject = new MySerialazableClass();

            myObject.nome = "Giorgio";
            myObject.eta = 42;

            // Serializzare: Prendere il nostro oggetto ed ottenere una stringa in formato XML
            // XmlSerializer è il tipo che ci consente ciò, ma necessita il tipo del nostro oggetto da serializzare
            XmlSerializer mySerializer = new XmlSerializer(typeof(MySerialazableClass));
            
            StreamWriter myWriter = new StreamWriter(@"..\..\..\SerializableClass.xml"); // Per scrivere nel file
            mySerializer.Serialize(myWriter, myObject); // Scrive nel nostro file "myWriter" l'oggetto serializzato "myObject"
            myWriter.Close();



        }

        private void btnLeggiXML_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(MySerialazableClass));
            StreamReader myReader = new StreamReader(@"..\..\..\SerializableClass.xml"); // Per scrivere nel file
            
            myObject = new MySerialazableClass();
            
            myObject = (MySerialazableClass) mySerializer.Deserialize(myReader); // Dobbiamo mettere un cast in quanto "Deserialize"
                                                                                 // restituisce un tipo object. Stiamo eseguendo
                                                                                 // un down-casting da object, quindi un unboxing.

            myReader.Close();

            MessageBox.Show($"Valori letti dal file:\n\tNome: {myObject.nome}\n\tEtà: {myObject.eta}");
        }
        private void btnScriviArrayXML_Click(object sender, RoutedEventArgs e)
        {
            MySerialazableClass[] myObj = new MySerialazableClass[3]
            {
                new MySerialazableClass(),
                new MySerialazableClass(),
                new MySerialazableClass()
            };

            string[] nomi = { "Giorgio", "Antono", "Marcos" };
            int[] eta = { 20, 19, 12 };

            for (int i = 0; i < myObj.Length; i++)
            {
                myObj[i].nome = nomi[i];
                myObj[i].eta = eta[i];
            }


            XmlSerializer mySerializer = new XmlSerializer(typeof(MySerialazableClass[])); // Da notare che il tipo è un array

            StreamWriter myWriter = new StreamWriter(@"..\..\..\SerializableClassArray.xml"); 
            mySerializer.Serialize(myWriter, myObj);
            myWriter.Close();

        }
        private void btnLeggiArrayXML_Click(object sender, RoutedEventArgs e)
        {
            MySerialazableClass[] myObj;

            XmlSerializer mySerializer = new XmlSerializer(typeof(MySerialazableClass[])); // Da notare che il tipo è un array

            StreamReader myReader = new StreamReader(@"..\..\..\SerializableClassArray.xml");

            myObj = (MySerialazableClass[]) mySerializer.Deserialize(myReader); // Da notare che il cast è verso un array

            myReader.Close();

            string s = "Valori letti nel file:";
            
            for (int i = 0; i < myObj.Length; i++)
                s += $"\n\tOggetto {i+1}:\n\t\tNome: {myObj[i].nome}\n\t\tEta: {myObj[i].eta}";

            MessageBox.Show(s);
        }

        private void btnScriviJSON_Click(object sender, RoutedEventArgs e)
        {
            MySerialazableClass myObj = new MySerialazableClass();

            myObj.nome = "ProvaJson";
            myObj.eta = 20;
            

            using (StreamWriter sw = new StreamWriter(@"..\..\..\ProvaJson.json")) // Per chiudere il file all'uscita dello using
                                                                                   // (Come with in python)
            {
                JsonSerializer jsonSer = new JsonSerializer();
                jsonSer.Serialize(sw, myObj);
            }

        }

        private void btnLeggiJSON_Click(object sender, RoutedEventArgs e)
        {
            MySerialazableClass myObj = new MySerialazableClass();


            using (StreamReader sr = new StreamReader(@"..\..\..\ProvaJson.json")) 
            {
                JsonSerializer jsonSer = new JsonSerializer(); // Posso saltare questa parte se scrivo
                                                               // File.OpenText(...) al posto di StreamReader
                
                using (JsonReader jsonReader = new JsonTextReader(sr))
                    myObj = (MySerialazableClass) jsonSer.Deserialize(jsonReader, typeof(MySerialazableClass));
                
            }

            MessageBox.Show($"Valori letti dal file:\n\tNome: {myObj.nome}\n\tEtà: {myObj.eta}");
        }
        
        private void btnScriviArrayJSON_Click(object sender, RoutedEventArgs e)
        {
            MySerialazableClass[] myObj = new MySerialazableClass[3]
            {
                new MySerialazableClass(),
                new MySerialazableClass(),
                new MySerialazableClass()
            };

            string[] nomi = { "Prova", "Per", "Json" };
            int[] eta = { 20, 19, 12 };

            for (int i = 0; i < myObj.Length; i++)
            {
                myObj[i].nome = nomi[i];
                myObj[i].eta = eta[i];
            }

            using (StreamWriter sw = new StreamWriter(@"..\..\..\ProvaArrayJson.json")) // Per chiudere il file all'uscita dello using
                                                                                   // (Come with in python)
            {
                JsonSerializer jsonSer = new JsonSerializer();
                jsonSer.Serialize(sw, myObj);
            }
        }
        
        private void btnLeggiArrayJSON_Click(object sender, RoutedEventArgs e)
        {
            MySerialazableClass[] myObj;
            using (StreamReader sr = File.OpenText(@"..\..\..\ProvaArrayJson.json"))
            {
                JsonSerializer jsonSer = new JsonSerializer();

                using (JsonReader jsonReader = new JsonTextReader(sr))
                    myObj = (MySerialazableClass[])jsonSer.Deserialize(jsonReader, typeof(MySerialazableClass[]));

            }

            string s = "Valori letti nel file:";

            for (int i = 0; i < myObj.Length; i++)
                s += $"\n\tOggetto {i + 1}:\n\t\tNome: {myObj[i].nome}\n\t\tEta: {myObj[i].eta}";

            MessageBox.Show(s);

        }
    }
}
