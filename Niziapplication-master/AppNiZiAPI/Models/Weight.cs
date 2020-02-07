namespace AppNiZiAPI.Models
{
    class Weight
    {
        public Weight(float amount, WeightUnit unit)
        {
            this.amount = amount;
            this.unit = unit;
        }

        public float amount { get; set; }
        public WeightUnit unit { get; set; }
        
    }
}
