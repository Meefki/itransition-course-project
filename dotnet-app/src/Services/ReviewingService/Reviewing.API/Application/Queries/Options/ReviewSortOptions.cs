namespace Reviewing.API.Application.Queries.Options;

public enum SortFields
{
    Name,
    Likes,
    PublishedDate,
    SubjectName,
    SubjectGroup,
    SubjectGrade
}

public enum SortTypes
{
    asc,
    desc
}

public class ReviewSortOptions
{
    public SortFields SortField { get; set; }
    public SortTypes SortType { get; set; }

    public static SortFields MapStringToSortField(string sortField)
    {
        return sortField switch
        {
            "name" => SortFields.Name,
            "likes" => SortFields.Likes,
            "publisheddate" => SortFields.PublishedDate,
            "subjectname" => SortFields.SubjectName,
            "subjectgroup" => SortFields.SubjectGroup,
            "subjectgrade" => SortFields.SubjectGrade,
            _ => SortFields.Name
        };
    }

    public static SortTypes MapStringToSortType(string sortType)
    {
        return sortType switch
        {
            "asc" => SortTypes.asc,
            "desc" => SortTypes.desc,
            _ => SortTypes.asc
        };
    }

    public static string GetTableFieldName(SortFields sortField)
    {
        return sortField switch
        {
            SortFields.Name => "[Name]",
            SortFields.Likes => "likesCount",
            SortFields.PublishedDate => "PublishedDate",
            SortFields.SubjectName => "Subject_Name",
            SortFields.SubjectGroup => "[Name]",
            SortFields.SubjectGrade => "Subject_Grade",
            _ => "[Name]"
        };
    }
}
    