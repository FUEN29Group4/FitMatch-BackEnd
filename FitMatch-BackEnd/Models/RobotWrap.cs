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



        public int RobotId
        {
            get { return _Robot.RobotId; }
            set { _Robot.RobotId = value; }
        }



        public string? Type
        {
            get { return _Robot.Type; }
            set { _Robot.Type = value; }
        }


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
