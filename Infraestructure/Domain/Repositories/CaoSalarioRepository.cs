using Domain.Entities;
using Domain.IRepositories;
using Domain.UnitOfWork;

namespace Infraestructure.Domain.Repositories
{
    public partial class CaoSalarioRepository : Repository<CaoSalario>, ICaoSalarioRepository
    {
        public CaoSalarioRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
