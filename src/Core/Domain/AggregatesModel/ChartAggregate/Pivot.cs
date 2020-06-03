namespace Domain.AggregatesModel.ChartAggregate
{
    public class Pivot
    {
        public double S3 { get; protected set; }
        public double S2 { get; protected set; }
        public double S1 { get; protected set; }
        public double P { get; protected set; }
        public double R1 { get; protected set; }
        public double R2 { get; protected set; }

        public double R3 { get; protected set; }

        public Pivot(double s3, double s2, double s1, double p, double r1, double r2, double r3)
        {
            S3 = s3;
            S2 = s2;
            S1 = s1;
            P = p;
            R1 = r1;
            R2 = r2;
            R3 = r3;
        }
    }
}
