using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTPV.Models {
    public class Product : INotifyPropertyChanged {
        private string _name;
        private string _imgPath;
        //private string _category;
        private double _price;
        private int _Quantity;
        private bool _ordered;

        public event PropertyChangedEventHandler PropertyChanged;

        //TODO eliminar y crear nuevamente todos los productos cuando se hace el order inicial
        public Product(string _name, string _imgPath, double _price) {
            this._name = _name;
            this._imgPath = "/ProjectTPV;component/Resources/Images/"+_imgPath;
            //this._category = _category;
            this._price = _price;
            this._Quantity = 0;
            this._ordered = false;
        }
        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        public string FormatName {
            get { return (_name.Length > 22) ? _name.Substring(0, 22)+"..." : _name; }
            set { _name = value; }
        }
        public string ImgPath {
            get { return _imgPath; }
            set { _imgPath = value; }
        }
        /*public string Category {
            get { return _category; }
            set { _category = value; }
        }*/
        public double Price {
            get { return _price; }
            set { _price = value; }
        }
        public string PriceTxt {
            get { return _price.ToString("N2")+" €"; }
        }
        public int Quantity {
            get { return _Quantity; }
            set {
                _Quantity = value;
                OnPropertyChanged("Quantity");
                OnPropertyChanged("TotalPrice");
            }
        }

        protected void OnPropertyChanged(string NameProperty) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null) {
                handler(this, new PropertyChangedEventArgs(NameProperty));
            }

        }

        public double TotalPrice {
            get { return _Quantity * _price; }
        }

        public string TotalPriceTxt {
            get { return TotalPrice.ToString("N2"); }
        }
    }


}
