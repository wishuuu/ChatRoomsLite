using ChatRoomsLite.Models.SearchCriterias;

namespace ChatRoomsLite.Models.Repositories;

public interface IEntityRepository<TEntity>
    where TEntity : BaseEntity
{
    int GetNextId();
    TEntity Get(int id);
    IEnumerable<TEntity> GetAll();
    IEnumerable<TEntity> Find(SearchCriteria<TEntity> searchCriteria);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}