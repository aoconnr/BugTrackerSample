namespace AireBugTracker.Controller
{
    public class CreateBugRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssignedUserId { get; set; }
    }
}
