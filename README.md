# phone-assesment  açıklamalar


#docker compose'u çalıştırmak için 

<code>docker-compose up -d --build  </code>

#docker compose kapatmak için

<code>docker-compose down</code>

Compose'da ayağa 1 mongodb, 1 rabbitmq servisi ve 1 adet api ve 1 adet worker servis ayağa kalkacaktır.

Docker ayağa kalktıktan sonra api'ye  [link](https://localhost:9081/swagger/index.html) adresinden ulaşabilirsiniz.

İstemiş olduğunuz mimaride rabbitmq ile rapor isteğini başlatmak için  Api'de ***/CreateLocationReport*** endpointini tetiklemeniz yeterlidir.
Oluşan rapor isteği önce mongo'da phonebookreports altına kayıt atılıyor, ardından rabbitmq'ya mesaj gönderiyor. Bu mesaj rabbitmq'yu dinleyen
worker servisi tarafından işlenip veritabanında oluşan raporun durumu güncelleniyor. Burada raporu işlendiğini göstermek için koda 7 saniyelik bir bekleme koydum.

Bu arada ***/CreateLocationReport*** üzerinde dönen traceid değeri ile ***/GetReportStatusById*** endpointi ile oluşan durumu sorgulayabilirsiniz. ***/GetReportDetailById*** endpointi
ile detay bilgisi gelecektir.


Unit test skorlarına bakmak için
Phonebook.XUnit.Tests altında coveragereports dosyası göreceksiniz. Bunun içinde index.html altında skoru görebilirsiniz.

07.04.2023
- veritabanları sayısı ikiye çıkarıldı
- report api eklendi.
- phone api üzerinde yer alan report endpointleri, gelen isteği arka planda report apiye yönlendirmektedir.
