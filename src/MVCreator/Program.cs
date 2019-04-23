using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MVCreator
{
    internal class Program
    {
        private static string _projectName = "";
        private static string _fullPath = "";
        private static List<string> _lines;
        private static readonly List<string> NewLines = new List<string>();

        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                WriteProjectName();
                CreateFolderStructure();
                CreateBaseFiles();
                foreach (var s in args)
                {
                    CreateFiles(s);
                }

                AppendLines();
                RecreateProjectFile();
            }
            else
            {
                Console.WriteLine("MVC structure creator by youstinus\nProvide parameters separated by space\nWrite only objects\nExample: MVCreator.exe Customer Restaurant Product Table Waiter\n");
            }
        }

        private static void CreateFolderStructure()
        {
            Directory.CreateDirectory(Constants.BaseFolder);
            Directory.CreateDirectory(Constants.Models);
            Directory.CreateDirectory(Constants.Controllers);
            Directory.CreateDirectory(Constants.Services);
            Directory.CreateDirectory(Constants.Repositories);
            Directory.CreateDirectory(Constants.BInterfaces);
            Directory.CreateDirectory(Constants.DtoModels);
            Directory.CreateDirectory(Constants.CInterfaces);
            Directory.CreateDirectory(Constants.SInterfaces);
            Directory.CreateDirectory(Constants.RInterfaces);
        }

        private static void CreateBaseFiles()
        {
            File.AppendAllText(Path.Combine(Constants.BInterfaces, "IBaseModel.cs"), GetBaseModelInterfaceTemplate());
            File.AppendAllText(Path.Combine(Constants.BaseFolder, "BaseModel.cs"), GetBaseModelTemplate());
            File.AppendAllText(Path.Combine(Constants.BInterfaces, "IBaseDto.cs"), GetBaseModelDtoInterfaceTemplate());
            File.AppendAllText(Path.Combine(Constants.BaseFolder, "BaseDto.cs"), GetBaseModelDtoTemplate());
            File.AppendAllText(Path.Combine(Constants.BInterfaces, "IBaseController.cs"), GetBaseControllerInterfaceTemplate());
            File.AppendAllText(Path.Combine(Constants.BaseFolder, "BaseController.cs"), GetBaseControllerTemplate());
            File.AppendAllText(Path.Combine(Constants.BInterfaces, "IBaseService.cs"), GetBaseServiceInterfaceTemplate());
            File.AppendAllText(Path.Combine(Constants.BaseFolder, "BaseService.cs"), GetBaseServiceTemplate());
            File.AppendAllText(Path.Combine(Constants.BInterfaces, "IBaseRepository.cs"), GetBaseRepositoryInterfaceTemplate());
            File.AppendAllText(Path.Combine(Constants.BaseFolder, "BaseRepository.cs"), GetBaseRepositoryTemplate());

            NewLines.Add($"    <Compile Include=\"{Constants.BInterfaces}\\IBaseModel.cs\" />");
            NewLines.Add($"    <Compile Include=\"{Constants.BaseFolder}\\BaseModel.cs\" />");
            NewLines.Add($"    <Compile Include=\"{Constants.BInterfaces}\\IBaseDto.cs\" />");
            NewLines.Add($"    <Compile Include=\"{Constants.BaseFolder}\\BaseDto.cs\" />");
            NewLines.Add($"    <Compile Include=\"{Constants.BInterfaces}\\IBaseController.cs\" />");
            NewLines.Add($"    <Compile Include=\"{Constants.BaseFolder}\\BaseController.cs\" />");
            NewLines.Add($"    <Compile Include=\"{Constants.BInterfaces}\\IBaseService.cs\" />");
            NewLines.Add($"    <Compile Include=\"{Constants.BaseFolder}\\BaseService.cs\" />");
            NewLines.Add($"    <Compile Include=\"{Constants.BInterfaces}\\IBaseRepository.cs\" />");
            NewLines.Add($"    <Compile Include=\"{Constants.BaseFolder}\\BaseRepository.cs\" />");
        }

        private static void WriteProjectName()
        {
            var files = Directory.GetFiles(".").Where(x => x.EndsWith(".csproj")).ToList();
            if (!files.Any())
                return;

            var file = files.First();
            _fullPath = file;
            _lines = File.ReadAllLines(_fullPath).ToList();
            _projectName = file.Split('\\').Last();
            _projectName = _projectName.Substring(0, _projectName.Length - 6);
        }

        private static void AppendLines()
        {
            var items = _lines.Where(x => x.Contains("<Compile Include="));
            if (!items.Any())
                return;

            var item = _lines.Last(x => x.Contains("<Compile Include="));

            var index = _lines.LastIndexOf(item) + 1;
            _lines.InsertRange(index, NewLines);
        }

        private static void RecreateProjectFile()
        {
            if (string.IsNullOrWhiteSpace(_fullPath))
                return;

            using (var writer = new StreamWriter(_fullPath))
            {
                foreach (var line in _lines)
                {
                    writer.WriteLine(line);
                }
            }
        }

        private static void CreateFiles(string s)
        {
            File.AppendAllText(Path.Combine(Constants.Models, s + ".cs"), GetModelTemplate(s));
            File.AppendAllText(Path.Combine(Constants.DtoModels, s + "Dto.cs"), GetModelDtoTemplate(s));
            File.AppendAllText(Path.Combine(Constants.CInterfaces, "I" + s + "sController.cs"), GetControllerInterfaceTemplate(s));
            File.AppendAllText(Path.Combine(Constants.Controllers, s + "sController.cs"), GetControllerTemplate(s));
            File.AppendAllText(Path.Combine(Constants.SInterfaces, "I" + s + "sService.cs"), GetServiceInterfaceTemplate(s));
            File.AppendAllText(Path.Combine(Constants.Services, s + "sService.cs"), GetServiceTemplate(s));
            File.AppendAllText(Path.Combine(Constants.RInterfaces, "I" + s + "sRepository.cs"), GetRepositoryInterfaceTemplate(s));
            File.AppendAllText(Path.Combine(Constants.Repositories, s + "sRepository.cs"), GetRepositoryTemplate(s));

            NewLines.Add($"    <Compile Include=\"{Constants.Models}\\{s}.cs\" />");
            NewLines.Add($"    <Compile Include=\"{Constants.DtoModels}\\{s}Dto.cs\" />");
            NewLines.Add($"    <Compile Include=\"{Constants.CInterfaces}\\I{s}sController.cs\" />");
            NewLines.Add($"    <Compile Include=\"{Constants.Controllers}\\{s}sController.cs\" />");
            NewLines.Add($"    <Compile Include=\"{Constants.SInterfaces}\\I{s}sService.cs\" />");
            NewLines.Add($"    <Compile Include=\"{Constants.Services}\\{s}sService.cs\" />");
            NewLines.Add($"    <Compile Include=\"{Constants.RInterfaces}\\I{s}sRepository.cs\" />");
            NewLines.Add($"    <Compile Include=\"{Constants.Repositories}\\{s}sRepository.cs\" />");
        }

        private static string GetModelTemplate(string s)
        {
            return $"using {_projectName}Base;\r\n\r\nnamespace {_projectName}Models\r\n{{\r\n    public class {s} : BaseEntity\r\n    {{\r\n        \r\n    }}\r\n}}";
        }

        private static string GetModelDtoTemplate(string s)
        {
            return $"using {_projectName}Base;\r\n\r\nnamespace {_projectName}Models.Dto\r\n{{\r\n    public class {s}Dto : BaseDto\r\n    {{\r\n\r\n    }}\r\n}}";
        }

        private static string GetBaseModelTemplate()
        {
            return $"using {_projectName}Base.Interfaces;\r\n\r\nnamespace { _projectName}Base\r\n{{\r\n    public class BaseEntity : IBaseEntity\r\n    {{\r\n    }}\r\n}}";
        }

        private static string GetBaseModelDtoTemplate()
        {
            return $"using {_projectName}Base.Interfaces;\r\n\r\nnamespace { _projectName}Base\r\n{{\r\n    public class BaseDto : IBaseDto\r\n    {{\r\n    }}\r\n}}";
        }

        private static string GetBaseModelInterfaceTemplate()
        {
            return $"namespace {_projectName}Base.Interfaces\r\n{{\r\n    public interface IBaseEntity\r\n    {{\r\n    }}\r\n}}";
        }

        private static string GetBaseModelDtoInterfaceTemplate()
        {
            return $"namespace {_projectName}Base.Interfaces\r\n{{\r\n    public interface IBaseDto\r\n    {{\r\n    }}\r\n}}";
        }

        private static string GetControllerInterfaceTemplate(string s)
        {
            return $"using {_projectName}Base.Interfaces;\r\nusing {_projectName}Models;\r\nusing {_projectName}Models.Dto;\r\n\r\nnamespace {_projectName}Controllers.Interfaces\r\n{{\r\n    public interface I{s}sController : IBaseController<{s}, {s}Dto>\r\n    {{\r\n    }}\r\n}}";
        }

        private static string GetControllerTemplate(string s)
        {
            return $"using {_projectName}Base;\r\nusing {_projectName}Models;\r\nusing {_projectName}Models.Dto;\r\nusing {_projectName}Controllers.Interfaces;\r\n\r\nnamespace {_projectName}Controllers\r\n{{\r\n    public class {s}sController : BaseController<{s}, {s}Dto>, I{s}sController\r\n    {{\r\n    }}\r\n}}\r\n";
        }

        private static string GetServiceInterfaceTemplate(string s)
        {
            return $"using {_projectName}Base.Interfaces;\r\nusing {_projectName}Models;\r\nusing {_projectName}Models.Dto;\r\n\r\nnamespace {_projectName}Services.Interfaces\r\n{{\r\n    public interface I{s}sService : IBaseService<{s}, {s}Dto>\r\n    {{\r\n    }}\r\n}}";
        }

        private static string GetServiceTemplate(string s)
        {
            return $"using {_projectName}Base;\r\nusing {_projectName}Models;\r\nusing {_projectName}Models.Dto;\r\nusing {_projectName}Services.Interfaces;\r\n\r\nnamespace {_projectName}Services\r\n{{\r\n    public class {s}sService : BaseService<{s}, {s}Dto>, I{s}sService\r\n    {{\r\n    }}\r\n}}\r\n";
        }

        private static string GetRepositoryInterfaceTemplate(string s)
        {
            return $"using {_projectName}Base.Interfaces;\r\nusing {_projectName}Models;\r\n\r\nnamespace {_projectName}Repositories.Interfaces\r\n{{\r\n    public interface I{s}sRepository : IBaseRepository<{s}>\r\n    {{\r\n    }}\r\n}}";
        }

        private static string GetRepositoryTemplate(string s)
        {
            return $"using {_projectName}Base;\r\nusing {_projectName}Models;\r\nusing {_projectName}Repositories.Interfaces;\r\n\r\nnamespace {_projectName}Repositories\r\n{{\r\n    public class {s}sRepository : BaseRepository<{s}>, I{s}sRepository\r\n    {{\r\n    }}\r\n}}\r\n";
        }

        private static string GetBaseServiceInterfaceTemplate()
        {
            return $"namespace {_projectName}Base.Interfaces\r\n{{\r\n    public interface IBaseService<T, TDto> where T : class, IBaseEntity where TDto : class, IBaseDto\r\n    {{\r\n    }}\r\n}}";
        }

        private static string GetBaseServiceTemplate()
        {
            return $"using {_projectName}Base.Interfaces;\r\n\r\nnamespace {_projectName}Base\r\n{{\r\n    public class BaseService<T, TDto> : IBaseService<T, TDto> where T : class, IBaseEntity where TDto : class, IBaseDto\r\n    {{\r\n    }}\r\n}}\r\n";
        }

        private static string GetBaseRepositoryInterfaceTemplate()
        {
            return $"namespace {_projectName}Base.Interfaces\r\n{{\r\n    public interface IBaseRepository<T> where T : class, IBaseEntity\r\n    {{\r\n    }}\r\n}}";
        }

        private static string GetBaseRepositoryTemplate()
        {
            return $"using {_projectName}Base.Interfaces;\r\n\r\nnamespace {_projectName}Base\r\n{{\r\n    public class BaseRepository<T> : IBaseRepository<T> where T : class, IBaseEntity\r\n    {{\r\n    }}\r\n}}\r\n";
        }

        private static string GetBaseControllerInterfaceTemplate()
        {
            return $"namespace {_projectName}Base.Interfaces\r\n{{\r\n    public interface IBaseController<T, TDto> where T : class, IBaseEntity where TDto : class, IBaseDto\r\n    {{\r\n    }}\r\n}}";
        }

        private static string GetBaseControllerTemplate()
        {
            return $"using {_projectName}Base.Interfaces;\r\nusing Microsoft.AspNetCore.Mvc;\r\n\r\nnamespace {_projectName}Base\r\n{{\r\n    public class BaseController<T, TDto> : ControllerBase, IBaseController<T, TDto> where T : class, IBaseEntity where TDto : class, IBaseDto\r\n    {{\r\n    }}\r\n}}\r\n";
        }
    }
}
