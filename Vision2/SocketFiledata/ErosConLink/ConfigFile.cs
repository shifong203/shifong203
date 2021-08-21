using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ErosSocket.ErosConLink
{
    public class ConfigFile : IConfig
    {
        // Fields
        private string _fileName;

        private static ConfigFile _Instance;

        // Methods
        private ConfigFile()
        {
        }

        public bool CreateFile()
        {
            bool flag = false;
            if (File.Exists(this.fileName))
            {
                return flag;
            }
            using (File.Create(this.fileName))
            {
            }
            return true;
        }

        public bool KeyExists(string Key)
        {
            return (this.Keys as ICollection<string>).Contains(Key);
        }

        // Properties
        public string fileName
        {
            get
            {
                return this._fileName;
            }
            set
            {
                this._fileName = value;
            }
        }

        public static ConfigFile Instanse
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ConfigFile();
                }
                return _Instance;
            }
        }

        public string this[string Key]
        {
            get
            {
                if (!this.CreateFile())
                {
                    foreach (string str in File.ReadAllLines(this.fileName, Encoding.UTF8))
                    {
                        Match match = Regex.Match(str, @"(\w+)=([\w\W]+)");
                        string str2 = match.Groups[1].Value;
                        string str3 = match.Groups[2].Value;
                        if (str2 == Key)
                        {
                            return str3;
                        }
                    }
                }
                return "";
            }
            set
            {
                if (this.CreateFile())
                {
                    File.AppendAllText(this.fileName, Key + "=" + value + "\r\n");
                }
                else
                {
                    string[] contents = File.ReadAllLines(this.fileName, Encoding.UTF8);
                    for (int i = 0; i < contents.Length; i++)
                    {
                        string input = contents[i];
                        Match match = Regex.Match(input, @"(\w+)=(\w+)");
                        string str2 = match.Groups[1].Value;
                        string text1 = match.Groups[2].Value;
                        if (str2 == Key)
                        {
                            contents[i] = str2 + "=" + value;
                            File.WriteAllLines(this.fileName, contents);
                            return;
                        }
                    }
                    File.AppendAllText(this.fileName, Key + "=" + value + "\r\n");
                }
            }
        }

        public string[] Keys
        {
            get
            {
                List<string> list = new List<string>();
                if (!this.CreateFile())
                {
                    foreach (string str in File.ReadAllLines(this.fileName, Encoding.UTF8))
                    {
                        string item = Regex.Match(str, @"(\w+)=(\w+)").Groups[1].Value;
                        list.Add(item);
                    }
                }
                return list.ToArray();
            }
        }
    }

    public interface IConfig
    {
        // Methods
        bool KeyExists(string Key);

        // Properties
        string this[string Key] { get; set; }

        string[] Keys { get; }
    }
}