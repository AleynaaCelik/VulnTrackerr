Zafiyet Yönetimi API - Backend (C#/.NET Core)


Bu proje, zafiyet yönetimi sistemini sağlamak amacıyla geliştirilmiş bir Backend API'yi içermektedir. API, kullanıcıların zafiyet bilgilerini eklemelerine, güncellemelerine, silmelerine ve listelemelerine olanak sağlar. Proje, JWT ile kimlik doğrulama, iki faktörlü kimlik doğrulama (2FA), zafiyet yönetimi ve daha birçok özellik ile geliştirilmiştir.

Özellikler


Authentication ve Authorization


Kullanıcı oturum açma ve yetkilendirme işlemleri JWT ile yapılmaktadır.


Kullanıcı modeli ile kimlik doğrulama yapılır.


Login ve Register İşlemleri


İki faktörlü kimlik doğrulama (2FA) sistemi eklenmiştir.


Tasarım Deseni


Proje, geliştirme tercihlerine bağlı olarak seçilen bir tasarım deseni kullanılarak yapılandırılmıştır.


Zafiyet Yönetimi Modeli


Zafiyetler için aşağıdaki model yapılandırılmıştır:

ID: Zafiyetin benzersiz kimliği.


Name: Zafiyet adı.


Description: Zafiyet açıklaması.


Severity: Zafiyetin önemi.


CVSS: Common Vulnerability Scoring System puanı.


CVE: Common Vulnerabilities and Exposures kodu.


Status: Zafiyetin durumu (Açık/Kapalı).


CVSS Puanı Hesaplama


CVSS puanı, belirli bir algoritmaya dayalı olarak hesaplanır ve veritabanında ilgili alana kaydedilir.


Zafiyet Listeleme ve Filtreleme


Zafiyetler, GET endpoint’i ile listelenebilir.


Filtreleme parametreleri:


Name


Severity


Status


CRUD İşlemleri


Zafiyetler için aşağıdaki API endpoint'leri sağlanmıştır:


GET: Zafiyet detayları ve listeleme.


POST: Yeni zafiyet ekleme.


PUT: Zafiyet güncelleme.


DELETE: Zafiyet silme.


Validasyon


Örneğin:


"Name" alanı boş olamaz.


"Severity" alanı yalnızca belirli değerlerde olmalıdır.


Dockerize


Proje, Docker ile konteynerleştirilmiştir.


Github


Proje GitHub'a yüklenmiştir.


GitHub üzerinde her değişiklikte otomatik olarak yeni bir build alınması için GitHub Actions kullanılmıştır.


Swagger Entegrasyonu


Kullanıcılar Swagger üzerinden:


Zafiyetleri listeleyebilir.


Detaylarını görüntüleyebilir.


Güncelleme ve silme işlemleri yapabilir.


Yeni zafiyetler ekleyebilir.


Zafiyetlerin durum yönetimi sırasında kullanıcı bilgilendirilir ve hata durumları düzgün şekilde yönetilir.


Teknolojiler


C# / .NET Core: Backend geliştirme.


PostgreSQL: Veritabanı yönetimi.


JWT: Kimlik doğrulama ve yetkilendirme.


Docker: Konteynerleştirme.


Swagger: API belgelemesi ve kullanıcı etkileşimi.


GitHub Actions: Otomatik build işlemleri.


![api](https://github.com/user-attachments/assets/4e351782-f93c-4a20-9e55-e922c6fa0c50)



![docker](https://github.com/user-attachments/assets/a8ceb39e-0af5-4cc1-9ed3-77a953f482c8)



![container](https://github.com/user-attachments/assets/21bb79c9-2eda-411d-9241-114c5c3893e4)
