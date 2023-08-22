namespace FitMatch_BackEnd.Models
{
    public class RobotWrap
    {
        private Robot _Robot;
        public Robot Robot
        {
            get { return _Robot; }
            set { _Robot = value; }
        }
        public RobotWrap()
        {
            _Robot = new Robot();
        }
        //public int ProductId
        //{
        //    get { return _Product.ProductId; }
        //    set { _Product.ProductId = value; }
        //}

        public int RobotId { get; internal set; }

        public string? DefaultQuestion
        {
            get { return _Robot.DefaultQuestion; }
            set { _Robot.DefaultQuestion = value; }
        }
        public string? DefaultResponse
        {
            get { return _Robot.DefaultResponse; }
            set { _Robot.DefaultResponse = value; }
        }

       
        
    }
}
