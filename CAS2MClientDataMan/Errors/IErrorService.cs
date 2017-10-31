//using CAS2MClientDataMan.Domain;
//using CAS2MClientDataMan.Enums;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CAS2MClientDataMan.Errors
//{
//    public interface IErrorService
//    {
//        Task<ErrorOutput> GetAsync(string id);
//        ErrorOutput Get(string id);
//        void Insert(ErrorInput input);
//        Task InsertAsync(ErrorInput input);
//        void Insert(string taskId, ErrorType errorType, string title, string description);
//        Task InsertAsync(string taskId, ErrorType errorType, string title, string description);
//        IEnumerable<ErrorOutput> Search(int page, int recordsPerPage, string term, string taskId, ErrorType? errorType, ErrorSortBy sortBy, SortOrder sortOrder, out int pageSize, out int totalItemCount);
//        Task DeleteManyAsync(string taskId);
//        Task InsertManyAsync(Guid taskToken, IEnumerable<ErrorInput> inputs);
//    }
//}
