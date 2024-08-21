using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace kiemtra
{
    public partial class Form2 : Form
    {
        SqlConnection sqlConnection;
        string conn = "Data Source=DESKTOP-M84T3VE;Initial Catalog=DuLieu;User ID=sa;Password=123";

        public Form2()
        {
            InitializeComponent();
            LoadTable();
            this.dgvDanhSach.SelectionChanged += new System.EventHandler(this.dgvDanhSach_SelectionChanged);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không ? ", "Thông báo !", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }

        private void LoadTable()
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.tblNhanVien", con))
                    {
                        SqlDataAdapter adt = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adt.Fill(dt);

                        // Clear columns and rows in DataGridView
                        dgvDanhSach.Columns.Clear();
                        dgvDanhSach.Rows.Clear();

                        // Add columns to DataGridView based on the structure of DataTable
                        foreach (DataColumn column in dt.Columns)
                        {
                            dgvDanhSach.Columns.Add(column.ColumnName, column.ColumnName);
                        }

                        // Add data to DataGridView from DataTable
                        foreach (DataRow row in dt.Rows)
                        {
                            dgvDanhSach.Rows.Add(row.ItemArray);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void add(string maNV, string tenNV, string soDT, string gioiTinh, string phongBan, double mucLuong)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(conn))
                {
                    sqlConnection.Open();

                    string query = "INSERT INTO tblNhanVien (MaNV, TenNV, SoDT, GioiTinh, PhongBan, MucLuong) VALUES (@MaNV, @TenNV, @SoDT, @GioiTinh, @PhongBan, @MucLuong)";

                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@MaNV", maNV);
                        sqlCommand.Parameters.AddWithValue("@TenNV", tenNV);
                        sqlCommand.Parameters.AddWithValue("@SoDT", soDT);
                        sqlCommand.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                        sqlCommand.Parameters.AddWithValue("@PhongBan", phongBan);
                        sqlCommand.Parameters.AddWithValue("@MucLuong", mucLuong);

                        sqlCommand.ExecuteNonQuery();
                        MessageBox.Show("Thêm nhân viên thành công");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        public void edit(string maNV, string tenNV, string soDT, string gioiTinh, string phongBan, double mucLuong)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(conn))
                {
                    sqlConnection.Open();

                    string query = "UPDATE tblNhanVien SET TenNV=@TenNV, SoDT=@SoDT, GioiTinh=@GioiTinh, PhongBan=@PhongBan, MucLuong=@MucLuong WHERE MaNV=@MaNV ";

                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@MaNV", maNV);
                        sqlCommand.Parameters.AddWithValue("@TenNV", tenNV);
                        sqlCommand.Parameters.AddWithValue("@SoDT", soDT);
                        sqlCommand.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                        sqlCommand.Parameters.AddWithValue("@PhongBan", phongBan);
                        sqlCommand.Parameters.AddWithValue("@MucLuong", mucLuong);

                        sqlCommand.ExecuteNonQuery();
                        MessageBox.Show("Sửa thông tin nhân viên thành công");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog.FileName);
            }
        }

        private void dgvDanhSach_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDanhSach.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvDanhSach.SelectedRows[0];
                txtMaNv.Text = selectedRow.Cells["MaNV"].Value.ToString();
                txtTenNv.Text = selectedRow.Cells["TenNV"].Value.ToString();
                txtSoDt.Text = selectedRow.Cells["SoDT"].Value.ToString();
                // Continue with other columns if needed
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maNV = txtMaNv.Text;
            string tenNV = txtTenNv.Text;
            string soDT = txtSoDt.Text;
            string gioiTinh = rdoNam.Checked ? "Nam" : "Nữ";
            string phongBan = cbbPhongBan.Text;
            double mucLuong = Convert.ToDouble(txtMucLuong.Text);
            add(maNV, tenNV, soDT, gioiTinh, phongBan, mucLuong);
            LoadTable(); // Reload DataGridView after adding
        }

        private void btnSua_Click_1(object sender, EventArgs e)
        {
            string maNV = txtMaNv.Text;
            string tenNV = txtTenNv.Text;
            string soDT = txtSoDt.Text;
            string gioiTinh = rdoNam.Checked ? "Nam" : "Nữ";
            string phongBan = cbbPhongBan.Text;
            double mucLuong = Convert.ToDouble(txtMucLuong.Text);
            edit(maNV, tenNV, soDT, gioiTinh, phongBan, mucLuong);
            LoadTable(); // Reload DataGridView after editing
        }

        private void cbbPhongBan_DropDown(object sender, EventArgs e)
        {
            cbbPhongBan.Items.Clear();
            cbbPhongBan.Items.Add("Thu ngân1");
            cbbPhongBan.Items.Add("Thu ngân2");
            cbbPhongBan.Items.Add("Thu ngân3");
            cbbPhongBan.Items.Add("Thu ngân4");


        }
    }
}
