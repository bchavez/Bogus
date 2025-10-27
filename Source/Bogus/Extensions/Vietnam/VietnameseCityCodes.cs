using System.Collections.Generic;

namespace Bogus.Extensions.Vietnam;

public static class VietnameseCityCodes
{
   /// <summary>
   /// The dictionary containing the city names and their corresponding city codes
   /// used for generating Vietnamese Citizen Identity Card Number (Căn Cước Công Dân - CCCD).
   /// </summary>
   /// <seealso href="https://vi.wikipedia.org/wiki/C%C4%83n_c%C6%B0%E1%BB%9Bc_c%C3%B4ng_d%C3%A2n_(Vi%E1%BB%87t_Nam)"/>
   public static readonly IDictionary<string, string> CityCodes = new Dictionary<string, string>
   {
      { "Hà Nội", "001" },
      { "Hà Giang", "002" },
      { "Cao Bằng", "004" },
      { "Bắc Kạn", "006" },
      { "Tuyên Quang", "008" },
      { "Lào Cai", "010" },
      { "Điện Biên", "011" },
      { "Lai Châu", "012" },
      { "Sơn La", "014" },
      { "Yên Bái", "015" },
      { "Hòa Bình", "017" },
      { "Thái Nguyên", "019" },
      { "Lạng Sơn", "020" },
      { "Quảng Ninh", "022" },
      { "Bắc Giang", "024" },
      { "Phú Thọ", "025" },
      { "Vĩnh Phúc", "026" },
      { "Bắc Ninh", "027" },
      { "Hải Dương", "030" },
      { "Hải Phòng", "031" },
      { "Hưng Yên", "033" },
      { "Thái Bình", "034" },
      { "Hà Nam", "035" },
      { "Nam Định", "036" },
      { "Ninh Bình", "037" },
      { "Thanh Hoá", "038" },
      { "Nghệ An", "040" },
      { "Hà Tĩnh", "042" },
      { "Quảng Bình", "044" },
      { "Quảng Trị", "045" },
      { "Thừa Thiên-Huế", "046" },
      { "Đà Nẵng", "048" },
      { "Quảng Nam", "049" },
      { "Quảng Ngãi", "051" },
      { "Bình Định", "052" },
      { "Phú Yên", "054" },
      { "Khánh Hoà", "056" },
      { "Ninh Thuận", "058" },
      { "Bình Thuận", "060" },
      { "Kon Tum", "062" },
      { "Gia Lai", "064" },
      { "Đắk Lắk", "066" },
      { "Đắk Nông", "067" },
      { "Lâm Đồng", "068" },
      { "Bình Phước", "070" },
      { "Tây Ninh", "072" },
      { "Bình Dương", "074" },
      { "Đồng Nai", "075" },
      { "Bà Rịa-Vũng Tàu", "077" },
      { "TP. Hồ Chí Minh", "079" },
      { "Long An", "080" },
      { "Tiền Giang", "082" },
      { "Bến Tre", "083" },
      { "Trà Vinh", "084" },
      { "Vĩnh Long", "086" },
      { "Đồng Tháp", "087" },
      { "An Giang", "089" },
      { "Kiên Giang", "091" },
      { "Cần Thơ", "092" },
      { "Hậu Giang", "093" },
      { "Sóc Trăng", "094" },
      { "Bạc Liêu", "095" },
      { "Cà Mau", "096" }
   };
}