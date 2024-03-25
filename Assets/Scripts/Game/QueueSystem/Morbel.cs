namespace Scirpts
{
    public class Morbel : BaseLoadObject
    {
        public MorbleType MorbleType;

        public float TravelPros => ProgressPercentage;
        
        public Morbel(MorbaleConfig morbaleConfig,ILoader loader,int id) : base(loader,morbaleConfig.speed,id)
        {
            MorbleType = morbaleConfig.morbleType;
        }
    }
}