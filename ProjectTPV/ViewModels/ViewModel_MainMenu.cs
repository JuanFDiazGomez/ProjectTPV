using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectTPV.Models;
using System.IO;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;

namespace ProjectTPV.ViewModels {
    public class ViewModel_MainMenu {
        private MainMenu _MainMenu;
        private OpenNewWindowCommand _OpenNewWindowCommand;

        public ViewModel_MainMenu() {
            _MainMenu = new MainMenu();
            _OpenNewWindowCommand = new OpenNewWindowCommand(this);
        }

        public string NameTPV {
            get { return _MainMenu.NameTPV; }
        }

        public string NameSugerencias {
            get { return _MainMenu.NameSugerencias; }
        }

        public string NameCaja {
            get { return _MainMenu.NameCaja; }
        }

        public string NameAcercaDe {
            get { return _MainMenu.NameAcercaDe; }
        }

        public string PathImgTPV {
            get { return _MainMenu.ImgPathTPV; }
        }

        public string PathImgSugerencias {
            get { return _MainMenu.ImgPathSugerencias; }
        }

        public string PathImgCaja {
            get { return _MainMenu.ImgPathCaja; }
        }

        public string PathImgAcercaDe {
            get { return _MainMenu.ImgPathAcercaDe; }
        }

        public Window OrderWindow {
            get { return _MainMenu.OrderWindow; }
        }
        public Window SalesWindow {
            get { return _MainMenu.SalesWindow; }
        }
        public Window InfoWindow {
            get { return _MainMenu.InfoWindow; }
        }

        public void OpenNewWindow(Window view) {
            view.Show();
        }

        public OpenNewWindowCommand OpenNewWindowCommand {
            get { return _OpenNewWindowCommand; }
        }
    }

    public class OpenNewWindowCommand : ICommand {

        public event EventHandler CanExecuteChanged;
        private ViewModel_MainMenu _viewModel_MainMenu;

        public OpenNewWindowCommand(ViewModel_MainMenu _viewModel_MainMenu) {
            this._viewModel_MainMenu = _viewModel_MainMenu;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            _viewModel_MainMenu.OpenNewWindow((Window)parameter);
        }
    }
}
