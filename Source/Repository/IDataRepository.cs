using System.Threading.Tasks;

namespace MinimalApi.Repository;

public interface IDataRepository
{
   Task<PagedResults<TodoItemOutput>> GetPagedResults(ClaimsPrincipal user, int? page = 1, int? pageSize = 10);
}
