namespace FitMatch_BackEnd.Models
{
    public class CProductWrap
    {
        private Product _Product;
        public Product Product
        {
            get { return _Product; }
            set { _Product = value; }
        }
        public CProductWrap()
        {
            _Product = new Product();
        }
        //public int ProductId
        //{
        //    get { return _Product.ProductId; }
        //    set { _Product.ProductId = value; }
        //}

        public string? ProductName
        {
            get { return _Product.ProductName; }
            set { _Product.ProductName = value; }
        }
        public string? ProductDescription
        {
            get { return _Product.ProductDescription; }
            set { _Product.ProductDescription = value; }
        }
        public string? Photo
        {
            get { return _Product.Photo; }
            set { _Product.Photo = value; }
        }

        public int? Price
        {
            get { return _Product.Price; }
            set { _Product.Price = value; }
        }
        public int? TypeId
        {
            get { return _Product.TypeId; }
            set { _Product.TypeId = value; }
        }
        public bool? Approved
        {
            get { return _Product.Approved; }
            set { _Product.Approved = value; }
        }

        public int? ProductInventory
        {
            get { return _Product.ProductInventory; }
            set { _Product.ProductInventory = value; }
        }
        public IFormFile photo { get; set; }
        public int ProductId { get; internal set; }
    }
}
