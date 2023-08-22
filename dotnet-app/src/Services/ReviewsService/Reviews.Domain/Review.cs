namespace Reviews.Domain;

public class Review
{
    public Review(ReviewId id)
    {
        Id = id;
    }

    public ReviewId Id { get; }
    public string Name { get; private set; } // TODO: VO


}