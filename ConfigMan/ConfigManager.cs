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

        private Type _type;

        private string _default_directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public string DefaultDirectory
        {
            get { return _default_directory; }
            set { _default_directory = value; }
        }

        private object _configData;
        public object ConfigData
        {
            get { return _configData; }
        }

        public Type ConfigType
        {
            get { return _type; }
        }

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
            Object jobj = JsonConvert.DeserializeObject(str_json,type);
            _configData = jobj;
            return _configData;
        }

        public void Save(object config)
        {
            if (!_dir.Exists) { _dir.Create(); }
            JObject jobj = JObject.FromObject(config);
            _type = config.GetType();
            File.WriteAllText(_file.FullName, jobj.ToString());
        }

        private DirectoryInfo GetDir()
        {
            return new DirectoryInfo(_default_directory + "/" + _directory);
        }

        private FileInfo GetFile()
        {
            return new FileInfo(GetDir() + "/" + _file_name + ".config");
        }

        public void Remove()
        {
            _file.Delete();
        }

    }
}
