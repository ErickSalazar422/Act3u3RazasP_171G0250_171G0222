﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PerrosDelMundo.Models;

namespace PerrosDelMundo.Repositories
{
    public class PaisesRepository : Repository<Paises>
    {
        public PaisesRepository(sistem14_razasContext context) : base(context)
        {

        }
        public override IEnumerable<Paises> GetAll()
        {
            return Context.Paises.OrderBy(x => x.Nombre);
        }
    }
}
