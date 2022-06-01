using Domain.Entities;
using Domain.IRepositories;
using Domain.UnitOfWork;

namespace Infraestructure.Domain.Repositories
{
    public partial class CaoUsuarioRepository : Repository<CaoUsuario>, ICaoUsuarioRepository
    {
        public CaoUsuarioRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
