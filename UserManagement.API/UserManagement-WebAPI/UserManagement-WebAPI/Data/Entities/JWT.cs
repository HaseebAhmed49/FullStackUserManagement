using System;
namespace UserManagement_WebAPI.Data.Entities
{
    public class JWT
    {
        public string Key { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}

