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
        public int FId
        {
            get { return _restaurant.RestaurantsId; }
            set { _restaurant.RestaurantsId = value; }
        }

        public string? FName
        {
            get { return _restaurant.RestaurantsName; }
            set { _restaurant.RestaurantsName = value; }
        }

        public string? FPhone
        {
            get { return _restaurant.Phone; }
            set { _restaurant.Phone = value; }
        }
        public string? FAddress
        {
            get { return _restaurant.Address; }
            set { _restaurant.Address = value; }
        }
        public string? FPhoto
        {
            get { return _restaurant.Photo; }
            set { _restaurant.Photo = value; }
        }
        public string? FRestaurantsDescription
        {
            get { return _restaurant.RestaurantsDescription; }
            set { _restaurant.RestaurantsDescription = value; }
        }

        public bool? FStatus
        {
            get { return _restaurant.Status; }
            set { _restaurant.Status = value; }
        }
    }
}
