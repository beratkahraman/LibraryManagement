# LibraryManagementApi

Basit bir ASP.NET Core Web API projesi.  
Entity Framework Core Code-First kullanarak `Author` ve `Book` CRUD operasyonlarını sağlar.  
Global bir error-handling middleware ile tutarlı JSON hata yanıtları ve Swagger UI üzerinden kolay test imkânı sunar.

## Özellikler

- **Entities**  
  - `Author` (Id, Name, BirthDate)  
  - `Book`   (Id, Title, PublishedYear, Price, AuthorId)  
- **Veritabanı**  
  - EF Core Code-First, LocalDB  
  - `DeleteBehavior.Restrict` ile ilişkili kitap varsa yazar silinmez  
- **Validation**  
  - DataAnnotations: `[Required]`, `[MaxLength]`, `[Range]`, `[Column(TypeName = "decimal(18,2)")]`  
- **Error Handling**  
  - Global middleware: tüm beklenmedik hatalar `{ statusCode, message, details }` formatında JSON döner  
- **Swagger UI**  
  - Otomatik dokümantasyon ve “Try it out” ile endpoint test imkânı  

## Gereksinimler

- .NET 9 SDK  
- SQL Server LocalDB (Visual Studio ile birlikte gelir)  

## Kurulum & Çalıştırma

1. Repo’yu klonla ve dizine gir  
   ```bash
   git clone https://github.com/<kullaniciAdiniz>/LibraryManagementApi.git
   cd LibraryManagementApi/LibraryManagementApi
2. Paketleri yükle ve derle
   ```bash
   dotnet restore
   dotnet build
3. Veritabanını oluştur
   ```bash
   dotnet ef database update
4. Uygulamayı çalıştır
   ```bash
   dotnet run
  Swagger UI’a şu adresten ulaşabilirsiniz:
  http://localhost:5114/swagger
## API Endpoint’leri

### Authors

- **GET**    `/api/authors`  
- **GET**    `/api/authors/{id}`  
- **POST**   `/api/authors`  
  ```json
  {
    "name": "Jane Austen",
    "birthDate": "1775-12-16T00:00:00"
  }
- **PUT**    `/api/authors/{id}`
  ```json
  {
  "id": 1,
  "name": "Jane Austen",
  "birthDate": "1775-12-16T00:00:00"
  }
- **DELETE**    `/api/authors/{id}`
### Books

- **GET**    `/api/books`  
- **GET**    `/api/books/{id}`  
- **POST**   `/api/books`
  ```json
  {
  "title": "Pride and Prejudice",
  "publishedYear": 1813,
  "price": 19.99,
  "authorId": 1
  }
- **PUT**    `/api/books/{id}`
  ```json
  {
  "id": 5,
  "title": "Pride and Prejudice (Updated)",
  "publishedYear": 1813,
  "price": 21.50,
  "authorId": 1
  }
- **DELETE**    `/api/books/{id}`
