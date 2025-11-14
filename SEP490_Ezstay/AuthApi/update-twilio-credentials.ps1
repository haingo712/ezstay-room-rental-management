# Script nhanh để cập nhật Twilio credentials
# Sử dụng: Chỉnh sửa các giá trị bên dưới rồi chạy script này

Write-Host "=== Twilio Credentials Updater ===" -ForegroundColor Cyan
Write-Host ""

# ============================================
# OPTION 1: Dùng API Key (Khuyến nghị)
# ============================================
$USE_API_KEY = $true  # Đổi thành $false nếu muốn dùng Auth Token

$ACCOUNT_SID = "ACb08bf5fb1d20fab86c85ddc14206de74"
$MESSAGING_SERVICE_SID = "MG3422d580b6c76abab16ecb2bdaf1ed70"

if ($USE_API_KEY) {
    # Điền API Key credentials (lấy từ: https://console.twilio.com/us1/account/keys-credentials/api-keys)
    $API_KEY_SID = "SKxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"  # ← THAY ĐỔI TẠI ĐÂY
    $API_KEY_SECRET = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"  # ← THAY ĐỔI TẠI ĐÂY
    
    Write-Host "✓ Sử dụng API Key authentication" -ForegroundColor Green
    Write-Host "  Account SID: $ACCOUNT_SID"
    Write-Host "  API Key SID: $($API_KEY_SID.Substring(0,4))...${($API_KEY_SID.Substring($API_KEY_SID.Length-4))}"
    
    $config = @{
        TwilioSettings = @{
            AccountSid = $ACCOUNT_SID
            ApiKeySid = $API_KEY_SID
            ApiKeySecret = $API_KEY_SECRET
            MessagingServiceSid = $MESSAGING_SERVICE_SID
        }
    }
} else {
    # ============================================
    # OPTION 2: Dùng Auth Token
    # ============================================
    # Điền Auth Token (lấy từ: https://console.twilio.com)
    $AUTH_TOKEN = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"  # ← THAY ĐỔI TẠI ĐÂY
    
    Write-Host "✓ Sử dụng Auth Token authentication" -ForegroundColor Green
    Write-Host "  Account SID: $ACCOUNT_SID"
    
    $config = @{
        TwilioSettings = @{
            AccountSid = $ACCOUNT_SID
            AuthToken = $AUTH_TOKEN
            MessagingServiceSid = $MESSAGING_SERVICE_SID
        }
    }
}

Write-Host ""
Write-Host "Đang cập nhật appsettings.json..." -ForegroundColor Yellow

# Đọc file appsettings.json hiện tại
$appsettingsPath = "appsettings.json"
$currentSettings = Get-Content $appsettingsPath -Raw | ConvertFrom-Json

# Cập nhật TwilioSettings
$currentSettings.TwilioSettings = $config.TwilioSettings

# Ghi lại file với format đẹp
$currentSettings | ConvertTo-Json -Depth 10 | Set-Content $appsettingsPath

Write-Host "✓ Đã cập nhật credentials thành công!" -ForegroundColor Green
Write-Host ""
Write-Host "Tiếp theo:" -ForegroundColor Cyan
Write-Host "  1. Restart ứng dụng: dotnet run" -ForegroundColor White
Write-Host "  2. Kiểm tra logs khi app khởi động" -ForegroundColor White
Write-Host "  3. Test endpoint: POST /api/auth/send-phone-otp" -ForegroundColor White
Write-Host ""
