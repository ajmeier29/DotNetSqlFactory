using AutoMapper;
using AutoMapper.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSqlFactory.DataOperations
{
    public class DTOMapper <T>
    {
        private T _value;
        private MapperConfiguration _mapperConfig;
        private IMapper _mapper;
        public DTOMapper(T value)
        {
            _value = value;
            CreateMapping();
        }
        /// <summary>
        /// Create the config and dynamic mapping for the DTO 
        /// </summary>
        private void CreateMapping()
        {
            _mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddDataReaderMapping(true);
                cfg.CreateMap<IDataRecord, T>();
            });
            _mapper = _mapperConfig.CreateMapper();
        }
        /// <summary>
        /// Returns a List<typeparamref name="T"/> from the IDataReader passed into this method. 
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public List<T> IDataReaderToDtoList(IDataReader dataReader)
        {
            return _mapper.Map<IDataReader, IEnumerable<T>>(dataReader).ToList();
        }
    }
}
