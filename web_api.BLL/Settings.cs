using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_api.BLL
{
    public static class Settings
    {
        // paths
        public static string FilesRootPath = string.Empty;
        public const string ImagesPath = "images";
        public const string ManufacturesPath = "manufactures";
        public const string CarsPath = "cars";
        public const string UsersPath = "users";
        //roles
        public const string AdminRole = "admin";
        public const string UserRole = "user";
    }
}
