using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;
using System.Runtime;
using Newtonsoft.Json;
using System.Security.Cryptography;
using DieuKhienTiepDon.Moduls;
using DevExpress.Entity.Model;
using Emgu.CV.ML;
using Model;
using System.Net.Http;
using System.Diagnostics.Eventing.Reader;

namespace DangKyKhamTuDong
{
    public partial class frm_DangKyKham : Form
    {
        public string strConnectString = "";
        private SqlConnection ConnectSQL = null;
        private bool LoggedIn = false;
        private string ServerName = "";
        private string UName = "", Pass = "";
        private int DoTreNhan = 2;

        private string IDToken = "";
        private string Access_Token = "";
        private NameValueCollection values;
        private WebClient client;
        public string MaKetQua;
        // Public thebhyt As clsTheBHYT
        private clsTheBHYT2018 thebhyt2018 = new clsTheBHYT2018();

        string ThongTinThe = "";
        string MaDTthe = "";
        string SotheBHYT = "";
        string NoiDKKCBBD = "";
        DateTime HanTu = default(DateTime), HanDen = default(DateTime), TgDu5Nam = default(DateTime);
        string HoTen = default(string), DiaChi = default(string);
        int NamSinh = default(int), ThangSinh = default(int), NgaySinh = default(int), GioiTinh = default(int);
        bool isUTien = false;

        //--thong tin thieu
        DateTime? TgMienChiTraTrongNam =  null;
        bool MienChiTraTrongNam = false;
        string TenBHYTTinh = "";
        string noicongtac = "", lienhe = "";
        int loaituyen = 0;
        string khuVuc = "";
        string maPhongKham = "", maDichVu = "";
        //--------------------------------------------------
        public frm_DangKyKham()
        {
            InitializeComponent();
        }

        private void frm_DangKyKham_Load(object sender, EventArgs e)
        {
            //IniFile ini = new IniFile(Application.StartupPath + @"\DieuKhien.ini");
            
            //ServerName = ini.GetIniFileString("XepHangPhongKham", "ServerName", "172.16.2.3");
            //int DoTreGoi = int.Parse(ini.GetIniFileString("XepHangPhongKham", "DoTreGoi", "2000"));
            //DoTreNhan = int.Parse(ini.GetIniFileString("XepHangPhongKham", "DoTreNhan", "2"));
            //strConnectString = String.Format["Data Source={0};Initial Catalog=NamDinh_KhamBenh; User ID={1}; Password={2}", ServerName, "sa", ""];
            strConnectString = String.Format("Data Source={0};Initial Catalog=NamDinh_KhamBenh; User ID={1}; Password={2}", "113.160.217.66,1433", "Admin_HIS", "vcntt@2020");


            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, pb_face.Width - 20, pb_face.Height - 3);
            Region rg = new Region(gp);
            pb_face.Region = rg;

            Load_DM();


            //frm_Face frm = new frm_Face();
            //if(frm.ShowDialog() == DialogResult.OK)
            //{
            //    //1.. Tien hanh xac thuc khuon mat voi du lieu
            //    //2.. Load data neu face da duoc dang ky
            //}    
        }

        private void Load_DM()
        {
            SqlConnection CALL_ConnectSQL = new SqlConnection();
            CALL_ConnectSQL.ConnectionString = strConnectString;
            CALL_ConnectSQL.Open();
            string SQL = "";
            SqlCommand Cmd = default(SqlCommand);
            DataSet DsDM = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            //1. DM Quoc tich
            SQL = "Select MaNuocNgoai,[TenNuocNgoai] from [NAMDINH_SYSDB].[dbo].[DmNuocNgoai] where sudung = 1 order by TenNuocNgoai";
            Cmd = new SqlCommand(SQL, CALL_ConnectSQL);
            da.SelectCommand = Cmd;
            da.Fill(DsDM, "DmQuocTich");
            lu_QuocTich.Properties.DataSource = DsDM.Tables["DmQuocTich"];
            lu_QuocTich.EditValue = "000";
            Cmd.Dispose();

            //2. DM Dan toc
            SQL = "Select Ma_XML As MaDT,TenDT from DMDANTOC order by TenDT";
            Cmd = new SqlCommand(SQL, CALL_ConnectSQL);
            da.SelectCommand = Cmd;
            da.Fill(DsDM, "DmDanToc");
            lu_DanToc.Properties.DataSource = DsDM.Tables["DmDanToc"];
            lu_DanToc.EditValue = "000";
            Cmd.Dispose();
            //3. DM Tinh
            SQL = "Select Ma_XML As MaTinh, TenTinh from NamDinh_SYSDB.dbo.DMTinh order by TenTinh";
            Cmd = new SqlCommand(SQL, CALL_ConnectSQL);
            da.SelectCommand = Cmd;
            da.Fill(DsDM, "DmTinh");
            lu_Tinh.Properties.DataSource = DsDM.Tables["DmTinh"];
            lu_Tinh.EditValue = "000";
            Cmd.Dispose();
            //4. DM Huyen
            SQL = "Select Ma_XML As MaQuanHuyen,TenQuanHuyen from NamDinh_SYSDB.dbo.DMQuanHuyen  order by Ma_XML";
            Cmd = new SqlCommand(SQL, CALL_ConnectSQL);
            da.SelectCommand = Cmd;
            da.Fill(DsDM, "DmHuyen");
            lu_Huyen.Properties.DataSource = DsDM.Tables["DmHuyen"];
            lu_Huyen.EditValue = "000";
            Cmd.Dispose();
            //5. DM Xa 
            SQL = "Select Ma_XML As MaXaPhuong,TenXaPhuog As TenXaPhuong from NamDinh_SYSDB.dbo.DMXaPhuong  order by Ma_XML";
            Cmd = new SqlCommand(SQL, CALL_ConnectSQL);
            da.SelectCommand = Cmd;
            da.Fill(DsDM, "DmXa");
            lu_Xa.Properties.DataSource = DsDM.Tables["DmXa"];
            lu_Xa.EditValue = "000";
            Cmd.Dispose();
            //6. DM Nghe nghiep
            SQL = "Select * from DMNGHENGHIEP WHERE SoThuTu < 2 order by SoThuTu,TenNgheNghiep";
            Cmd = new SqlCommand(SQL, CALL_ConnectSQL);
            da.SelectCommand = Cmd;
            da.Fill(DsDM, "DMNgheNghiep");
            slu_NgheNghiep.Properties.DataSource = DsDM.Tables["DMNgheNghiep"];
            Cmd.Dispose();
            //7. DM Phong kham
            SQL = "SELECT T1.MaKhoa,T1.TenKhoa,MaDichvu,TenDichvu,T2.soluong,T2.dakham,T2.chokham FROM ("
                  + "  select kp.MaKhoa,kp.TenKhoa,dv.MaDichvu,dv.TenDichvu"
                  + "    from DMKHOAPHONG kp"
                  + "   inner join NAMDINH_SYSDB.dbo.DMDICHVU_KHOA dvk on kp.MaKhoa = dvk.MaKhoa"
                  + "   inner join DMDICHVU dv on dvk.MaDichvu = dv.MaDichvu"
                  + "   where dv.Khongsudung = 0 and dv.Noitru_Ngoaitru = 2 and kp.MaKhoa in ('NV13201', 'NV13204', 'NV13204', 'NV13221', 'NV13220', 'NV13212')"
                  + "   ) T1 LEFT JOIN ("
                  + "   SELECT MaKhoa, sum(dangkykham) soluong, sum(dakham) dakham, sum(chokham) chokham"
                  + "   FROM("
                  + "   SELECT KQ.Khoathuchien MaKhoa, 1 dangkykham,"
                  + "       case when HuongGQ > 0 then 1 else 0 end dakham,"
                  + "       case when HuongGQ = 0 then 1 else 0 end chokham"
                  + "   FROM NAMDINH_KHAMBENH.dbo.tblKHAMBENH_KQDVKHAM KQ"
                  + "   WHERE KQ.Khoathuchien in ('NV13201', 'NV13204', 'NV13204', 'NV13221', 'NV13220', 'NV13212') and SUBSTRING(KQ.MaphieuCD ,3,6) = convert(varchar, getdate(), 12)"
                   + "  ) C group by MaKhoa) T2 on T1.MaKhoa = T2.MaKhoa";
            Cmd = new SqlCommand(SQL, CALL_ConnectSQL);
            da.SelectCommand = Cmd;
            da.Fill(DsDM, "DMPhongKham");
            grd_PhongKham.DataSource = DsDM.Tables["DMPhongKham"];
            grd_Card_PhongKham.DataSource = DsDM.Tables["DMPhongKham"];
            Cmd.Dispose();
            da.Dispose();
            CALL_ConnectSQL.Close();
        }

