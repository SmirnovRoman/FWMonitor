проект к статье 
[![DOI](https://zenodo.org/badge/DOI/10.5281/zenodo.8288619.svg)](https://doi.org/10.5281/zenodo.8288619)


**Установка и использование**

    1. Создать базу данных postgres с логином паролем
	пример:
        sudo -u postgres psql -U postgres -c "drop database dbfwmon";
        sudo -u postgres psql -U postgres -c "create user dbfwmon with encrypted password '1'";
        sudo -u postgres psql -U postgres -c "CREATE DATABASE dbfwmon WITH ENCODING='UTF8' OWNER=dbfwmon"
        sudo -u postgres psql -U postgres -d dbfwmon -c "create extension ltree;"


    2. Прописать в настройках в файле fwmonitor.json  - строку соединения 
        не занятый для www порт для прослушивания

    {
	"ConnectionString":"Host=192.168.56.2;Database=db1;Username=db1;Password=1",
	"ListenURL":"http://localhost:7001/",
	"LogFile":"fwmonitor_log.txt",
	"Show2Console":true,
	"StorageLocation":"Data/Storage",
	"DownloadCab":false,
	"ParseCab":false,
	"ParseUEIF":false
    }


    3. Запустить 
        fwmonitor setup
        будут созданы соответствующие таблицы 

    4. Прописать в crontab запуск вызова с параметром download
        например ежедневно в 13.00 

        crontab -l > crontab.tab
        echo "0 13 * * *   fwmonitor download" >> crontab.tab
        crontab crontab.tab
        
        для импорта отдельного файла 

        fwmonitor import имя_файла комментарий

    5. Для просмотра лога загрузки через веб интерфейс необходимо запустить
        fwmonitor www    

    
