namespace AireBugTracker.Exceptions
{
    public class ObjectNotFoundException : Exception
    {
        public String ObjectName { get; }
        public ObjectNotFoundException(String objectName) : base()
        {
            ObjectName = objectName;
        }
    }
}
