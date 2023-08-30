using System;
using System.Collections.Generic;
using System.ComponentModel;
using FitMatch_BackEnd.Models;


namespace FitMatch_BackEnd.ViewModel
{
    public class CListViewModel
    {
        public CUserViewModel User { get; set; }

        public MatchViewModel match { get; set; }
        public OrderViewModel order { get; set; }
        public CRestaurantWrap RestaurantWrap { get; set; }
        public IEnumerable<Restaurant> RestaurantDatas { get; set; }
        public IEnumerable<Member> MemberDatas { get; set; }
        public IEnumerable<ClassType> ClassTypeDatas { get; set; }
        public IEnumerable<Gym> GymDatas { get; set; }
        public IEnumerable<CustomerService> CustomerServiceDatas { get; set; }
        public IEnumerable<Article> ArticleDatas { get; set; }
        public IEnumerable<Order> OrderDatas { get; set; }
        public IEnumerable<Trainer> TrainerDatas { get; set; }



    }
}
