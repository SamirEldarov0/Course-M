//using AutoMapper.Configuration;
using Lesson21_API_.Data.Entities;
using System.Collections.Generic;

namespace Lesson21_API_.Services
{
    public interface IJwtService
    {
        public string Generate(IList<string> roles, AppUser admin);
    }
}
