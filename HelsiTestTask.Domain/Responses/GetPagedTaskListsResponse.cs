namespace HelsiTestTask.Domain.Responses
{
    public class GetPagedTaskListsResponse
    {
        public IEnumerable<TaskListResponse> TaskLists { get; set; }

        public int TotalCount { get; set; }
    }
}
