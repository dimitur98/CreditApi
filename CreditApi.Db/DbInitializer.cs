using Dapper.Base;

namespace Credit.Api.Db
{
    public class DbInitializer :IDbInitializer
    {
        private readonly IDapperBase _dapper;

        public DbInitializer(IDapperBase dapper)
        {
            _dapper = dapper;
        }

        public async Task InitializeAsync()
        {
            var sql = @"
            CREATE TABLE IF NOT EXISTS CreditStatus (
                Id INTEGER PRIMARY KEY  AUTOINCREMENT NOT NULL,
                Name TEXT NOT NULL UNIQUE
            );

            INSERT OR IGNORE INTO CreditStatus (name) VALUES
            ('Paid'),
            ('AwaitingPayment'),
            ('Created');

            CREATE TABLE IF NOT EXISTS Credit (
                Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                CreditNumber TEXT UNIQUE NOT NULL,
                CustomerName TEXT NOT NULL,
                Amount REAL NOT NULL,
                DateRequested DATETIME NOT NULL,
                StatusId INTEGER NOT NULL,
                FOREIGN KEY (StatusId) REFERENCES CreditStatus(Id)
            );

            CREATE TABLE IF NOT EXISTS Invoice (
                Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                InvoiceNumber TEXT UNIQUE NOT NULL,
                Amount REAL NOT NULL,
                CreditId INTEGER NOT NULL,
                FOREIGN KEY (CreditId) REFERENCES Credit(Id) ON DELETE CASCADE
            );

            INSERT OR IGNORE INTO Credit (CreditNumber, CustomerName, Amount, DateRequested, StatusId) 
            VALUES 
                ('CRD001', 'John Doe', 5000, '2024-02-20 12:21', 1),
                ('CRD002', 'Jane Smith', 3000, '2024-02-21 13:42', 2),                
                ('CRD003', 'Kael Draven', 10500, '2025-01-15 10:15', 3),
                ('CRD004', 'Bodie Sinclair', 9000, '2025-02-10 16:45', 2);

            INSERT OR IGNORE INTO Invoice (InvoiceNumber, Amount, CreditId) 
            VALUES 
                ('INV001', 2000, 1),
                ('INV003', 3000, 1),
                ('INV004', 3000, 2),
                ('INV005', 3000, 3);";

            await _dapper.ExecuteAsync(sql);
        }
    }
}