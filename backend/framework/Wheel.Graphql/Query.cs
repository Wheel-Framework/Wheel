using HotChocolate.Authorization;

namespace Wheel.Graphql
{
    [Authorize]
    public class Query : IQuery
    {
    }

    [InterfaceType]
    public interface IQuery
    {

    }

    public interface IQueryExtendObjectType
    {

    }

    //[ExtendObjectType(typeof(IQuery))]
    //public class SampleQuery : IQueryExtendObjectType
    //{
    //    public List<string> Sample()
    //    {
    //        return new List<string> { "sample1", "sample2" };
    //    }
    //}
    //[ExtendObjectType(typeof(IQuery))]
    //public class Sample2Query : IQueryExtendObjectType
    //{
    //    public string Sample2(string id)
    //    {
    //        return id;
    //    }
    //}
}
