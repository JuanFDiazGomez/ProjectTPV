using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTPV.Models {
    public class Category {
        private string _name;
        private ObservableCollection<Product> _productList = new ObservableCollection<Product>();

        public Category(string _name) {
            this._name = _name;
        }

        public string Name {
            get { return _name; }
        }

        public ObservableCollection<Product> ProductList {
            get { return _productList; }
        }

        public void AddProduct(Product product) {
            _productList.Add(product);
        }
    }
}
