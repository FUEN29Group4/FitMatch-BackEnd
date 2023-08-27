namespace FitMatch_BackEnd.Models
{
    public class CRestaurantWrap
    {
        private Restaurant _restaurant;
        public Restaurant restaurant
        {
            get { return _restaurant; }
            set { _restaurant = value; }
        }
        public CRestaurantWrap()
        {
            _restaurant = new Restaurant();
        }
        public int RestaurantsId
        {
            get { return _restaurant.RestaurantsId; }
            set { _restaurant.RestaurantsId = value; }
        }

        public string? RestaurantsName
        {
            get { return _restaurant.RestaurantsName; }
            set { _restaurant.RestaurantsName = value; }
        }

        public string? Phone
        {
            get { return _restaurant.Phone; }
            set { _restaurant.Phone = value; }
        }
        public string? Address
        {
            get { return _restaurant.Address; }
            set { _restaurant.Address = value; }
        }
        public string? Photo
        {
            get { return _restaurant.Photo; }
            set { _restaurant.Photo = value; }
        }
        public string? RestaurantsDescription
        {
            get { return _restaurant.RestaurantsDescription; }
            set { _restaurant.RestaurantsDescription = value; }
        }
        public DateTime? CreateAt
        {
            get { return _restaurant.CreateAt; }
            set { _restaurant.CreateAt = value; }
        }
        public int? Status
        {
            get { return _restaurant.Status; }
            set { _restaurant.Status = value; }
        }

        public IFormFile photo { get; set; }
    }
}
