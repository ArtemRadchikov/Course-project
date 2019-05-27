namespace Helper.Model
{

    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection.Emit;

    public class HelperContext : DbContext
    {
        // Контекст настроен для использования строки подключения "HelperContext" из файла конфигурации  
        // приложения (App.config или Web.config). По умолчанию эта строка подключения указывает на базу данных 
        // "Helper.Model.HelperContext" в экземпляре LocalDb. 
        // 
        // Если требуется выбрать другую базу данных или поставщик базы данных, измените строку подключения "HelperContext" 
        // в файле конфигурации приложения.
        public HelperContext()
            : base("name=HelperContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Decision> Decisions { get; set; }
        public DbSet<KeyWordItem> KeyWordItems { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<OriginalValue> OriginalValues { get; set; }
        public DbSet<Coefficient_a0> Coefficient_a0s { get; set; }
        public DbSet<Coefficient_an> Coefficient_ans { get; set; }
        public DbSet<Coefficient_bn> Coefficient_bns { get; set; }
        public DbSet<FourierSeries> FourierSeriess { get; set; }
        public DbSet<PartialSum_k> PartialSum_ks { get; set; }

        
    }

    
}