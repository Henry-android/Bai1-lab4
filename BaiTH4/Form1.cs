using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTH4
{
    public partial class frmSV : Form
    {
        public frmSV()
        {
            InitializeComponent();
        }

        private void frmSV_Load(object sender, EventArgs e)
        {
            try
            {
                Model1 context = new Model1();
                List<Faculty> listFalcultys = context.Faculty.ToList(); //lấy các khoa
                List<Student> listStudent = context.Student.ToList(); //lấy sinh viên
                FillFalcultyCombobox(listFalcultys);
                BindGrid(listStudent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FillFalcultyCombobox(List<Faculty> listFalcultys)
        {
            this.cmbKhoa.DataSource = listFalcultys;
            this.cmbKhoa.DisplayMember = "FacultyName";
            this.cmbKhoa.ValueMember = "FacultyID";
        }

        private void BindGrid(List<Student> listStudent)
        {
            dgvSV.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dgvSV.Rows.Add();
                dgvSV.Rows[index].Cells[0].Value = item.StudentID;
                dgvSV.Rows[index].Cells[1].Value = item.FullName;
                dgvSV.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dgvSV.Rows[index].Cells[3].Value = item.AverageScore;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                using (Model1 db = new Model1())
                {
                    string studentIDText = txtmssv.Text.Trim();
                    string fullName = txtHoTen.Text.Trim();
                    string averageScoreText = txtDiemTb.Text.Trim();

                    if (string.IsNullOrEmpty(studentIDText))
                    {
                        MessageBox.Show("Mã sinh viên không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!int.TryParse(studentIDText, out int studentID))
                    {
                        MessageBox.Show("Mã sinh viên phải là một số nguyên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (string.IsNullOrEmpty(fullName))
                    {
                        MessageBox.Show("Tên sinh viên không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!int.TryParse(cmbKhoa.SelectedValue?.ToString(), out int facultyID))
                    {
                        MessageBox.Show("Vui lòng chọn một khoa hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!double.TryParse(averageScoreText, out double averageScore))
                    {
                        MessageBox.Show("Điểm trung bình không hợp lệ. Vui lòng nhập số.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var newStudent = new Student
                    {
                        StudentID = studentID, // Gán kiểu int
                        FullName = fullName,
                        FacultyID = facultyID,
                        AverageScore = averageScore
                    };

                    db.Student.Add(newStudent);
                    db.SaveChanges();

                    BindGrid(db.Student.ToList());

                    MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm sinh viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                using (Model1 db = new Model1()) // Thay StudentContextDB bằng Model1
                {
                    // Kiểm tra dữ liệu đầu vào
                    string studentIDText = txtmssv.Text.Trim();
                    string fullName = txtHoTen.Text.Trim();
                    string averageScoreText = txtDiemTb.Text.Trim();

                    if (string.IsNullOrEmpty(studentIDText))
                    {
                        MessageBox.Show("Mã sinh viên không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!int.TryParse(studentIDText, out int studentID))
                    {
                        MessageBox.Show("Mã sinh viên phải là một số nguyên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (string.IsNullOrEmpty(fullName))
                    {
                        MessageBox.Show("Tên sinh viên không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!int.TryParse(cmbKhoa.SelectedValue?.ToString(), out int facultyID))
                    {
                        MessageBox.Show("Vui lòng chọn một khoa hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!double.TryParse(averageScoreText, out double averageScore))
                    {
                        MessageBox.Show("Điểm trung bình không hợp lệ. Vui lòng nhập số.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Tìm sinh viên cần cập nhật
                    var student = db.Student.FirstOrDefault(s => s.StudentID == studentID);
                    if (student == null)
                    {
                        MessageBox.Show("Không tìm thấy sinh viên với mã đã nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Cập nhật thông tin sinh viên
                    student.FullName = fullName;
                    student.FacultyID = facultyID;
                    student.AverageScore = averageScore;

                    db.SaveChanges();

                    // Cập nhật lại danh sách trên DataGridView
                    BindGrid(db.Student.ToList());

                    MessageBox.Show("Cập nhật thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật thông tin sinh viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                using (Model1 db = new Model1())
                {
                    // Lấy mã sinh viên từ ô nhập liệu
                    string studentIDText = txtmssv.Text.Trim();

                    if (!int.TryParse(studentIDText, out int studentID))
                    {
                        MessageBox.Show("Mã sinh viên phải là một số nguyên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Tìm sinh viên cần xóa
                    var student = db.Student.SingleOrDefault(s => s.StudentID == studentID);

                    if (student == null)
                    {
                        MessageBox.Show("Không tìm thấy sinh viên với mã này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Xác nhận trước khi xóa
                    var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (confirmResult == DialogResult.Yes)
                    {
                        db.Student.Remove(student);
                        db.SaveChanges();

                        // Cập nhật lại danh sách trên DataGridView
                        BindGrid(db.Student.ToList());

                        MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa sinh viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
 
        

        private void dgvSV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex >= 0)
            //{
            //    DataGridViewRow row = dgvSV.Rows[e.RowIndex];

            //    txtmssv.Text = row.Cells[0].Value?.ToString();
            //    txtHoTen.Text = row.Cells[1].Value?.ToString();
            //    cmbKhoa.SelectedValue = row.Cells[2].Value;
            //    txtDiemTb.Text = row.Cells[3].Value?.ToString();
            //}
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSV.Rows[e.RowIndex];

                txtmssv.Text = row.Cells[0].Value?.ToString();
                txtHoTen.Text = row.Cells[1].Value?.ToString();

                // Đặt giá trị cho cmbKhoa dựa trên FacultyID
                using (Model1 db = new Model1())
                {
                    var facultyName = row.Cells[2].Value?.ToString();
                    var faculty = db.Faculty.FirstOrDefault(f => f.FacultyName == facultyName);
                    if (faculty != null)
                    {
                        cmbKhoa.SelectedValue = faculty.FacultyID;
                    }
                }

                txtDiemTb.Text = row.Cells[3].Value?.ToString();
            }
        }
    }
}


