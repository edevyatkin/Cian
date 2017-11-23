[![Build Status](https://travis-ci.org/edevyatkin/Cian.svg?branch=master)](https://travis-ci.org/edevyatkin/Cian)

# Сборщик писем от ЦИАН
## Зачем нужно
Если мы подписываемся на новые предложения с ЦИАНа, то периодически на почту приходят письма. Если подписок много, с разными фильтрами, то писем становится много. Если не просматривать почту несколько дней, то последующий их перебор очень монотонное и скучное дело. 

Для просмотра содержимого всех писем одним списком было написано данное приложение.
## Как работает
1. Скачивает свежие письма из вашего почтового ящика.
2. Разбирает каждое письмо и вытаскивает из него все предложения.
3. Формирует общий список всех предложений.
Есть веб-версия и just for fun - консольная.
## Как запустить
Требует .NET Core 2.0

Редактируем файл imapConfig.json, вписывая нужные параметры для вашей электронной почты. Дополнительно указываем папку, куда ваш почтовый сервис сложил письма (я настраивал фильтр на яндекс почте по адресу noreply@cian.ru и теме "Свежие предложения по вашей подписке на ЦИАН", переместить в папку "Недвижимость").

Запускаем веб-версию с помощью .bat-файла launch.bat.
Дальше открываем в браузере http://localhost:8888

Консольная версия запускается как обычно: dotnet run.

## Контакты
skype: evgeniy.devyatkin

почта: mail гав edevyatkin.com

## Лицензия 
MIT. Ну вы поняли.
