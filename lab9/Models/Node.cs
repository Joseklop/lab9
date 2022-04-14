using System;
using System.Collections.ObjectModel;
using System.IO;
using ReactiveUI;

namespace lab9.Models
{
    public class Node: ReactiveObject
    {
        public string Name { get; set; }
        public string path { get; private set; }
        public Node(string path)
        {
            items = null;
            this.path = path;
            var file = new FileInfo(path);
            if (!file.Exists && !file.Attributes.HasFlag(FileAttributes.Directory))
            {
                throw new NullReferenceException();
            }
            isDirectory = file.Attributes.HasFlag(FileAttributes.Directory);
            if (file.Name.Length == 0) Name = path;
            else Name = file.Name;
        }

        public bool isDirectory;

        ObservableCollection<Node>? items;
        public ObservableCollection<Node>? Items
        {
            get
            {
                if (items == null && isDirectory)
                {
                    items = new ObservableCollection<Node>();

                    string[] str;
                    try
                    {
                        str = Directory.GetDirectories(path);
                        foreach (string file in str)
                        {
                            items.Add(new Node(file));
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {

                    }
                    try
                    {
                        str = Directory.GetFiles(path);
                        foreach (string file in str)
                        {
                            FileInfo fileInfo = new FileInfo(file);
                            string extension = fileInfo.Extension;
                            if (extension == ".png" || extension == ".jpeg" || extension == ".jpg" ||  extension == ".bmp")
                            {
                                items.Add(new Node(file));
                            }
                        }
                    }
                    catch (UnauthorizedAccessException) { }
                }
                return items;
            }
        }
    }
}
