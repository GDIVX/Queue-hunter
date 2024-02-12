namespace Tzipory.Systems.SaveLoadSystem
{
    /// <summary>
    /// A tag that is used to catalog which class should be saved
    /// </summary>
    public interface ISave
    {
        public string ObjectName { get; }
    }
}