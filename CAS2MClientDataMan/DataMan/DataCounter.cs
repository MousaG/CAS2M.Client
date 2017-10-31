using GolestanData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CAS2MClientDataMan.DataMan
{
    public class DataCounter
    {
        public long Count<TSource>(int formCode, Expression<Func<TSource, bool>> predicate)
        {

            MethodInfo method = this.GetType().GetMethod("CountF" + formCode.ToString(), BindingFlags.NonPublic | BindingFlags.Instance);
            if (method == null)
                throw new Exception("Method not found >>" + "CountF" + formCode.ToString());
            return Convert.ToInt64(method.Invoke(this, new object[] { predicate }));
        }

        private long CountF10(Expression<Func<TVNBranchDataView, bool>> predicate)
        {
            using (var tx = new AndisheDBEntities())
            {
                //    this.context.ExecuteStoreCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                tx.Database.CommandTimeout = 3600;
                //MethodInfo method = tx.TVNBranchDataViews.GetType().GetMethod("Count");
                //MethodInfo generic = method.MakeGenericMethod(tx.TVNBranchDataViews.GetType());
                //return Convert.ToInt64(generic.Invoke(this, new object[] { predicate }));
                return tx.TVNBranchDataViews.Count(predicate);

            }
        }
        private long CountF30(Expression<Func<TVNNEWForm30View, bool>> predicate)
        {
            using (var tx = new AndisheDBEntities())
            {

                tx.Database.CommandTimeout = 3600;
                return tx.TVNNEWForm30View.Count(predicate);

            }
        }
        private long CountF50(Expression<Func<TVNF50FormView, bool>> predicate)
        {
            using (var tx = new AndisheDBEntities())
            {

                tx.Database.CommandTimeout = 3600;
                return tx.TVNF50FormView.Count(predicate);

            }
        }
    }
}
