using System.IO;
using System.Windows.Input;

namespace MiniTC.ViewModel
{
    using Model;
    using Baseclass;
    using System;

    public class ViewModel : ViewModelBase
    {

        public ExplorerSidePanelViewModel LeftSidePanel { get; }
        public ExplorerSidePanelViewModel RightSidePanel { get; }

        public ViewModel()
        {
            LeftSidePanel = new ExplorerSidePanelViewModel();
            RightSidePanel = new ExplorerSidePanelViewModel();
        }

        // ICommands

        private ICommand _copy = null;

        public ICommand Copy
        {
            get
            {
                if (_copy == null)
                {
                    _copy = new RelayCommand(
                        arg => { CopyMet();},
                        arg => (LeftSidePanel.PathName != null && RightSidePanel.PathName != null)
                    );
                }

                return _copy;

            }
        }

        // Metoda kopiująca 
        private void CopyMet()
        {
            string fileName, destinationFile;
            if (LeftSidePanel.File == null)
            {
                if (Directory.Exists(LeftSidePanel.PathName))
                {
                    var allFiles = Directory.GetFiles(LeftSidePanel.PathName);
                    foreach (var f in allFiles)
                    {
                        fileName = Path.GetFileName(f);
                        destinationFile = Path.Combine(RightSidePanel.PathName, fileName);
                        
						try
						{
							File.Copy(f,destinationFile, true);
						}
						catch (Exception e)
						{
							MessageBox.Show($"Błąd: {e}");
						}
                    }
                }
            }
            else
            {
                fileName = LeftSidePanel.File.File;
                var f = Path.GetFileName(fileName);
                destinationFile = Path.Combine(RightSidePanel.PathName, f);
                LeftSidePanel.File = null;
				
				try
				{
					File.Copy(fileName,destinationFile,true);
				}
				catch (Exception e)
				{
					MessageBox.Show($"Błąd: {e}");
				}
            }

            LeftSidePanel.RefreshList(LeftSidePanel.PathName);
            RightSidePanel.RefreshList(RightSidePanel.PathName);
        }
    }
}