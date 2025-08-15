# Transactional Outbox Pattern

Transactional outbox pattern'i kullanımı hakkında uygulama örneği.

## İçindekiler
- [Outbox pattern nedir?](#outbox-pattern-nedir)
  - [Örnek: Dual-write problemi nasıl oluşur ve Outbox nasıl çözer?](#örnek-dual-write-problemi-nasıl-oluşur-ve-outbox-nasıl-çözer)
- [Diyagram](#diyagram)
- [Kullanılan Teknolojiler](#kullanılan-teknolojiler)
- [Proje nasıl çalıştırılır](#proje-nasıl-çalıştırılır)
  - [Önkoşullar](#önkoşullar)
  - [Docker Compose](#docker-compose)
  - [Yerel çalıştırma (.NET CLI)](#yerel-çalıştırma-net-cli)

## Outbox pattern nedir?
- Mikroservislerin hem veritabanına yazıp hem de mesaj kuyruğuna güvenilir şekilde mesaj yayınlaması için kullanılan bir desendir.
- Veritabanı transactionı kullanılarak hem domain verisi hem de mesaj kuyruğuna eklenecek event kaydı ("outbox" tablosuna) aynı veritabanına kaydedilir. Böylece mesaj kuyruğuna çift-yazma (dual-write) problemi ortadan kalkar.
- Ayrı bir arka plan işlemcisi outbox tablosunu periyodik olarak tarar, mesaj kuyruğuna (ör. RabbitMQ) yayınlar, başarılı olursa kaydı işlendi (processed) olarak işaretler.
- Avantajlar: atomiklik, en az bir kez teslimat (at-least-once).

### Örnek: Dual-write problemi nasıl oluşur ve Outbox nasıl çözer?
- Senaryo: Sipariş oluşturulduğunda "OrderCreated" eventi yayınlanacak ve e-posta gönderilecek.
- Outbox OLMADAN (iki ayrı yazma):
  1) Orders tablosuna INSERT -> COMMIT
  2) RabbitMQ'ya "OrderCreated" publish
  - Eğer 2. adım ağ hatasıyla başarısız olursa: DB'de sipariş var ama event yayınlanmadı; diğer servisler habersiz, e-posta gitmez.
  - Tersi durumda: Publish başarılı, fakat DB COMMIT başarısız olursa; diğer servislerde hayalet (olmayan) sipariş görülür.
- Outbox İLE (tek transaction):
  1) Aynı DB transaction'ında: Orders INSERT + Outbox tablosuna "OrderCreated" event kaydı.
  2) Transaction COMMIT edilince her iki kayıt da garanti altına alınır. Sonrasında outbox'tan event alınır, RabbitMQ'ya yayınlanır; başarılıysa outboxta kaydı işlendi olarak işaretler.
  - Sonuç: Ya ikisi birlikte commit olur ya da hiçbiri; eventin yayınlanması en az bir kez garanti edilir. (at-least-once)

## Diyagram

![Transactional Outbox Pattern](./transactional-outbox-pattern.png)

- `TransactionalOutboxPatternApp` (ASP.NET Core): HTTP isteklerini alır, iş kuralı verisini ve outbox kaydını aynı SQL transactionında yazar.
- `Publisher` Outbox tablosunu tarar, gönderilmemiş kayıtları RabbitMQ'ya yayınlar; başarılı yayından sonra kaydı "Processed" olarak işaretler.
- `MSSQL` (1433): `Order` tablosu + `Outbox` tablosu.
- `RabbitMQ` (5672, yönetim 15672): Event mesajlarının yayınlandığı mesaj aracısı.
- `MailWorkerService`: RabbitMQ'dan ilgili kuyruğu/konuyu dinler, mesajı işler (ör. e-posta gönderimi), idempotent olacak şekilde ACK/NACK yönetir.
- `Papercut Mail Server` (37409→37408): Geliştirici dostu SMTP sunucusu; işlenen e-postaları görüntülemek için kullanılır.

## Kullanılan Teknolojiler
- .NET 9 (ASP.NET Core, Razor Pages)
- Entity Framework Core 9 + SQL Server sağlayıcısı
- Microsoft SQL Server 2019 (mcr.microsoft.com/mssql/server:2019-latest)
- RabbitMQ 3 (management UI’lı alpine image)
- Docker ve Docker Compose
- Papercut SMTP (jijiechen/papercut)
- RabbitMQ için MessageQueue katmanı (publisher)

## Proje nasıl çalıştırılır

Aşağıdaki adımlar Linux için yazılmıştır (bash). Windows/Mac için komutlar benzerdir.

### Önkoşullar
- Docker Engine + Docker Compose (v2)
- .NET 9 SDK (yerel çalıştırmak için)

### Docker Compose
1) Proje klasörüne gidin:
   - `Architecture/microservices/OutboxPattern`
2) Tüm servisleri başlatın:
   - `docker compose up -d`
3) Servislerin ayaklanmasını bekleyin ve durumunu kontrol edin:
   - `docker compose ps`
   - Gerekirse logları izleyin: `docker compose logs -f mssql rabbitmq transactionaloutboxpatternapp mailworkerservice`
4) Uygulamayı açın ve deneyin:
   - App (Razor Pages): http://localhost:8080
   - RabbitMQ Management: http://localhost:15672 (kullanıcı: guest / şifre: guest)
   - Papercut (SMTP Web UI): http://localhost:37409

### Yerel çalıştırma (.NET CLI)
Yalnızca bağımlılıkları Docker’da çalıştırıp uygulamaları yerel olarak debug etmek için:
1) Bağımlılıkları başlatın:
   - `docker compose up -d mssql rabbitmq mailserver`
2) App’i başlatın:
   - `dotnet run --project TransactionalOutboxPatternApp`
3) Worker’ı başlatın (ayrı terminalde):
   - `dotnet run --project MailWorkerService`