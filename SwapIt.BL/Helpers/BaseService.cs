using AutoMapper;
using SwapIt.Data.IPersistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.Helpers
{
    public class BaseService
    {
        #region Fields

        public IMapper _mapper;
        public IUnitOfWork _unitOfWork;

        #endregion

        #region Ctor

        public BaseService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        #endregion

        #region Methods

        public void SaveChanges()
        {
            _unitOfWork.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return _unitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
