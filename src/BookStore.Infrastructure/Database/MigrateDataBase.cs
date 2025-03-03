using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace BookStore.Infrastructure.Database;

public static class MigrationExtensions
{
    public static void MigrateDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BookStoreDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<BookStoreDbContext>>();

        try
        {
            logger.LogInformation("Iniciando migração do banco de dados...");
                

            bool databaseExists = dbContext.Database.CanConnect();
                
            if (!databaseExists)
            {
 
                logger.LogInformation("Banco de dados não existe. Criando esquema...");
                dbContext.Database.EnsureCreated();
                logger.LogInformation("Esquema do banco de dados criado com sucesso!");
            }
            else
            {
                logger.LogInformation("Banco de dados já existe. Verificando migrações...");
                    
                try
                {

                    dbContext.Database.Migrate();
                    logger.LogInformation("Migrações aplicadas com sucesso!");
                }
                catch (Exception migrationEx)
                {
                    logger.LogWarning("Erro ao aplicar migrações: {Error}. O banco pode ter sido criado sem histórico de migrações.", migrationEx.Message);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro durante a inicialização do banco de dados");
            throw;
        }
    }
}
