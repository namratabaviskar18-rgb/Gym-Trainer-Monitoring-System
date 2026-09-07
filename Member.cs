namespace GymTrainerSystem.Models
{
    public class Member
    {
        public int MemberID { get; set; }
        public int UserID { get; set; }
        public decimal HeightCm { get; set; }
        public decimal CurrentWeightKg { get; set; }
        public decimal TargetWeightKg { get; set; }
        public string GoalType { get; set; }      // WeightGain / WeightLoss
        public string Gender { get; set; }
        public int Age { get; set; }
        public DateTime JoinDate { get; set; }
        public bool IsActive { get; set; }

        // Calculated property
        public decimal BMI
        {
            get
            {
                if (HeightCm <= 0) return 0;
                decimal heightM = HeightCm / 100;
                return Math.Round(CurrentWeightKg / (heightM * heightM), 2);
            }
        }

        public string BMICategory
        {
            get
            {
                decimal bmi = BMI;
                if (bmi < 18.5m) return "Underweight";
                if (bmi < 25m) return "Normal";
                if (bmi < 30m) return "Overweight";
                return "Obese";
            }
        }

        public decimal WeightToChange
        {
            get { return Math.Abs(TargetWeightKg - CurrentWeightKg); }
        }
    }
}
