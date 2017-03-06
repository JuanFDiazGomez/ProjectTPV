using ProjectTPV.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProjectTPV.ViewModels {
    public class ViewModel_Categories : INotifyPropertyChanged {
        private const int LIMIT_PRODUCT_PAGE = 24;
        private int _currentPage;
        private List<Category> _categories;
        private ObservableCollection<Product> _currentProductList;
        private ObservableCollection<Product> _ShownProducts;
        private ObservableCollection<Product> _orderProducts;
        private ChangeProductListCommand _changeProductList;
        private OrderProductCommand _orderProductCommand;
        private ReturnCommand _returnCommand;
        private OneMoreCommand _oneMoreCommand;
        private OneLessCommand _oneLessCommand;
        private DeleteOrderProductCommand _deleteOrderProductCommand;
        private NextProductPageCommand _nextProductPageCommand;
        private PrevProductPageCommand _prevProductPageCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel_Categories() {
            _categories = new List<Category>();
            _ShownProducts = new ObservableCollection<Product>();
            _orderProducts = new ObservableCollection<Product>();
            _changeProductList = new ChangeProductListCommand(this);
            _orderProductCommand = new OrderProductCommand(this);
            _returnCommand = new ReturnCommand(this);
            _oneMoreCommand = new OneMoreCommand(this);
            _oneLessCommand = new OneLessCommand(this);
            _deleteOrderProductCommand = new DeleteOrderProductCommand(this);
            _nextProductPageCommand = new NextProductPageCommand(this);
            _prevProductPageCommand = new PrevProductPageCommand(this);
            _currentPage = 1;
            InitilizeCategories();
        }

        public string CurrentPageAndMaxPage {
            get { return _currentPage.ToString() + '/' + (((_currentProductList.Count) + (LIMIT_PRODUCT_PAGE - 1)) / LIMIT_PRODUCT_PAGE).ToString(); }
        }

        protected void OnPropertyChanged(string NameProperty) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null) {
                handler(this, new PropertyChangedEventArgs(NameProperty));
            }

        }

        public int CurrentPage {
            get { return _currentPage; }
            set {
                _currentPage = value;
                OnPropertyChanged("CurrentPageAndMaxPage");
            }
        }

        public int CurrentMaxPage {
            get { return ((_currentProductList.Count) + (LIMIT_PRODUCT_PAGE - 1)) / LIMIT_PRODUCT_PAGE; }
        }

        public List<Category> Categories {
            get { return _categories; }
        }
        public ObservableCollection<Product> Products {
            get { return _ShownProducts; }
        }
        public ObservableCollection<Product> OrderProducts {
            get { return _orderProducts; }
        }

        public ChangeProductListCommand ChangeProductListCommand {
            get { return _changeProductList; }
        }
        public OrderProductCommand OrderProduct {
            get { return _orderProductCommand; }
        }
        public ReturnCommand ReturnCommand {
            get { return _returnCommand; }
        }
        public OneMoreCommand OneMoreCommand {
            get { return _oneMoreCommand; }
        }
        public OneLessCommand OneLessCommand {
            get { return _oneLessCommand; }
        }
        public DeleteOrderProductCommand DeleteOrderProductCommand {
            get { return _deleteOrderProductCommand; }
        }
        public NextProductPageCommand NextProductPageCommand {
            get { return _nextProductPageCommand; }
        }
        public PrevProductPageCommand PrevProductPageCommand {
            get { return _prevProductPageCommand; }
        }
        public void InitilizeCategories() {
            string[] Name = { "Cous Cous", "Shushi", "Gamba Roja", "Marmitako de Atun", "Helado de Limon" };
            string ImgPath = "/ProjectTPV;component/Resources/Images/img_en_construccion.png";
            string[] Category = { "Bebidas", "Vinos y Cavas", "Entrantes", "Principales", "Postres" };
            double[] Price = { 222.55, 3, 4, 5, 2 };
            for(int i = 0 ; i < Category.Length ; i++) {
                Category category = new Category(Category[i]);
                for(int j = 0 ; j < 60 ; j++) {
                    category.AddProduct(new Product(Name[i], ImgPath, Category[i], Price[i]));
                }
                _categories.Add(category);
            }
            ChangeProductList(_categories.First<Category>().ProductList);
        }

        internal void ChangeProductList(ObservableCollection<Product> list) {
            CurrentPage = 1;
            _currentProductList = list;
            _ShownProducts.Clear();
            foreach(Product product in list.Take<Product>(LIMIT_PRODUCT_PAGE)) {
                _ShownProducts.Add(product);
            }
        }

        internal void AddProductOrder(Product product) {
            if(product.Quantity == 0) {
                product.Quantity = 1;
                _orderProducts.Add(product);
            } else {
                product.Quantity += 1;
            }
            OnPropertyChanged("TotalPriceList");
        }

        internal void Return(Window window) {
            window.Hide();
        }

        internal void OneMore(Product product) {
            product.Quantity++;
            OnPropertyChanged("TotalPriceList");
        }

        internal void OneLess(Product product) {
            product.Quantity--;
            OnPropertyChanged("TotalPriceList");
        }

        internal void DeleteOrderProduct(Product product) {
            _orderProducts.Remove(product);
            product.Quantity = 0;
            OnPropertyChanged("TotalPriceList");
        }

        internal void NextProductPage() {
            _ShownProducts.Clear();
            foreach(Product product in _currentProductList.Skip<Product>(_currentPage * LIMIT_PRODUCT_PAGE).Take<Product>(LIMIT_PRODUCT_PAGE)) {
                _ShownProducts.Add(product);
            }
            CurrentPage++;

        }

        public double TotalPriceList {
            // obten la suma de la propiedad en todos los elementos dentro de la lista
            get { return _orderProducts.Sum<Product>(x => x.TotalPrice); }
        }

        internal void PrevProductPage() {
            _ShownProducts.Clear();
            CurrentPage--;
            foreach(Product product in _currentProductList.Skip<Product>((_currentPage - 1) * LIMIT_PRODUCT_PAGE).Take<Product>(LIMIT_PRODUCT_PAGE)) {
                _ShownProducts.Add(product);
            }

        }
    }

    public class ChangeProductListCommand : ICommand {
        private ViewModel_Categories _viewModel_Categories;
        public event EventHandler CanExecuteChanged;

        public ChangeProductListCommand(ViewModel_Categories _viewModel_Categories) {
            this._viewModel_Categories = _viewModel_Categories;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            _viewModel_Categories.ChangeProductList(parameter as ObservableCollection<Product>);

        }
    }

    public class OrderProductCommand : ICommand {
        private ViewModel_Categories _viewModel_Categories;
        public event EventHandler CanExecuteChanged;

        public OrderProductCommand(ViewModel_Categories _viewModel_Categories) {
            this._viewModel_Categories = _viewModel_Categories;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            //MessageBox.Show(parameter.ToString());
            _viewModel_Categories.AddProductOrder(parameter as Product);

        }
    }

    public class ReturnCommand : ICommand {
        private ViewModel_Categories _viewModel_Categories;
        public event EventHandler CanExecuteChanged;

        public ReturnCommand(ViewModel_Categories _viewModel_Categories) {
            this._viewModel_Categories = _viewModel_Categories;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            //MessageBox.Show(parameter.ToString());
            _viewModel_Categories.Return(parameter as Window);

        }
    }

    public class OneMoreCommand : ICommand {
        private ViewModel_Categories _viewModel_Categories;

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }

        public OneMoreCommand(ViewModel_Categories _viewModel_Categories) {
            this._viewModel_Categories = _viewModel_Categories;
        }

        public bool CanExecute(object parameter) {
            return (parameter != null);
        }

        public void Execute(object parameter) {
            _viewModel_Categories.OneMore(parameter as Product);

        }
    }

    public class OneLessCommand : ICommand {
        private ViewModel_Categories _viewModel_Categories;

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }

        public OneLessCommand(ViewModel_Categories _viewModel_Categories) {
            this._viewModel_Categories = _viewModel_Categories;
        }

        public bool CanExecute(object parameter) {
            return (parameter != null && (parameter as Product).Quantity > 1);
        }

        public void Execute(object parameter) {
            _viewModel_Categories.OneLess(parameter as Product);

        }
    }

    public class DeleteOrderProductCommand : ICommand {
        private ViewModel_Categories _viewModel_Categories;

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }

        public DeleteOrderProductCommand(ViewModel_Categories _viewModel_Categories) {
            this._viewModel_Categories = _viewModel_Categories;
        }

        public bool CanExecute(object parameter) {
            return (parameter != null);
        }

        public void Execute(object parameter) {
            _viewModel_Categories.DeleteOrderProduct(parameter as Product);

        }
    }

    public class NextProductPageCommand : ICommand {
        private ViewModel_Categories _viewModel_Categories;

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }

        public NextProductPageCommand(ViewModel_Categories _viewModel_Categories) {
            this._viewModel_Categories = _viewModel_Categories;
        }

        public bool CanExecute(object parameter) {
            return (_viewModel_Categories.CurrentPage < _viewModel_Categories.CurrentMaxPage);
        }

        public void Execute(object parameter) {
            _viewModel_Categories.NextProductPage();

        }
    }
    public class PrevProductPageCommand : ICommand {
        private ViewModel_Categories _viewModel_Categories;

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }

        public PrevProductPageCommand(ViewModel_Categories _viewModel_Categories) {
            this._viewModel_Categories = _viewModel_Categories;
        }

        public bool CanExecute(object parameter) {
            return (_viewModel_Categories.CurrentPage > 1);
        }

        public void Execute(object parameter) {
            _viewModel_Categories.PrevProductPage();

        }
    }
}
