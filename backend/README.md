# AuthService
AuthService 

# 資料庫更新指令
dotnet ef dbcontext scaffold Name=DefaultConnection Microsoft.EntityFrameworkCore.SqlServer -o Models -f --use-database-names


# 功能
Register:當user table內沒有資料才可以新增(測試用),
預設帳號admin
密碼1111

Login:登入取得token/並儲存在DB

Logout:登出刪除token/DB

RefreshToken:token刷新

# 網站設定
Properties > launchSettings.json
預設: "https://localhost:8001"



