using System.IO;

namespace MVCreator
{
    public static class Constants
    {
        public static string BaseFolder = "Base";
        public static string Models = "Models";
        public static string Controllers = "Controllers";
        public static string Services = "Services";
        public static string Repositories = "Repositories";
        public static string BInterfaces = Path.Combine(BaseFolder, "Interfaces");
        public static string DtoModels = Path.Combine(Models, "Dto");
        public static string CInterfaces = Path.Combine(Controllers, "Interfaces");
        public static string SInterfaces = Path.Combine(Services, "Interfaces");
        public static string RInterfaces = Path.Combine(Repositories, "Interfaces");
    }
}
