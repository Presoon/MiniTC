using MiniTC.ViewModel.Baseclass;
using MiniTC.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace MiniTC.ViewModel
{
    public class ExplorerSidePanelViewModel : ViewModelBase
    {
        private ObservableCollection<FileP> _files;

        public ObservableCollection<FileP> Files
        {
            get => _files;
            set => SetField(ref _files, value);
        }

        private ObservableCollection<Drive> _drives;

        public ObservableCollection<Drive> Drives
        {
            get => _drives;
            set => SetField(ref _drives, value);
        }

        public Drive SelectedDrive { get; set; }

        public FileP File { get; set; }

        private string _pathName;

        public string PathName
        {
            get => _pathName;
            set
            {
                _pathName = value;
                onPropertyChanged(nameof(PathName));
            }
        }


        public ExplorerSidePanelViewModel()
        {
            _drives = new ObservableCollection<Drive>();
            AddDrives(Drives);
        }


        private bool TryOpen(string path)
        {
            //MessageBox.Show("TryOpenDir");
            try
            {
                var dirs = Directory.GetDirectories(path);
                return true;
            }
            catch { return false; }
        }


        public void RefreshList(string path)
        {
            //MessageBox.Show("RefreshList");
            PathName = path;
            Files = new ObservableCollection<FileP>();
            if (Directory.GetParent(path) != null)
            {
                Files.Add(new FileP(Directory.GetParent(path).FullName, "..."));
            }
            var dirs = Directory.GetDirectories(path);
            var name = Directory.GetFiles(path);

            foreach (var x in dirs)
            {
                Files.Add(new FileP(x, "<d>" + x.Substring(3)));
            }

            foreach (var x in name)
            {
                Files.Add(new FileP(x, x.Substring(3)));
            }


        }

        public void AddDrives(ObservableCollection<Drive> drivesCollection)
        {
            //MessageBox.Show("Add drives");
            var allDrives = DriveInfo.GetDrives();

            foreach (var d in allDrives)
            {
                if (d.IsReady == true)
                {
                    drivesCollection.Add(new Drive(d.Name)
                    {
                        Name = d.Name,
                    });
                }
            }
        }

        private ICommand _update = null;

        public ICommand Update
        {
            get
            {
                return _update ?? (_update = new RelayCommand(
                    arg => { RefreshList(SelectedDrive.Name); },
                    arg => (SelectedDrive != null)
                ));
            }

        }

        private ICommand _updateList = null;

        public ICommand UpdateList
        {
            get
            {
                return _updateList ?? (_updateList = new RelayCommand(
                    arg => { RefreshList(File.File); },
                    arg => (File != null && (File.Name.Contains("<d>") || File.Name.Contains("...")) &&
                            TryOpen(File.File))
                ));
            }

        }
    }
}