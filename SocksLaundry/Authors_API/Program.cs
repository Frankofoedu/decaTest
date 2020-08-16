using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Authors_API
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static readonly PageData _pagedata = new PageData();
        static async Task Main(string[] args)
        {
            //downloads data
            await DownloadData();


            #region Get Usernames
            //Console.WriteLine("List of most active authors");
            //var result = GetUsernames(40);
            //result.ForEach(x => { Console.WriteLine($"Username :{x}"); }); 
            #endregion

            #region Sorted UserNames
            //Console.WriteLine("User Names sorted by record date");
            //var result2 = GetUsernamesSortedByRecordDate(2);
            //result2.ForEach(x => { Console.WriteLine($"Username : {x}"); });
            #endregion

            #region User with highest comment count

            //Console.WriteLine("User Name with highest comment count");
            //var r3 = GetUsernameWithHighestCommentCount();
            //Console.WriteLine($"Username : {r3}");
            #endregion

        }

        public static List<string> GetUsernames(int threshold)
        {
           return _pagedata.data.Where(x => x.submission_count >= threshold).Select(x => x.username).ToList();
        }

        public static string GetUsernameWithHighestCommentCount()
        {
            return _pagedata.data.OrderByDescending(x => x.comment_count).FirstOrDefault()?.username;
        }

        public static List<string> GetUsernamesSortedByRecordDate(int threshold)
        {
           return _pagedata.data.Where(x => x.created_at >= threshold).OrderBy(x => x.created_at).Select(x => x.username).ToList();
        }

        private static async Task DownloadData()
        {
            var urlPage1 = $"https://jsonmock.hackerrank.com/api/article_users/search?page={1}";
            var urlPage2 = $"https://jsonmock.hackerrank.com/api/article_users/search?page={2}";

            var downloadtask1 = client.GetFromJsonAsync<PageData>(urlPage1);
            var downloadtask2 = client.GetFromJsonAsync<PageData>(urlPage2);


            await Task.WhenAll(downloadtask1, downloadtask2);


            _pagedata.data.AddRange(downloadtask1.Result.data);
            _pagedata.data.AddRange(downloadtask2.Result.data);
        }

        public class PageData
        {
            public string page { get; set; }
            public int per_page { get; set; }
            public int total { get; set; }
            public int total_pages { get; set; }
            public List<Datum> data { get; set; } = new List<Datum>();
        }

        public class Datum
        {
            public int id { get; set; }
            public string username { get; set; }
            public string about { get; set; }
            public int submitted { get; set; }
            public DateTime updated_at { get; set; }
            public int submission_count { get; set; }
            public int comment_count { get; set; }
            public int created_at { get; set; }
        }

    }
}
