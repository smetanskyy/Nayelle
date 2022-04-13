using Nayelle.Data.Entities;
using Nayelle.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Nayelle.Services
{
    public class AccountService
    {
        public UserLogin User { get; set; }
        public static readonly string FilePath = "/app_data/content/Users.json";
        public AccountService()
        {
            Refresh();
            if (string.IsNullOrWhiteSpace(User.Guid))
            {
                AddDefault();
            }
        }
        public void Refresh()
        {
            //add encoding in order to set correct font on the data
            if (!SystemFile.Exists(FilePath))
            {
                SystemFile.Update(FilePath, "");
            }

            string json = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(FilePath));
            JavaScriptSerializer jss = new JavaScriptSerializer();
            User = jss.Deserialize<UserLogin>(json);
            User = User ?? new UserLogin();
        }

        public void AddDefault()
        {
            var login = new UserLogin()
            {
                Guid = Guid.NewGuid().ToString("D"),
                Username = "admin",
                Password = "@Tanya2022"
            };

            SaveJson(login);
        }

        public void SaveJson(UserLogin login)
        {
            //Create new file with json information
            var json = JsonConvert.SerializeObject(login);
            System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath(FilePath), json);
        }

        public string GetGuidByCredentials(string username, string password)
        {
            if (User.Username == username && User.Password == password)
            {
                return User.Guid;
            }
            return string.Empty;
        }

        public bool IsGuidValid(string guid)
        {
            return User.Guid == guid;
        }
    }
}