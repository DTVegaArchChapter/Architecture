# DDD (Domain-Driven-Design) ile Goal Management System Uygulaması

Domain driven design kullanarak Goal Management System implementasyonunu içeren örnek uygulamadır.

## Eventstorming Diagram

![Eventstorming Diagram](./eventstorming.png)

## Strategic Design

### Diagram

![Strategic Design Diagram](./StrategicDesign.png)

### Identity Management

Authentication, authorization ve kullanıcı profili yönetimi işlemlerini yapar. Goal Management sistemine özel bir business domain'i olmadığı için generic sub-domain olarak değerlendirildi. Identity Management işlevini gerçekleştirebilecek piyasada birçok hazır çözüm mevcuttur ve Identity Management sub-domain'i, Goal Management sistemi içerisinde değerlendirdiğimizde rakip ürünlere karşı rekabetçi avantaj sağlama gibi bir özelliği bulunmamaktadır.

Asp.Net Core Identity kütüphanesi kullanılmıştır. Identity Management sayfaları da Identity Scaffolder ile üretilmiştir. Çok karmaşık iş kuralları barındırmadığı ve generic sub-domain olduğu için DDD kullanılmadan gelitirilmiştir.

### Organisation

Organizasyonlar, takımlar, takım üyeleri ve takım sorumluluklarının atanması gibi organizasyonel işler burada yönetilmektedir.

Ana amacı Goal Management ve Performance Evaluation core domain'lerini destekleyecek yapıyı sağlamaktadır. Goal management uygulamasının ana odağı değildir. Bu nedenlerden dolayı supporting sub-domain olarak değerlendirilmiştir.

### Goal Management

Goal management uygulamasının ana hedeflerinden birini gerçekleştirmektedir. Hedef tanımlarını, izler ve yönetir. Kullanıcıların veya organizasyonların hedeflerini yönetmelerine ve hedeflerine ulaşmalarına yardımcı olmaktadır. Uygulamayı rakiplerden farklılaştırdığı ve rekabet avantajı sağladığı için core sub-domain olarak değerlendirilmiştir.
