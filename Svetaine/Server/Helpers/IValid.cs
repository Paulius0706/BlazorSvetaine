namespace Svetaine.Server.Helpers
{
    public interface IValid
    {
        public bool Valid(out string error, object entity);
    }
}
