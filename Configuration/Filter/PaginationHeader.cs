namespace DatingApp.API.Configuration.Filter
{
    public class PaginationHeader
    {
        public PaginationHeader(int currentPage, int itemPerPage, int totalpage,int totalItem)
        {
            this.currentPage=currentPage;
            this.itemPerPage=itemPerPage;
            this.totalPage=totalpage;
            this.totalItem=totalItem;
        }

        public int currentPage { get; private set; }
        public int itemPerPage { get; private set; }
        public int totalPage { get; private set; }
        public int totalItem { get; private set; }
    }
}