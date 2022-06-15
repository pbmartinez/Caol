using Domain.Entities;
using Domain.IRepositories;
using Domain.UnitOfWork;

namespace Infraestructure.Domain.Repositories
{
    public partial class CaoClienteRepository : Repository<CaoCliente>, ICaoClienteRepository
    {
        public CaoClienteRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
