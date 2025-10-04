# Keylogger - C# Windows Forms Uygulaması

Bu proje, C# Windows Forms kullanılarak geliştirilmiş bir keylogger uygulamasıdır. Uygulama, klavye girişlerini yakalayabilir ve belirli aralıklarla email ile gönderebilir.

## ⚠️ ÖNEMLİ UYARI

Bu uygulama **eğitim amaçlı** geliştirilmiştir. Sadece kendi bilgisayarınızda veya izin verilen ortamlarda kullanın. Başkalarının bilgisayarlarında izinsiz kullanım **yasadışıdır** ve etik değildir.

## 🚀 Özellikler

- **Toggle Özelliği**: Tek tıkla keylogger'ı başlat/durdur
- **Email Entegrasyonu**: Yakalanan verileri email ile gönderme
- **Kullanıcı Dostu Arayüz**: Basit ve anlaşılır form tasarımı
- **Güvenli**: Sadece aktif olduğunda çalışır
- **Log Dosyası**: Veriler masaüstünde keylog.txt dosyasına kaydedilir

## 📋 Gereksinimler

- Windows 10/11
- .NET Framework 4.7.2 veya üzeri
- Visual Studio 2019/2022 (geliştirme için)
- Gmail hesabı (email gönderimi için)

## 🛠️ Kurulum

1. **Repository'yi klonlayın:**
   ```bash
   git clone https://github.com/brkysbnc/Keylogger.git
   ```

2. **Visual Studio ile açın:**
   - `Keylogger.sln` dosyasını Visual Studio ile açın

3. **Projeyi derleyin:**
   - Build → Build Solution (Ctrl+Shift+B)
   - Veya F5 ile çalıştırın

## 📧 Email Ayarları

Email gönderimi için Gmail kullanmanız gerekiyor:

1. **Gmail'de 2-Factor Authentication'ı aktifleştirin**
2. **App Password oluşturun:**
   - Google Account → Security → 2-Step Verification → App passwords
   - "Mail" seçin ve cihaz adı girin
   - Oluşturulan 16 haneli şifreyi kopyalayın

3. **Uygulamada ayarları güncelleyin:**
   - Gönderen Email: Gmail adresiniz
   - App Password: Oluşturduğunuz 16 haneli şifre
   - Alıcı Email: Verilerin gönderileceği email adresi

## 🎯 Kullanım

1. **Uygulamayı çalıştırın**
2. **Email ayarlarını yapın** (isteğe bağlı)
3. **"Keylogger'ı Başlat" butonuna tıklayın**
4. **Klavye girişleriniz yakalanmaya başlar**
5. **"Keylogger'ı Durdur" butonuna tıklayarak durdurun**

## 📁 Dosya Yapısı

```
Keylogger/
├── Keylogger.sln          # Solution dosyası
├── Keylogger/
│   ├── Form1.cs           # Ana form kodu
│   ├── Form1.Designer.cs  # Form tasarımı
│   ├── Program.cs         # Uygulama giriş noktası
│   └── Keylogger.csproj   # Proje dosyası
├── README.md              # Bu dosya
└── .gitignore             # Git ignore dosyası
```

## 🔧 Teknik Detaylar

### Kullanılan Teknolojiler
- **C# Windows Forms**: Arayüz geliştirme
- **Windows API**: Klavye hook'ları
- **System.Net.Mail**: Email gönderimi
- **.NET Framework 4.7.2**: Hedef framework

### Ana Fonksiyonlar
- `SetHook()`: Klavye hook'unu kurar
- `HookCallback()`: Tuş basımlarını yakalar
- `LogKey()`: Verileri dosyaya yazar
- `SendEmail()`: Email gönderir

## 📊 Log Formatı

Log dosyası (`keylog.txt`) şu formatta veri saklar:
```
[2025-01-27 14:30:15] A
[2025-01-27 14:30:16] B
[2025-01-27 14:30:17] Space
[2025-01-27 14:30:18] Enter
```

## 🚨 Güvenlik Notları

- Uygulama sadece kendi bilgisayarınızda kullanın
- Email şifrelerinizi güvenli tutun
- Log dosyalarını düzenli olarak temizleyin
- Antivirus yazılımı uygulamayı tehdit olarak görebilir

## 📝 Lisans

Bu proje eğitim amaçlı geliştirilmiştir. Ticari kullanım için lisans gereklidir.

## 🤝 Katkıda Bulunma

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/AmazingFeature`)
3. Commit yapın (`git commit -m 'Add some AmazingFeature'`)
4. Push yapın (`git push origin feature/AmazingFeature`)
5. Pull Request oluşturun

## 📞 İletişim

- GitHub: [@brkysbnc](https://github.com/brkysbnc)
- Proje Linki: [https://github.com/brkysbnc/Keylogger](https://github.com/brkysbnc/Keylogger)

## ⚖️ Yasal Uyarı

Bu yazılım sadece eğitim ve araştırma amaçlıdır. Yazılımı kullanmadan önce yerel yasaları kontrol edin. Yazılımın kötüye kullanımından doğacak sorumluluk kullanıcıya aittir.
