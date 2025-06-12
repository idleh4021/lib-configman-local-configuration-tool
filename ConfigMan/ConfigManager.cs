using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Management.Instrumentation;
using System.Security.Cryptography;

namespace ConfigMan
{
    /// <summary>
    /// Nuget 패키지 설치 : NewtonSoft.Json
    /// <para>install-package newtonsoft.Json</para>
    /// </summary>
    public class ConfigManager
    {
        private DirectoryInfo _dir;
        private FileInfo _file;
        private string _directory;
        private string _file_name;
        private string _directory_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private object _configData;
        private bool _encryption=false;
        private string _encrypt_key;
        private string _myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public string MyDocumentsPath
        {
            get { return _myDocumentsPath; }
        }
        //private Type _type;
        public string DirectoryPath
        {
            get { return _directory_path; }
            set { _directory_path = value; }
        }
        public object ConfigData
        {
            get { return _configData; }
        }

        public bool Encryption { get => _encryption; set => _encryption = value; }
        public string Encryptkey { /*get => _encrypt_key; */set => _encrypt_key = value; }

        //public Type ConfigType
        //{
        //    get { return _type; }
        //}

        public ConfigManager(string directory, string file_name)
        {
            _directory = directory;
            _file_name = file_name;
            _dir = GetDir();
            _file = GetFile();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">변환 타입을 선언합니다,(ex: typeof(MyClass))</param>
        /// <returns></returns>
        public Object Open(Type type)
        {
            string str_json = File.ReadAllText(_file.FullName);
            if (Encryption) { str_json = Decrypt(str_json, _encrypt_key); }
            Object jobj = JsonConvert.DeserializeObject(str_json,type);
            _configData = jobj;
            return _configData;
        }

        public void Save(object config)
        {
            string strconfig;
            if (!_dir.Exists) { _dir.Create(); }
            JObject jobj = JObject.FromObject(config);
            //_type = config.GetType();
            strconfig = (Encryption) ? Encrypt(jobj.ToString(),_encrypt_key) : jobj.ToString();
            File.WriteAllText(_file.FullName, strconfig);
        }

        public DirectoryInfo GetDir()
        {
            if(string.IsNullOrEmpty(_directory)) return new DirectoryInfo(_directory_path);
            else return new DirectoryInfo(_directory_path + "/" + _directory);
        } 

        public FileInfo GetFile()
        {
            return new FileInfo(GetDir() + "/" + _file_name + ".config");
        }

        public void Remove()
        {
            _file.Delete();
        }


        private string Encrypt(string sourceText,string key)
        {
            if(string.IsNullOrEmpty(key)) { throw new Exception("암호화 키가 설정되어 있지 않습니다."); }
            byte[] iv = new byte[16];
            byte[] array;
            byte[] keyBytes = GetKeyBytes(key);
            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;//Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(sourceText);
                        }
                        array = ms.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }

        private string Decrypt(string sourceText,string key)
        {
            if (string.IsNullOrEmpty(key)) { throw new Exception("암호화 키가 설정되어 있지 않습니다."); }
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(sourceText);
            byte[] keyBytes = GetKeyBytes(key);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;//Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }

        private byte[] GetKeyBytes(string key)
        {
            // 입력된 키를 SHA256 해시로 변환하여 256비트(32바이트) 키 생성
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
        }

    }
}
