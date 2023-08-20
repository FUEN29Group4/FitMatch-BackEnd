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
        public int FId
        {
            get { return _classtype.ClassTypeId; }
            set { _classtype.ClassTypeId = value; }
        }

        public string? FName
        {
            get { return _classtype.ClassName; }
            set { _classtype.ClassName = value; }
        }

        public string? FIntroduction
        {
            get { return _classtype.Introduction; }
            set { _classtype.Introduction = value; }
        }

    }
}
