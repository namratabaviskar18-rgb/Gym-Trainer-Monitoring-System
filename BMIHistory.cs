namespace GymTrainerSystem.Models
{
    public class BMIHistory
    {
        public int HistoryID { get; set; }
        public int MemberID { get; set; }
        public decimal WeightKg { get; set; }
        public decimal BMI { get; set; }
        public DateTime RecordDate { get; set; }
        public string Notes { get; set; }
    }
}
