using MySql.Data.MySqlClient;
using ProjectTPV.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Drawing.Printing;
using System.Drawing;
using System.Reflection;
using PdfSharp.Pdf;
using System.Diagnostics;
using PdfSharp.Drawing;
using System.IO;

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
        private PrintTicketCommand _printTicketCommand;

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
            _printTicketCommand = new PrintTicketCommand(this);
            _currentPage = 1;
            InitilizeCategories();
        }

        public string CurrentPageAndMaxPage {
            get { return _currentPage.ToString() + '/' + (((_currentProductList.Count) + (LIMIT_PRODUCT_PAGE - 1)) / LIMIT_PRODUCT_PAGE).ToString(); }
        }

        protected void OnPropertyChanged(string NameProperty) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
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
        public PrintTicketCommand PrintTicketCommand {
            get { return _printTicketCommand; }
        }
        public void InitilizeCategories() {
            String serv = "server=localhost;";
            String db = "database=tpv_project;";
            String user = "uid=root;";
            String pwd = "pwd=7891;";
            String port = "port=3306;";
            String security = "persistsecurityinfo = True;";
            MySqlConnection conex = new MySqlConnection(serv + db + user + pwd + port + security);
            conex.Open();
            MySqlCommand cmd = conex.CreateCommand();
            string sql =
                "SELECT name,img_path,price,P.category_name " +
                "FROM products P,categories C " +
                "WHERE P.category_name = C.category_name " +
                "ORDER BY category_position";
            cmd.CommandText = sql;
            MySqlDataReader mdr = cmd.ExecuteReader();
            string PrevCategory = "";
            Category category = null;
            while (mdr.Read()) {
                if (!PrevCategory.Equals(mdr.GetString(3))) {
                    PrevCategory = mdr.GetString(3);
                    if(category != null) { _categories.Add(category); }
                    category = new Category(mdr.GetString(3));
                }
                category.AddProduct(new Product(mdr.GetString(0), mdr.GetString(1), mdr.GetDouble(2)));
            }
            _categories.Add(category);
            ChangeProductList(_categories.First<Category>().ProductList);
            mdr.Close();
            conex.Close();
            /*string[] FormatName = { "Cous Cous", "Shushi", "Gamba Roja", "Marmitako de Atun", "Helado de Limon" };
            string[] ImgPath = { "tellinas.png", "gamba.png", "tartar_atun.png", "finca_colina.png", "helado.png" };
            string[] Category = { "Bebidas", "Vinos y Cavas", "Entrantes", "Principales", "Postres" };
            double[] Price = { 222.55, 3, 4, 5, 2 };
            for (int i = 0; i < Category.Length; i++) {
                Category category = new Category(Category[i]);
                for (int j = 0; j < 60; j++) {
                    category.AddProduct(new Product(FormatName[i], ImgPath[i], Category[i], Price[i]));
                }
                _categories.Add(category);
            }
            ChangeProductList(_categories.First<Category>().ProductList);*/
        }

        internal void ChangeProductList(ObservableCollection<Product> list) {
            CurrentPage = 1;
            _currentProductList = list;
            _ShownProducts.Clear();
            foreach (Product product in list.Take<Product>(LIMIT_PRODUCT_PAGE)) {
                _ShownProducts.Add(product);
            }
        }

        internal void AddProductOrder(Product product) {
            if (product.Quantity == 0) {
                product.Quantity = 1;
                _orderProducts.Add(product);
            } else {
                product.Quantity += 1;
            }
            OnPropertyChanged("TotalPriceListTxt");
        }

        internal void Return(Window window) {
            window.Hide();
        }

        internal void OneMore(Product product) {
            product.Quantity++;
            OnPropertyChanged("TotalPriceListTxt");
        }

        internal void OneLess(Product product) {
            product.Quantity--;
            OnPropertyChanged("TotalPriceListTxt");
        }

        internal void DeleteOrderProduct(Product product) {
            _orderProducts.Remove(product);
            product.Quantity = 0;
            OnPropertyChanged("TotalPriceListTxt");
        }

        internal void NextProductPage() {
            _ShownProducts.Clear();
            foreach (Product product in _currentProductList.Skip<Product>(_currentPage * LIMIT_PRODUCT_PAGE).Take<Product>(LIMIT_PRODUCT_PAGE)) {
                _ShownProducts.Add(product);
            }
            CurrentPage++;

        }

        public double TotalPriceList {
            // obten la suma de la propiedad en todos los elementos dentro de la lista
            get { return _orderProducts.Sum<Product>(x => x.TotalPrice); }
        }

        public string TotalPriceListTxt {
            get { return TotalPriceList.ToString("C2"); }
        }

        internal void PrevProductPage() {
            _ShownProducts.Clear();
            CurrentPage--;
            foreach (Product product in _currentProductList.Skip<Product>((_currentPage - 1) * LIMIT_PRODUCT_PAGE).Take<Product>(LIMIT_PRODUCT_PAGE)) {
                _ShownProducts.Add(product);
            }

        }

        internal void PrintTicket() {
            PdfDocument Pdf = new PdfDocument();
            PdfPage PdfPage = Pdf.AddPage();
            PdfPage.Width = 300;
            PdfPage.Height = 155+(_orderProducts.Count*20)+100;
            XGraphics graph = XGraphics.FromPdfPage(PdfPage);
            XFont font = new XFont("Verdana",8, XFontStyle.Regular);
            string root = Directory.GetParent(Assembly.GetEntryAssembly().Location).Parent.Parent.FullName;
            string image = "\\Resources\\Images\\paymentLogo\\logoRestaurant.png";
            XPen pen = new XPen(XColor.FromArgb(93, 25, 22), 2);
            graph.DrawImage(XImage.FromFile(root+image), 25, 20);
            graph.DrawRectangle(pen, 5, 5, 290, 145 + (_orderProducts.Count * 20) + 100);
            pen.Width = 1;
            graph.DrawLine(pen, 10, 140, 290, 140);
            graph.DrawString("Cant.", font, XBrushes.Black, 10,150);
            graph.DrawString("Producto", font, XBrushes.Black, 35,150);
            graph.DrawString("Precio", font, XBrushes.Black, 240, 150);
            for (int i = 0; i < _orderProducts.Count; i++) {
                Product product = _orderProducts.ElementAt(i);
                graph.DrawString(product.Quantity.ToString(), font, XBrushes.Black, 15, (i * 20) + 170);
                graph.DrawString(product.Name.ToString(), font, XBrushes.Black, 35, (i * 20) + 170);
                graph.DrawString(product.TotalPriceTxt, font, XBrushes.Black, 240, (i * 20) + 170);
            }
            graph.DrawLine(pen, 10, (_orderProducts.Count*20)+155, 290, (_orderProducts.Count * 20) + 155);
            graph.DrawString("Precio total: "+TotalPriceListTxt, font, XBrushes.Black, 150, (_orderProducts.Count * 20) + 165,XStringFormats.Center);
            graph.DrawString("IVA aplicado: " + (TotalPriceList*0.21).ToString("C2"), font, XBrushes.Black, 150, (_orderProducts.Count * 20) + 180, XStringFormats.Center);
            font = new XFont("Verdana", 20, XFontStyle.Italic);
            graph.DrawString("Gracias por su visita", font, XBrushes.Black, 160, (_orderProducts.Count * 20) + 215, XStringFormats.Center);
            //graph.DrawString("This is my first PDF document", font, XBrushes.Black,new XRect(0, 130, PdfPage.Width.Point, PdfPage.Height.Point), XStringFormats.Center);
            Pdf.Save("ja.pdf");
            Process.Start("ja.pdf");

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

    public class PrintTicketCommand : ICommand {
        private ViewModel_Categories _viewModel_Categories;
        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }

        public PrintTicketCommand(ViewModel_Categories _viewModel_Categories) {
            this._viewModel_Categories = _viewModel_Categories;
        }

        public bool CanExecute(object parameter) {
            return (_viewModel_Categories.OrderProducts.Count > 0);
        }

        public void Execute(object parameter) {
            _viewModel_Categories.PrintTicket();

        }
    }
}
