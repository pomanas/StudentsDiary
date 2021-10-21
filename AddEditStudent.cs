﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class AddEditStudent : Form
    {
        private int _studentId;
        private Student _student;
        private List<Group> _groups;

        private FileHelper<List<Student>> _fileHelper =
            new FileHelper<List<Student>>(Program.FilePath);

        public AddEditStudent(int id = 0)
        {
            InitializeComponent();
            _studentId = id;

            _groups = GroupsHelper.GetGroups("Brak");

            InitGroupsCombobox();
            GetStudentData();
            tbFirstName.Select();
        }

        private void InitGroupsCombobox()
        {
            combGroup.DataSource = _groups;
            combGroup.DisplayMember = "Name";
            combGroup.ValueMember = "Id";
        }

        private void GetStudentData()
        {
            if (_studentId != 0)
            {
                Text = "Edytowanie ucznia";

                var students = _fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentId);

                if (_student == null)
                    throw new Exception("Brak użytkownika o podanym Id");

                FillTextBoxes();
            }
        }

        private void FillTextBoxes()
        {
            tbId.Text = _student.Id.ToString();
            tbFirstName.Text = _student.FirstName;
            tbLastName.Text = _student.LastName;
            tbMath.Text = _student.Physics;
            tbTechnology.Text = _student.Technology;
            tbPolishLang.Text = _student.PolishLang;
            tbForeignLang.Text = _student.ForeignLang;
            rtbComments.Text = _student.Comments;
            cbAditionalActivities.Checked = _student.AditionalActivities;
            combGroup.SelectedItem = _groups.FirstOrDefault(x => x.Id == _student.GroupId);
        }


        private async void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            if (_studentId != 0)
                students.RemoveAll(x => x.Id == _studentId);
            else
                AssignIdToNewStudent(students);

            AddNewUserToList(students);

            _fileHelper.SerializeToFile(students);

            await LongProcessAsync();

            Close();
        }

        private async Task LongProcessAsync()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(3000);
            });

            
        }

        private void AddNewUserToList(List<Student> students)
        {
            var student = new Student
            {
                Id = _studentId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Comments = rtbComments.Text,
                ForeignLang = tbForeignLang.Text,
                Math = tbMath.Text,
                Physics = tbPhysics.Text,
                PolishLang = tbPolishLang.Text,
                Technology = tbTechnology.Text,
                AditionalActivities = cbAditionalActivities.Checked,
                GroupId = (combGroup.SelectedItem as Group).Id
            };
            students.Add(student);
        }

        private void AssignIdToNewStudent(List<Student> students)
        {
            var studentWithHighestId = students.OrderByDescending(x => x.Id).FirstOrDefault();

            _studentId = studentWithHighestId == null ? 1 : studentWithHighestId.Id + 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void AddEditStudent_Load(object sender, EventArgs e)
        {

        }

        private void cbAditionalActivities_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
