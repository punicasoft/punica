using Punica.Bp.Application.Query;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Punica.Bp.Application.EFCore.Query
{
    public class Queries<TEntity> : IQueries<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;
        private readonly IMapper _mapper;

        public Queries(DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        //protected DbSet<TEntity> Entities => _dbContext.Set<TEntity>();


        public Task<List<TEntity>> GetList(ISpecification<TEntity> query, List<string> columns)
        {
            var selectedColumns = columns.Select(column => $"Person.{column}");
            var selectExpression = string.Join(",", selectedColumns);
            //var projectionQuery = query.Select($"new {{ {selectExpression} }}");

            return _dbContext.Set<TEntity>()
                .AsNoTracking()
                .Where(query.Expression)
                .Select(a=> a)
                //.ProjectTo<TModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }


    }


    
}
