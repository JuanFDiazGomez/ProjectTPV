using ProjectTPV.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ProjectTPV.Models {
    class MainMenu {
        /// <summary>
        /// Atributos de la case, nombres de secciones y paths de imagenes
        /// </summary>
        private string _nameTPV = "PEDIR";
        private string _nameSugerencias = "SUGERENCIAS";
        private string _nameCaja = "CAJA";
        private string _nameAcercaDe = "INFORMACIÓN";

        private string _imgPathTPV = "/ProjectTPV;component/Resources/Images/img_en_construccion.png";
        private string _imgPathSugerencias = "/ProjectTPV;component/Resources/Images/img_en_construccion.png";
        private string _imgPathCaja = "/ProjectTPV;component/Resources/Images/img_en_construccion.png";
        private string _imgPathAcercaDe = "/ProjectTPV;component/Resources/Images/img_en_construccion.png";

        private OrderWindow _orderWindow = new OrderWindow();

        public MainMenu() {}

        public string NameTPV {
            get { return _nameTPV; }
        }

        public string NameSugerencias {
            get { return _nameSugerencias; }
        }

        public string NameCaja {
            get { return _nameCaja; }
        }

        public string NameAcercaDe {
            get { return _nameAcercaDe; }
        }

        public string ImgPathTPV {
            get { return _imgPathTPV; }
        }
        public string ImgPathSugerencias {
            get { return _imgPathSugerencias; }
        }
        public string ImgPathCaja {
            get { return _imgPathCaja; }
        }
        public string ImgPathAcercaDe {
            get { return _imgPathAcercaDe; }
        }

        public Window OrderWindow {
            get { return _orderWindow; }
        }
    }
}
