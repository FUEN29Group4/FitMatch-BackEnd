using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitMatch_BackEnd.Models
{
    public class CClassTypeWrap
    {
        private ClassType _classtype;
        public ClassType classtype
        {
            get { return _classtype; }
            set { _classtype = value; }
        }
        public CClassTypeWrap()
        {
            _classtype = new ClassType();
        }
        public int ClassTypeId
        {
            get { return _classtype.ClassTypeId; }
            set { _classtype.ClassTypeId = value; }
        }

        public string? ClassName
        {
            get { return _classtype.ClassName; }
            set { _classtype.ClassName = value; }
        }

        public string? Photo
        {
            get { return _classtype.Photo; }
            set { _classtype.Photo = value; }
        }

        public string? Introduction
        {
            get { return _classtype.Introduction; }
            set { _classtype.Introduction = value; }
        }

        public int? Status
        {
            get { return _classtype.Status; }
            set { _classtype.Status = value; }
        }

        public IFormFile photo { get; set; }
        public object FileToUpload { get; internal set; }
    }
}
