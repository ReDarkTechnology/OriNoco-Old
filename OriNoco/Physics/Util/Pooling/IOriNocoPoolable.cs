namespace OriNoco
{
  public interface IOriNocoPoolable<T>
    where T : IOriNocoPoolable<T>
  {
    IOriNocoPool<T> Pool { get; set; }
    void Reset();
  }
}
