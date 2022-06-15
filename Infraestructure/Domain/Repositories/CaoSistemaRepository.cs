using Domain.Entities;
using Domain.IRepositories;
using Domain.UnitOfWork;

namespace Infraestructure.Domain.Repositories
{
    public partial class CaoSistemaRepository : Repository<CaoSistema>, ICaoSistemaRepository
    {
        public CaoSistemaRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
