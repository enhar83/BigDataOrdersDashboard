.NET & ML.NET tabanlı Büyük Veri Analitiği ve Tahminleme Paneli:
Bu proje, yaklaşık 2 milyon satırlık gerçek bir sipariş veri kümesi (dataset) üzerinde analiz, görselleştirme ve yapay zeka tabanlı tahminleme süreçlerini yönetmek amacıyla geliştirilmiş kapsamlı bir veri analitiği platformudur. Sistem, veritabanı yönetiminden makine öğrenmesi hattına (ML Pipeline) kadar tam entegre bir mimari sunar.

Mimari Yapı ve Tasarım Desenleri:
Proje, sürdürülebilirlik ve test edilebilirlik ilkeleri doğrultusunda N-Tier (Katmanlı) Mimari üzerine inşa edilmiştir. Veri yönetimi ve iş mantığı süreçlerinde aşağıdaki profesyonel yaklaşımlar benimsenmiştir:

Onion & N-Tier Architecture: Proje; Core, Data, Business ve Presentation olmak üzere dört temel katmana ayrılmıştır. Bu sayede her katman kendi sorumluluğuna odaklanır ve bağımlılıklar minimize edilir.


Unit of Work & Repository Pattern: Veritabanı işlemleri merkezi bir noktadan, soyutlanmış bir yapı ile yönetilir. Bu desenler, özellikle milyonlarca satırlık verinin işlendiği senaryolarda veri tutarlılığını ve kodun yeniden kullanılabilirliğini sağlar.

Dependency Injection: Servisler ve repository yapıları, .NET'in yerleşik DI konteyneri kullanılarak yönetilir, bu da sistemin esnekliğini artırır.

Proje İçeriği ve Analitik Yetenekler: 
Platform, büyük veriyi ham halinden alıp stratejik kararlar alınmasını sağlayacak anlamlı verilere dönüştüren şu özellikleri barındırır:

1. Makine Öğrenmesi ve Tahminleme (ML.NET)

Satış ve Gelir Forecasting: Ülke ve şehir bazlı tarihsel veriler kullanılarak geleceğe yönelik satış ve gelir tahminleri (forecasting) üretilir.


Duygu Analizi (Sentiment Analysis): Müşteri geri bildirimleri ve mesajları, ML.NET Prediction Engine kullanılarak "Olumlu" veya "Olumsuz" olarak sınıflandırılır.


Geleceğe Yönelik Tahminleme: Sipariş sayıları ve gelir trendleri üzerinden ileriye dönük projeksiyonlar oluşturulur.

2. Büyük Veri Analizi ve Müşteri Analitiği

Müşteri Sadakat Skoru (Loyalty Score): Veri madenciliği teknikleriyle her bir müşterinin satın alma davranışı analiz edilerek özel sadakat skorları hesaplanır.

RFM Analizi: Gerçek veriler üzerinden Recency (Yenilik), Frequency (Sıklık) ve Monetary (Tutar) analizi yapılarak müşteri segmentasyonu gerçekleştirilir.


Lokasyon Bazlı Analiz: Şehir bazlı satış yoğunlukları ve performans verileri analiz edilir.

3. İnteraktif Dashboard ve Veri Görselleştirme

Dinamik Grafikler: Chart.js ve Bootstrap entegrasyonu ile karmaşık veri setleri anlaşılır ve görsel grafiklere dönüştürülür.


ASP.NET Core ViewComponent: Panel üzerindeki veriler, ViewComponent yapısıyla asenkron ve modüler bir şekilde sunulur.
