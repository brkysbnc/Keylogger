========================================
        KEYLOGGER PORTABLE PAKET
========================================

OGRENCI BILGILERI:
- Isim Soyisim: Berkay Sabuncu
- Ogrenci No: 240542029
- Fakulte: Teknoloji Fakultesi
- Bolum: Yazilim Muhendisligi
- Sinif/Sube: 2/A

========================================
        PROJE HAKKINDA
========================================

Bu proje, klavye girdilerini yakalama ve kaydetme 
islevi goren bir Windows Forms uygulamasidir.

KULLANILAN TEKNOLOJILER:
- Programlama Dili: C#
- Framework: .NET Framework 4.7.2
- IDE: Visual Studio
- Platform: Windows Forms
- Hedef Platform: Windows 7 ve uzeri

========================================
        KURULUM VE CALISTIRMA
========================================

1. ADIM - Dosyalari Kopyalama:
   - Keylogger_Portable klasorunun tamamini hedef bilgisayara kopyalayin
   - USB bellek veya herhangi bir tasinabilir depolama kullanabilirsiniz

2. ADIM - Sistem Gereksinimlerini Kontrol Etme:
   - check_dotnet.bat dosyasina cift tiklayin
   - Bu dosya .NET Framework 4.7.2 nin yuklu olup olmadigini kontrol eder
   - Eger .NET Framework yuklu degilse, Microsoft resmi sitesinden indirin

3. ADIM - Uygulamayi Baslatma:
   - Keylogger.exe dosyasina cift tiklayin
   - Uygulama acilacak ve klavye girdilerini yakalamaya baslayacak

========================================
        KULLANIM TALIMATLARI
========================================

UYGULAMA ACILDIKTAN SONRA:
1. Ana pencere acilacak
2. Uygulama otomatik olarak klavye girdilerini yakalamaya baslar
3. Yakalanan veriler uygulama icinde goruntulenir
4. Uygulamayi kapatmak icin X butonuna tiklayin

ONEMLI NOTLAR:
- Bu uygulama sadece EGITIM AMACLI gelistirilmistir
- Gercek ortamda kullanmadan once gerekli izinleri alin
- Uygulama calisirken antivirus yazilimlari uyari verebilir (normal durum)
- Uygulama sadece Windows isletim sisteminde calisir

========================================
        SORUN GIDERME
========================================

SORUN: "Uygulama acilmiyor"
COZUM: 
- check_dotnet.bat calistirin
- .NET Framework 4.7.2 yukleyin
- Uygulamayi yonetici olarak calistirin

SORUN: "Antivirus uygulamayi engelliyor"
COZUM:
- Antivirus ayarlarindan uygulamayi istisna olarak ekleyin
- Gecici olarak antivirus korumasini kapatabilirsiniz

SORUN: "Uygulama calisiyor ama veri yakalamiyor"
COZUM:
- Uygulamanin yonetici yetkileri ile calistigindan emin olun
- Windows Defender veya diger guvenlik yazilimlarini kontrol edin

========================================
        TEKNIK DETAYLAR
========================================

PROJE YAPISI:
- Form1.cs: Ana form ve kullanici arayuzu
- Program.cs: Uygulama giris noktasi
- Keylogger.exe: Derlenmis executable dosya
- Keylogger.exe.config: Uygulama konfigurasyon dosyasi

DESTEKLENEN OZELLIKLER:
- Gercek zamanli klavye girdisi yakalama
- Windows Forms tabanli kullanici arayuzu
- Portable calistirma (kurulum gerektirmez)
- .NET Framework bagimliligi

========================================
