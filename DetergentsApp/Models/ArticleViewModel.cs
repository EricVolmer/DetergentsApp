using System.Collections.Generic;

namespace DetergentsApp.Models
{
    public class ArticleViewModel
    {
        public string articleId { get; set; }
        public string articleTextReceipt { get; set; }
        public string price { get; set; }

        public string vatCode { get; set; }

        public string internalArticleId { get; set; }

        public string articleGroupId { get; set; }

        public string articleGroupDescription { get; set; }
        public string linkedArticle { get; set; }
    }

    public class ArticleData
    {
        public List<ArticleViewModel> articles { get; set; }
    }
}