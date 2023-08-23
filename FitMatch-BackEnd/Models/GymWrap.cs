using FitMatch_BackEnd.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace FitMatch_BackEnd.Models
{
    public class GymWrap
    {
        private Gym _gym;
        public Gym gym
        {
            get { return _gym; }
            set { _gym = value; }
        }

        public GymWrap()
        {
            _gym = new Gym();
        }

        public int GymId
        {
            get { return _gym.GymId; }
            set { _gym.GymId = value; }
        }

        public string GymName
        {
            get { return _gym.GymName; }
            set { _gym.GymName = value; }
        }

        public string Address
        {
            get { return _gym.Address; }
            set { _gym.Address = value; }
        }

        public string Phone
        {
            get { return _gym.Phone; }
            set { _gym.Phone = value; }
        }

        public DateTime? OpentimeStart
        {
            get { return _gym.OpentimeStart; }
            set { _gym.OpentimeStart = value; }
        }

        public DateTime? OpentimeEnd
        {
            get { return _gym.OpentimeStart; }
            set { _gym.OpentimeStart = value; }
        }

        public bool? Approved
        {
            get { return gym.Approved; }
            set { gym.Approved = value; }
        }
        public IFormFile Photo { get; set; }
    }
}
