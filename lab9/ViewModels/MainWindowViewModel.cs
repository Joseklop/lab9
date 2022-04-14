using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using lab9.Models;
using ReactiveUI;


namespace lab9.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        Node selectedItem;
        public Node SelectedItem
        {
            get => selectedItem;
            set => this.RaiseAndSetIfChanged(ref selectedItem, value);
        }

        Bitmap? image;
        public Bitmap? Image
        {
            get => image;
            set => this.RaiseAndSetIfChanged(ref image, value);
        }

        bool moveEnable;
        public bool MoveEnable
        {
            get => moveEnable;
            set => this.RaiseAndSetIfChanged(ref moveEnable, value);
        }

        ObservableCollection<Node> items;
        public ObservableCollection<Node> Items
        {
            get => items;
            set => this.RaiseAndSetIfChanged(ref items, value);
        }

        string currentPath;
        public string CurrentPath
        {
            get => currentPath;
            set => this.RaiseAndSetIfChanged(ref currentPath, value);
        }
        List<string> imageMoveList;
        int moveListIndex;
        void LoadImage()
        {
            if (selectedItem != null && !selectedItem.isDirectory)
            {
                Image = new Bitmap(selectedItem.path);
                CurrentPath = selectedItem.path;
                imageMoveList = new List<string>();
                int index = 0;
                foreach (string file in Directory.GetFiles(Directory.GetParent(selectedItem.path).FullName))
                {
                    string ext = Path.GetExtension(file);
                    if (ext == ".png" || ext == ".jpeg" || ext == ".jpg" || ext == ".bmp")
                    {
                        if (file == selectedItem.path) moveListIndex = index;
                        imageMoveList.Add(file);
                        index++;
                    }

                }
                if (imageMoveList.Count > 1) MoveEnable = true;
            }
            else
            {
                Image = null;
                MoveEnable = false;
            }
        }
        public MainWindowViewModel()
        {
            Image = null;
            MoveEnable = false;
            items = new ObservableCollection<Node>();
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                items.Add(new Node(drive.Name));
            }
            this.WhenAnyValue((x) => x.SelectedItem).Subscribe((x) => LoadImage());
        }
        void MoveImage(int direction)
        {
            int nIndex = moveListIndex + direction;
            if (nIndex < 0) nIndex = imageMoveList.Count - 1;
            if (nIndex >= imageMoveList.Count) nIndex = 0;
            moveListIndex = nIndex;
            CurrentPath = imageMoveList[moveListIndex];
            Image = new Bitmap(imageMoveList[moveListIndex]);

        }
    }
}
