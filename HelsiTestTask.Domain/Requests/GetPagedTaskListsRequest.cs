namespace HelsiTestTask.Domain.Requests
{
    public class GetPagedTaskListsRequest
    {
        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}