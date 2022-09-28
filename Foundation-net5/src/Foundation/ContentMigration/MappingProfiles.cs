using AutoMapper;
using Foundation.Features.Blog.BlogItemPage;

namespace Foundation.ContentMigration
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<T4.Article, BlogItemPage>()
                .ForMember(x => x.Name, y => y.MapFrom(from => from.Title.Value))
                .ForMember(x => x.TeaserText, y => y.MapFrom(from => from.Summary.Value))
                .ForMember(x => x.MainBody, y => y.MapFrom(from => from.Body.Value));
              

        }
    }
}
