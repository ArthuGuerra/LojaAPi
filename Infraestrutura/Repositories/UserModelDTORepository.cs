using Domain.Entities;
using Domain.Interfaces;
using Infraestrutura.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Repositories
{
    public class UserModelDTORepository : Repository<UserModelDTO>, IUserModelDTO
    {
        public UserModelDTORepository(LojinhaContext context) : base(context)
        {
        }
    }
}
