using SoftwareTechnikProjekt.Data;
using System;
using System.Windows.Forms;

namespace SoftwareTechnikProjekt
{
    public partial class GradePopUp : Form
    {
        private CollegeModule currentModule;
        private double _chosenGrade;
        public double ChosenGrade { get => _chosenGrade; }

        public GradePopUp()
        {
            InitializeComponent();
            
            foreach (var grade in ApplicationConstants.Grades)
            {
                GradeDropDown.Items.Add(grade.ToString());
            }
        }

        private void GradeSubmitButton_Click(object sender, EventArgs e)
        {
            if (GradeDropDown.SelectedItem == null)
            {
                var hint = MessageBox.Show("Please choose a grade.");
                return;
            }

            _chosenGrade = double.Parse(GradeDropDown.SelectedItem.ToString());

            CourseDataHandler.Instance.UpdateModuleGrade(currentModule, chosenGrade);

            this.Close();
        }

        internal void SetupAndShow(CollegeModule moduleData)
        {
            currentModule = moduleData;
            base.Show();
        }

        private void GradePopUp_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_chosenGrade == 0)
            {
                var hint = MessageBox.Show("Please submit your grade.");
                e.Cancel = true;
            }
        }
    }
}
