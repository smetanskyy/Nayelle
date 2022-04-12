using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nayelle.Models
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

    public class LoginVM
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class SilkSerum
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public string Ingredient { get; set; }
        public string Property { get; set; }
        public string Picture { get; set; }
    }
}