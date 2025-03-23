using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Otimizacao
{
    public abstract class QueryStringsParameters
    {
        const int maxPgaeSize = 50;

        public int PageNumber { get; set; }
        private int _pageSize = maxPgaeSize;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPgaeSize) ? maxPgaeSize : value;
            }
        }
    }
}
