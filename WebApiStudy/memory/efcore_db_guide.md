---
name: EF Core 数据库知识汇总
description: 本项目涉及的 EF Core + SQL Server 全部知识点和代码写法
type: reference
---

## 1. 项目配置（三个必需部分）

### 1.1 NuGet 包（WebApiStudy.csproj）
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.7" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.7" />
```

### 1.2 连接字符串（appsettings.json）
```json
{
  "ConnectionStrings": {
    "ShirtStoreManagement": "Data Source=(local);Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True"
  }
}
```
- `Data Source=(local)` — 本机 SQL Server
- `Integrated Security=True` — Windows 身份验证
- `Trust Server Certificate=True` — 信任 SSL 证书

### 1.3 注册 DbContext（Program.cs）
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShirtStoreManagement"));
});
```

---

## 2. DbContext 定义

```csharp
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Shirt> Shirts { get; set; }  // 一张表

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Shirt>().HasData(  // 种子数据
            new Shirt { ShirtId = 1, Brand = "Nike", ... },
            new Shirt { ShirtId = 2, Brand = "Adidas", ... }
        );
    }
}
```

- `DbSet<T>` 对应数据库的一张表，类名映射为表名
- `HasData()` 在数据库初始化时插入种子数据
- 按约定，`Id` 或 `类名+Id` 自动被识别为主键

---

## 3. Model 定义

```csharp
public class Shirt
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ShirtId { get; set; }      // 主键，自动生成
    [Required]
    public string? Brand { set; get; }    // NOT NULL
    [Required]
    public string? Color { get; set; }
    public int? Size { get; set; }        // 可空
    [Required]
    public string? Gender { get; set; }
    public double Price { get; set; }
}
```

- `[DatabaseGenerated(Identity)]` — ID 由数据库自动生成，忽略客户端传入的值
- `[Required]` — 数据库 NOT NULL，ModelState 自动校验

---

## 4. 依赖注入两种方式

### 方式 A：构造函数注入（需要 TypeFilter）
```csharp
public class XxxFilterAttribute : ActionFilterAttribute
{
    private readonly ApplicationDbContext db;

    public XxxFilterAttribute(ApplicationDbContext db)
    {
        this.db = db;
    }
    // 使用 this.db 操作数据库
}
// Controller 中必须用 [TypeFilter(typeof(XxxFilterAttribute))]
```

### 方式 B：HttpContext 获取（不需要 TypeFilter）
```csharp
public class XxxFilterAttribute : ActionFilterAttribute
{
    // 没有构造函数注入

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var db = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
        // 使用 db
    }
}
// Controller 中直接写 [XxxFilterAttribute]，不需要 TypeFilter
```

### 为什么直接写 [Attribute] 不行？
普通的 `[Attribute]` 是 C# 编译时 `new` 出来的，不走 DI 容器。如果构造函数有参数（如 ApplicationDbContext），框架不知道怎么传 → 报错"未提供所需参数"。
`[TypeFilter]` 告诉框架"去 DI 容器里解析这个 Filter"，容器里有 ApplicationDbContext 所以能注入。

---

## 5. CRUD 操作（写在 Controller 里）

```csharp
private readonly ApplicationDbContext db;
public XxxController(ApplicationDbContext db)
{
    this.db = db;       // 构造函数里 this.db = db 必须写 this
}

// 查全部
var list = db.Shirts.ToList();

// 按主键查
var shirt = db.Shirts.Find(id);

// 按条件查
var exists = db.Shirts.Any(x => x.Brand == "Nike" && x.Color == "Red");
var shirt = db.Shirts.FirstOrDefault(x => x.ShirtId == 5);

// 新增
db.Shirts.Add(shirt);
db.SaveChanges();

// 修改
db.Shirts.Update(shirt);
db.SaveChanges();

// 删除
var shirt = db.Shirts.Find(id);
if (shirt != null)
{
    db.Shirts.Remove(shirt);
    db.SaveChanges();
}
```

---

## 6. Filter 里的数据库校验

### 校验 ID 存在
```csharp
var existingShirt = db.Shirts.Find(shirtId.Value);
if (existingShirt == null)
{
    // 返回 404
    context.Result = new NotFoundObjectResult(problemDetail);
}
// 存在就存到 HttpContext.Items 供后续使用（可选）
context.HttpContext.Items["shirt"] = existingShirt;
```

### 校验新增时是否重复
```csharp
var exists = db.Shirts.Any(x =>
    x.Brand == shirt.Brand &&
    x.Color == shirt.Color &&
    x.Gender == shirt.Gender &&
    x.Size == shirt.Size);
if (exists)
{
    // 返回 400，衬衫已存在
}
```

---

## 7. LINQ-to-Entities 注意事项

| ✅ 能用 | ❌ 不能用 |
|---------|-----------|
| `x.Brand == "Nike"` | `x.Brand.Equals("Nike")` |
| `x.Size == 9` | `string.IsNullOrWhiteSpace(x.Brand)` |
| `x.Price > 20` | `x.Brand.Contains(...)`（部分版本有限制） |

**原因**：EF Core 要把 C# 代码翻译成 SQL。`==` 能翻译成 `=`，`String.Equals()` 翻不了。

---

## 8. HttpContext 是什么

每次 HTTP 请求来临时，ASP.NET 创建一个 HttpContext 对象，把请求的所有信息装进去：

| 属性 | 内容 |
|------|------|
| `Request` | URL、Headers、Body |
| `Response` | 要返回的 StatusCode、Body |
| `Items` | 键值对字典，同一个请求内跨 Filter/Action 传递数据 |
| `RequestServices` | DI 容器，可以手动获取已注册的服务 |

请求结束后 HttpContext 自动销毁，不影响下一个请求。

### ⚠️ HttpContext.Items 的问题（不推荐用于 CRUD 传值）
- 依赖 Filter 先执行，顺序错了就拿到 null
- key 是字符串，拼写错误编译器不报错
- 返回 `object?`，没有类型检查
- **推荐直接用 `db.Shirts.Find(id)` 在 Action 里查，比 Items 传值更可靠**
```

---

## 9. 常见错误速查

| 错误 | 原因 | 解决 |
|------|------|------|
| `DbUpdateException` | 主键冲突（ID 已存在） | 加 `[DatabaseGenerated(Identity)]` |
| "未提供所需参数 db" | Filter 有注入但没加 TypeFilter | 改用 `[TypeFilter(typeof(Xxx))]` |
| Nullable 警告 | 开启了 `<Nullable>enable</Nullable>` | 加 null 检查 或 用 `!` 后缀 |
| `string.IsNullOrWhiteSpace` 在 LINQ 中报错 | EF Core 无法翻译 | 改用 `==` 比较 |
