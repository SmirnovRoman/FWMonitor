public class Settings{
    /// <summary>
    /// Строка соединения
    /// </summary>
    public string ConnectionString {get;set;}        
    /// <summary>
    /// адрес прослушивания веб интерфейса http://xxx:8000/ 
    /// </summary>
    public string ListenURL {get;set;}    
    /// <summary>
    /// Полный путь к лог файлу
    /// </summary>
    public string LogFile {get;set;}
    /// <summary>
    /// Писать в консоль
    /// </summary>
    public bool Show2Console {get;set;}

    /// <summary>
    /// Расположениее хранилища для сохранения индексов
    /// </summary>
    public string StorageLocation { get;set;}

    /// <summary>
    /// Загружать cab файлы
    /// </summary>
    public bool DownloadCab {get;set;}

    /// <summary>
    /// Парсить cab файлы в базу
    /// </summary>
    public bool ParseCab {get;set;}
    
    /// <summary>
    /// Парсить UEFI файлы
    /// </summary>
    public bool ParseUEFI {get;set;}

}