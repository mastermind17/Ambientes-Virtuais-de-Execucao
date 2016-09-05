using System.Collections;
using System.Collections.Generic;

namespace AutoMapper
{
	public interface Mapper<TSrc, TDest>
	{
		TDest Map(TSrc src);

		TColDest Map<TColDest>(IEnumerable<TSrc> src) where TColDest : ICollection<TDest>;

        TDest[] MapToArray(IEnumerable<TSrc> src);

        IEnumerable<TDest> MapLazy(IEnumerable<TSrc> src);
	}
}