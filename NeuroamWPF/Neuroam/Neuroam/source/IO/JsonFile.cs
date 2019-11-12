using System;
using System.IO;
using Newtonsoft.Json;

namespace Neuroam.IO
{
    public class JsonFile
    {
        string m_FileName;
        public JsonFile(string fileName)
        {
            try
            {
                File.Open(fileName, FileMode.OpenOrCreate);
            }
            catch (Exception e)
            {
                Logger.Instance.Write($"Failed to open/create file {fileName}.", e);
            }
        }

        public string ReadAll()
        {
            return File.ReadAllText(m_FileName);
        }

        public void WriteAll(string data)
        {
            File.WriteAllText(m_FileName, data);
        }
    }
}
