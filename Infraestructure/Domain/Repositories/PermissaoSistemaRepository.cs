using Domain.Entities;
using Domain.IRepositories;
using Domain.UnitOfWork;

namespace Infraestructure.Domain.Repositories
{
    public partial class PermissaoSistemaRepository : Repository<PermissaoSistema>, IPermissaoSistemaRepository
    {
        public PermissaoSistemaRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
