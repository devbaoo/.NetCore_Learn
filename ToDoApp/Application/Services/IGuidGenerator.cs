namespace TodoApp.Application.Services
{
    public interface IGuidGenerator
    {
        Guid Generate();
    }

    public class GuidGenerator : IGuidGenerator
    {
        private readonly Guid guid;

        public GuidGenerator()
        {
            guid = Guid.NewGuid();
        }

        public Guid Generate()
        {
            return guid;
        }
    }
}