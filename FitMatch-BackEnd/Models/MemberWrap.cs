using FitMatch_BackEnd.Models;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitMatch_BackEnd.Controllers
{
    public class MemberWarap
    {
        private Member _Member;
        public Member Member
        {
            get { return _Member; }
            set { _Member = value; }
        }

        public MemberWarap()
        {
            _Member = new Member();
        }
        public int MemberId
        {
            get { return _Member.MemberId; }
            set { _Member.MemberId = value; }
        }


        [Required(ErrorMessage = "姓名必填")]
        public string? MemberName
        {
            get { return _Member.MemberName; }
            set { _Member.MemberName = value; }
        }

        public bool? Gender
        {
            get { return _Member.Gender; }
            set { _Member.Gender = value; }
        }

        public DateTime? Birth
        {
            get { return _Member.Birth; }
            set { _Member.Birth = value; }
        }

        public string? Phone
        {
            get { return _Member.Phone; }
            set { _Member.Phone = value; }
        }

        public string? Address
        {
            get { return _Member.Address; }
            set { _Member.Address = value; }
        }

        public string? Email
        {
            get { return _Member.Email; }
            set { _Member.Email = value; }
        }

       

        public string? Password
        {
            get { return _Member.Password; }
            set { _Member.Password = value; }
        }

        public DateTime? CreatedAt
        {
            get { return _Member.CreatedAt; }
            set { _Member.CreatedAt = value; }
        }
        public bool? Status
        {
            get { return _Member.Status; }
            set { _Member.Status = value; }
        }
        public string? Photo
        {
            get { return _Member.Photo; }
            set { _Member.Photo = value; }
        }
        public IFormFile photo { get; set; }
    }
}
