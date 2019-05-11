namespace SoftwareTechnikProjekt
{
    internal class CollegeModule
    {
        public string Title { get; set; }
        public int ID { get; set; }
        public bool IsFinished { get; set; }
        public double Grade { get; set; }
        public bool IsVisible { get; set; }
        public bool IsCompulsory { get; set; }
        public string FurtherInfo { get; set; }
        public CollegeModule[] DependandModules { get; set; }
        public string ExamType { get; set; }
    }
}