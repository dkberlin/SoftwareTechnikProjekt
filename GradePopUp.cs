using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftwareTechnikProjekt
{
    public partial class GradePopUp : Form
    {
        private CollegeModule currentModule;
        private List<double> grades;
        private double chosenGrade;
        public double ChosenGrade { get => chosenGrade; }

        public GradePopUp()
        {
            InitializeComponent();
            grades = new List<double> { 1.0, 1.3, 1.7, 2.0, 2.3, 2.7, 3.0, 3.3, 3.7, 4.0, 5.0 };
            
            foreach (var grade in grades)
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

            chosenGrade = double.Parse(GradeDropDown.SelectedItem.ToString());

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
            if (chosenGrade == 0)
            {
                var hint = MessageBox.Show("Please submit your grade.");
                e.Cancel = true;
            }
        }
    }
}
