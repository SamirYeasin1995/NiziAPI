using System;

namespace AppNiZiAPI.Models
{
    class WaterConsumptionModel
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public int PatientId { get; set; }
    }

    class WaterConsumptionViewModel : WaterConsumptionModel
    {
        public WeightUnitModel WeightUnit { get; set; }
        public bool Error { get; set; }
    }
}
