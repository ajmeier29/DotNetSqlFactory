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
    /// <summary>
    /// This class Maps a DataTable to a class using AutoMapper.
    /// TODO: Remove IDataReader dependancy and use Datatable for the mapping to remove uneccessary DataTable.CreateDataReader() converstion. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DTOMapper <T>
    {
        private MapperConfiguration _mapperConfig;
        private IMapper _mapper;
        public DTOMapper()
        {
            CreateMapping();
        }
        /// <summary>
        /// Create the config and dynamic mapping for the DTO 
        /// </summary>
        private void CreateMapping()
        {
            _mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddDataReaderMapping(false);
                cfg.CreateMap<IDataRecord, T>();
            });
            _mapper = _mapperConfig.CreateMapper();
        }
        /// <summary>
        /// Returns a List<typeparamref name="T"/> from the DataTable passed into this method. 
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public List<T> DataTableToDtoList(DataTable dataTable)
        {
            return _mapper.Map<IDataReader, IEnumerable<T>>(dataTable.CreateDataReader()).ToList(); ;
        }
    }
}
