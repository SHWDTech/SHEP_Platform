namespace SHEP_Platform.Common
{
    public class WdContext
    {
        public WdContext Current { get; private set; }

        public T_Users User { get; set; }

        public T_Country Country { get; set; }

        public string UserId { get; set; }
        public WdContext()
        {
            Current = this;
        }
    }
}