namespace FitMatch_BackEnd.Models
{
    public class CProductWrap
    {
        private Product _product;
        public Product product
        {
            get { return _product; }
            set { _product = value; }
        }
        public CProductWrap()
        {
            _product = new Product();
        }
        public int ProductId
        {
            get { return _product.ProductId; }
            set { _product.ProductId = value; }
        }

        public string? ProductName
        {
            get { return _product.ProductName; }
            set { _product.ProductName = value; }
        }
        public string? ProductDescription
        {
            get { return _product.ProductDescription; }
            set { _product.ProductDescription = value; }
        }
        public string? Photo
        {
            get { return _product.Photo; }
            set { _product.Photo = value; }
        }

        public int? Price
        {
            get { return _product.Price; }
            set { _product.Price = value; }
        }
        public int? TypeId
        {
            get { return _product.TypeId; }
            set { _product.TypeId = value; }
        }
        public bool? Approved
        {
            get { return _product.Approved; }
            set { _product.Approved = value; }
        }

        public int? ProductInventory
        {
            get { return _product.ProductInventory; }
            set { _product.ProductInventory = value; }
        }

        public bool? Status
        {
            get { return _product.Status; }
            set { _product.Status = value; }
        }
  
        public IFormFile photo { get; set; }
        //    public int ProductId { get; internal set; }
    }
}
