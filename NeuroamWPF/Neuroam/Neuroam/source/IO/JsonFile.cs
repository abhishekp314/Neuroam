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
            m_FileName = fileName;

            try
            {
                FileStream stream = File.Open(m_FileName, FileMode.OpenOrCreate);
                stream.Close();
            }
            catch (Exception e)
            {
                Logger.Instance.LogException($"Failed to open/create file {m_FileName}.", e);
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
