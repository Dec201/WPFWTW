using System.IO;

namespace WPFWTW.Persistence
{
    public class FilePersistence : IPersistence
    {

        private readonly string _path;

        public FilePersistence(string path)
        {
            _path = path;
        }


        public void Save(string data)
        {
            using (var streamWriter = new StreamWriter(_path, true))
            {
                streamWriter.WriteLine(data);
            }
        }
    }
}