        //Ham doc the moi
        private void Read_TheBHYT_New(string sTheBHYT)
        {
            string[] strKeys;
            strKeys = sTheBHYT.Trim().Split('|');

            if (strKeys.Length != 17)
                return;

            MaDTthe = "";
            txtMaThe.Text = strKeys[0];
            txtHoTen.Text = FromHex(strKeys[1]).ToUpper();
            // ThongTinThe = ThongTinThe.Substring(ViTri)
            if (strKeys[2].Split('/').Length == 3)
            {
                NamSinh = Convert.ToInt32(strKeys[2].Split('/')[2]);
                NgaySinh = Convert.ToInt32(strKeys[2].Split('/')[0]);
                ThangSinh = Convert.ToInt32(strKeys[2].Split('/')[1]);
            }
            else
                NamSinh = Convert.ToInt32(strKeys[2]);
            txtNamSinh.Text = NamSinh.ToString();
            GioiTinh = Convert.ToInt32(Convert.ToDouble(strKeys[3]) - (double)1);
            if (strKeys[4].Trim() == "-")
                strKeys[4] = "";
            DiaChi = FromHex(strKeys[4]);

            NoiDKKCBBD = strKeys[5].Replace(" ", "").Replace("-", "");

            string[] strHanTu = strKeys[6].Split('/');
            HanTu = new DateTime(Convert.ToInt32(strHanTu[2]), Convert.ToInt32(strHanTu[1]), Convert.ToInt32(strHanTu[0])); ;

            // 'Thuc hien kiem tra tren cong de lay thong tin the day du: MaDT + muc huong + the
            //CheckTheBHYT_2021(txtMaThe.Text.Trim(), txtHoTen.Text.Trim().ToUpper(), NamSinh.ToString());
            //if (MaKetQua != "000" & MaKetQua != "001" & MaKetQua != "002" & MaKetQua != "130")
            //{
            //    MessageBox.Show(Mes);
            //}
            //else
            //{
            //    txtMaThe.Text = thebhyt2018.maThe.Substring(2);
            //    MaDTthe = thebhyt2018.maThe.Substring(0, 2);
            //    HanTu = new DateTime(Convert.ToInt32(thebhyt2018.gtTheTu.Split('/')[2]), Convert.ToInt32(thebhyt2018.gtTheTu.Split('/')[1]), Convert.ToInt32(thebhyt2018.gtTheTu.Split('/')[0]));
            //    HanDen = new DateTime(Convert.ToInt32(thebhyt2018.gtTheDen.Split('/')[2]), Convert.ToInt32(thebhyt2018.gtTheDen.Split('/')[1]), Convert.ToInt32(thebhyt2018.gtTheDen.Split('/')[0]));
            //    TgDu5Nam = new DateTime(Convert.ToInt32(thebhyt2018.ngayDu5Nam.Split('/')[2]), Convert.ToInt32(thebhyt2018.ngayDu5Nam.Split('/')[1]), Convert.ToInt32(thebhyt2018.ngayDu5Nam.Split('/')[0]));
            //}

            LoadData_Old(MaDTthe, txtMaThe.Text.Trim());
            // 0204287018|486fc3a06e67205875c3a26e2048e1baa56e|30/07/1981|1|-|79 - 034|01/02/2021|-|20/02/2021|79020204287018|-|4| 01/01/2015|15e89ac07ee8517f-7102|4|5175e1baad6e2031322c205468c3a06e68207068e1bb912048e1bb93204368c3ad204d696e68|$
            //if (txtMaDT.Text.Trim.ToUpper == "TE" | txtMaDT.Text.Trim.ToUpper == "CK" | txtMaDT.Text.Trim.ToUpper == "CC"]
            //    chkUuTien.Checked = true;

            //if (txtMaDT.Text.Trim.ToUpper == "HT" & txtSotheBHYT.Text.Trim().Substring(0, 1) == "1"]
            //    chkUuTien.Checked = true;
            //if (Ngayhientai.Year - Convert.ToInt32(txtTuoi.Value) >= 80)
            //    chkUuTien.Checked = true;
        }
        private void Read_TheCCCD(string sSoTheCCCD)
        {
            string[] strKeys;
            strKeys = sSoTheCCCD.Trim().Split('|');
            int i = 0;

            if (strKeys.Length != 7)
                return;
            txt_SoCCCD.Tag = "1";
            txt_SoCCCD.Text = strKeys[0];
            string sSoCCCD = strKeys[0];
            txtHoTen.Text = strKeys[2].ToUpper();
            txtNgaySinh.Text = strKeys[3].Substring(0, 2);
            txtThangSinh.Text = strKeys[3].Substring(2, 2);
            txtNamSinh.Text = strKeys[3].Substring(4, 4);

            //036165001786|161445729|Trần Thị Hồng|05051965|Nữ|Thôn 3, Mỹ Trung, Mỹ Lộc, Nam Định|08052021

            //// 'Thuc hien kiem tra tren cong de lay thong tin the day du: MaDT + muc huong + the
            FillDataHC_TheCCCD(sSoCCCD);
            
            txt_SoCCCD.Text = sSoCCCD;
            txt_SoCCCD.Tag = "";
        }

