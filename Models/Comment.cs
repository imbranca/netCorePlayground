public class Comment : BaseEntity
{
  public int Id {get;set;}
  public int CommenterId {get;set;}
  public int CommenterType {get;set;} //Student / Instructor
  public string Text {get; set;}
  public DateTime CreatedAt {get; set;}
  public DateTime? UpdatedAt {get; set;}
  public DateTime? DeletedAt {get; set;}
}
