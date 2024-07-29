using System;
using System.Collections.Generic;

namespace Model
{
    public partial class ThemPhieuKham
    {
        public string MaKhambenh { get; set; } 

        public string MaBenhnhan { get; set; } 
        public string TenBenhnhan { get; set; }
        public int? NamSinh { get; set; }
        public int? ThangSinh { get; set; }
        public int? NgaySinh { get; set; }
        public int? GioiTinh { get; set; }

        public DateTime ThoigianDangky { get; set; }

        public string Doituong { get; set; } 

        public string Diachi { get; set; }

        public string Noicongtac { get; set; }

        public byte Tuyen { get; set; }

        public string SotheBhyt { get; set; }

        public string Doituongthe { get; set; }

        public string NoidangkyKcbbd { get; set; }

       
        public DateTime? HantheBhytTu
        {
            get
            ;
            set
            ;
        }


        public DateTime? HantheBhytDen { get; set; }

        public string NoicaptheBhyt { get; set; }

        public string Nghenghiep { get; set; }

        public string SotheTe { get; set; }

        public string Lienhe { get; set; }

        public string Noigioithieu { get; set; }

        public string ChandoanNgt { get; set; }

        /// <summary>
        /// 0 - chua thanh toan;1 da thanh toan
        /// </summary>
        public byte? DaTinhPhi { get; set; }

        public string NhanvienCd { get; set; }

        public byte? IsTruc { get; set; }

        public DateTime? HkNgayHenKham { get; set; }

        public string HkSoHoSo { get; set; }

        public int? MaBenhId { get; set; }

        public bool? UuTien { get; set; }

        /// <summary>
        /// Thời gian đủ 5 năm liên tục
        /// </summary>
        public DateTime? TgDu5Nam { get; set; }

        /// <summary>
        /// 0: không miễn, 1 miễn
        /// </summary>
        public bool? MienChiTraTrongNam { get; set; }

        /// <summary>
        /// ngày BN nhận quyết định miễn chi trả trong năm
        /// </summary>
        public DateTime? ThoiDiemMienChiTraTrongNam { get; set; }

        public string MaNoiChuyen { get; set; }

        public DateTime? Thoigianthanhtoan { get; set; }

        public byte? DaInTongKet { get; set; }

        /// <summary>
        /// Mã mức hưởng: từ 1-&gt;5
        /// </summary>
        public byte? MaMucHuong { get; set; }

        public string SoBhxh { get; set; }

        public string SoCccd { get; set; }

        public string SoDienThoai { get; set; }

        public byte? IsDeleted { get; set; }

        public string DeletedUname { get; set; }

        public DateTime? DeletedTime { get; set; }

        public string MaQuocTich { get; set; }

        public string MaTinh { get; set; }

        public string MaHuyen { get; set; }

        public string MaXa { get; set; }

        public string MaDanToc { get; set; }

        public int LoaiTuyen { get; set; }

        public string SoChuyenTuyen { get; set; }

        public string MaDoiTuongKcb { get; set; }

        public string MaLoaiKcb { get; set; }

        public string SoPhieuHenKhamLai { get; set; }

        public string MaKhuVuc { get; set; }

        public string GhiChu { get; set; }

        public string YeuCauKham { get; set; }

        public string PhongKham { get; set; }
    }
}