        public void FillDataHC_TheCCCD(string CCCD) // ma = Mã khám bệnh
        {
            string SQL;
            SqlConnection CALL_ConnectSQL = new SqlConnection();
            CALL_ConnectSQL.ConnectionString = strConnectString;
            CALL_ConnectSQL.Open();
            SqlCommand Cmd;
            SqlDataAdapter Adap;
            DataSet ds;
            try
            {
                if (CCCD.Trim() == "")
                    return;
                
                SQL = "SELECT top 1 tblBenhnhan.*,tblKhambenh.*,isnull(su.TenDu,'') as TenDayDu, "
                        + " isnull(So_BHXH,'') As SoBHXH,isnull(So_CCCD,'') As SoCCCD,isnull(So_Dien_Thoai,'') As SoDienThoai,"
                        + " DMDTTHE_BHYT.TenDT as TenDTThe,DMNOIDKKCBBD_BHYT.Tennoicap"
                        + " FROM  tblkhambenh "
                        + " left join tblBenhnhan on tblKhambenh.Mabenhnhan = tblBenhnhan.Mabenhnhan "
                        + " left join DMDTTHE_BHYT on tblKhambenh.Doituongthe  = DMDTTHE_BHYT.MaDT "
                        + " left join DMNOIDKKCBBD_BHYT on tblKhambenh.NoidangkyKCBBD  = DMNOIDKKCBBD_BHYT.Manoicap "
                        + " left join DMDOITUONGBN on tblKhambenh.Doituong  = DMDOITUONGBN.MaDT "
                        + " LEFT JOIN SYSUSER su ON su.UName = tblkhambenh.NhanvienCD "
                        + " left join DMNGHENGHIEP on tblKhambenh.Nghenghiep  = DMNGHENGHIEP.MaNghenghiep "
                        + " where tblkhambenh.So_CCCD = N'" + CCCD + "' Order by Thoigiandangky DESC";
                

                Cmd = new SqlCommand(SQL, CALL_ConnectSQL);
                Adap = new SqlDataAdapter();
                Adap.SelectCommand = Cmd;
                ds = new DataSet();
                Adap.Fill(ds, "Hoso");
                Adap.Dispose();
                Cmd.Dispose();
                CALL_ConnectSQL.Close();
                if (ds.Tables["Hoso"].Rows.Count > 0)
                {
                    // Fill Benh nhan
                    txtMaBenhNhan.Text = ds.Tables["Hoso"].Rows[0]["MaBenhnhan"].ToString();
                    txtHoTen.Text = ds.Tables["Hoso"].Rows[0]["TenBenhnhan"].ToString();
                    txtNamSinh.Text = ds.Tables["Hoso"].Rows[0]["Namsinh"].ToString();
                    cmbGioiTinh.SelectedIndex = ds.Tables["Hoso"].Rows[0]["Gioitinh"].ToString() == "1" ? 0 : 1 ;
                    // Fill Kham benh
                    //txtMaKhamBenh.Text = ds.Tables["Hoso"].Rows[0]["MaKhambenh"].ToString();
                    // cmbDoituong.SelectedValue = ds.Tables["Hoso"].Rows[0].Item["Doituong"]
                    //if (ds.Tables["Hoso"].Rows[0]["Doituong"].ToString() == "1")
                    //{
                    //    cmbGioiTinh.Enabled = true;
                    //    txtNamSinh.ReadOnly = true;
                    //}
                    //else
                    //{
                    //    cmbGioiTinh.Enabled = false;
                    //    txtNamSinh.ReadOnly = false;
                    //}
                    txtDiaChi.Text = ds.Tables["Hoso"].Rows[0]["Diachi"].ToString();
                    if (ds.Tables["Hoso"].Rows[0]["Nghenghiep"] == DBNull.Value || ds.Tables["Hoso"].Rows[0]["Nghenghiep"].ToString() == "")
                        slu_NgheNghiep.EditValue = "00000";
                    else
                        slu_NgheNghiep.EditValue = ds.Tables["Hoso"].Rows[0]["Nghenghiep"].ToString();

                    //txtNoicongtac.Text = ds.Tables["Hoso"].Rows[0].Item["Noicongtac"];
                    // txtLienhe.Text = ds.Tables["Hoso"].Rows[0].Item["Lienhe"]

                    txt_SoDienThoai.Text = ds.Tables["Hoso"].Rows[0]["SoDienThoai"].ToString();
                    //txt_So_BHXH.Text = ds.Tables["Hoso"].Rows[0].Item["SoBHXH"];
                    //txt_SoCCCD.Text = ds.Tables["Hoso"].Rows[0].Item["SoCCCD"];
                   // txtTheTE.Text = ds.Tables["Hoso"].Rows[0].Item["SotheTE"];
                    //txtNoigioithieu.Text = ds.Tables["Hoso"].Rows[0].Item["Noigioithieu"];
                   // txtChandoan.Text = ds.Tables["Hoso"].Rows[0].Item["ChandoanNGT"];
                    // ' txtThoigianDangky.Value = ds.Tables["Hoso"].Rows[0].Item["ThoigianDangky"]
                    chkUuTien.Checked = ds.Tables["Hoso"].Rows[0]["UuTien"].ToString() == "1"? true : false;
                    //txtNgaySinh.Text = ds.Tables["Hoso"].Rows[0]["NgaySinh"] == null? "" : ds.Tables["Hoso"].Rows[0]["NgaySinh"].ToString();
                    //txtThangSinh.Text = ds.Tables["Hoso"].Rows[0]["ThangSinh"] == null ?"" : ds.Tables["Hoso"].Rows[0]["ThangSinh"].ToString();
                    //cmbTuyen.SelectedIndex = Convert.ToInt32(ds.Tables["Hoso"].Rows[0]["Tuyen"]); // IIf(ds.Tables["Hoso"].Rows[0]["Tuyen"] = False, 0, 1)
                    //if (IsDBNull(ds.Tables["Hoso"].Rows[0]["LoaiTuyen"]) == false)
                    //    cmb_LoaiTuyen.SelectedValue = ds.Tables["Hoso"].Rows[0]["LoaiTuyen"];
                    if (ds.Tables["Hoso"].Rows[0]["Ma_QuocTich"] != null)
                        lu_QuocTich.EditValue = ds.Tables["Hoso"].Rows[0]["Ma_QuocTich"];
                    else
                        lu_QuocTich.EditValue = "000";

                    if (ds.Tables["Hoso"].Rows[0]["Ma_DanToc"] != null)
                        lu_DanToc.EditValue = ds.Tables["Hoso"].Rows[0]["Ma_DanToc"];
                    else
                        lu_DanToc.EditValue = "22";

                    if (ds.Tables["Hoso"].Rows[0]["Ma_Tinh"] != null)
                        lu_Tinh.EditValue = ds.Tables["Hoso"].Rows[0]["Ma_Tinh"];
                    else
                        lu_Tinh.EditValue = 0;
                    if (ds.Tables["Hoso"].Rows[0]["Ma_Huyen"] != null)
                        lu_Huyen.EditValue = ds.Tables["Hoso"].Rows[0]["Ma_Huyen"];
                    else
                        lu_Huyen.EditValue = 0;

                    if (ds.Tables["Hoso"].Rows[0]["Ma_Xa"] != null)
                        lu_Xa.EditValue = ds.Tables["Hoso"].Rows[0]["Ma_Xa"];
                    else
                        lu_Xa.EditValue = 0;

                    //if (IsDBNull(ds.Tables["Hoso"].Rows[0]["MaKhuVuc"]) == false)
                    //    cmb_KhuVuc.Text = ds.Tables["Hoso"].Rows[0]["MaKhuVuc"];
                    //else
                    //    cmb_KhuVuc.Text = "";

                    //txt_SoGiayHenKhamLai.Text = IIf(IsDBNull(ds.Tables["Hoso"].Rows[0]["SoPhieuHenKhamLai"]), string.Empty, ds.Tables["Hoso"].Rows[0]["SoPhieuHenKhamLai"]);


                    if (ds.Tables["Hoso"].Rows[0]["Doituong"].ToString() == "1")
                    {
                        txtMaThe.Text = ds.Tables["Hoso"].Rows[0]["DoituongThe"].ToString() + "-" + ds.Tables["Hoso"].Rows[0]["SotheBHYT"].ToString();
                        MaDTthe = ds.Tables["Hoso"].Rows[0]["DoituongThe"].ToString();
                        //txtTenDT.Text = ds.Tables["Hoso"].Rows[0]["TenDTThe"].ToString();
                        txtManoiDKKCBBD.Text = ds.Tables["Hoso"].Rows[0]["NoidangkyKCBBD"].ToString();
                        txtTenNoiDKKCBBD.Text = ds.Tables["Hoso"].Rows[0]["Tennoicap"].ToString();
                        dtBHTu.Value = Convert.ToDateTime(ds.Tables["Hoso"].Rows[0]["HantheBHYT_Tu"]);
                        dtBHDen.Value = Convert.ToDateTime(ds.Tables["Hoso"].Rows[0]["HantheBHYT_Den"]);
                        if(ds.Tables["Hoso"].Rows[0]["TgDu5Nam"] != null)
                            TgDu5Nam =  Convert.ToDateTime(ds.Tables["Hoso"].Rows[0]["TgDu5Nam"]);
                        string svl = ds.Tables["Hoso"].Rows[0]["ThoiDiemMienChiTraTrongNam"].ToString();
                        if (ds.Tables["Hoso"].Rows[0]["ThoiDiemMienChiTraTrongNam"] != null && svl != "")
                            TgMienChiTraTrongNam =  Convert.ToDateTime(ds.Tables["Hoso"].Rows[0]["ThoiDiemMienChiTraTrongNam"]);
                        MienChiTraTrongNam = (ds.Tables["Hoso"].Rows[0]["MienChiTraTrongNam"].ToString() == "1"? true: false);
                        TenBHYTTinh = ds.Tables["Hoso"].Rows[0]["NoicaptheBHYT"].ToString();
                    }
                    else
                    {
                        if (MessageBox.Show("Bệnh nhân đã có trong CSDL nhưng chưa có thông tin Bảo hiểm y tế. Bạn có muốn thực hiện check thông tin trên cổng BHYT?", "Thông báo !", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            CheckTheBHYT2018(txt_SoCCCD.Text.Trim(), txtHoTen.Text.Trim().ToUpper(), txtNamSinh.Text);
                            if (MaKetQua != "000" & MaKetQua != "001" & MaKetQua != "002" & MaKetQua != "130")
                            {
                                //FrmMes frm = new FrmMes();
                                //frm._Mes = Mes;
                                //frm._GtTheTu = thebhyt2018.gtTheTu;
                                //frm._GtTheDen = thebhyt2018.gtTheDen;
                                //frm.ShowDialog();
                                MessageBox.Show(Mes);
                            }
                            else
                            {
                                txtMaThe.Text = thebhyt2018.maThe.Substring(0, 2) + "-" + thebhyt2018.maThe.Substring(2);
                                MaDTthe = thebhyt2018.maThe.Substring(0, 2);
                                dtBHTu.Value = Convert.ToDateTime(thebhyt2018.gtTheTu);
                                dtBHDen.Value = Convert.ToDateTime(thebhyt2018.gtTheDen);
                                txtManoiDKKCBBD.Text = thebhyt2018.maDKBD;
                                //Set_LoaiTuyen(txtManoiDKKCBBD.Text.Trim);
                                txtManoiDKKCBBD_Validated(); //null/* TODO Change to default(_) if this is not a reference type */, null/* TODO Change to default(_) if this is not a reference type */);
                                cmbGioiTinh.SelectedIndex = thebhyt2018.gioiTinh.ToUpper() == "NAM" ? 0 : 1;
                                txtDiaChi.Text = thebhyt2018.diaChi;
                                Read_DiaChi_2_Combo(txtDiaChi.Text.Trim());
                                TgDu5Nam = Convert.ToDateTime(thebhyt2018.ngayDu5Nam);

                                if (MaDTthe.ToUpper() == "TE" | MaDTthe.ToUpper() == "CK" | MaDTthe.ToUpper() == "CC")
                                    chkUuTien.Checked = true;

                                if (MaDTthe.ToUpper() == "HT" & thebhyt2018.maThe.Substring(2).Substring(0, 1) == "1")
                                    chkUuTien.Checked = true;
                                if (thebhyt2018.maKV == "K1" | thebhyt2018.maKV == "K2" | thebhyt2018.maKV == "K3")
                                    khuVuc= thebhyt2018.maKV;
                                else
                                {
                                    khuVuc = "";
                                }
                            
                        }
                        }
                        else
                        {
                            txtMaThe.Text = "";
                            //txtMaDT.Text = "";
                            //txtTenDT.Text = "";
                            txtManoiDKKCBBD.Text = "";
                            txtTenNoiDKKCBBD.Text = "";

                            //chkMienChiTraTrongNam.Checked = false;
                            //txtTenBHYTTinh.Text = "";
                        }

                        
                    }
                    
                }
                else if (MessageBox.Show("Bệnh nhân chưa có trong CSDL. Bạn có muốn thực hiện check thông tin trên cổng BHYT?", "Thông báo !", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    CheckTheBHYT2018(txt_SoCCCD.Text.Trim(), txtHoTen.Text.Trim().ToUpper(), txtNamSinh.Text);
                    if (MaKetQua != "000" & MaKetQua != "001" & MaKetQua != "002" & MaKetQua != "130")
                    {
                        //FrmMes frm = new FrmMes();
                        //frm._Mes = Mes;
                        //frm._GtTheTu = thebhyt2018.gtTheTu;
                        //frm._GtTheDen = thebhyt2018.gtTheDen;
                        //frm.ShowDialog();
                        MessageBox.Show(Mes);
                    }
                    else
                    {
                        txtMaThe.Text = thebhyt2018.maThe.Substring(0, 2) + "-" + thebhyt2018.maThe.Substring(2);
                        MaDTthe = thebhyt2018.maThe.Substring(0, 2);
                        dtBHTu.Value = Convert.ToDateTime(thebhyt2018.gtTheTu);
                        dtBHDen.Value = Convert.ToDateTime(thebhyt2018.gtTheDen);
                        txtManoiDKKCBBD.Text = thebhyt2018.maDKBD;
                        txtManoiDKKCBBD_Validated();// null/* TODO Change to default(_) if this is not a reference type */, null/* TODO Change to default(_) if this is not a reference type */);
                        cmbGioiTinh.SelectedIndex = thebhyt2018.gioiTinh.ToUpper() == "NAM" ? 0 : 1;
                        txtDiaChi.Text = thebhyt2018.diaChi;
                        Read_DiaChi_2_Combo(txtDiaChi.Text.Trim());
                        TgDu5Nam = Convert.ToDateTime(thebhyt2018.ngayDu5Nam);

                        if (MaDTthe.ToUpper() == "TE" | MaDTthe.ToUpper() == "CK" | MaDTthe.ToUpper() == "CC")
                            chkUuTien.Checked = true;

                        if (MaDTthe.ToUpper() == "HT" & thebhyt2018.maThe.Substring(2).Substring(0, 1) == "1")
                                    chkUuTien.Checked = true;
                        //if (Ngayhientai.Year - Convert.ToInt32(txtTuoi.Value) >= 80)
                        //    chkUuTien.Checked = true;
                        if (thebhyt2018.maKV == "K1" | thebhyt2018.maKV == "K2" | thebhyt2018.maKV == "K3")
                            khuVuc = thebhyt2018.maKV;
                        else
                        {
                            khuVuc = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Read_DiaChi_2_Combo(string DiaChi)
        {
            if ((DiaChi == ""))
            {
                lu_Tinh.EditValue = null;
                lu_Huyen.EditValue = null;
                lu_Xa.EditValue = null;
                return;
            }
            string[] strKeys;
            DiaChi = DiaChi.Replace("-", ",");
            DiaChi = DiaChi.Replace("X.", "Xã");
            DiaChi = DiaChi.Replace("H.", "Huyện");
            DiaChi = DiaChi.Replace("P.", "Phường");
            DiaChi = DiaChi.Replace("Q.", "Quận");
            DiaChi = DiaChi.Replace("TP.", "Thành phố");
            DiaChi = DiaChi.Replace("TP", "Thành phố");

            strKeys = DiaChi.Trim().Split(',');
            var i = 1;

            for (i = 1; i <= strKeys.Length - 1; i++)
            {
                if (strKeys[i].IndexOf("Tỉnh") >= 0 | strKeys[i].IndexOf("Thành phố") >= 0)
                {
                    string sTinh = strKeys[i].Replace("Tỉnh", "").Replace("Thành phố", "").Trim();
                    var iIndex = lu_Tinh.Properties.GetKeyValueByDisplayText(sTinh);
                    lu_Tinh.EditValue = iIndex;
                }
            }
            for (i = 1; i <= strKeys.Length - 1; i++)
            {
                if (strKeys[i].IndexOf("Quận") >= 0 | strKeys[i].IndexOf("Huyện") >= 0 | strKeys[i].IndexOf("Thành phố") >= 0)
                {
                    string sHuyen = strKeys[i].Trim();
                    var iIndex = lu_Huyen.Properties.GetKeyValueByDisplayText(sHuyen);
                    lu_Huyen.EditValue = iIndex;
                }
            }
            for (i = 1; i <= strKeys.Length - 1; i++)
            {
                if (strKeys[i].IndexOf("Xã") >= 0 | strKeys[i].IndexOf("Phường") >= 0 | strKeys[i].IndexOf("Thị trấn") >= 0)
                {
                    string sXa = strKeys[i].Trim();
                    var iIndex = lu_Xa.Properties.GetKeyValueByDisplayText(sXa);
                    lu_Xa.EditValue = iIndex;
                }
            }
        }

        private void txtManoiDKKCBBD_Validated()
        {
            SqlConnection CALL_ConnectSQL = new SqlConnection();
            CALL_ConnectSQL.ConnectionString = strConnectString;
            CALL_ConnectSQL.Open();
            string SQL;
            SqlCommand cmd;
            SqlDataReader rd;
            SqlDataAdapter Adap;
            DataSet Ds;
            try
            {
                
                if (txtManoiDKKCBBD.Text.Trim() == "")
                {
                    txtTenNoiDKKCBBD.Text = "";
                    return;
                }
                SQL = "Select * from DMNOIDKKCBBD_BHYT where Manoicap = '" + txtManoiDKKCBBD.Text.Trim() + "'";
                cmd = new SqlCommand(SQL, CALL_ConnectSQL);
                Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Ds = new DataSet();
                Adap.Fill(Ds, "DMNOIDKKCBBD_BHYT");
                if (Ds.Tables["DMNOIDKKCBBD_BHYT"].Rows.Count > 0)
                    txtTenNoiDKKCBBD.Text = Ds.Tables["DMNOIDKKCBBD_BHYT"].Rows[0]["Tennoicap"].ToString();
                else if (MessageBox.Show("Mã nơi đăng ký KCB ban đầu chưa có trong danh mục.Bạn có muốn thêm mới ?", "Thông báo!", MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    txtTenNoiDKKCBBD.Text = "";
                    txtTenNoiDKKCBBD.ReadOnly = false;
                    txtTenNoiDKKCBBD.Focus();
                }
                else
                {
                    txtManoiDKKCBBD.Text = "";
                    txtManoiDKKCBBD.Focus();
                    MessageBox.Show("Phải nhập đúng nơi đăng ký thẻ",  "Thông báo!");
                }
                // txtNoigioithieu.Text = txtTennoiDKKCBBD.Text
                Adap.Dispose();

                cmd.Dispose();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Public lskb As List(Of clsLichSuKCB)
        //public List<clsLichSuKCB2018> lskb2018;
        public string Mes = "";
        private bool CheckTheBHYT2018(string MaThe, string HoTen, string NgaySinh)
        {

            if (!GetSession())
                return false;
            client = new WebClient();
            values = new NameValueCollection();
            values.Add("maThe", MaThe);
            values.Add("hoTen", HoTen);
            values.Add("ngaySinh", NgaySinh);
            bool kq = true;
            byte[] response = client.UploadValues(string.Format("https://egw.baohiemxahoi.gov.vn/api/egw/NhanLichSuKCB2018?" + "token={0}&id_token={1}&username={2}&password={3}", Access_Token, IDToken, "36001_BV", getMd5Hash("123456")), values);
            string responseString = Encoding.UTF8.GetString(response);
            string[] SwitchKeys = new[] { "dsLichSuKCB2018\":", "dsLichSuKT2018\":" };
            string[] ValuesKeys = responseString.Split(SwitchKeys, StringSplitOptions.RemoveEmptyEntries);
            ValuesKeys[0] = ValuesKeys[0].Substring(0, ValuesKeys[0].Length - 2) + "}";
            thebhyt2018 = JsonConvert.DeserializeObject<clsTheBHYT2018>(ValuesKeys[0]);
            // Dim ns As String() = thebhyt2018.ngaySinh.Split["/"]
            // thebhyt2018.Day_NS = ns[0]
            // thebhyt2018.Month_NS = ns(1)
            // thebhyt2018.Year_NS = ns(2)
            MaKetQua = thebhyt2018.maKetQua;
            switch (MaKetQua)
            {
                case "401":
                    {
                        Mes = "Lỗi xác thực tại máy trạm";
                        kq = false;
                        break;
                    }

                case "500":
                    {
                        Mes = "Lỗi khi kết nối tới cổng giám định BHYT";
                        kq = false;
                        break;
                    }

                default:
                    {
                        ValuesKeys[1] = ValuesKeys[1].Substring(0, ValuesKeys[1].Length - 2);
                        ValuesKeys[2] = ValuesKeys[2].Substring(0, ValuesKeys[2].Length - 1);
                        // List<clsLichSuKCB2018> lskb2018 = JsonConvert.DeserializeObject<List<clsLichSuKCB2018>>(ValuesKeys[1]);
                        //List<clsLichSuKT2018> lskt2018 = JsonConvert.DeserializeObject<List<clsLichSuKT2018>>(ValuesKeys[2]);
                        switch (MaKetQua)
                        {
                            case "000":
                                {
                                    Mes = "Thông tin thẻ chính xác";
                                    break;
                                }

                            case "001":
                                {
                                    Mes = "Thẻ do Bộ quốc phòng quản lý, yêu cầu trình giấy tờ tùy thân";
                                    break;
                                }

                            case "002":
                                {
                                    Mes = "Thẻ do Bộ công an quản lý, yêu cầu trình giấy tờ tùy thân";
                                    break;
                                }

                            case "003":
                                {
                                    Mes = "Thẻ cũ hết giá trị sử dụng nhưng đã được cấp thẻ mới";
                                    break;
                                }

                            case "010":
                                {
                                    Mes = "Thẻ hết giá trị sử dụng";
                                    break;
                                }

                            case "051":
                                {
                                    Mes = "Mã thẻ không đúng";
                                    break;
                                }

                            case "052":
                                {
                                    Mes = "Mã tỉnh cấp thẻ (ký tự thứ 4,5 của mã thẻ) không đúng";
                                    break;
                                }

                            case "053":
                                {
                                    Mes = "Mã quyền lợi thẻ(ký tự thứ 3 của mã thẻ) không đúng";
                                    break;
                                }

                            case "050":
                                {
                                    Mes = "Không tìm thấy thông tin thẻ bhyt";
                                    break;
                                }

                            case "060":
                                {
                                    Mes = "Thẻ sai họ tên";
                                    break;
                                }

                            case "061":
                                {
                                    Mes = "Thẻ sai họ tên(Đúng ký tự đầu)";
                                    break;
                                }

                            case "070":
                                {
                                    Mes = "Thẻ sai ngày sinh";
                                    break;
                                }

                            case "100":
                                {
                                    Mes = "Lỗi khi lấy dữ liệu số thẻ";
                                    break;
                                }

                            case "101":
                                {
                                    Mes = "Lỗi máy chủ tại cổng giám định BHYT";
                                    break;
                                }

                            case "110":
                                {
                                    Mes = "Thẻ đã thu hồi";
                                    break;
                                }

                            case "120":
                                {
                                    Mes = "Thẻ đã báo giảm";
                                    break;
                                }

                            case "121":
                                {
                                    Mes = "Thẻ đã báo giảm. Giảm chuyển ngoại tỉnh";
                                    break;
                                }

                            case "122":
                                {
                                    Mes = "Thẻ đã báo giảm. Giảm chuyển nội tỉnh";
                                    break;
                                }

                            case "123":
                                {
                                    Mes = "Thẻ đã báo giảm. Thu hồi do tăng tại cùng đơn vị";
                                    break;
                                }

                            case "124":
                                {
                                    Mes = "Thẻ đã báo giảm. Ngừng tham gia";
                                    break;
                                }

                            case "130":
                                {
                                    Mes = "Trẻ em không xuất trình thẻ";
                                    break;
                                }

                            default:
                                {
                                    Mes = "Lỗi không xác định";
                                    break;
                                }
                        }
                        break;
                    }
            }
            return kq;
        }

        private bool CheckTheBHYT_2021(string MaThe, string HoTen, string NgaySinh)
        {

            if (!GetSession_2021())
                return false;
            client = new WebClient();
            values = new NameValueCollection();
            values.Add("maThe", MaThe);
            values.Add("hoTen", HoTen);
            values.Add("ngaySinh", NgaySinh);
            bool kq = true;
            byte[] response = client.UploadValues(string.Format("http://ctndaotao.bhxh.gov.vn/api/egw/NhanLichSuKCB2018?" + "token={0}&id_token={1}&username={2}&password={3}", Access_Token, IDToken, "36001_BV", getMd5Hash("123456")), values);
            string responseString = Encoding.UTF8.GetString(response);
            string[] SwitchKeys = new[] { "dsLichSuKCB2018\":", "dsLichSuKT2018\":" };
            string[] ValuesKeys = responseString.Split(SwitchKeys, StringSplitOptions.RemoveEmptyEntries);
            ValuesKeys[0] = ValuesKeys[0].Substring(0, ValuesKeys[0].Length - 2) + "}";
            thebhyt2018 = JsonConvert.DeserializeObject<clsTheBHYT2018>(ValuesKeys[0]);
            // Dim ns As String() = thebhyt2018.ngaySinh.Split["/"]
            // thebhyt2018.Day_NS = ns[0]
            // thebhyt2018.Month_NS = ns(1)
            // thebhyt2018.Year_NS = ns(2)
            MaKetQua = thebhyt2018.maKetQua;
            switch (MaKetQua)
            {
                case "401":
                    {
                        Mes = "Lỗi xác thực tại máy trạm";
                        kq = false;
                        break;
                    }

                case "500":
                    {
                        Mes = "Lỗi khi kết nối tới cổng giám định BHYT";
                        kq = false;
                        break;
                    }

                default:
                    {
                        ValuesKeys[1] = ValuesKeys[1].Substring(0, ValuesKeys[1].Length - 2);
                        ValuesKeys[2] = ValuesKeys[2].Substring(0, ValuesKeys[2].Length - 1);
                        // List<clsLichSuKCB2018> lskb2018 = JsonConvert.DeserializeObject<List<clsLichSuKCB2018>>(ValuesKeys[1]);
                        //List<clsLichSuKT2018> lskt2018 = JsonConvert.DeserializeObject<List<clsLichSuKT2018>>(ValuesKeys[2]);
                        switch (MaKetQua)
                        {
                            case "000":
                                {
                                    Mes = "Thông tin thẻ chính xác";
                                    break;
                                }

                            case "001":
                                {
                                    Mes = "Thẻ do Bộ quốc phòng quản lý, yêu cầu trình giấy tờ tùy thân";
                                    break;
                                }

                            case "002":
                                {
                                    Mes = "Thẻ do Bộ công an quản lý, yêu cầu trình giấy tờ tùy thân";
                                    break;
                                }

                            case "003":
                                {
                                    Mes = "Thẻ cũ hết giá trị sử dụng nhưng đã được cấp thẻ mới";
                                    break;
                                }

                            case "010":
                                {
                                    Mes = "Thẻ hết giá trị sử dụng";
                                    break;
                                }

                            case "051":
                                {
                                    Mes = "Mã thẻ không đúng";
                                    break;
                                }

                            case "052":
                                {
                                    Mes = "Mã tỉnh cấp thẻ (ký tự thứ 4,5 của mã thẻ) không đúng";
                                    break;
                                }

                            case "053":
                                {
                                    Mes = "Mã quyền lợi thẻ(ký tự thứ 3 của mã thẻ) không đúng";
                                    break;
                                }

                            case "050":
                                {
                                    Mes = "Không tìm thấy thông tin thẻ bhyt";
                                    break;
                                }

                            case "060":
                                {
                                    Mes = "Thẻ sai họ tên";
                                    break;
                                }

                            case "061":
                                {
                                    Mes = "Thẻ sai họ tên(Đúng ký tự đầu)";
                                    break;
                                }

                            case "070":
                                {
                                    Mes = "Thẻ sai ngày sinh";
                                    break;
                                }

                            case "100":
                                {
                                    Mes = "Lỗi khi lấy dữ liệu số thẻ";
                                    break;
                                }

                            case "101":
                                {
                                    Mes = "Lỗi máy chủ tại cổng giám định BHYT";
                                    break;
                                }

                            case "110":
                                {
                                    Mes = "Thẻ đã thu hồi";
                                    break;
                                }

                            case "120":
                                {
                                    Mes = "Thẻ đã báo giảm";
                                    break;
                                }

                            case "121":
                                {
                                    Mes = "Thẻ đã báo giảm. Giảm chuyển ngoại tỉnh";
                                    break;
                                }

                            case "122":
                                {
                                    Mes = "Thẻ đã báo giảm. Giảm chuyển nội tỉnh";
                                    break;
                                }

                            case "123":
                                {
                                    Mes = "Thẻ đã báo giảm. Thu hồi do tăng tại cùng đơn vị";
                                    break;
                                }

                            case "124":
                                {
                                    Mes = "Thẻ đã báo giảm. Ngừng tham gia";
                                    break;
                                }

                            case "130":
                                {
                                    Mes = "Trẻ em không xuất trình thẻ";
                                    break;
                                }

                            default:
                                {
                                    Mes = "Lỗi không xác định";
                                    break;
                                }
                        }
                        break;
                    }
            }
            return kq;
        }

        private void txt_SoCCCD_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txt_SoCCCD.Text.Trim() != "" && txt_SoCCCD.Text.Trim().Split('|').Length == 7)
            {
                Read_TheCCCD(txt_SoCCCD.Text.Trim());
            }
        }

        string strNumber = "0123456789";
        private void txtMaThe_Leave(object sender, EventArgs e)
        {
            if (txtMaThe.Text.Length == 15 && strNumber.IndexOf(txtMaThe.Text.Trim().Substring(0, 1)) < 0 && strNumber.IndexOf(txtMaThe.Text.Trim().Substring(1, 1)) < 0)
            {
                LoadData_Old(txtMaThe.Text.Trim().Substring(0, 2), txtMaThe.Text.Trim().Substring(2));
            }
        }

        int ViTri = 0;
        string str = "", tenBHYTTinh = "";

        private void btn_XacNhan_Click(object sender, EventArgs e)
        {
            //1. check dieu kien hop le
            if (!check_hople()) return;
            //2. them phieu kham
            ThemPhieuKham tpk = new ThemPhieuKham();
            tpk.MaKhambenh = "";

            tpk.MaBenhnhan = txtMaBenhNhan.Text.Trim();
            tpk.TenBenhnhan = txtHoTen.Text.Trim().ToUpper();
            tpk.NamSinh = Convert.ToInt32(txtNamSinh.Text);
            tpk.ThangSinh = Convert.ToInt32(txtThangSinh.Text);
            tpk.NgaySinh = Convert.ToInt32(txtNgaySinh.Text);
            tpk.GioiTinh = cmbGioiTinh.Text.ToUpper() == "NAM" ? 1 : 0;

            tpk.ThoigianDangky = DateTime.Now;
            tpk.Doituong = txtMaThe.Text.Trim() != "" ? "1" : "3";
            tpk.Diachi = txtDiaChi.Text.Trim();
            tpk.Noicongtac = noicongtac;
            tpk.Tuyen = 0;
            tpk.SotheBhyt = txtMaThe.Text.Trim().Split('-')[1];
            tpk.Doituongthe = MaDTthe;
            tpk.NoidangkyKcbbd = txtManoiDKKCBBD.Text;
            if (txtMaThe.Text.Trim() != "")
                tpk.HantheBhytTu = dtBHTu.Value;
            else
                tpk.HantheBhytTu = null;

            if (txtMaThe.Text.Trim() != "")
                tpk.HantheBhytDen = dtBHDen.Value;
            else
                tpk.HantheBhytDen = null;

            tpk.NoicaptheBhyt = TenBHYTTinh;
            tpk.Nghenghiep = slu_NgheNghiep.EditValue.ToString();
            tpk.SotheTe = "";
            tpk.Lienhe = "";
            tpk.Noigioithieu = "";
            tpk.ChandoanNgt = "";
            tpk.DaTinhPhi = 0;
            tpk.NhanvienCd = "";
            tpk.IsTruc = 0;
            tpk.HkNgayHenKham = null;
            tpk.HkSoHoSo = "";
            tpk.MaBenhId = 0;
            tpk.UuTien = chkUuTien.Checked;
            tpk.TgDu5Nam = TgDu5Nam;
            tpk.MienChiTraTrongNam = MienChiTraTrongNam;
            tpk.ThoiDiemMienChiTraTrongNam = TgMienChiTraTrongNam;
            tpk.MaNoiChuyen = "";
            tpk.Thoigianthanhtoan = null;
            tpk.DaInTongKet = 0;
            tpk.MaMucHuong = 1;
            tpk.SoBhxh = txtMaThe.Text.Trim().Split('-')[1].Substring(3);
            tpk.SoCccd = txt_SoCCCD.Text.Trim();
            tpk.SoDienThoai = txt_SoDienThoai.Text.Trim();
            tpk.IsDeleted = 0;
            tpk.DeletedUname = "";
            tpk.DeletedTime = null;
            tpk.MaQuocTich = lu_QuocTich.EditValue.ToString();
            tpk.MaTinh = lu_Tinh.EditValue.ToString();
            tpk.MaHuyen = lu_Huyen.EditValue.ToString();
            tpk.MaXa = lu_Xa.EditValue.ToString();
            tpk.MaDanToc = lu_DanToc.EditValue.ToString();
            tpk.LoaiTuyen = 1;
            tpk.SoChuyenTuyen = "";
            tpk.MaDoiTuongKcb = "1.1";
            tpk.MaLoaiKcb = "01";
            tpk.SoPhieuHenKhamLai = "";
            tpk.MaKhuVuc = khuVuc;
            tpk.GhiChu = "";
            tpk.YeuCauKham = maDichVu;
            tpk.PhongKham = maPhongKham;
            string url = "http://localhost:5292/themphieukham";
            HttpClient _httpClient = new HttpClient();
            string str_json = JsonConvert.SerializeObject(tpk);
            using (var content = new StringContent(JsonConvert.SerializeObject(tpk), System.Text.Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result = _httpClient.PostAsync(url, content).Result;
                if(result.StatusCode.ToString() == "200")
                {
                    dynamic json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result);
                    var qrObject = JsonConvert.DeserializeObject<ResultValues>(result.Content.ReadAsStringAsync().Result);
                    txtMaBenhNhan.Text = qrObject.MaBenhNhan;
                    txtMaKhamBenh.Text = qrObject.MaKhamBenh;
                    MessageBox.Show("Thực hiện đăng ký khám thành công.!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    //In phieu kham benh
                }
                else
                {
                    MessageBox.Show("Thực hiện đăng ký khám không thành công. Vui lòng kiểm tra lại thông tin!","", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                

            }
            //3. luu khuon mat vao he thong -> neu chua co
        }

        public class ResultValues
        {
            public string MaBenhNhan { get; set; }
            public string MaKhamBenh { get; set; }
        }
        private bool check_hople()
        {
            bool re_value = true;
            if (txt_SoCCCD.Text.Trim() == "" || txtMaThe.Text.Trim() == "")
            {
                MessageBox.Show("Bắt buộc phải có thông tin Số CCCD hoặc Số thẻ BHYT.");
                txt_SoCCCD.Focus();
                return false;
            }

            if (txtHoTen.Text.Trim() == "")
            {
                MessageBox.Show("Chưa có thông tin Họ Tên của người bệnh.");
                txtHoTen.Focus();
                return false;
            }

            if (Convert.ToInt32(txtNamSinh.Text.Trim()) > DateTime.Now.Year)
            {
                MessageBox.Show("Năm sinh không được lớn hơn năm hiện tại.");
                txtNamSinh.Focus();
                return false;
            }

            if ((lu_Tinh.EditValue == null || lu_Tinh.EditValue.ToString() == "000") 
                || (lu_Huyen.EditValue == null || lu_Huyen.EditValue.ToString() == "000")
                || (lu_Xa.EditValue == null || lu_Xa.EditValue.ToString() == "000"))
            {
                MessageBox.Show("Yêu cầu nhập đủ thông tin: Tỉnh - Huyện - Xã.");
                txtNamSinh.Focus();
                return false;
            }    

            if(maPhongKham == "" || maDichVu == "")
            {
                MessageBox.Show("Chưa chọn phòng khám để đăng ký khám.");
                return false;
            }    
            return re_value;
        }
        private void txt_SoCCCD_KeyDown(object sender, KeyEventArgs e)
        {
            
                if (txt_SoCCCD.Text.Trim() != "" & txt_SoCCCD.Text.Trim().Split('|').Length == 7)
                    Read_TheCCCD(txt_SoCCCD.Text.Trim());
                else if (e.KeyCode == Keys.Enter & txt_SoCCCD.Text.Trim() != "" & txt_SoCCCD.Text.Trim().Split('|').Length == 7)
                    Read_TheCCCD(txt_SoCCCD.Text.Trim());
        }

        private void grv_PhongKham_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (grv_PhongKham.RowCount < 1) return;
            maPhongKham = grv_PhongKham.GetFocusedRowCellValue("MaKhoa").ToString();
            maDichVu = grv_PhongKham.GetFocusedRowCellValue("MaDichvu").ToString();
        }

        private void lu_Tinh_EditValueChanged(object sender, EventArgs e)
        {
            SqlConnection CALL_ConnectSQL = new SqlConnection();
            CALL_ConnectSQL.ConnectionString = strConnectString;
            CALL_ConnectSQL.Open();
            string SQL = "";
            SqlCommand Cmd = default(SqlCommand);
            DataSet DsDM = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
           
            //4. DM Huyen
            SQL = "Select Ma_XML As MaQuanHuyen,TenQuanHuyen from NamDinh_SYSDB.dbo.DMQuanHuyen WHERE MaTinh = '" + lu_Tinh.EditValue.ToString() + "'  order by Ma_XML";
            Cmd = new SqlCommand(SQL, CALL_ConnectSQL);
            da.SelectCommand = Cmd;
            da.Fill(DsDM, "DmHuyen");
            lu_Huyen.Properties.DataSource = DsDM.Tables["DmHuyen"];
            lu_Huyen.EditValue = "000";
            Cmd.Dispose();
            da.Dispose();
            DsDM.Dispose();
            CALL_ConnectSQL.Close();
        }

        private void lu_Huyen_EditValueChanged(object sender, EventArgs e)
        {
            SqlConnection CALL_ConnectSQL = new SqlConnection();
            CALL_ConnectSQL.ConnectionString = strConnectString;
            CALL_ConnectSQL.Open();
            string SQL = "";
            SqlCommand Cmd = default(SqlCommand);
            DataSet DsDM = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            //4. DM xA
            SQL = "Select Ma_XML As MaXaPhuong,TenXaPhuog As TenXaPhuong from NamDinh_SYSDB.dbo.DMXaPhuong Where MaHuyen = '" + lu_Huyen.EditValue.ToString() + "'  order by Ma_XML";
            Cmd = new SqlCommand(SQL, CALL_ConnectSQL);
            da.SelectCommand = Cmd;
            da.Fill(DsDM, "DmXa");
            lu_Xa.Properties.DataSource = DsDM.Tables["DmXa"];
            lu_Xa.EditValue = "000";
            Cmd.Dispose();
            da.Dispose();
            DsDM.Dispose();
            CALL_ConnectSQL.Close();
        }

        private void txtMaThe_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (txtMaThe.Text.Trim().Length == 0)
                    return;
                if (txtMaThe.Text.Trim().Substring(txtMaThe.Text.Trim().Length - 1) == "$")
                {
                    //Kiem tra xem la moi hay cu
                    if (txtMaThe.Text.Trim().Split('|').Length == 17)
                    {
                        Read_TheBHYT_New(txtMaThe.Text.Trim());
                    }
                    else
                    {
                        ThongTinThe = txtMaThe.Text.Trim();
                        ViTri = ThongTinThe.IndexOf("|");
                        MaDTthe = ThongTinThe.Substring(0, 2);
                        SotheBHYT = ThongTinThe.Substring(2, ViTri - 2);
                        txtMaThe.Text = MaDTthe + "-" + SotheBHYT;

                        if(LoadData_Old(MaDTthe, SotheBHYT)) return; //Neu da co du lieu cu thi bo qua luon
                        if (SotheBHYT.Length > 13)
                        {
                            //txtMaThe.Text = SotheBHYT;
                            tenBHYTTinh = LayTenCoquanBHYT(SotheBHYT.Substring(1, 2));

                            ThongTinThe = ThongTinThe.Substring(ViTri + 1);
                            ViTri = ThongTinThe.IndexOf("|");
                            HoTen = FromHex2Unicode(ThongTinThe.Substring(0, ViTri));
                            txtHoTen.Text = HoTen.ToUpper();
                            ThongTinThe = ThongTinThe.Substring(ViTri + 1);
                            if (ThongTinThe.IndexOf("|") > 5)
                            {
                                NamSinh = Convert.ToInt32(ThongTinThe.Substring(6, 4));
                                NgaySinh = Convert.ToInt32(ThongTinThe.Substring(0, 2));
                                txtNgaySinh.Text = NgaySinh.ToString();
                                ThangSinh = Convert.ToInt32(ThongTinThe.Substring(3, 2));
                                txtThangSinh.Text = ThangSinh.ToString();
                            }
                            else
                                NamSinh = Convert.ToInt32(ThongTinThe.Substring(0, 4));
                            txtNamSinh.Text = NamSinh.ToString();
                            ViTri = ThongTinThe.IndexOf("|");
                            ThongTinThe = ThongTinThe.Substring(ViTri + 1);
                            GioiTinh = Convert.ToInt32(Convert.ToDouble(ThongTinThe.Substring(0, 1)) - (double)1);
                            cmbGioiTinh.SelectedIndex = GioiTinh;
                            ViTri = ThongTinThe.IndexOf("|");
                            ThongTinThe = ThongTinThe.Substring(ViTri + 1);
                            ViTri = ThongTinThe.IndexOf("|");
                            DiaChi = FromHex2Unicode(ThongTinThe.Substring(0, ViTri));
                            ThongTinThe = ThongTinThe.Substring(ViTri + 1);
                            ViTri = ThongTinThe.IndexOf("|");
                            NoiDKKCBBD = ThongTinThe.Substring(0, ViTri - 1).Replace(" ", "").Replace("-", "");

                            ThongTinThe = ThongTinThe.Substring(ViTri + 1);
                            ViTri = ThongTinThe.IndexOf("|");
                            HanTu = Convert.ToDateTime(ThongTinThe.Substring(0, ViTri - 1));
                            ThongTinThe = ThongTinThe.Substring(ViTri + 1);
                            ViTri = ThongTinThe.IndexOf("|");
                            HanDen = new DateTime(Convert.ToInt32(ThongTinThe.Substring(6, 4)), Convert.ToInt32(ThongTinThe.Substring(3, 2)), Convert.ToInt32(ThongTinThe.Substring(0, 2))); ;// Convert.ToDateTime(ThongTinThe.Substring(0, ViTri - 1));
                            int i;
                            for (i = 0; i <= 4; i++)
                            {
                                ViTri = ThongTinThe.IndexOf("|");
                                ThongTinThe = ThongTinThe.Substring(ViTri + 1).Trim();
                            }
                            TgDu5Nam = new DateTime(Convert.ToInt32(ThongTinThe.Substring(6, 4)), Convert.ToInt32(ThongTinThe.Substring(3, 2)), Convert.ToInt32(ThongTinThe.Substring(0, 2))); ;// Convert.ToDateTime(ThongTinThe.Substring(0, 10));
                        }
                        else
                        {

                            txtMaThe.Text = SotheBHYT;
                            //tenBHYTTinh = LayTenCoquanBHYT(SotheBHYT.Substring(1, 2));
                            ThongTinThe = ThongTinThe.Substring(ViTri + 1);
                            ViTri = ThongTinThe.IndexOf("|");
                            HoTen = FromHex(ThongTinThe.Substring(0, ViTri));
                            txtHoTen.Text = HoTen.ToUpper();
                            ThongTinThe = ThongTinThe.Substring(ViTri + 1);
                            if (ThongTinThe.IndexOf("|") > 5)
                            {
                                NamSinh = Convert.ToInt32(ThongTinThe.Substring(6, 4));
                                NamSinh = Convert.ToInt32(ThongTinThe.Substring(6, 4));
                                NgaySinh = Convert.ToInt32(ThongTinThe.Substring(0, 2));
                                txtNgaySinh.Text = NgaySinh.ToString();
                                ThangSinh = Convert.ToInt32(ThongTinThe.Substring(3, 2));
                                txtThangSinh.Text = ThangSinh.ToString();
                            }
                            else
                                NamSinh = Convert.ToInt32(ThongTinThe.Substring(0, 4));
                            txtNamSinh.Text = NamSinh.ToString();

                            ViTri = ThongTinThe.IndexOf("|");
                            ThongTinThe = ThongTinThe.Substring(ViTri + 1);
                            GioiTinh = Convert.ToInt32(Convert.ToDouble(ThongTinThe.Substring(0, 1)) - (double)1);
                            cmbGioiTinh.SelectedIndex = GioiTinh;
                            ViTri = ThongTinThe.IndexOf("|");
                            ThongTinThe = ThongTinThe.Substring(ViTri + 1);
                            ViTri = ThongTinThe.IndexOf("|");
                            DiaChi = FromHex(ThongTinThe.Substring(0, ViTri));
                            txtDiaChi.Text = DiaChi;
                            ThongTinThe = ThongTinThe.Substring(ViTri + 1);
                            ViTri = ThongTinThe.IndexOf("|");
                            NoiDKKCBBD = ThongTinThe.Substring(0, ViTri).Replace(" ", "").Replace("-", "");
                            txtManoiDKKCBBD.Text = NoiDKKCBBD;
                            ThongTinThe = ThongTinThe.Substring(ViTri + 1);
                            ViTri = ThongTinThe.IndexOf("|");
                            string strTu = ThongTinThe.Substring(0, ViTri);
                            string[] strHanTu = strTu.Split('/');
                            HanTu = new DateTime(Convert.ToInt32(strHanTu[2]), Convert.ToInt32(strHanTu[1]), Convert.ToInt32(strHanTu[0]));
                            dtBHTu.Value = HanTu;
                            if (ThongTinThe.Substring(ViTri + 1, 1) == "-")
                            {
                                ThongTinThe = ThongTinThe.Substring(ViTri + 3);
                                ViTri = ThongTinThe.IndexOf("|");
                                string strDen = ThongTinThe.Substring(0, ViTri);
                                string[] strHanDen = strTu.Split('/');
                                HanDen = new DateTime(Convert.ToInt32(ThongTinThe.Substring(6, 4)), Convert.ToInt32(ThongTinThe.Substring(3, 2)), Convert.ToInt32(ThongTinThe.Substring(0, 2))); // Convert.ToDateTime(ThongTinThe.Substring(0, ViTri));
                                dtBHDen.Value = HanDen;                                                                                                                                                                 // dtBHDen.Value = HanDen
                                int i;
                                for (i = 0; i < 4; i++)
                                {
                                    ViTri = ThongTinThe.IndexOf("|");
                                    ThongTinThe = ThongTinThe.Substring(ViTri + 1).Trim();
                                }
                                TgDu5Nam = new DateTime(Convert.ToInt32(ThongTinThe.Substring(6, 4)), Convert.ToInt32(ThongTinThe.Substring(3, 2)), Convert.ToInt32(ThongTinThe.Substring(0, 2)));// Convert.ToDateTime(ThongTinThe.Substring(0, 10));
                            }
                            else
                            {
                                ThongTinThe = ThongTinThe.Substring(ViTri + 1);
                                ViTri = ThongTinThe.IndexOf("|");
                                string strDen = ThongTinThe.Substring(0, ViTri);
                                string[] strHanDen = strTu.Split('/');
                                HanDen = new DateTime(Convert.ToInt32(ThongTinThe.Substring(6, 4)), Convert.ToInt32(ThongTinThe.Substring(3, 2)), Convert.ToInt32(ThongTinThe.Substring(0, 2))); // Convert.ToDateTime(ThongTinThe.Substring(0, ViTri));
                                dtBHDen.Value = HanDen;
                                ThongTinThe = ThongTinThe.Substring(ViTri + 11);
                                // dtBHDen.Value = HanDen
                                int i;
                                for (i = 0; i < 4; i++)
                                {
                                    ViTri = ThongTinThe.IndexOf("|");
                                    ThongTinThe = ThongTinThe.Substring(ViTri + 1).Trim();
                                }
                                TgDu5Nam = new DateTime(Convert.ToInt32(ThongTinThe.Substring(6, 4)), Convert.ToInt32(ThongTinThe.Substring(3, 2)), Convert.ToInt32(ThongTinThe.Substring(0, 2)));// Convert.ToDateTime(ThongTinThe.Substring(0, 10));
                            }

                        }
                    }

                    //if (isUTien)
                    //{
                    //    Luu_Capso(4);
                    //}
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public string FromHex2Unicode(string Text)
        {
            if (Text == null || Text.Length == 0)
                return string.Empty;

            List<byte> Bytes = new List<byte>();
            var loopTo = Text.Length - 1;
            for (int Index = 0; Index <= loopTo; Index += 2)
                Bytes.Add(Convert.ToByte(Text.Substring(Index, 2), 16));

            System.Text.Encoding E = System.Text.Encoding.Unicode;
            return E.GetString(Bytes.ToArray());
        }

        public string FromHex(string Text)
        {
            if (Text == null || Text.Length == 0)
                return string.Empty;

            List<byte> Bytes = new List<byte>();
            var loopTo = Text.Length - 1;
            for (int Index = 0; Index < loopTo; Index += 2)
                Bytes.Add(Convert.ToByte(Text.Substring(Index, 2), 16));

            System.Text.Encoding E = System.Text.Encoding.UTF8;
            return E.GetString(Bytes.ToArray());
        }

        public string LayTenCoquanBHYT(string Ma)
        {
            SqlConnection CALL_ConnectSQL = new SqlConnection();
            CALL_ConnectSQL.ConnectionString = strConnectString;
            CALL_ConnectSQL.Open();
            string LayTenCoquanBHYTRet = default(string);
            string SQL = default(string);
            SqlCommand Cmd = default(SqlCommand);
            SqlDataReader Dr = default(SqlDataReader);
            LayTenCoquanBHYTRet = "";
            try
            {
                SQL = "Select Ten from NamDinh_Sysdb.dbo.DMCOQUANBHYT where Ma = @Ma";
                Cmd = new SqlCommand(SQL, CALL_ConnectSQL);
                Cmd.Parameters.Clear();
                Cmd.Parameters.AddWithValue("@Ma", Ma);
                Dr = Cmd.ExecuteReader();
                if (!Dr.HasRows)
                {
                    Dr.Close();
                    return LayTenCoquanBHYTRet;
                }
                else
                {
                    Dr.Read();
                    LayTenCoquanBHYTRet = Dr["Ten"].ToString();
                    Dr.Close();
                }
                Dr.Close();
                Cmd.Dispose();
                CALL_ConnectSQL.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Dr.Close();
                Cmd.Dispose();
                CALL_ConnectSQL.Close();
            }
            return LayTenCoquanBHYTRet;
        }

        private string getMd5Hash(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i <= data.Length - 1; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }

        private bool GetSession()
        {
            client = new WebClient();
            values = new NameValueCollection();
            values.Add("username", "36001_BV");
            values.Add("password", getMd5Hash("123456"));
            byte[] response = client.UploadValues("https://egw.baohiemxahoi.gov.vn/api/token/take", values);
            string responseString = Encoding.UTF8.GetString(response);
            string[] SwitchKeys = new[] { "\"maKetQua\"", "\"APIKey\"", "\"access_token\"", "\"id_token\"", "\"token_type\"" };
            string[] ValuesKeys = responseString.Split(SwitchKeys, StringSplitOptions.None);
            MaKetQua = ValuesKeys[1].Substring(2, 3);
            bool kq = true;
            switch (MaKetQua)
            {
                case "200":
                    {
                        Access_Token = ValuesKeys[3].Substring(0, ValuesKeys[3].Length - 2).Substring(2, ValuesKeys[3].Length - 4);
                        IDToken = ValuesKeys[4].Substring(0, ValuesKeys[4].Length - 2).Substring(2, ValuesKeys[4].Length - 4);
                        break;
                    }

                case "401":
                    {
                        Mes = "Lỗi xác thực tại máy trạm";
                        kq = false;
                        break;
                    }

                case "500":
                    {
                        Mes = "Lỗi khi kết nối tới cổng giám định BHYT";
                        kq = false;
                        break;
                    }

                default:
                    {
                        Mes = "Lỗi không xác định";
                        kq = false;
                        break;
                    }
            }
            return kq;
        }


        private bool GetSession_2021()
        {
            client = new WebClient();
            values = new NameValueCollection();
            values.Add("username", "36001_BV");
            values.Add("password", getMd5Hash("123456"));
            byte[] response = client.UploadValues("http://ctndaotao.bhxh.gov.vn/api/token/take", values);
            string responseString = Encoding.UTF8.GetString(response);
            string[] SwitchKeys = new[] { "\"maKetQua\"", "\"APIKey\"", "\"access_token\"", "\"id_token\"", "\"token_type\"" };
            string[] ValuesKeys = responseString.Split(SwitchKeys, StringSplitOptions.None);
            MaKetQua = ValuesKeys[1].Substring(2, 3);
            bool kq = true;
            switch (MaKetQua)
            {
                case "200":
                    {
                        Access_Token = ValuesKeys[3].Substring(0, ValuesKeys[3].Length - 2).Substring(2, ValuesKeys[3].Length - 4);
                        IDToken = ValuesKeys[4].Substring(0, ValuesKeys[4].Length - 2).Substring(2, ValuesKeys[4].Length - 4);
                        break;
                    }

                case "401":
                    {
                        Mes = "Lỗi xác thực tại máy trạm";
                        kq = false;
                        break;
                    }

                case "500":
                    {
                        Mes = "Lỗi khi kết nối tới cổng giám định BHYT";
                        kq = false;
                        break;
                    }

                default:
                    {
                        Mes = "Lỗi không xác định";
                        kq = false;
                        break;
                    }
            }
            return kq;
        }

        private bool LoadData_Old(string MaDT, string MaThe)
        {
            isUTien = false;
            SqlConnection CALL_ConnectSQL = new SqlConnection();
            CALL_ConnectSQL.ConnectionString = strConnectString;
            CALL_ConnectSQL.Open();
            SqlDataAdapter Adap;
            DataSet ds;
            string LayTenCoquanBHYTRet = default(string);
            string SQL = default(string);
            SqlCommand Cmd = default(SqlCommand);
            LayTenCoquanBHYTRet = "";
            try
            {
                SQL = "SELECT top 1 tblBenhnhan.*,tblKhambenh.*,isnull(su.TenDu,'') as TenDayDu, "
                        + " isnull(So_BHXH,'') As SoBHXH,isnull(So_CCCD,'') As SoCCCD,isnull(So_Dien_Thoai,'') As SoDienThoai,"
                        + " DMDTTHE_BHYT.TenDT as TenDTThe,DMNOIDKKCBBD_BHYT.Tennoicap"
                        + " FROM  tblkhambenh "
                        + " left join tblBenhnhan on tblKhambenh.Mabenhnhan = tblBenhnhan.Mabenhnhan "
                        + " left join DMDTTHE_BHYT on tblKhambenh.Doituongthe  = DMDTTHE_BHYT.MaDT "
                        + " left join DMNOIDKKCBBD_BHYT on tblKhambenh.NoidangkyKCBBD  = DMNOIDKKCBBD_BHYT.Manoicap "
                        + " left join DMDOITUONGBN on tblKhambenh.Doituong  = DMDOITUONGBN.MaDT "
                        + " LEFT JOIN SYSUSER su ON su.UName = tblkhambenh.NhanvienCD "
                        + " left join DMNGHENGHIEP on tblKhambenh.Nghenghiep  = DMNGHENGHIEP.MaNghenghiep "
                        + " where rtrim(Doituongthe) ='"  + MaDT + "' AND rtrim(SotheBHYT) = '" + MaThe + "'  Order by Thoigiandangky DESC";

                Cmd = new SqlCommand(SQL, CALL_ConnectSQL);
                Adap = new SqlDataAdapter();
                Adap.SelectCommand = Cmd;
                ds = new DataSet();
                Adap.Fill(ds, "Hoso");
                Adap.Dispose();
                Cmd.Dispose();
                CALL_ConnectSQL.Close();
                if (ds.Tables["Hoso"].Rows.Count > 0)
                {

                    // Fill Benh nhan
                    txtMaBenhNhan.Text = ds.Tables["Hoso"].Rows[0]["MaBenhnhan"].ToString();
                    txtHoTen.Text = ds.Tables["Hoso"].Rows[0]["TenBenhnhan"].ToString();
                    txtNamSinh.Text = ds.Tables["Hoso"].Rows[0]["Namsinh"].ToString();
                    cmbGioiTinh.SelectedIndex = ds.Tables["Hoso"].Rows[0]["Gioitinh"].ToString() == "1" ? 0 : 1;
                    
                    txtDiaChi.Text = ds.Tables["Hoso"].Rows[0]["Diachi"].ToString();
                    if (ds.Tables["Hoso"].Rows[0]["Nghenghiep"] == DBNull.Value || ds.Tables["Hoso"].Rows[0]["Nghenghiep"].ToString() == "")
                        slu_NgheNghiep.EditValue = "00000";
                    else
                        slu_NgheNghiep.EditValue = ds.Tables["Hoso"].Rows[0]["Nghenghiep"].ToString();

                    noicongtac = ds.Tables["Hoso"].Rows[0]["Noicongtac"].ToString() ;
                    lienhe = ds.Tables["Hoso"].Rows[0]["Lienhe"].ToString();

                    txt_SoDienThoai.Text = ds.Tables["Hoso"].Rows[0]["SoDienThoai"].ToString();
                    txt_SoCCCD.Text = ds.Tables["Hoso"].Rows[0]["SoCCCD"].ToString();
                    
                    chkUuTien.Checked = ds.Tables["Hoso"].Rows[0]["UuTien"].ToString() == "1" ? true : false;
                    if (ds.Tables["Hoso"].Rows[0]["NgaySinh"] == DBNull.Value || ds.Tables["Hoso"].Rows[0]["NgaySinh"].ToString() == "")
                        txtNgaySinh.Text = "" ;
                    else
                        txtNgaySinh.Text = ds.Tables["Hoso"].Rows[0]["NgaySinh"].ToString();
                    if (ds.Tables["Hoso"].Rows[0]["ThangSinh"] == DBNull.Value || ds.Tables["Hoso"].Rows[0]["ThangSinh"].ToString() == "")
                        txtThangSinh.Text = "";
                    else
                        txtThangSinh.Text = ds.Tables["Hoso"].Rows[0]["ThangSinh"].ToString();
                    
                    
                    if (ds.Tables["Hoso"].Rows[0]["Ma_QuocTich"] != null)
                        lu_QuocTich.EditValue = ds.Tables["Hoso"].Rows[0]["Ma_QuocTich"];
                    else
                        lu_QuocTich.EditValue = "000";

                    if (ds.Tables["Hoso"].Rows[0]["Ma_DanToc"] != null)
                        lu_DanToc.EditValue = ds.Tables["Hoso"].Rows[0]["Ma_DanToc"];
                    else
                        lu_DanToc.EditValue = "22";

                    if (ds.Tables["Hoso"].Rows[0]["Ma_Tinh"] != null)
                        lu_Tinh.EditValue = ds.Tables["Hoso"].Rows[0]["Ma_Tinh"];
                    else
                        lu_Tinh.EditValue = 0;
                    if (ds.Tables["Hoso"].Rows[0]["Ma_Huyen"] != null)
                        lu_Huyen.EditValue = ds.Tables["Hoso"].Rows[0]["Ma_Huyen"];
                    else
                        lu_Huyen.EditValue = 0;

                    if (ds.Tables["Hoso"].Rows[0]["Ma_Xa"] != null)
                        lu_Xa.EditValue = ds.Tables["Hoso"].Rows[0]["Ma_Xa"];
                    else
                        lu_Xa.EditValue = 0;

                    if (ds.Tables["Hoso"].Rows[0]["MaKhuVuc"] != null)
                        khuVuc = ds.Tables["Hoso"].Rows[0]["MaKhuVuc"].ToString();
                    else
                        khuVuc = "";

                        txtMaThe.Text = ds.Tables["Hoso"].Rows[0]["DoituongThe"].ToString() + "-" + ds.Tables["Hoso"].Rows[0]["SotheBHYT"].ToString();
                        MaDTthe = ds.Tables["Hoso"].Rows[0]["DoituongThe"].ToString();
                        txtManoiDKKCBBD.Text = ds.Tables["Hoso"].Rows[0]["NoidangkyKCBBD"].ToString();
                        txtTenNoiDKKCBBD.Text = ds.Tables["Hoso"].Rows[0]["Tennoicap"].ToString();
                        dtBHTu.Value = Convert.ToDateTime(ds.Tables["Hoso"].Rows[0]["HantheBHYT_Tu"]);
                        dtBHDen.Value = Convert.ToDateTime(ds.Tables["Hoso"].Rows[0]["HantheBHYT_Den"]);
                        if (ds.Tables["Hoso"].Rows[0]["TgDu5Nam"] != null)
                            TgDu5Nam = Convert.ToDateTime(ds.Tables["Hoso"].Rows[0]["TgDu5Nam"]);
                        string svl = ds.Tables["Hoso"].Rows[0]["ThoiDiemMienChiTraTrongNam"].ToString();
                        if (ds.Tables["Hoso"].Rows[0]["ThoiDiemMienChiTraTrongNam"] != null && svl != "")
                            TgMienChiTraTrongNam = Convert.ToDateTime(ds.Tables["Hoso"].Rows[0]["ThoiDiemMienChiTraTrongNam"]);
                        MienChiTraTrongNam = (ds.Tables["Hoso"].Rows[0]["MienChiTraTrongNam"].ToString() == "1" ? true : false);
                        TenBHYTTinh = ds.Tables["Hoso"].Rows[0]["NoicaptheBHYT"].ToString();                    
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }

    class clsTheBHYT2018
    {
        public string maKetQua;
        public string ghiChu;
        public string maThe;
        public string hoTen;
        public string ngaySinh;
        public string Day_NS;
        public string Month_NS;
        public string Year_NS;
        public string gioiTinh;
        public string diaChi;
        public string maDKBD;
        public string cqBHXH;
        public string gtTheTu;
        public string gtTheDen;
        public string maKV;
        public string ngayDu5Nam;
        public string maSoBHXH;
        public string maTheCu; // chính là mã thẻ ở trên
        public string maTheMoi;
        public string gtTheTuMoi;
        public string gtTheDenMoi;
    }
}
