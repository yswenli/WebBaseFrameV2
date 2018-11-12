namespace WebBaseFrame.Models
{
    public partial class Article
    {
        ArticleKind _ArticleKind = new ArticleKind();

        public ArticleKind ArticleKind
        {
            get
            {
                if (this.KID > 0 && (_ArticleKind == null || _ArticleKind.ID == 0))
                {
                    _ArticleKind = new ArticleKindRepository().Search().Where(b => b.ID == KID).First();
                }
                if (_ArticleKind == null) _ArticleKind = new ArticleKind();
                return _ArticleKind;
            }
            set { _ArticleKind = value; }
        }
    }
}
