using ChatRoomsLite.Models;
using ChatRoomsLite.Models.Repositories;
using ChatRoomsLite.Models.SearchCriterias;
using Microsoft.EntityFrameworkCore;

namespace ChatRoomsLite.Infrastructure.Repositories;

public class EntityRepository<TEntity> : IEntityRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly DbSet<TEntity> DbSet;
    protected readonly DataContext Context;
    
    public EntityRepository(DataContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }
    
    public int GetNextId()
    {
        return DbSet.Any() ? DbSet.Max(e => e.Id) + 1 : 1;
    }

    public TEntity Get(int id)
    {
        return DbSet.Find(id);
    }

    public IEnumerable<TEntity> GetAll()
    {
        return DbSet.AsEnumerable();
    }

    public IEnumerable<TEntity> Find(SearchCriteria<TEntity> searchCriteria)
    {
        return DbSet.Where(searchCriteria.Predicate).AsEnumerable();
    }

    public void Add(TEntity entity)
    {
        entity.Id = GetNextId();
        DbSet.Add(entity);
        Context.SaveChanges();
    }

    public void Update(TEntity entity)
    {
        DbSet.Update(entity);
        Context.SaveChanges();
    }

    public void Delete(TEntity entity)
    {
        DbSet.Remove(entity);
        Context.SaveChanges();
    }
}